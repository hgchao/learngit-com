using App.Core.Authorization.Apis.Dto;
using App.Core.Authorization.Modules;
using App.Core.Authorization.Modules.Dto;
using Microsoft.AspNetCore.Mvc;
using App.Web.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace App.Web.Controllers.Auth
{
    /// <summary>
    /// 验证授权-模块
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/modules")]
    public class ModuleController : ControllerBase
    {
        private IModuleService _moduleService;
        public ModuleController(IModuleService moduleService)
        {
            _moduleService = moduleService;
        }

        /// <summary>
        /// 获取所有模块
        /// </summary>
        /// <returns></returns>
        [MyAuthorize]
        [HttpGet("")]
        public List<GetModuleOutput> GetAll()
        {
            return _moduleService.GetAll();
        }

        /// <summary>
        /// 获取模块详情
        /// </summary>
        /// <returns></returns>
        [MyAuthorize]
        [HttpGet("{id:int}")]
        public GetModuleOutput Get(int id)
        {
            return _moduleService.Get(id);
        }

        /// <summary>
        /// 获取模块的子模块
        /// </summary>
        /// <returns></returns>
        [MyAuthorize]
        [HttpGet("{id:int}/children")]
        public List<GetModuleOutput> GetChildren(int id)
        {
            return _moduleService.GetChildren(id);
        }

        /// <summary>
        /// 新增模块
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        [MyAuthorize]
        [HttpPost("")]
        public IActionResult Add([FromBody]AddModuleInput module)
        {
            int id = _moduleService.Add(module);
            return Created("", new { id });
        }

        /// <summary>
        /// 更新模块
        /// </summary>
        /// <param name="id"></param>
        /// <param name="module"></param>
        /// <returns></returns>
        [MyAuthorize]
        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody]UpdateModuleInput module)
        {
            module.Id = id;
            _moduleService.Update(module);
            return Created("", new { module.Id });
        }

        /// <summary>
        /// 删除模块
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [MyAuthorize]
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            _moduleService.Delete(id);
            return NoContent();
        }
    }
}