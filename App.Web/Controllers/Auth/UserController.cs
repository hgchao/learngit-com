using App.Core.Authorization.Roles.Dto;
using App.Core.Authorization.Users;
using App.Core.Authorization.Users.Dto;
using App.Core.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using App.Web.Filters;
using Microsoft.AspNetCore.Mvc;
using App.Core.Authorization.Accounts;
using Newtonsoft.Json;

namespace App.Web.Controllers.Auth
{
    /// <summary>
    /// 验证授权-用户
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/users")]
    public class UserController: ControllerBase
    {
        private IUserService _userService;
        private IAuthInfoProvider _authInfoProvider;

        public UserController(
            IUserService userService,
            IAuthInfoProvider authInfoProvider
            )
        {
            _authInfoProvider = authInfoProvider;
            _userService = userService;
        }


        /// <summary>
        /// 更新密码
        /// </summary>
        /// <param name="id"></param>
        /// <param name="passwordObject"></param>
        /// <returns></returns>
        [MyAuthorize]
        [HttpPut("{id:int}/password")]
        public IActionResult UpdatePassword(int id, [FromBody] PasswordObject passwordObject)
        {
            _userService.UpdatePassword(id, passwordObject.Password);
            return Created("", new { id});
        }

        /// <summary>
        /// 更新快捷方式
        /// </summary>
        /// <param name="id"></param>
        /// <param name="menuIdList"></param>
        /// <returns></returns>
        [MyAuthorize]
        [HttpPut("{id:int}/shortcut")]
        public IActionResult UpdateShortCut(int id, [FromBody] List<int> menuIdList)
        {
            _userService.SetShortcut(id, menuIdList);
            return Created("", new { id});
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [MyAuthorize]
        [HttpGet("{id:int}")]
        public GetUserOutput Get(int id)
        {
            return _userService.Get(id);
        }

        /// <summary>
        /// 获取用户简要信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [MyAuthorize]
        [HttpGet("poor/{id:int}")]
        public GetUserPoorOutput GetPoor(int id)
        {
            return _userService.GetPoor(id);
        }

        /// <summary>
        /// 获取用户简要信息列表
        /// </summary>
        /// <param name="idListStr"></param>
        /// <returns></returns>
        [MyAuthorize]
        [HttpGet("poor")]
        public List<GetUserPoorOutput> GetPoor(string idListStr)
        {
            return _userService.GetPoor(idListStr);
        }

        /// <summary>
        /// 获取所有用户简要信息列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [MyAuthorize]
        [HttpGet("all-poor")]
        public List<GetUserPoorOutput> GetAllPoor()
        {
            return _userService.GetAllPoor();
        }

        /// <summary>
        /// 分页获取用户信息
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页条数</param>
        /// <param name="keyword">关键字</param>
        /// <returns></returns>
        [MyAuthorize]
        [HttpGet("")]
        public PaginationData<GetUserListOutput> Get(int pageIndex, int pageSize, string keyword = null)
        {
            return _userService.Get(pageIndex, pageSize, keyword);
        }

        /// <summary>
        /// 根据姓名查询用户列表
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [MyAuthorize]
        [HttpGet("by-name")]
        public List<GetUserListOutput> GetByName(string name)
        {
            return _userService.Get(u => u.Name.Contains(name));
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [MyAuthorize]
        [HttpPost("")]
        public IActionResult AddUser([FromBody]AddUserInput user)
        {
            var request = Request.Query;
            int id = _userService.Add(user);
            return Created("", new { id });
        }


        /// <summary>
        /// 更新用户
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        [MyAuthorize]
        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody]UpdateUserInput user)
        {
            user.Id = id;
            _userService.Update(user);
            return Created("", new { user.Id });
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [MyAuthorize]
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            _userService.Delete(id);
            return NoContent();
        }

    }
    public class PasswordObject
    {
        public string Password { get; set; }
    }
}