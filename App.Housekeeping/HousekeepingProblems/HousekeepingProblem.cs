using App.Core.Common.Entities;
using App.Core.Parameter.Selections;
using App.Core.Workflow.ProcessInstances;
using App.Housekeeping.HousekeepingProblemRectifications;
using App.Housekeeping.HousekeepingProblems;
using App.Projects.ProjectBaseInfos;
using Oa.Project;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace App.Housekeeping.Housekeepings
{
    public class HousekeepingProblem: EntityWithAuditHaveTenant
    {
        public int ProjectId { get; set; }

        /// <summary>
        /// 所在项目
        /// </summary>
        [ForeignKey("ProjectId")]
        public Project Project { get; set; }

        /// <summary>
        /// 文明施工问题内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 问题图片
        /// </summary>
        public List<HousekeepingProblemAttachment> ProblemPhotoSets { get; set; }

        /// <summary>
        /// 整改情况
        /// </summary>
        public RectificationState RectificationState { get; set; } = RectificationState.NotStarted;

        /// <summary>
        /// 限期整改时间
        /// </summary>
        public DateTime Deadline { get; set; }

        /// <summary>
        /// 整改完成时间
        /// </summary>
        public DateTime? CompletionTime { get; set; }

        /// <summary>
        /// 整改完成图片
        /// </summary>
        public List<HousekeepingCompletionAttachment> CompletionPhotoSets { get; set; }

        /// <summary>
        /// 整改进展
        /// </summary>
        public List<HousekeepingProblemRectification> Rectifications { get; set; }


        /// <summary>
        /// 主体id
        /// </summary>
        public int? Mid { get; set; }
        [ForeignKey("Mid")]
        public HousekeepingProblem MainObject { get; set; }
        /// <summary>
        /// 当前状态
        /// </summary>
        public DataState State { get; set; }
        /// <summary>
        /// 流程实例id 
        /// </summary>
        public int? ProcessInstanceId { get; set; }
        [ForeignKey("ProcessInstanceId")]
        public Wf_Hi_ProcessInstance ProcessInstance { get; set; }

        public HousekeepingProblem()
        {
            Rectifications = new List<HousekeepingProblemRectification>();
        }
    }
}
