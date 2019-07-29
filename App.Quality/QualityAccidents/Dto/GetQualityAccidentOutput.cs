using App.Core.FileManagement.AttachmentFileMetas.Dto;
using App.Quality.PmQualityAccidentDisposals.Dto;
using App.Quality.QualityAccidentDisposals;
using Newtonsoft.Json;
using PoorFff.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Quality.QualityAccidents.Dto
{
    public class GetQualityAccidentOutput
    {
        public int Id { get; set; }
        /// <summary>
        /// 所在项目的Id
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

        // <summary>
        /// 处置状态
        /// </summary>
        [JsonConverter(typeof(EnumJsonConverter))]
        public DisposalState DisposalState { get; set; }


        /// <summary>
        /// 事故内容描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 质量事故图片
        /// </summary>
        public List<GetAttachmentFileMetaOutput> AccidentPhotoSets { get; set; }

        /// <summary>
        /// 处置进展
        /// </summary>
        public List<GetQualityAccidentDisposalOutput> Disposals { get; set; }

        /// <summary>
        /// 解决的时间
        /// </summary>
        public DateTime? SettlementTime { get; set; }

        /// <summary>
        /// 解决后图片
        /// </summary>
        public List<GetAttachmentFileMetaOutput> SettlementPhotoSets { get; set; }

        public GetQualityAccidentOutput()
        {
            Disposals = new List<GetQualityAccidentDisposalOutput>();
            AccidentPhotoSets = new List<GetAttachmentFileMetaOutput>();
            SettlementPhotoSets = new List<GetAttachmentFileMetaOutput>();

        }

    }
}
