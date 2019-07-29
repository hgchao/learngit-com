using App.Core.Common.Entities;
using App.Projects.ProjectBaseInfos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace App.ProjectPlan.MonthlyPlans
{
    public class MonthlyPlan: EntityWithAuditHaveTenant
    {
        public int ProjectId { get; set; }

        /// <summary>
        /// 所属项目
        /// </summary>
        [ForeignKey("ProjectId")]
        public Project Project { get; set; }

        /// <summary>
        /// 记录日期
        /// </summary>
        public  DateTime RecordDate { get; set; }

        /// <summary>
        /// 计划征拆费
        /// </summary>
        [Column(TypeName="decimal(15,2)")]
        public decimal  PlannedDemolitionFee { get; set; }

        /// <summary>
        /// 计划工程费
        /// </summary>
        [Column(TypeName = "decimal(15,2)")]
        public decimal PlannedProjectCosts { get; set; }
    }
}
