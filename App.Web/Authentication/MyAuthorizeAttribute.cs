using App.Core;
using App.Core.Authorization;
using App.Core.Authorization.Accounts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using App.Web;
using App.Web.ApiExplorerService;
using App.Web.Authentication;
using PoorFff.Dependencies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace App.Web.Filters
{

    public class MyAuthorizeAttribute: TypeFilterAttribute
    {
        public string RoleNames { get; set; }
        public MyAuthorizeAttribute(bool needAuthorize = false):base(typeof(MyAuthorizeFilter))
        {
            RoleNames = "";
            Arguments = new object[] { needAuthorize, RoleNames };
        }
    }

    public  class MyAuthorizeFilter : IAuthorizationFilter
    {
        private bool _needAuthorize;
        private List<string> _roleNameList;
        public MyAuthorizeFilter(bool needAuthorize, string RoleNames)
        {
            _needAuthorize = needAuthorize;
            _roleNameList = string.IsNullOrEmpty(RoleNames)?new List<string>():RoleNames.Split(",").ToList();
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var accountService = context.HttpContext.RequestServices.GetService(typeof(IAccountService)) as IAccountService;
            var authInfoProvider = context.HttpContext.RequestServices.GetService(typeof(IAuthInfoProvider)) as IAuthInfoProvider;
            var token = context.HttpContext.Request.Query["token"];
            try
            {
                var (checkResult, auth) = AuthHelper.CheckAuth(accountService, token, _needAuthorize, ActionTool.GetUid(context.ActionDescriptor), _roleNameList);
                switch (checkResult)
                {
                    case CheckResult.Success:
                        authInfoProvider.SetCurrent(auth);
                        return;
                    case CheckResult.AuthenticationFailed:
                        context.Result = new ContentResult
                        {
                            StatusCode = 401,
                            Content = "身份验证失败"
                        };
                        break;
                    case CheckResult.AuthorizationFailed:
                        context.Result = new ContentResult
                        {
                            StatusCode = 403,
                            Content = "没有授权"
                        };
                        break;
                    default:
                        context.Result = new ContentResult
                        {
                            StatusCode = 401,
                            Content = "身份验证失败"
                        };
                        break;
                }
            }
            catch(Exception ex)
            {
                context.Result = new ContentResult
                {
                    StatusCode = 401,
                    Content = ex.ToString()
                };
            }
        }
    }
}