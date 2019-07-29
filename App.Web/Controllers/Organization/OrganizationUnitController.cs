using App.Core.Authorization.OrganizationUnits;
using App.Core.Authorization.OrganizationUnits.Dto;
using App.Core.Authorization.Users;
using App.Core.Authorization.Users.Dto;
using Microsoft.AspNetCore.Mvc;
using App.Web.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace App.Web.Controllers.Organization
{
    /// <summary>
    /// 组织机构-单位部门
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/organization-units")]
    public class OrganizationUnitController : ControllerBase
    {
        private IOrganizationUnitService _orgUnitService;
        public OrganizationUnitController(
            IOrganizationUnitService orgUnitService
            )
        {
            _orgUnitService = orgUnitService;
        }


        /// <summary>
        /// 获取单位部门树
        /// </summary>
        /// <returns></returns>
        [MyAuthorize]
        [HttpGet("")]
        public List<GetOrganizationUnitOutput> GetAll()
        {
            return _orgUnitService.GetAll();
        }

        /// <summary>
        /// 获取带人员的单位部门树
        /// </summary>
        /// <param name="roleStr">角色列表，逗号隔开</param>
        /// <returns></returns>
        [MyAuthorize]
        [HttpGet("with-user")]
        public List<GetOrganizationUnitWithUserOutput> GetAllWithUser(string roleStr)
        {
            Expression<Func<User, bool>> expr = null;
            if (!string.IsNullOrEmpty(roleStr))
            {
                List<string> roleList = roleStr.Split(",").ToList();
                expr = u => u.UserRoles.Any(v => roleStr.Contains(v.Role.Name));
            }
            return _orgUnitService.GetAllWithUser(expr);
        }


        /// <summary>
        /// 获取单位详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [MyAuthorize]
        [HttpGet("{id:int}")]
        public GetOrganizationUnitOutput GetUnit(int id)
        {
            return _orgUnitService.Get(id);
        }

        /// <summary>
        /// 获取单位的子单位
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [MyAuthorize]
        [HttpGet("{id:int}/children")]
        public List<GetOrganizationUnitOutput> GetChildren(int id)
        {
            return _orgUnitService.GetChildren(id);
        }

        /// <summary>
        /// 获取单位下的人员列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [MyAuthorize]
        [HttpGet("{id:int}/users")]
        public List<GetUserListOutput> GetUsers(int id)
        {
            return _orgUnitService.GetUsersInUnit(id);
        }

        /// <summary>
        /// 新增组织机构
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        [MyAuthorize]
        [HttpPost()]
        public IActionResult Add([FromBody]AddOrganizationUnitInput unit)
        {
            int id = _orgUnitService.Add(unit);
            return Created("", new {id});
        }

        /// <summary>
        /// 更新组织机构
        /// </summary>
        /// <param name="id"></param>
        /// <param name="unit"></param>
        /// <returns></returns>
        [MyAuthorize]
        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody] UpdateOrganizationUnitInput unit)
        {
            unit.Id = id;
            _orgUnitService.Update(unit);
            return Created("", new {id});
        }

        /// <summary>
        /// 删除单位部门
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [MyAuthorize]
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            _orgUnitService.Delete(id);
            return NoContent();
        }


    }
}