using App.Core.Common.Entities;
using App.Core.Parameter.Selections;
using App.Core.Workflow.ProcessInstances;
using App.Projects.ProjectBaseInfos;
using App.Safety.SafetyAccidentDisposals;
using Oa.Project;
using PoorFff.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace App.Safety.SafetyAccidents
{
    public class SafetyAccident: EntityWithAuditHaveTenant
    {

        public int ProjectId { get; set; }

        /// <summary>
        /// 所在项目
        /// </summary>
        [ForeignKey("ProjectId")]
        public Project Project { get; set; }

        public int SourceId { get; set; }
        /// <summary>
        /// 事故起因
        /// </summary>
        [ForeignKey("SourceId")]
        public Option Source { get; set; }

        public int CategoryId { get; set; }
        /// <summary>
        /// 事故类别
        /// </summary>
        [ForeignKey("CategoryId")]
        public Option Category { get; set; }

        /// <summary>
        /// 安全事故标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 安全事故内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 安全事故发现时间
        /// </summary>
        public DateTime DiscoveryTime { get; set; }

        /// <summary>
        /// 处置状态
        /// </summary>
        public DisposalState DisposalState { get; set; } = DisposalState.NotStarted;

        /// <summary>
        /// 事故内容描述
        /// </summary>
        public string Description { get; set; }

        public int SeverityId { get; set; }
        /// <summary>
        /// 事故严重程度
        /// </summary>
        [ForeignKey("SeverityId")]
        public Option Severity { get; set; }

        /// <summary>
        /// 受伤人数
        /// </summary>
        public int InjuredNumber { get; set; }

        /// <summary>
        /// 死亡人数
        /// </summary>
        public int DeathNumber { get; set; }

        /// <summary>
        /// 事故图片
        /// </summary>
        public List<SafetyAccidentAttachment> AccidentPhotoSets { get; set; }

        /// <summary>
        /// 处置进展
        /// </summary>
        public List<SafetyAccidentDisposal> Disposals { get; set; }

        /// <summary>
        /// 解决的时间
        /// </summary>
        public DateTime? SettlementTime { get; set; }

        /// <summary>
        /// 解决后图片
        /// </summary>
        public List<SafetySettlementAttachment> SettlementPhotoSets { get; set; }

        /// <summary>
        /// 主体id
        /// </summary>
        public int? Mid { get; set; }
        [ForeignKey("Mid")]
        public SafetyAccident MainObject { get; set; }
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

        public SafetyAccident()
        {
            Disposals = new List<SafetyAccidentDisposal>();
            AccidentPhotoSets = new List<SafetyAccidentAttachment>();
            SettlementPhotoSets = new List<SafetySettlementAttachment>();
        }
    }

}
