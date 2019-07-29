
using App.Core.FileManagement.AttachmentFileMetas.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Problems.ProblemRectifications.Dto
{
    public class GetProblemCoordinationOutput
    {
        public int Id { get; set; }
        /// <summary>
        /// 协调日期
        /// </summary>
        public DateTime? ExecDate { get; set; }

        /// <summary>
        /// 协调描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 协调图片
        /// </summary>
        public List<GetAttachmentFileMetaOutput> CoordinationPhotoSets { get; set; }
        public GetProblemCoordinationOutput()
        {
            CoordinationPhotoSets = new List<GetAttachmentFileMetaOutput>();
        }
    }
}
