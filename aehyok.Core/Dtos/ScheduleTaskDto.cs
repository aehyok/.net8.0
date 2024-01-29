﻿using aehyok.Core.Domains;
using aehyok.Infrastructure.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aehyok.Core.Dtos
{
    public class ScheduleTaskDto: DtoBase
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 定时任务类名
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Cron表达式
        /// </summary>
        public string Expression { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public ScheduleTaskStatus Status { get; set; }

        /// <summary>
        /// 下一次执行时间
        /// </summary>
        public DateTime NextExecuteTime { get; set; }
        
        /// <summary>
        /// 最后一次执行时间
        /// </summary>
        public DateTime LastExecuteTime { get; set; }


    }
}
