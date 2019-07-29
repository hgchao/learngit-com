using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Web.Exceptions
{
    public static 
        class HttpExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseHttpExceptionHandler(this IApplicationBuilder app)
        {
            return app.UseMiddleware<HttpExceptionMiddleware>();
        }
    }
}
