using App.Core.FileManagement.AttachmentFileMetas.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.ProjectGantts.Tasks.Dto
{
    public class CompleteProjectTaskInput
    {
        public int Id { get; set; }
        /// <summary>
        /// 进度
        /// </summary>
        public decimal Progress { get; set; }
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
        public List<UpdateAttachmentFileMetaInput> Attachments { get; set; }
        public CompleteProjectTaskInput()
        {
            Attachments = new List<UpdateAttachmentFileMetaInput>();
        }
    }
}
