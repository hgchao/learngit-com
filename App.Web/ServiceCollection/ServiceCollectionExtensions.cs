using App.Core.Authorization.Accounts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using App.Web.Authentication;
using App.Web.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Web.ServiceCollection
{
    public static class ServiceCollectionExtensions
    {
        public static void AddFilters(this IServiceCollection services)
        {
            services.AddScoped<MyAuthorizeFilter>();
        }

        //public static void AddDbContext(this IServiceCollection services)
        //{
        //    services.AddScoped<PmDbContext>();
        //}

        //public static void AddAuthInfoProvider(this IServiceCollection services)
        //{
        //    services.AddScoped<IAuthInfoProvider, AuthInfoProvider>();
        //}

    }
}
