using App.Core.Authorization.PriviledgedPersons;
using App.Core.Authorization.PriviledgedPersons.Dto;
using App.Web.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oa.Web.Controllers.Auth
{
    /// <summary>
    /// 验证授权-特权人员
    /// </summary>
    [ApiVersion("1.0")]
    [MyAuthorize]
    [Route("api/v{version:apiVersion}/privileged-persons")]
    public class PrivilegedPersonController: ControllerBase
    {
        private IPrivilegedPersonService _privilegedPersonService;
        public PrivilegedPersonController(IPrivilegedPersonService privilegedPersonService)
        {
            _privilegedPersonService = privilegedPersonService;
        }

        /// <summary>
        /// 查看所有项目人员
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<GetPrivilegedPersonOutput> Get(string moduleName)
        {
            return _privilegedPersonService.Get(moduleName);
        }

        /// <summary>
        /// 设置项目人员
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Set([FromBody] SetPrivilegedPersonInput input)
        {
            _privilegedPersonService.Set(input);
            return Created("", null);
        }
    }
}
