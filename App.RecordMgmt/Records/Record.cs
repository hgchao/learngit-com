using App.Core.Common.Entities;
using App.Core.Common.Repositories;
using App.Core.Parameter.Selections;
using App.Core.Workflow.ProcessInstances;
using App.Projects.ProjectBaseInfos;
using Oa.Project;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace App.RecordMgmt.Records
{
    public class Record : EntityWithAuditHaveTenant 
    {
        public int ProjectId { get; set; }
        [DisableUpdate]
        [ForeignKey("ProjectId")]
        public Project Project { get; set; }
        /// <summary>
        /// 档案类型optionid
        /// </summary>
        public int RecordTypeId { get; set; }
        [DisableUpdate]
        [ForeignKey("RecordTypeId")]
        public Option RecordType { get; set;}
        /// <summary>
        /// 档案名称
        /// </summary>
        public string RecordName { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 文件
        /// </summary>
        public List<RecordAttachment> Attachments { get; set; }

        /// <summary>
        /// 主体id
        /// </summary>
        public int? Mid { get; set; }
        [ForeignKey("Mid")]
        public Record MainObject { get; set; }
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
    }
}
