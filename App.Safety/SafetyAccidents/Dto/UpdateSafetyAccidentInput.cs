
using App.Core.FileManagement.AttachmentFileMetas.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Safety.SafetyAccidents.Dto
{
    public class UpdateSafetyAccidentInput
    {
        public int Id { get; set; }
        /// <summary>
        /// 所在项目的Id
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// 事故起因OptionId
        /// </summary>
        public int SourceId { get; set; }

        /// <summary>
        /// 事故分类OptionId
        /// </summary>
        public int CategoryId { get; set; }


        /// <summary>
        /// 安全事故标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 安全事故内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 安全事故发现时间
        /// </summary>
        public DateTime DiscoveryTime { get; set; }

        /// <summary>
        /// 事故内容描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 事故图片
        /// </summary>
        public List<AddAttachmentFileMetaInput> AccidentPhotoSets { get; set; }


        /// <summary>
        /// 事故严重程OptionId
        /// </summary>
        public int SeverityId { get; set; }

        /// <summary>
        /// 受伤人数
        /// </summary>
        public int InjuredNumber { get; set; }

        /// <summary>
        /// 死亡人数
        /// </summary>
        public int DeathNumber { get; set; }
        public UpdateSafetyAccidentInput()
        {
            AccidentPhotoSets = new List<AddAttachmentFileMetaInput>();
        }
    }
}
