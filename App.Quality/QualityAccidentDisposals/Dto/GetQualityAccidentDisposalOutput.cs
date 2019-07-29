using App.Core.FileManagement.AttachmentFileMetas.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Quality.PmQualityAccidentDisposals.Dto
{
    public class GetQualityAccidentDisposalOutput
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
        public List<GetAttachmentFileMetaOutput> DisposalPhotoSets { get; set; }
        public GetQualityAccidentDisposalOutput()
        {
            DisposalPhotoSets = new List<GetAttachmentFileMetaOutput>();
        }

    }
}
