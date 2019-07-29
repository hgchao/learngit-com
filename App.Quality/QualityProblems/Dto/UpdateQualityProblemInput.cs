
using App.Core.FileManagement.AttachmentFileMetas.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Quality.QualityProblems.Dto
{
    public class UpdateQualityProblemInput
    {
        public int Id { get; set; }
        /// <summary>
        /// 所在项目Id
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// 质量问题分类OptionId
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// 质量问题来源
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// 质量问题描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 问题图片
        /// </summary>
        public List<AddAttachmentFileMetaInput> ProblemPhotoSets { get; set; }
        public UpdateQualityProblemInput()
        {
            ProblemPhotoSets = new List<AddAttachmentFileMetaInput>();
        }
    }
}
