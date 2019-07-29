using App.Core.Common.Entities;
using App.ProjectPlan.MonthlyPlans;
using App.ProjectPlan.MonthlyPlans.Dto;
using App.Web.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Web.Controllers.ProjectPlan
{

    /// <summary>
    /// 项目计划-月计划
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/monthlyplan")]
    public class MonthlyPlanController : ControllerBase
    {
        private IMonthlyPlanService _planService;
        public MonthlyPlanController(IMonthlyPlanService planService)
        {
            _planService = planService;
        }
        /// <summary>
        /// 统计首页折线图，数据为不同月份项目计划金额
        /// </summary>
        /// <returns></returns>
        [HttpGet("monthmoney")]
        public CountMoney MonthMoney(string year)
        {
            return new CountMoney { CountStr = _planService.MonthMoney(year) };
        }
        public class CountMoney
        {
            public string CountStr { get; set; }
        }
        /// <summary>
        /// 获取月计划详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [MyAuthorize]
        [HttpGet("{id:int}")]
        public GetMonthlyPlanOutput Get(int id)
        {
            return _planService.Get(id);
        }

        /// <summary>
        /// 新增月计划
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [MyAuthorize]
        [HttpPost()]
        public IActionResult Add([FromBody]AddMonthlyPlanInput input)
        {
            int id = _planService.Add(input);
            return Created("", new { id });
        }


        /// <summary>
        /// 更新月计划
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [MyAuthorize]
        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody]UpdateMonthlyPlanInput input)
        {
            input.Id = id;
            _planService.Update(input);
            return Created("", new { id });
        }

        /// <summary>
        /// 删除月计划
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id:int}")]
        public void Delete(int id)
        {
            _planService.Delete(id);
        }
    }
}
