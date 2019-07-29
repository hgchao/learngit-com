using App.Core.FileManagement.AttachmentFileMetas.Dto;
using App.Safety.SafetyProblemRectifications;
using App.Safety.SafetyProblemRectifications.Dto;
using Newtonsoft.Json;
using PoorFff.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Safety.SafetyProblems.Dto
{
    public class GetSafetyProblemOutput
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
        /// <summary>
        /// 安全问题分类Id
        /// </summary>
        public int CategoryId { get; set; }
        /// <summary>
        /// 安全问题分类
        /// </summary>
        public string Category { get; set; }
        /// <summary>
        /// 安全问题来源Id
        /// </summary>
        public int SourceId { get; set; }
        /// <summary>
        /// 安全问题来源
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// 安全问题描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 安全问题创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 问题图片
        /// </summary>
        public List<GetAttachmentFileMetaOutput> ProblemPhotoSets { get; set; }

        /// <summary>
        /// 整改情况
        /// </summary>
        [JsonConverter(typeof(EnumJsonConverter))]
        public RectificationState RectificationState { get; set; }

        /// <summary>
        /// 整改进展
        /// </summary>
        public List<GetSafetyProblemRectificationOutput> Rectifications { get; set; }

        /// <summary>
        /// 限期整改时间
        /// </summary>
        public DateTime Deadline { get; set; }

        /// <summary>
        /// 整改完成时间
        /// </summary>
        public DateTime? CompletionTime { get; set; }

        /// <summary>
        /// 整改完成图片
        /// </summary>
        public List<GetAttachmentFileMetaOutput> CompletionPhotoSets { get; set; }


        public GetSafetyProblemOutput()
        {
            Rectifications = new List<GetSafetyProblemRectificationOutput>();
            ProblemPhotoSets = new List<GetAttachmentFileMetaOutput>();
            CompletionPhotoSets = new List<GetAttachmentFileMetaOutput>();
        }
    }
}
