using App.Core.FileManagement.AttachmentFileMetas.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.ProjectGantts.Tasks.Dto
{
    public class AddProjectTaskInput
    {
        public int GanttId { get; set; }
        public string Text { get; set; }
        public DateTime Start_date { get; set; }
        public int Duration { get; set; }
        public decimal Progress { get; set; }
        public int? Parent { get; set; }
        public string Type { get; set; }
        /// <summary>
        /// 任务内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 节点执行人
        /// </summary>
        public int Assignee { get; set; }
        /// <summary>
        /// 实际开始时间
        /// </summary>
        public DateTime? ActualStartDate { get; set; }
        /// <summary>
        /// 实际持续天数
        /// </summary>
        public int? ActualDuration { get; set; }
        /// <summary>
        /// 备注 
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 附件
        /// </summary>
        public List<AddAttachmentFileMetaInput> Attachments { get; set; }
        public AddProjectTaskInput()
        {
            Attachments = new List<AddAttachmentFileMetaInput>();
        }
    }
}
