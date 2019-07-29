using App.Core.Common.Entities;
using App.Core.Workflow;
using App.Core.Workflow.Engine.Definition;
using App.Core.Workflow.Providers;
using Microsoft.AspNetCore.Mvc;
using App.Web.Authentication;
using App.Workflow.ProcessDefinitions;
using App.Workflow.ProcessDefinitions.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using App.Web.Filters;

namespace App.Web.Controllers.Workflow
{
    /// <summary>
    /// 流程-流程定义
    /// </summary>
    [ApiVersion("1.0")]
    [MyAuthorize]
    [Route("api/v{version:apiVersion}/process-definitions")]
    public class ProcessDefinitionController : ControllerBase
    {
        private IDefinitionProvider _processProvider;
        private IProcessDefinitionService _processDefinitionService;
        public ProcessDefinitionController(IWfEngine wfEngine, IProcessDefinitionService processDefinitionService)
        {
            _processProvider = wfEngine.GetDefinitionProvider();
            _processDefinitionService = processDefinitionService;
        }

        /// <summary>
        /// 获取流程定义
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        public ProcessDefinition Get(int id)
        {
            return _processProvider.Get(id);
        }

        /// <summary>
        /// 获取带角色的流程定义
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("for-edit/{id:int}")]
        public ProcessDefinitionWithRole GetForEdit(int id)
        {
            var process = _processProvider.Get(id);
            return new ProcessDefinitionWithRole
            {
                ProcessDefinition = process,
                RoleIds = _processDefinitionService.GetIdByProcess(process.Id) 
            };
        }

        /// <summary>
        /// 通过名称获取流程定义
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("by-name")]
        public ProcessDefinition GetByName(string name)
        {
            return _processProvider.Get(name);
        }


        /// <summary>
        /// 新增流程定义
        /// </summary>
        /// <param name="definition"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Add([FromBody]ProcessDefinitionWithRole definition)
        {
            int id = _processDefinitionService.Add(definition);
            return Created("", new { });
        }

        /// <summary>
        /// 分页获取流程定义列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyword"></param>
        /// <param name="category">分类</param>
        /// <returns></returns>
        [HttpGet]
        public PaginationData<App.Core.Workflow.ProcessDefinitions.Dto.GetProcessDefinitionListOutput> Get(int pageIndex, int pageSize, string keyword, string category)
        {
            Expression<Func<App.Core.Workflow.ProcessDefinitions.Wf_Re_ProcessDefinition, bool>> condition = null;
            if (!string.IsNullOrEmpty(category))
            {
                condition = u => u.Category == category;
            }
            return _processDefinitionService.Get(pageIndex, pageSize, keyword, condition);
        }

    }
}
