
using App.Core.FileManagement.AttachmentFileMetas.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Problems.Problems.Dto
{
    public class UpdateProblemInput
    {
        public int Id { get; set; }
        /// <summary>
        /// 所在项目
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// 存在问题分类OptionId
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// 问题内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 建议解决方案
        /// </summary>
        public string ProposalSolution { get; set; }

        /// <summary>
        /// 计划完成时间
        /// </summary>
        public DateTime? PlannedCompletionTime { get; set; }

        /// <summary>
        /// 问题图片
        /// </summary>
        public List<AddAttachmentFileMetaInput> ProblemPhotoSets { get; set; }
        public UpdateProblemInput()
        {
            ProblemPhotoSets = new List<AddAttachmentFileMetaInput>();
        }
    }
}
