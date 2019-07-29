using App.Core.Authorization;
using App.Core.Authorization.Accounts;
using App.Core.Common.WebSockets;
using App.Core.Messaging.WebSocket;
using Microsoft.AspNetCore.Http;
using App.Web.Authentication;
using PoorFff.Dependencies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Web.WebSockets
{
    public class WebSocketMiddleware
    {
        private readonly RequestDelegate _next;

        public WebSocketMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var accountService = context.RequestServices.GetService(typeof(IAccountService)) as IAccountService;
            var authInfoProvider = context.RequestServices.GetService(typeof(IAuthInfoProvider)) as IAuthInfoProvider;
            var webSocketHandler = context.RequestServices.GetService(typeof(IMessagingWebSocketHandler)) as IMessagingWebSocketHandler;
            var (checkResult, auth) = AuthHelper.CheckAuth(accountService, context.Request.Query["token"], false, null, null);
            switch (checkResult)
            {
                case CheckResult.Success:
                    authInfoProvider.SetCurrent(auth);
                    break;
                case CheckResult.AuthenticationFailed:
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("身份验证失败");
                    break;
                case CheckResult.AuthorizationFailed:
                    context.Response.StatusCode = 403;
                    await context.Response.WriteAsync($"{auth?.User?.Account}未授权");
                    break;
                default:
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("身份验证失败");
                    break;
            }
            if (!context.WebSockets.IsWebSocketRequest)
            {
                return;
            }
            var client = new Client {
                UserId = auth.User.Id,
                ClientType = auth.ClientType.ToString()
            };
            var socket = await context.WebSockets.AcceptWebSocketAsync();
            await webSocketHandler.Link(client, socket);
        }
    }
}
