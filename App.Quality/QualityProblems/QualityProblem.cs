using App.Core.Common.Entities;
using App.Core.Parameter.Selections;
using App.Core.Workflow.ProcessInstances;
using App.Projects.ProjectBaseInfos;
using App.Quality.QualityProblemRectifications;
using Oa.Project;
using PoorFff.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace App.Quality.QualityProblems
{
    public class QualityProblem: EntityWithAuditHaveTenant
    {
        public int ProjectId { get; set; }

        /// <summary>
        /// 所在项目
        /// </summary>
        [ForeignKey("ProjectId")]
        public Project Project { get; set; }

        public int CategoryId { get; set; }
        /// <summary>
        /// 质量问题分类
        /// </summary>
        [ForeignKey("CategoryId")]
        public Option Category { get; set; }

        /// <summary>
        /// 质量问题来源
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// 质量问题描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 整改情况
        /// </summary>
        public RectificationState RectificationState { get; set; } = RectificationState.NotStarted;

        /// <summary>
        /// 整改进展
        /// </summary>
        public List<QualityProblemRectification> Rectifications { get; set; }

        /// <summary>
        /// 问题图片
        /// </summary>
        public List<QualityProblemAttachment> ProblemPhotoSets { get; set; }

        /// <summary>
        /// 整改完成时间
        /// </summary>
        public DateTime? CompletionTime { get; set; }

        /// <summary>
        /// 整改完成图片
        /// </summary>
        public List<QualityCompletionAttachment> CompletionPhotoSets { get; set; }

        /// <summary>
        /// 主体id
        /// </summary>
        public int? Mid { get; set; }
        [ForeignKey("Mid")]
        public QualityProblem MainObject { get; set; }
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

        public QualityProblem()
        {
            Rectifications = new List<QualityProblemRectification>();
        }
    }
}
