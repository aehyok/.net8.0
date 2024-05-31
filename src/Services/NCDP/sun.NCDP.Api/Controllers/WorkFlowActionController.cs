﻿using Ardalis.Specification;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sun.Core.Domains;
using sun.Core.Dtos.WorkFlow;
using sun.Core.Services.WorkFlow;
using sun.EntityFrameworkCore.Repository;
using sun.Infrastructure.Exceptions;

namespace sun.NCDP.Api.Controllers
{
    /// <summary>
    /// 工作流动作定义
    /// </summary>
    /// <param name="workFlowActionService"></param>
    public class WorkFlowActionController(IWorkFlowActionService workFlowActionService) : NCDPControllerBase
    {
        /// <summary>
        /// 获取工作流动作定义列表
        /// </summary>
        /// <param name="stateId">工作流程定义Id</param>
        /// <returns></returns>
        [HttpGet("list/{stateId}")]
        public async Task<List<WorkFlowActionDto>> GetStateListAsync(long stateId)
        {
            var spec = Specifications<WorkFlowAction>.Create();

            spec.Query.Where(a => a.WorkFlowStateId == stateId);

            return await workFlowActionService.GetListAsync<WorkFlowActionDto>(spec);
        }


        /// <summary>
        /// 获取工作流动作定义详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("detail/{id}")]
        public async Task<WorkFlowActionDto> GetStateByIdAsync(long id)
        {
            return await workFlowActionService.GetAsync<WorkFlowActionDto>(a => a.Id == id);
        }

        /// <summary>
        /// 添加工作流动作定义
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<long> PostStateAsync(CreateWorkFlowActionDto model)
        {
            var entity = this.Mapper.Map<WorkFlowAction>(model);
            await workFlowActionService.InsertAsync(entity);
            return entity.Id;
        }

        /// <summary>
        /// 修改工作流动作定义
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpPut]
        public async Task<StatusCodeResult> PutStatusAsync(long id, CreateWorkFlowActionDto model)
        {
            var entity = await workFlowActionService.GetAsync(a => a.Id == id) ?? throw new Exception("修改的数据不存在");

            entity = this.Mapper.Map(model, entity);

            await workFlowActionService.UpdateAsync(entity);
            return Ok();
        }

        /// <summary>
        /// 删除工作流动作定义
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<StatusCodeResult> DeleteStateAsync(long id)
        {
            await workFlowActionService.DeleteAsync(id);
            return Ok();
        }

        /// <summary>
        /// 启用工作流动作定义
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="ErrorCodeException"></exception>
        [HttpPut("Enable/{id}")]
        public async Task<StatusCodeResult> EnableDefine(long id)
        {
            var entity = await workFlowActionService.GetByIdAsync(id);

            if (entity is null)
            {
                throw new ErrorCodeException(-1, "启用的数据不存在");
            }
            entity.IsEnable = true;

            await workFlowActionService.UpdateAsync(entity);

            return Ok();
        }

        /// <summary>
        /// 禁用工作流动作定义
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="ErrorCodeException"></exception>
        [HttpPut("Disable/{id}")]
        public async Task<StatusCodeResult> DisableDefine(long id)
        {
            var entity = await workFlowActionService.GetByIdAsync(id);

            if (entity is null)
            {
                throw new ErrorCodeException(-1, "禁用的数据不存在");
            }
            entity.IsEnable = false;

            await workFlowActionService.UpdateAsync(entity);

            return Ok();
        }
    }
}