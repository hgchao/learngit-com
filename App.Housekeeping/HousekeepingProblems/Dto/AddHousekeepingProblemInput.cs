
using App.Core.FileManagement.AttachmentFileMetas.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Housekeeping.Housekeepings.Dto
{
    public class AddHousekeepingProblemInput
    {
        /// <summary>
        /// 所在项目
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// 文明施工问题内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 限期整改时间
        /// </summary>
        public DateTime Deadline { get; set; }

        /// <summary>
        /// 问题图片
        /// </summary>
        public List<AddAttachmentFileMetaInput> ProblemPhotoSets { get; set; }
        public AddHousekeepingProblemInput()
        {
            ProblemPhotoSets = new List<AddAttachmentFileMetaInput>();
        }
    }
}
