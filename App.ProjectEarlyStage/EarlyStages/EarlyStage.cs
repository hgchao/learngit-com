using App.Core.Common.Entities;
using App.Core.Parameter.Selections;
using App.Core.Workflow.ProcessInstances;
using App.Projects.ProjectBaseInfos;
using Newtonsoft.Json;
using Oa.Project;
using PoorFff.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace App.ProjectEarlyStage.EarlyStages
{
    public class EarlyStage : EntityWithAuditHaveTenant
    {
        public int ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        public Project Project { get; set; }

        public int NodeId { get; set; }
        /// <summary>
        /// 环节
        /// </summary>
        [ForeignKey("NodeId")]
        public Option Node { get; set; }
        /// <summary>
        /// 类型(前期管理或竣工管理)
        /// </summary>
        public string TypeName { get; set; }
        /// <summary>
        /// 批复文号
        /// </summary>
        public string ReplyNumber { get; set; }

        /// <summary>
        /// 上报日期
        /// </summary>
        public DateTime? ReportDate { get; set; }

        /// <summary>
        /// 批复日期
        /// </summary>
        public DateTime? ReplyDate { get; set; }

        /// <summary>
        /// 文件
        /// </summary>
        public List<EarlyStageAttachment> Attachments { get; set; }

        /// <summary>
        /// 主体id
        /// </summary>
        public int? Mid { get; set; }
        [ForeignKey("Mid")]
        public EarlyStage MainObject { get; set; }
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

        public EarlyStage()
        {
            Attachments = new List<EarlyStageAttachment>();
        }
    }
   
}
