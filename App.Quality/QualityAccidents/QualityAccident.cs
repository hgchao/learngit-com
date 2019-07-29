using App.Core.Common.Entities;
using App.Core.Workflow.ProcessInstances;
using App.Projects.ProjectBaseInfos;
using App.Quality.QualityAccidentDisposals;
using Oa.Project;
using PoorFff.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace App.Quality.QualityAccidents
{
    public class QualityAccident: EntityWithAuditHaveTenant
    {

        public int ProjectId { get; set; }

        /// <summary>
        /// 所在项目
        /// </summary>
        [ForeignKey("ProjectId")]
        public Project Project { get; set; }

        /// <summary>
        /// 质量事故标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 质量事故内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 质量事故发现时间
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


        /// <summary>
        /// 处置进展
        /// </summary>
        public List<QualityAccidentDisposal> Disposals { get; set; }

        /// <summary>
        /// 质量事故图片
        /// </summary>
        public List<QualityAccidentAttachment> AccidentPhotoSets { get; set; }

        /// <summary>
        /// 解决的时间
        /// </summary>
        public DateTime? SettlementTime { get; set; }

        /// <summary>
        /// 解决后图片
        /// </summary>
        public List<QualitySettlementAttachment> SettlementPhotoSets { get; set; }

        /// <summary>
        /// 主体id
        /// </summary>
        public int? Mid { get; set; }
        [ForeignKey("Mid")]
        public QualityAccident MainObject { get; set; }
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

        public QualityAccident()
        {
            Disposals = new List<QualityAccidentDisposal>();
            AccidentPhotoSets = new List<QualityAccidentAttachment>();
            SettlementPhotoSets = new List<QualitySettlementAttachment>();
        }
    }

    
}
