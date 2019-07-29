using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.ProjectPlan.MonthlyPlans.Dto
{
    public class GetMonthlyPlanOutput
    {
        public int Id { get; set; }

        /// <summary>
        /// 项目Id
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }

        ///// <summary>
        ///// 记录日期
        ///// </summary>
        //public DateTime RecordDate { get; set; }

        /// <summary>
        /// 年
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// 月
        /// </summary>
        public int Month { get; set; }

        /// <summary>
        /// 计划征拆费
        /// </summary>
        public decimal PlannedDemolitionFee { get; set; }

        /// <summary>
        /// 计划工程费
        /// </summary>
        public decimal PlannedProjectCosts { get; set; }

        /// <summary>
        /// 计划投资
        /// </summary>
        public decimal PlannedInvestment { get; set; }

        /// <summary>
        /// 是否有权限操作
        /// </summary>
        public bool HasPermission { get; set; }
    }
}
