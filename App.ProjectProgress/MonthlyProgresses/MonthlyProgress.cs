using App.Core.Common.Entities;
using App.Core.Common.Repositories;
using App.Core.Workflow.ProcessInstances;
using App.Projects.ProjectBaseInfos;
using Oa.Project;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace App.ProjectProgress.PmMonthlyProgresses
{
    public class MonthlyProgress: EntityWithAuditHaveTenant
    {
        public int ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        public Project Project { get; set; }

        /// <summary>
        /// 进度月份
        /// </summary>
        public DateTime RecordDate { get; set; }

        /// <summary>
        /// 本月完成征拆费
        /// </summary>
        [Column(TypeName ="decimal(15,2)")]
        public decimal CompletedDemolitionFee { get; set; }

        /// <summary>
        /// 本月完成工程费
        /// </summary>
        [Column(TypeName ="decimal(15,2)")]
        public decimal CompletedProjectCosts { get; set; }

        /// <summary>
        /// 累计形象进度
        /// </summary>
        public string AccumulatedImageProgress { get; set; }

        /// <summary>
        /// 形象进度
        /// </summary>
        public string ImageProgress { get; set; }

        /// <summary>
        /// 督办
        /// </summary>
        public string Supervision { get; set; }

        /// <summary>
        /// 下月计划形象进度
        /// </summary>
        public string NextMonthPlannedImageProgress { get; set; }

        /// <summary>
        /// 主体id
        /// </summary>
        public int? Mid { get; set; }
        [DisableUpdate]
        [ForeignKey("Mid")]
        public MonthlyProgress MainObject { get; set; }
        /// <summary>
        /// 当前状态
        /// </summary>
        public DataState DataState { get; set; }
        /// <summary>
        /// 流程实例id
        /// </summary>
        public int? ProcessInstanceId { get; set; }
        [DisableUpdate]
        [ForeignKey("ProcessInstanceId")]
        public Wf_Hi_ProcessInstance ProcessInstance { get; set; }

    }
}
