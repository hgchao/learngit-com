using App.Core.FileManagement.AttachmentFileMetas.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.RecordMgmt.Records.Dto
{
    public class UpdateRecordInput
    {
        public int Id { get; set; }
        /// <summary>
        /// 档案类型optionid
        /// </summary>
        public int RecordTypeId { get; set; }
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
        public List<UpdateAttachmentFileMetaInput> Attachments { get; set; }
        public UpdateRecordInput()
        {
            Attachments = new List<UpdateAttachmentFileMetaInput>();
        }
    }
}
