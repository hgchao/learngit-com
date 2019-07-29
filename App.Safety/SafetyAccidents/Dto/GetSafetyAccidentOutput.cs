using App.Core.FileManagement.AttachmentFileMetas.Dto;
using App.Safety.SafetyAccidentDisposals;
using App.Safety.SafetyAccidentDisposals.Dto;
using Newtonsoft.Json;
using PoorFff.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Safety.SafetyAccidents.Dto
{
    public class GetSafetyAccidentOutput
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
        /// 事故起因Id
        /// </summary>
        public int SourceId { get; set; }
        /// <summary>
        /// 事故起因
        /// </summary>
        public string Source { get; set; }
        /// <summary>
        /// 事故分类Id
        /// </summary>
        public int CategoryId { get; set; }
        /// <summary>
        /// 事故分类
        /// </summary>
        public string Category { get; set; }

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
        /// 事故图片
        /// </summary>
        public List<GetAttachmentFileMetaOutput> AccidentPhotoSets { get; set; }



        /// <summary>
        /// 处置进展
        /// </summary>
        public List<GetSafetyAccidentDisposalOutput> Disposals { get; set; }

        public int SeverityId { get; set; }
        /// <summary>
        /// 事故严重程度
        /// </summary>
        public string Severity { get; set; }

        /// <summary>
        /// 受伤人数
        /// </summary>
        public int InjuredNumber { get; set; }

        /// <summary>
        /// 死亡人数
        /// </summary>
        public int DeathNumber { get; set; }

        /// <summary>
        /// 解决的时间
        /// </summary>
        public DateTime? SettlementTime { get; set; }

        /// <summary>
        /// 解决后图片
        /// </summary>
        public List<GetAttachmentFileMetaOutput> SettlementPhotoSets { get; set; }

        public GetSafetyAccidentOutput()
        {
            Disposals = new List<GetSafetyAccidentDisposalOutput>();
            AccidentPhotoSets = new List<GetAttachmentFileMetaOutput>();
            SettlementPhotoSets = new List<GetAttachmentFileMetaOutput>();
        }

    }
}
