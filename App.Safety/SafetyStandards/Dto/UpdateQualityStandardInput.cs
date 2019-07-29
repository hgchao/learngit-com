﻿
using App.Core.FileManagement.AttachmentFileMetas.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Safety.SafetyStandards.Dto
{
    public class UpdateSafetyStandardInput
    {
        public int Id { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 标准分类OptionId
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// 链接
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        /// 附件
        /// </summary>
        public List<AddAttachmentFileMetaInput> Attachments { get; set; }

        public UpdateSafetyStandardInput()
        {
            Attachments = new List<AddAttachmentFileMetaInput>();
        }
    }
}
