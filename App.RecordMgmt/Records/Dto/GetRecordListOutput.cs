using App.Core.FileManagement.AttachmentFileMetas.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.RecordMgmt.Records.Dto
{
    public class GetRecordListOutput
    {
        public int Id { get; set; }
        /// <summary>
        /// 类别
        /// </summary>
        public string RecordType { get; set; }
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
        public List<GetAttachmentFileMetaOutput> Attachments { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }
        public GetRecordListOutput()
        {
            Attachments = new List<GetAttachmentFileMetaOutput>();
        }
    }
}
