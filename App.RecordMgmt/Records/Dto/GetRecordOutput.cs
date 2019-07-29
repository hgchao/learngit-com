using App.Core.FileManagement.AttachmentFileMetas.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.RecordMgmt.Records.Dto
{
  public  class GetRecordOutput
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
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
        public List<GetAttachmentFileMetaOutput> Attachments { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }
        public GetRecordOutput()
        {
            Attachments = new List<GetAttachmentFileMetaOutput>();
        }
    }
}
