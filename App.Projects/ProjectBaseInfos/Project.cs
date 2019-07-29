using App.Core.Common.Entities;
using App.Core.Common.Repositories;
using App.Core.Parameter.Selections;
using App.Core.Workflow.ProcessInstances;
using App.Projects.ProjectAttachments;
using App.Projects.ProjectLocations;
using App.Projects.ProjectMembers;
using App.Projects.ProjectUnits;
using Oa.Project;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace App.Projects.ProjectBaseInfos
{
    public class Project: EntityWithAuditHaveTenant
    {
        /// <summary>
        /// 项目名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 项目编号 
        /// </summary>
        public string No { get; set; }

        public int? TypeId{ get; set; }
        /// <summary>
        /// 项目类型
        /// </summary>
        [DisableUpdate]
        [ForeignKey("TypeId")]
        public Option Type { get; set; }

        public int? ProjectNatureId { get; set; }
        /// <summary>
        /// 项目性质
        /// </summary>
        [DisableUpdate]
        [ForeignKey("ProjectNatureId")]
        public Option ProjectNature { get; set; }

        public int? StateId { get; set; }
        /// <summary>
        /// 项目状态
        /// </summary>
        [DisableUpdate]
        [ForeignKey("StateId")]
        public Option State { get; set; }

        public int? StageId { get; set; }
        /// <summary>
        /// 当前阶段
        /// </summary>
        [DisableUpdate]
        [ForeignKey("StageId")]
        public Option Stage { get; set; }

        /// <summary>
        /// 财务投资
        /// </summary>
        [Column(TypeName = "decimal(15,2)")]
        public decimal FinancialInvestment { get; set; }

        /// <summary>
        /// 非财务投资
        /// </summary>
        [Column(TypeName = "decimal(15,2)")]
        public decimal NonFinancialInvestment { get; set; }

        /// <summary>
        /// 项目概算
        /// </summary>
        [Column(TypeName = "decimal(15,2)")]
        public decimal GeneralEstimate { get; set; }

        /// <summary>
        /// 暂估总投资
        /// </summary>
        [Column(TypeName = "decimal(15,2)")]
        public decimal TentativeEstimatedTotalInvestment { get; set; }

        /// <summary>
        /// 开工时间
        /// </summary>
        public DateTime? CommencementDate { get; set; }

        /// <summary>
        /// 竣工时间
        /// </summary>
        public DateTime? FinishDate { get; set; }

        /// <summary>
        /// 项目地址
        /// </summary>
        public ProjectLocation Location { get; set; }

        /// <summary>
        /// 项目成员
        /// </summary>
        public List<ProjectMember> Members { get; set; }

        /// <summary>
        /// 项目参与单位
        /// </summary>
        public List<ProjectUnit> Units { get; set; }

        /// <summary>
        /// 项目概述
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// 附件
        /// </summary>
        public List<ProjectAttachment> Attachments { get; set; }



        /// <summary>
        /// 主体id
        /// </summary>
        public int? Mid { get; set; }
        [DisableUpdate]
        [ForeignKey("Mid")]
        public Project MainObject { get; set; }
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

        public Project()
        {
            Attachments = new List<ProjectAttachment>();
            Members = new List<ProjectMember>();
            Units = new List<ProjectUnit>();
        }
    }
}
