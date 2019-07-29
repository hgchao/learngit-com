
using App.Core.FileManagement.AttachmentFileMetas.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Safety.SafetyAccidentDisposals.Dto
{
    public class AddSafetyAccidentDisposalInput
    {
        /// <summary>
        /// 处置时间
        /// </summary>
        public DateTime? ExecDate { get; set; }

        /// <summary>
        /// 处置方案
        /// </summary>
        public string Solution { get; set; }

        /// <summary>
        /// 处置图片
        /// </summary>
        public List<AddAttachmentFileMetaInput> DisposalPhotoSets { get; set; }
        public AddSafetyAccidentDisposalInput()
        {
            DisposalPhotoSets = new List<AddAttachmentFileMetaInput>();
        }
    }
}
