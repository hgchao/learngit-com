﻿using System;
using System.Collections.Generic;
using System.Text;

namespace App.ProjectPlan.AnnualPlans.Dto
{
    public class AddAnnualPlanInput
    {
        /// <summary>
        /// 项目Id
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// 记录年份
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// 计划征拆费
        /// </summary>
        public decimal PlannedDemolitionFee { get; set; }

        /// <summary>
        /// 计划工程费
        /// </summary>
        public decimal PlannedProjectCosts { get; set; }
    }
}
