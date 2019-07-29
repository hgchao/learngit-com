using App.Core.Common.Exceptions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using App.Entry;
using App.Entry.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Web.Exceptions
{
    public class HttpExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public HttpExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await TrackException(ex, context);
            }
        }

        public async Task TrackException(Exception ex, HttpContext context)
        {

            if(ex is EntityException)
            {
                context.Response.StatusCode = 400;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonConvert.SerializeObject(new ErrorInfo(ex.Message)));
            }
            else if(ex is EntityPropertyException)
            {
                context.Response.StatusCode = 400;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonConvert.SerializeObject(new ErrorInfo(ex.Message)));
            }
            else if(ex is AppCoreException)
            {
                context.Response.StatusCode = 400;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonConvert.SerializeObject(new ErrorInfo(ex.Message)));
            }
            else
            {
                if(ex.InnerException == null)
                {
                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(new ErrorInfo(ex.Message)));
                }
                else
                {
                    await TrackException(ex.InnerException, context);
                }
            }
        }

    }

    internal class ErrorInfo
    {
        public string Message { get; set; }
        public ErrorInfo(string message)
        {
            Message = message;
        }
    }
}
