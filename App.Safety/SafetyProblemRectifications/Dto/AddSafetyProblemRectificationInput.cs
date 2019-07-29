
using App.Core.FileManagement.AttachmentFileMetas.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Safety.SafetyProblemRectifications.Dto
{
    public class AddSafetyProblemRectificationInput
    {
        /// <summary>
        /// 整改日期
        /// </summary>
        public DateTime? ExecDate { get; set; }

        /// <summary>
        /// 整改描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 整改图片
        /// </summary>
        public List<AddAttachmentFileMetaInput> RectificationPhotoSets { get; set; }
        public AddSafetyProblemRectificationInput()
        {
            RectificationPhotoSets = new List<AddAttachmentFileMetaInput>();
        }
    }
}
