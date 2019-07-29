using App.Core.Authorization.Apis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using App.Web.ApiExplorerService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using App.Base;

namespace App.Web.Controllers
{
    /// <summary>
    /// 内部工具-综合
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/tools")]
    public class ToolController: ControllerBase
    {
        private IApiDescriptionGroupCollectionProvider _apiExplorer;
        private IApiService _apiService;
        public ToolController(
            IApiDescriptionGroupCollectionProvider apiExplorer,
            IApiService apiService)
        {
            _apiExplorer = apiExplorer;
            _apiService = apiService;
        }

        /// <summary>
        /// 更新api接口
        /// </summary>
        /// <returns></returns>
        [HttpPut("apis")]
        public IActionResult SetApi()
        {
            var apiDescGroups =_apiExplorer.ApiDescriptionGroups.Items;
            var apis = new List<Api>();
            var xmlCommentTool = new XmlCommentTool(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
            foreach(var group in apiDescGroups)
            {
                foreach(var apiDesc in group.Items)
                {
                    var controllerActionDescriptor = apiDesc.ActionDescriptor as ControllerActionDescriptor;
                    if (controllerActionDescriptor != null)
                    {
                        var controllerName = controllerActionDescriptor.ControllerTypeInfo.FullName;
                        var methodInfoList = controllerActionDescriptor.MethodInfo.ToString().Split(" ").ToList();
                        methodInfoList[0] = controllerName + ".";
                        var actionName = string.Join(' ', methodInfoList).Replace(" ", string.Empty);
                        var controllerInfo = xmlCommentTool.GetControllerSummary(controllerName)?.Split("-")??new string[] {"", ""};
                        var actionInfo = xmlCommentTool.GetActionSummary(actionName);
                        var api = new Api
                        {
                            Uid = $"{AppWebContext.Instance.SystemIndex}:{actionName.GetHashCode()}",
                            Name = actionInfo,
                            Area = controllerInfo[0],
                            Controller = controllerInfo[controllerInfo.Length - 1],
                            Method = apiDesc.HttpMethod,
                            Url = apiDesc.ActionDescriptor.AttributeRouteInfo.Template
                        };
                        apis.Add(api);
                    }
                }
            }
            if(apis.Distinct(new ApiComparer()).Count() < apis.Count())
            {
                throw new Exception("有接口的hash值重复");
            }
            _apiService.UpdateApis(apis);
            return Created("", new { });
        }


        private class ApiComparer : IEqualityComparer<Api>
        {

            public bool Equals(Api x, Api y)
            {
                return x.Uid == y.Uid;
            }

            public int GetHashCode(Api obj)
            {
                return obj.Uid.GetHashCode();
            }
        }
    }

}
