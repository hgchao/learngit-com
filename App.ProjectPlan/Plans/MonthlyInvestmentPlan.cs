using System;
using System.Collections.Generic;
using System.Text;

namespace App.ProjectPlan.Plans
{
    public class MonthlyInvestmentPlan
    {
        public int Id { get; set; }
        public int Month { get; set; }
        public decimal Investment { get; set; }
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
