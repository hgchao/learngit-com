using App.Core.FileManagement.AttachmentFileMetas.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Quality.QualityAccidentDisposals.Dto
{
    public class AddQualityAccidentDisposalInput
    {
        /// <summary>
        /// 处置时间
        /// </summary>
        public DateTime? ExecDate { get; set; }

        /// <summary>
        /// 处置方案
        /// </summary>
        public string Plan { get; set; }

        /// <summary>
        /// 处置图片
        /// </summary>
        public List<AddAttachmentFileMetaInput> DisposalPhotoSets { get; set; }

        public AddQualityAccidentDisposalInput()
        {
            DisposalPhotoSets = new List<AddAttachmentFileMetaInput>();
        }
    }
}
