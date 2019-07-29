using App.Core.FileManagement.AttachmentFileMetas.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Quality.QualityAccidents.Dto
{
    public class AddQualityAccidentInput
    {
        /// <summary>
        /// 所在项目的Id
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// 质量事故标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 质量事故内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 质量事故发现时间
        /// </summary>
        public DateTime DiscoveryTime { get; set; }

        /// <summary>
        /// 事故内容描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 质量事故图片
        /// </summary>
        public List<AddAttachmentFileMetaInput> AccidentPhotoSets { get; set; }
        public AddQualityAccidentInput()
        {
            AccidentPhotoSets = new List<AddAttachmentFileMetaInput>();
        }
    }
}
