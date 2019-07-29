using App.Core.FileManagement.AttachmentFileMetas.Dto;
using App.Housekeeping.HousekeepingProblemRectifications;
using App.Housekeeping.HousekeepingProblemRectifications.Dto;
using Newtonsoft.Json;
using PoorFff.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Housekeeping.Housekeepings.Dto
{
    public class GetHousekeepingProblemOutput
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
        /// 文明施工问题内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 文明施工问题创建时间
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
        public List<GetHousekeepingProblemRectificationOutput> Rectifications { get; set; }

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

        public GetHousekeepingProblemOutput()
        {
            Rectifications = new List<GetHousekeepingProblemRectificationOutput>();
            ProblemPhotoSets = new List<GetAttachmentFileMetaOutput>();
            CompletionPhotoSets = new List<GetAttachmentFileMetaOutput>();
        }
    }
}
