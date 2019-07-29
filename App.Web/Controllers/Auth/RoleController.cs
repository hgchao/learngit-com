using App.Core.Authorization.Functions.Dto;
using App.Core.Authorization.Menus.Dto;
using App.Core.Authorization.Modules.Dto;
using App.Core.Authorization.Roles;
using App.Core.Authorization.Roles.Dto;
using App.Core.Common.Entities;
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
    /// 验证授权-角色信息
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/roles")]
    public class RoleController: ControllerBase
    {
        private IRoleService _roleService;
        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [MyAuthorize]
        [HttpGet("{id:int}")]
        public GetRoleOutput Get(int id)
        {
            return _roleService.Get(id);
        }


        /// <summary>
        /// 按条件分页获取角色信息
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页条数</param>
        /// <param name="keyword">搜索关键字</param>
        /// <returns></returns>
        [MyAuthorize]
        [HttpGet("")]
        public PaginationData<GetRoleListOutput> Get(int pageIndex, int pageSize, string keyword = null)
        {
            return _roleService.Get(pageIndex, pageSize, keyword);
        }

        /// <summary>
        /// 新增角色
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        [MyAuthorize]
        [HttpPost("")]
        public IActionResult Add([FromBody]AddRoleInput role)
        {
            int id = _roleService.Add(role);
            return Created("", new { id });
        }

        /// <summary>
        /// 更新角色
        /// </summary>
        /// <param name="id"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        [MyAuthorize]
        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody]UpdateRoleInput role)
        {
            role.Id = id;
            _roleService.Update(role);
            return Created("", new { role.Id });
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [MyAuthorize]
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            _roleService.Delete(id);
            return NoContent();
        }

    }
}