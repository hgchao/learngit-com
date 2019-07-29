using App.Core.Authorization.Apis;
using App.Core.Authorization.Apis.Dto;
using Microsoft.AspNetCore.Mvc;
using App.Web.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace App.Web.Controllers.Auth
{
    /// <summary>
    /// 验证授权-接口
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/app-apis")]
    public class AppController : ControllerBase
    {
        private IApiService _apiService;
        public AppController(IApiService apiService)
        {
            _apiService = apiService;
        }

        /// <summary>
        /// 获取所有接口
        /// </summary>
        /// <returns></returns>
        [HttpGet("")]
        public IEnumerable<GetApiOutput> Get()
        {
            return _apiService.GetAll();
        }

    }
}