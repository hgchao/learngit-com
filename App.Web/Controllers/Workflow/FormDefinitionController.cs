using App.Core.Common.Entities;
using App.Core.Form.FormDefinitions.Dto;
using App.Core.Workflow;
using App.Core.Workflow.Providers;
using Microsoft.AspNetCore.Mvc;
using App.Web.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Web.Filters;

namespace App.Web.Controllers.Workflow
{
    /// <summary>
    /// 流程-表单定义
    /// </summary>
    [ApiVersion("1.0")]
    [MyAuthorize]
    [Route("api/v{version:apiVersion}/form-definitions")]
    public class FormDefinitionController: ControllerBase
    {
        private IWfEngine _wfEngine;
        private IFormProvider _formProvider;
        public FormDefinitionController(IWfEngine wfEngine)
        {
            _wfEngine = wfEngine;
            _formProvider = _wfEngine.GetFormProvider();
        }

        /// <summary>
        /// 通过Id获取表单定义
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        public GetFormDefinitionOutput Get(int id)
        {
            return _formProvider.Get(id);
        }

        /// <summary>
        /// 通过名称获取表单定义
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("by-name")]
        public GetFormDefinitionOutput GetByName(string name)
        {
            return _formProvider.Get(name);
        }


        /// <summary>
        /// 新增表单定义
        /// </summary>
        /// <param name="definition"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Add([FromBody]AddFormDefinitionInput definition)
        {
            _formProvider.DeployNewFormDefinition(definition);
            return Created("", new { });
        }

        /// <summary>
        /// 分页获取表单定义列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [HttpGet]
        public PaginationData<GetFormDefinitionPoorOutput> Get(int pageIndex, int pageSize, string keyword)
        {
            return _formProvider.Get(pageIndex, pageSize, keyword);
        }

    }
}
