using System;
using System.Collections.Generic;
using System.Text;

namespace App.ProjectPlan.Plans
{
    public class ProjectInvestmentPlan
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public List<AnnualInvestmentPlan> AnnualInvestmentPlans { get; set; }
        /// <summary>
        /// 是否有权限操作
        /// </summary>
        public bool HasPermission { get; set; }
        public ProjectInvestmentPlan()
        {
            AnnualInvestmentPlans = new List<AnnualInvestmentPlan>();
        }
    }
}
