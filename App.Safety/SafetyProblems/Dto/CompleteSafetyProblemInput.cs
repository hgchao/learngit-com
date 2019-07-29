using App.Core.FileManagement.AttachmentFileMetas.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Safety.SafetyProblems.Dto
{
    public class CompleteSafetyProblemInput
    {
        public int Id { get; set; }
        /// <summary>
        /// 整改完成时间
        /// </summary>
        public DateTime? CompletionTime { get; set; }

        /// <summary>
        /// 整改完成图片
        /// </summary>
        public List<AddAttachmentFileMetaInput> CompletionPhotoSets { get; set; }
        public CompleteSafetyProblemInput()
        {
            CompletionPhotoSets = new List<AddAttachmentFileMetaInput>();
        }
    }
}
