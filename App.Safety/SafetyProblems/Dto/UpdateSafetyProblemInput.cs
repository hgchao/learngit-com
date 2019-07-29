using App.Core.FileManagement.AttachmentFileMetas.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Safety.SafetyProblems.Dto
{
    public class UpdateSafetyProblemInput
    {
        public int Id { get; set; }
        /// <summary>
        /// 所在项目Id
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// 安全问题分类OptionId
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// 安全问题来源OptionId
        /// </summary>
        public int SourceId { get; set; }

        /// <summary>
        /// 安全问题描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 限期整改时间
        /// </summary>
        public DateTime Deadline { get; set; }

        /// <summary>
        /// 问题图片
        /// </summary>
        public List<AddAttachmentFileMetaInput> ProblemPhotoSets { get; set; }
        public UpdateSafetyProblemInput()
        {
            ProblemPhotoSets = new List<AddAttachmentFileMetaInput>();
        }
    }
}
