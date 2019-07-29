
using App.Core.FileManagement.AttachmentFileMetas.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Memorabilia.MemorabiliaRecords.Dto
{
    public class UpdateMemorabiliaRecordInput
    {
        public int Id { get; set; }

        public int ProjectId { get; set; }

        /// <summary>
        /// 事项分类OptionId
        /// </summary>
        public int CategoryId { get; set; }

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
        /// 附件
        /// </summary>
        public List<AddAttachmentFileMetaInput> Attachments { get; set; }
        public UpdateMemorabiliaRecordInput()
        {
            Attachments = new List<AddAttachmentFileMetaInput>();
        }
    }
}
