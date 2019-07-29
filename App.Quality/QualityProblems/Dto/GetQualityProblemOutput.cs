using App.Core.FileManagement.AttachmentFileMetas.Dto;
using App.Quality.QualityProblemRectifications;
using App.Quality.QualityProblemRectifications.Dto;
using Newtonsoft.Json;
using PoorFff.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Quality.QualityProblems.Dto
{
    public class GetQualityProblemOutput
    {
        public int Id { get; set; }
        /// <summary>
        /// 所在项目Id
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// 所在项目编号
        /// </summary>
        public string ProjectNumber { get; set; }

        /// <summary>
        /// 所在项目名称
        /// </summary>
        public string ProjectName { get; set; }

        public int CategoryId { get; set; }
        /// <summary>
        /// 质量问题分类
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// 质量问题来源
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// 质量问题描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 质量问题创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 整改情况
        /// </summary>
        [JsonConverter(typeof(EnumJsonConverter))]
        public RectificationState RectificationState { get; set; }

        /// <summary>
        /// 整改进展
        /// </summary>
        public List<GetQualityProblemRectificationOutput> Rectifications { get; set; }


        /// <summary>
        /// 问题图片
        /// </summary>
        public List<GetAttachmentFileMetaOutput> ProblemPhotoSets { get; set; }

        /// <summary>
        /// 整改完成时间
        /// </summary>
        public DateTime? CompletionTime { get; set; }

        /// <summary>
        /// 整改完成图片
        /// </summary>
        public List<GetAttachmentFileMetaOutput> CompletionPhotoSets { get; set; }

        public GetQualityProblemOutput()
        {
            Rectifications = new List<GetQualityProblemRectificationOutput>();
            ProblemPhotoSets = new List<GetAttachmentFileMetaOutput>();
            CompletionPhotoSets = new List<GetAttachmentFileMetaOutput>();
        }
    }
}
