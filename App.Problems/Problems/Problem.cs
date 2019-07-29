using App.Core.Common.Entities;
using App.Core.Parameter.Selections;
using App.Core.Workflow.ProcessInstances;
using App.Problems.ProblemCoordinations;
using App.Problems.ProblemRectifications;
using App.Projects.ProjectBaseInfos;
using Oa.Project;
using PoorFff.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace App.Problems.Problems
{
    public class Problem: EntityWithAuditHaveTenant
    {
        public int ProjectId { get; set; }

        /// <summary>
        /// 所在项目
        /// </summary>
        [ForeignKey("ProjectId")]
        public Project Project { get; set; }

        public int CategoryId { get; set; }
        /// <summary>
        /// 存在问题分类
        /// </summary>
        [ForeignKey("CategoryId")]
        public Option Category { get; set; }

        /// <summary>
        /// 问题内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 建议解决方案
        /// </summary>
        public string ProposalSolution { get; set; }


        /// <summary>
        /// 计划完成时间
        /// </summary>
        public DateTime? PlannedCompletionTime { get; set; }

        /// <summary>
        /// 协调情况
        /// </summary>
        public CoordinationState CoordinationState { get; set; } = CoordinationState.NotStarted;


        /// <summary>
        /// 协调进展
        /// </summary>
        public List<ProblemCoordination> Coordinations { get; set; }

        /// <summary>
        /// 问题图片
        /// </summary>
        public List<ProblemAttachment> ProblemPhotoSets { get; set; }

        /// <summary>
        /// 实际完成时间
        /// </summary>
        public DateTime? ActualCompletionTime { get; set; }

        /// <summary>
        /// 主体id
        /// </summary>
        public int? Mid { get; set; }
        [ForeignKey("Mid")]
        public Problem MainObject { get; set; }
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

        public Problem()
        {
            Coordinations = new List<ProblemCoordination>();
        }
    }
}
