using App.Core.Authorization;
using App.Core.Authorization.Accounts;
using App.Core.Authorization.Users;
using App.Core.Authorization.Users.Dto;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using App.Entry;
using App.Web.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using App.Workflow.ProcessDefinitions;

namespace App.Web.Controllers.Auth
{
    /// <summary>
    /// 验证授权-账户
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/accounts")]
    public class AccountController: ControllerBase
    {
        private IAccountService _accountService;
        private IAuthInfoProvider _authInfoProvider;
        private IUserService _userService;
        private IProcessDefinitionService _processDefinitionService;

        public AccountController(
            IAccountService accountService, IAuthInfoProvider authInfoProvider, IUserService userService,
            IProcessDefinitionService processDefinitionService
            )
        {
            _processDefinitionService = processDefinitionService;
            _accountService = accountService;
            _authInfoProvider = authInfoProvider;
            _userService = userService;
        }

        /// <summary>
        /// 获取帐户自身信息
        /// </summary>
        /// <returns></returns>
        [MyAuthorize]
        [HttpGet()]
        public UserWithCompany Get()
        {
            var authInfo = _authInfoProvider.GetCurrent();
            var user = _userService.Get(authInfo.User.Id);
            var result = JsonConvert.DeserializeObject<UserWithCompany>(JsonConvert.SerializeObject(user));
            result.CompanyName = authInfo.CompanyName;
            result.ProcessNames = _processDefinitionService.GetNameByUser(authInfo.User.Id);
            return result;
        }

        /// <summary>
        /// 更新帐户自身信息
        /// </summary>
        /// <returns></returns>
        [MyAuthorize]
        [HttpPut()]
        public IActionResult UpdateSelf([FromBody]UpdateUserSelfInput input)
        {
            var authInfo = _authInfoProvider.GetCurrent();
            input.Id = authInfo.User.Id;
            _userService.UpdateSelf(input);
            return Created("", null);
        }

        /// <summary>
        /// 修改自身密码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [MyAuthorize]
        [HttpPut("password")]
        public IActionResult UpdatePassword([FromBody]UpdatePasswordInput input)
        {
            var authInfo = _authInfoProvider.GetCurrent();
            input.UserId = authInfo.User.Id;
            var result = _userService.UpdatePassword(input);
            switch (result)
            {
                case UpdatePasswordResult.NewPasswordInvalid:
                    return BadRequest("新密码无效");
                case UpdatePasswordResult.OldPasswordWrong:
                    return BadRequest("旧密码错误");
                default:
                    return Created("", null);
            }
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="account">账户</param>
        /// <param name="password">密码</param>
        /// <param name="clientType">客户端类型</param>
        /// <param name="weixinCode">微信登陆时，提交微信代码</param>
        /// <param name="appId">微信登陆时,提交登陆的appId</param>
        /// <returns></returns>
        [HttpGet("login")]
        public IActionResult GetToken(string account, string password, ClientType clientType= ClientType.PC)
        {
            string token = _accountService.GetToken(account, password, clientType);
            if (token != null)
            {

                _authInfoProvider.SetCurrent(new AuthInfo {
                    User = new GetUserOutput
                    {
                        Account = account
                    }
                });
                return Ok(new
                {
                    token = token
                });
            }
            else
            {
                return Unauthorized();
            }
        }

        /// <summary>
        /// 微信Code登陆
        /// </summary>
        /// <param name="weixinCode">账户绑定的微信代码</param>
        /// <param name="appid">微信AppId</param>
        /// <returns></returns>
        [HttpGet("login-wx")]
        public IActionResult GetTokenForWx(string weixinCode, string appid)
        {
            string token = _accountService.GetTokenForWx(weixinCode, appid);
            if (token != null)
            {
                return Ok(new
                {
                    token = token
                });
            }
            else
            {
                return Unauthorized();
            }
        }

        /// <summary>
        /// 企业微信登陆
        /// </summary>
        /// <param name="qyWxCode"></param>
        /// <returns></returns>
        [HttpGet("login-qywx")]
        public IActionResult GetTokenForQyWx(string qyWxCode)
        {
            string token = _accountService.GetTokenForQyWx(qyWxCode);
            if (token != null)
            {
                return Ok(new
                {
                    token = token
                });
            }
            else
            {
                return Unauthorized();
            }
        }

        /// <summary>
        /// 注销账号
        /// </summary>
        /// <param name="token">要注销的token</param>
        /// <returns></returns>
        [HttpDelete("logout")]
        public IActionResult Logout(string token)
        {
            _accountService.DisableToken(token);
            return NoContent();
        }

        /// <summary>
        /// 发送忘记密码邮件
        /// </summary>
        /// <param name="account">忘记密码的账户</param>
        /// <returns></returns>
        [HttpGet("send-forget-email")]
        public object ForgetPassword(string account)
        {
            SendPasswordResetEmailResult result = _accountService.SendPasswordResetEmail(account);
            switch (result.Code)
            {
                case PasswordResetEmailCode.AccountWrong:
                    return new { Code = "accountWrong", Msg = "账号错误" };
                case PasswordResetEmailCode.EmailFailed:
                    return new { Code = "emailFailed", Msg = "邮件发送失败" };
                case PasswordResetEmailCode.Success:
                    return new { Code = "success", Msg = "成功" };
                default:
                    return new { Code = "undefined" };
            }
        }

        /// <summary>
        /// 租户注册
        /// </summary>
        /// <param name="tenant"></param>
        [HttpPost("register")]
        public IActionResult Register([FromBody]TenantWithManager tenant)
        {
            _accountService.Register(tenant);
            return Created("", null);
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPut("reset-password")]
        public object ResetPassword([FromBody]ResetPasswordInfo info)
        {
            ResetPasswordResult result = _accountService.ResetPassword(info.Account, info.Password, info.Code);
            switch (result.Code)
            {
                case ResetPasswordCode.AccountWrong:
                    return new { Code = "accountWrong", Msg = "账号错误" };
                case ResetPasswordCode.CodeWrong:
                    return new { Code = "codeWrong", Msg = "代码错误" };
                case ResetPasswordCode.Success:
                    return new { Code = "success", Msg = "成功" };
                default:
                    return new { Code = "undefined" };
            }
        }

        [HttpGet("qywx-signature")]
        public object GetQywxJsApiSignature(string noncestr, long timestamp, string url)
        {
            var signature = _accountService.GetQywxJsApiSignature(noncestr, timestamp, url);
            return new { Signature = signature };
        }


        public class ResetPasswordInfo
        {
            public string Account { get; set; }
            public string Password { get; set; }
            public string Code { get; set; }
        }
    }

    public class UserWithCompany : GetUserOutput
    {
        public string CompanyName { get; set; }
        public List<string> ProcessNames { get; set; }
        public UserWithCompany()
        {
            ProcessNames = new List<string>();
        }
    }
}