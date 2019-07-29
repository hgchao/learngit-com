using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace App.Web.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/values")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        public class QueryParams
        {
            public string A { get; set; }
            public string B { get; set; }
        }
        /// <summary>
        /// 测试Get
        /// </summary>
        /// <param name="paras"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get([FromQuery]QueryParams paras)
        {
            return new string[] { "value1", "value2" };
        }

    }
}
