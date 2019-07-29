using App.Core.Common.Entities;
using App.ProjectPlan.Plans;
using App.Web.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Web.Controllers.ProjectPlan
{
    /// <summary>
    /// 项目计划
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/plans")]
    public class ProjectPlanController: ControllerBase
    {
        private IPlanService _planService;
        public ProjectPlanController(IPlanService planService)
        {
            _planService = planService;
        }

        /// <summary>
        /// 分页获取项目计划
        /// </summary>
        /// <param name="projectId">项目id</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyword"></param>
        ///<param name="sortField">需要排序的字段(年份:RecordDate,年投资:Plann,月投资:Monthly)</param>
        /// <returns></returns>
        [MyAuthorize]
        [HttpGet]
        public PaginationData<ProjectInvestmentPlan> Get(int? projectId, int pageIndex, int pageSize, string keyword, string sortField,string sortState)
        {
            return _planService.Get(projectId, pageIndex, pageSize, keyword, sortField, sortState);
        }
    }
}
