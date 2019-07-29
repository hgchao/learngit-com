
using App.Core.FileManagement.AttachmentFileMetas.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Problems.ProblemRectifications.Dto
{
    public class AddProblemCoordinationInput
    {
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
        public List<AddAttachmentFileMetaInput> CoordinationPhotoSets { get; set; }
        public AddProblemCoordinationInput()
        {
            CoordinationPhotoSets = new List<AddAttachmentFileMetaInput>();
        }
    }
}
