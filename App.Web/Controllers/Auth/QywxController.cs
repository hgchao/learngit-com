using App.Core.Authorization.Users;
using App.Core.Authorization.Users.Dto;
using Microsoft.AspNetCore.Mvc;
using App.Web.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Web.Controllers.Auth
{
    [ApiVersion("1.0")]
    [MyAuthorize]
    [Route("api/v{version:apiVersion}/qywx")]
    public class QywxController: ControllerBase
    {
        private IUserService _userService;
        public QywxController(
            IUserService userService
            )
        {
            _userService = userService;
        }
        /// <summary>
        /// 绑定企业微信账户
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult BindQyWeixin([FromBody]QyWeixinInput input)
        {
            _userService.BindQyWeixin(input);
            return Created("", null);
        }

        /// <summary>
        /// 取消绑定企业微信账户
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{id:int}")]
        public IActionResult UnbindQyWeixin(int id)
        {
            _userService.UnbindQyWeixin(id);
            return NoContent();
        }

        /// <summary>
        /// 获取企业微信成员列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("userlist")]
        public List<GetQyWeixinListOutput> GetQyWeixinUserList()
        {
            return _userService.GetQyWeixin();
        }
    }
}
