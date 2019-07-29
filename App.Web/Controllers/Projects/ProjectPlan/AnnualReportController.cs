using App.Core.Common.Entities;
using App.ProjectPlan.AnnualPlans;
using App.ProjectPlan.AnnualPlans.Dto;
using App.ProjectPlan.PmAnnualPlans.Dto;
using App.Web.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Web.Controllers.ProjectPlan
{

    /// <summary>
    /// 项目计划-年计划
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/annualplan")]
    public class AnnualPlanController : ControllerBase
    {
        private IAnnualPlanService _planService;
        public AnnualPlanController(IAnnualPlanService planService)
        {
            _planService = planService;
        }

        /// <summary>
        /// 获取年计划详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [MyAuthorize]
        [HttpGet("{id:int}")]
        public GetAnnualPlanOutput Get(int id)
        {
            return _planService.Get(id);
        }

        /// <summary>
        /// 新增年计划
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [MyAuthorize]
        [HttpPost()]
        public IActionResult Add([FromBody]AddAnnualPlanInput input)
        {
            int id = _planService.Add(input);
            return Created("", new { id });
        }


        /// <summary>
        /// 更新年计划
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [MyAuthorize]
        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody]UpdateAnnualPlanInput input)
        {
            input.Id = id;
            _planService.Update(input);
            return Created("", new { id });
        }

        /// <summary>
        /// 删除年计划
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id:int}")]
        public void Delete(int id)
        {
            _planService.Delete(id);
        }
    }
}
