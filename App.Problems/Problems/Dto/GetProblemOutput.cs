
using App.Core.FileManagement.AttachmentFileMetas.Dto;
using App.Problems.ProblemRectifications;
using App.Problems.ProblemRectifications.Dto;
using Newtonsoft.Json;
using PoorFff.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Problems.Problems.Dto
{
    public class GetProblemOutput
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
        /// 存在问题分类
        /// </summary>
        public string Category { get; set; }

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
        /// 实际完成时间
        /// </summary>
        public DateTime? ActualCompletionTime { get; set; }

        /// <summary>
        /// 问题创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 协调情况
        /// </summary>
        [JsonConverter(typeof(EnumJsonConverter))]
        public CoordinationState CoordinationState { get; set; }

        /// <summary>
        /// 协调进展
        /// </summary>
        public List<GetProblemCoordinationOutput> Coordinations { get; set; }

        /// <summary>
        /// 问题图片
        /// </summary>
        public List<GetAttachmentFileMetaOutput> ProblemPhotoSets { get; set; }

        public GetProblemOutput()
        {
            Coordinations = new List<GetProblemCoordinationOutput>();
            ProblemPhotoSets = new List<GetAttachmentFileMetaOutput>();
        }
    }
}
