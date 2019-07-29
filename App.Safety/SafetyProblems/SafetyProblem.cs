using App.Core.Common.Entities;
using App.Core.Parameter.Selections;
using App.Core.Workflow.ProcessInstances;
using App.Projects.ProjectBaseInfos;
using App.Safety.SafetyProblemProgresses;
using App.Safety.SafetyProblemRectifications;
using Oa.Project;
using PoorFff.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace App.Safety.SafetyProblems
{
    public class SafetyProblem: EntityWithAuditHaveTenant
    {
        public int ProjectId { get; set; }

        /// <summary>
        /// 所在项目
        /// </summary>
        [ForeignKey("ProjectId")]
        public Project Project { get; set; }

        public int CategoryId { get; set; }
        /// <summary>
        /// 安全问题分类
        /// </summary>
        [ForeignKey("CategoryId")]
        public Option Category { get; set; }

        public int SourceId { get; set; }
        /// <summary>
        /// 安全问题来源
        /// </summary>
        [ForeignKey("SourceId")]
        public Option Source { get; set; }

        /// <summary>
        /// 安全问题描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 整改情况
        /// </summary>
        public RectificationState RectificationState { get; set; } = RectificationState.NotStarted;

        /// <summary>
        /// 限期整改时间
        /// </summary>
        public DateTime Deadline { get; set; }

        /// <summary>
        /// 问题图片
        /// </summary>
        public List<SafetyProblemAttachment> ProblemPhotoSets { get; set; }

        /// <summary>
        /// 整改进展
        /// </summary>
        public List<SafetyProblemRectification> Rectifications { get; set; }

        /// <summary>
        /// 整改完成时间
        /// </summary>
        public DateTime? CompletionTime { get; set; }

        /// <summary>
        /// 整改完成图片
        /// </summary>
        public List<SafetyCompletionAttachment> CompletionPhotoSets { get; set; }


        /// <summary>
        /// 主体id
        /// </summary>
        public int? Mid { get; set; }
        [ForeignKey("Mid")]
        public SafetyProblem MainObject { get; set; }
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

        public SafetyProblem()
        {
            Rectifications = new List<SafetyProblemRectification>();
        }
    }
}
