using App.Core.Authorization.Accounts;
using App.Core.Common.Exceptions;
using PoorFff.Dependencies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Web.Authentication
{
    public enum CheckResult 
    {
        Success = 0,
        AuthenticationFailed = 1,
        AuthorizationFailed = 2
    }
    public class AuthHelper
    {
        public static (CheckResult, AuthInfo) CheckAuth(IAccountService accountService, string token, bool needAuthorize, string apiUid, List<string> allowedRoleNameList)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return (CheckResult.AuthenticationFailed, null);
            }

            AuthInfo auth = accountService.CheckToken(token);
            if (auth == null)
            {
                return (CheckResult.AuthenticationFailed, null);
            }
            else
            {
                // 不需要授权
                if (!needAuthorize)
                    return (CheckResult.Success, auth);
                // 用户为管理员权限拥有所有权限
                if (auth.User.Account == "super-admin" && AppWebContext.Instance.EnabledSuperAdmin)
                {
                    return (CheckResult.Success, auth);
                }
                if (auth.IsTenantManager)
                {
                    return (CheckResult.Success, auth);
                }
                // 判断用户有没有api的权限和判断用户有没有包含某个角色
                //if (auth.ApiUids.Contains(apiUid) || auth.RoleNames.Any(u => allowedRoleNameList.Contains(u)))
                if (auth.ApiUids.Contains(apiUid))
                {
                    return (CheckResult.Success, auth);
                }
                else
                {
                    return (CheckResult.AuthorizationFailed, auth);
                }
            }
        }
    }
}
