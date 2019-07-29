using App.Core.Common.Entities;
using App.Core.Common.Repositories;
using App.Core.Workflow.ProcessInstances;
using App.Projects.ProjectBaseInfos;
using Oa.Project;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace App.ProjectProgress.WeeklyProgresses
{
    public class WeeklyProgress: EntityWithAuditHaveTenant
    {
        public int ProjectId { get; set; }
        [DisableUpdate]
        [ForeignKey("ProjectId")]
        public Project Project { get; set; }

        /// <summary>
        /// 新增时间
        /// </summary>
        public DateTime AddDate { get; set; }

        ///// <summary>
        ///// 年份
        ///// </summary>
        //public int Year { get; set; }

        ///// <summary>
        ///// 周数
        ///// </summary>
        //public int Week { get; set; }

        /// <summary>
        /// 累计形象进度(无效)
        /// </summary>
        public string AccumulatedImageProgress { get; set; }

        /// <summary>
        /// 形象进度(无效)
        /// </summary>
        public string ImageProgress { get; set; }

        /// <summary>
        /// 督办(无效)
        /// </summary>
        public string Supervision { get; set; }

        /// <summary>
        /// 下周计划形象进度(无效)
        /// </summary>
        public string NextMonthPlannedImageProgress { get; set; }

        /// <summary>
        /// 详细信息
        /// </summary>
        public string Information { get; set; }

        /// <summary>
        /// 主体id
        /// </summary>
        public int? Mid { get; set; }
        [DisableUpdate]
        [ForeignKey("Mid")]
        public WeeklyProgress MainObject { get; set; }
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

        /// <summary>
        /// 附件
        /// </summary>
        public List<WeeklyProgressAttachment> Attachments { get; set; }

        public WeeklyProgress()
        {
            Attachments = new List<WeeklyProgressAttachment>();
        }
    }
}
