﻿using aehyok.Infrastructure.TypeFinders;
using aehyok.RabbitMQ.EventBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.Concurrent;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace aehyok.RabbitMQ
{
    /// <summary>
    /// RabbitMQ事件订阅者
    /// </summary>
    public class EventSubscriber: IEventSubscriber
    {
        private readonly ILogger<EventSubscriber> logger;
        private readonly IConnection connection;
        private readonly ConcurrentBag<Type> eventTypes;
        private readonly IServiceScopeFactory scopeFactory;
        private readonly ConcurrentDictionary<string, List<Type>> EventHandlerFactories;
        private readonly RabbitOptions options;

        private IModel consumerChannel;

        /// <summary>
        /// 添加构造函数
        /// </summary>
        public EventSubscriber(ILogger<EventSubscriber> logger, IConnection connection, IServiceScopeFactory scopeFactory, IOptions<RabbitOptions> options)
        {
            this.options = options.Value;
            this.logger = logger;
            this.connection = connection;
            this.scopeFactory = scopeFactory;
            this.eventTypes = new ConcurrentBag<Type>();
            this.EventHandlerFactories = new ConcurrentDictionary<string, List<Type>>();
            this.consumerChannel = CreateConsumerChannel();
        }

        public void Dispose()
        {
            this.logger.LogInformation($"IEventSubscriber Dispose");
            this.consumerChannel?.Dispose();
        }

        public void Subscribe(Type eventType, Type eventHandlerType)
        {
            var eventName = eventType.FullName;

            if(!this.eventTypes.Where(item=>item.FullName==eventName).Any())
            {
                this.eventTypes.Add(eventType);
            }

            this.EventHandlerFactories.AddOrUpdate(eventName, new List<Type> { eventHandlerType }, (key, list) =>
            {
                list.Add(eventHandlerType);
                return list;
            });

            this.consumerChannel.QueueBind(this.options.QueueName, this.options.ExchangeName, eventName);
        }

        private IModel CreateConsumerChannel()
        {
            var channel = this.connection.CreateModel();

            channel.ExchangeDeclare(this.options.ExchangeName, ExchangeType.Fanout, true);

            //exclusive false代表队列非独占
            channel.QueueDeclare(this.options.QueueName,true,exclusive: false);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.Received += OnConsumerMessageReceived;

            channel.BasicConsume(this.options.QueueName, false, consumer);

            return channel;
        }

        private async Task OnConsumerMessageReceived(object sender, BasicDeliverEventArgs eventArgs)
        {
            var eventName = eventArgs.RoutingKey;
            var message = Encoding.UTF8.GetString(eventArgs.Body.ToArray());

            // 在日志中打印消息
            this.logger.LogInformation($"Message Received: {eventName} => {message}");

            if (await ProcessEvent(eventName, message))
            {
                this.consumerChannel.BasicAck(eventArgs.DeliveryTag, false);
            }
            else
            {
                // 如果是订阅模式，则将消息重新入列
                //this.consumerChannel.BasicReject(eventArgs.DeliveryTag, eventArgs.Exchange == options.DirectExchangeName);
            }
        }

        private async Task<bool> ProcessEvent(string eventName, string message)
        {
            try
            {
                Type eventType = null;

                var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(item => item.FullName.StartsWith("aehyok."));

                eventType = this.eventTypes.SingleOrDefault(item => item.FullName == eventName);

                if(this.EventHandlerFactories.TryGetValue(eventName, out var eventHandlers) && eventHandlers.Count > 0)
                {
                    foreach (var eventHandler in eventHandlers)
                    {
                        var handler = (IEventHandler)Activator.CreateInstance(eventHandler);

                        var eventData = JsonSerializer.Deserialize(message, eventType, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                        using var scope = this.logger.BeginScope(new Dictionary<string, object>
                        {
                            ["EventBusId"] = ((EventBase)eventData).Id,
                            ["Handler"] = handler.GetType().FullName,
                        });

                        try
                        {
                            // 创建一个异步操作点，允许异步方法在执行时暂时释放线程，以允许其他任务在同一线程上执行。
                            // 使得方法的剩余部分可以在稍后的时间继续执行。
                            await Task.Yield();
                            var concreteType = typeof(IEventHandler<>).MakeGenericType(eventType);
                            this.logger.LogInformation($"开始执行 {eventName} 事件, 内容：{message}");
                            await (Task)concreteType.GetMethod("HandleAsync").Invoke(handler, new object[] { eventData });
                        }
                        catch (Exception ex)
                        {
                            this.logger.LogInformation($"事件处理程序处理事件时发生错误，消息内容:{message}");
                            this.logger.LogError(ex, ex.Message);
                        }
                        finally
                        {
                            this.logger.LogInformation($"事件 {eventName} 执行完成");
                            scope.Dispose();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return true;
        }
    }
}
