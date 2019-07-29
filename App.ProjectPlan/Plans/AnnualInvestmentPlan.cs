using System;
using System.Collections.Generic;
using System.Text;

namespace App.ProjectPlan.Plans
{
    public class AnnualInvestmentPlan
    {
        public int Id { get; set; }
        public int Year { get; set; }
        /// <summary>
        /// 计划征拆费
        /// </summary>
        public decimal PlannedDemolitionFee { get; set; }

        /// <summary>
        /// 计划工程费
        /// </summary>
        public decimal PlannedProjectCosts { get; set; }
        public decimal Investment { get; set; }
        public List<MonthlyInvestmentPlan> MonthlyInvestmentPlans { get; set; }

        public AnnualInvestmentPlan()
        {
            MonthlyInvestmentPlans = new List<MonthlyInvestmentPlan>();
        }
    }
}
