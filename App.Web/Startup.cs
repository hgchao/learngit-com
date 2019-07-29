using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using App.Core.Authorization;
using App.Core.Authorization.Accounts;
using App.Core.Authorization.Accounts.PasswordReset;
using App.Core.Web;
using App.Core.Weixin.QyWeixin;
using App.Core.Weixin.Weixin;
using App.Web;
using App.Web.ApiExplorerService;
using App.Web.Documentation;
using App.Web.Exceptions;
using App.Web.ServiceCollection;
using App.Web.WebSocket;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PoorFff.Serialization.JsonConverters;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace App.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            DocumentationHelper.Merge();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            JsonSerializerSettings setting = new JsonSerializerSettings();
            setting.ContractResolver = new CamelCasePropertyNamesContractResolver();
            setting.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            setting.Converters.Add(new DateTimeConverter());
            JsonConvert.DefaultSettings = new Func<JsonSerializerSettings>(() =>
            {
                return setting;
            });

            services.AddCors();
            services
                .AddMvc()
                .AddJsonOptions(opt =>
                {
                    opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    opt.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    opt.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                    opt.SerializerSettings.Converters.Add(new DateTimeConverter());
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // api版本管理
            services.AddApiVersioning(o => {
                o.ReportApiVersions = true;
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new ApiVersion(1, 0);
            });

            // 注册apiExplorer服务
            services.AddApiExplorerService();

            // 注册过滤器
            services.AddFilters();

            //services.AddDbContext<OaDbContext>(options => options.UseMySql(Configuration.GetConnectionString("Default")));

            services.AddSwaggerGen(c =>
            {
                c.DocInclusionPredicate((version, apiDescription) =>
                {
                    var values = apiDescription.RelativePath
                        .Split('/')
                        .Skip(2).ToList();
                    values.Insert(0, $"api/{version}");
                    apiDescription.RelativePath = string.Join("/", values);
                    var actionVersions = apiDescription.ActionAttributes().OfType<MapToApiVersionAttribute>().SelectMany(attr => attr.Versions);
                    var controllerVersions = apiDescription.ControllerAttributes().OfType<ApiVersionAttribute>().SelectMany(attr => attr.Versions);
                    var controllerAndActionVersionsOverlap = controllerVersions.Intersect(actionVersions).Any();
                    if (controllerAndActionVersionsOverlap)
                    {
                        return actionVersions.Any(v => $"v{v.MajorVersion}" == version);
                    }
                    return controllerVersions.Any(v => $"v{v.MajorVersion}" == version);
                });

                c.SwaggerDoc("v1", new Info
                {
                    Title = "App API",
                    Version = "v1",
                    Contact = new Contact
                    {
                        Name = "项目创建模板",
                        Email = string.Empty,
                        Url = ""
                    },
                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
            return services.AddPf<AppWebModule>(configuration => {
                // Common
                configuration.Set("Scope", Configuration.GetSection("Scope").Value);

                // AppBaseModule
                configuration.Set("AppBaseContext.RabbitMQConnectionString", Configuration.GetSection("RabbitMQ").Get<RabbitMQInfo>().ConnectionString);

                // AppWebModule
                configuration.Set("AppWebContext.SystemIndex", Convert.ToInt32(Configuration.GetSection("SystemIndex").Value));
                configuration.Set("AppWebContext.EnabledSuperAdmin", Convert.ToBoolean(Configuration.GetSection("EnabledSuperAdmin").Value));
                configuration.Set("AppWebContext.Domain", Configuration.GetSection("Domain").Value);


                // AppEntryModule
                configuration.Set("AppEntryContext.MainThreadId", Thread.CurrentThread.ManagedThreadId);
                configuration.Set("AppEntryContext.ConnectionString", Configuration.GetConnectionString("Default"));

                // FileManagementModule
                configuration.Set("FileManagementContext.FileLocation", Configuration.GetSection("FileLocation").Value);

                // WorkflowModule
                configuration.Set("WorkflowContext.FileLocation", Path.Combine(Configuration.GetSection("FileLocation").Value, "Workflow"));

                // AuthorizationModule
                configuration.Set("AuthorizationContext.AllowMultiLogin", true);
                configuration.Set("AuthorizationContext.CacheStoreWay", CacheStoreWay.Redis);
                configuration.Set("AuthorizationContext.ResetPasswordEmailConfig", Configuration.GetSection("ResetPasswordEmailConfig").Get<ResetPasswordEmailConfig>());
                configuration.Set("AuthorizationContext.TokenSecurityKey", Configuration.GetSection("TokenSecurityKey").Value);
                configuration.Set("AuthorizationContext.RedisConnectionString", Configuration.GetSection("Redis").Get<RedisInfo>().ConnectionString);
                configuration.Set("AuthorizationContext.RedisDbIndex", Configuration.GetSection("Redis").Get<RedisInfo>().AuthDbIndex);

                // MessagingModule
                configuration.Set("MessagingContext.RedisConnectionString", Configuration.GetSection("Redis").Get<RedisInfo>().ConnectionString);

                // MessagingQywxModule
                configuration.Set("MessagingQywxContext.Client", Configuration.GetSection("QyWeixinClient").Value);

                // WeixinModule
                configuration.Set("WeixinContext.RedisConnectionString", Configuration.GetSection("Redis").Get<RedisInfo>().ConnectionString);
                configuration.Set("WeixinContext.RedisDbIndex", Configuration.GetSection("Redis").Get<RedisInfo>().AuthDbIndex);
                configuration.Set("WeixinContext.WeixinInfoList", Configuration.GetSection("Weixin").Get<List<WeixinInfo>>());
                configuration.Set("WeixinContext.QyWeixinInfo", Configuration.GetSection("QyWeixin").Get<QyWeixinInfo>());

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UsePf();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());
            app.UseWebSockets();
            app.MapWebSocket("/ws");
            //app.UseUserAction();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Api API v1");
            });
            app.UseHttpExceptionHandler();
            app.UseMvc();
        }
    }
}
