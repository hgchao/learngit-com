using App.Core.Common.Entities;
using App.Core.Parameter.Selections;
using App.Core.Workflow.ProcessInstances;
using App.Projects.ProjectBaseInfos;
using Oa.Project;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace App.Memorabilia.MemorabiliaRecords
{
    public class MemorabiliaRecord : EntityWithAuditHaveTenant
    {
        public int ProjectId { get; set; }
        /// <summary>
        /// 所属项目
        /// </summary>
        [ForeignKey("ProjectId")]
        public Project Project { get; set; }

        public int CategoryId { get; set; }
        /// <summary>
        /// 事项类型
        /// </summary>
        [ForeignKey("CategoryId")]
        public Option Category { get; set; }

        /// <summary>
        /// 事项名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 事项内容描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 参与人员
        /// </summary>
        public string Participant { get; set; }

        
        /// <summary>
        /// 主体id
        /// </summary>
        public int? Mid { get; set; }
        [ForeignKey("Mid")]
        public MemorabiliaRecord MainObject { get; set; }
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
        /// <summary>
        /// 文件
        /// </summary>
        public List<MemorabiliaAttachment> Attachments { get; set; }
    }
}
