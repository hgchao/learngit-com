
using App.Core.FileManagement.AttachmentFileMetas.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Safety.SafetyStandards.Dto
{
    public class GetSafetyStandardOutput
    {
        public int Id { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        public int CategoryId { get; set; }
        /// <summary>
        /// 标准分类
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// 链接
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        /// 附件
        /// </summary>
        public List<GetAttachmentFileMetaOutput> Attachments { get; set; }

        public GetSafetyStandardOutput()
        {
            Attachments = new List<GetAttachmentFileMetaOutput>();
        }
    }
}
