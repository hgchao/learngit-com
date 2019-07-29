using App.Core.Common.WebSockets;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using App.Web.WebSockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Web.WebSocket
{
    public static class WebSocketMiddlewareExtensions
    {
        public static IApplicationBuilder MapWebSocket(this IApplicationBuilder app, PathString pathString)
        {
            return app.Map(pathString, (_app) => _app.UseMiddleware<WebSocketMiddleware>());
        }
    }
}
