using App.Safety.SafetyAccidentDisposals;
using Newtonsoft.Json;
using PoorFff.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Safety.SafetyAccidents.Dto
{
    public class GetSafetyAccidentListOutput
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
        /// 事故起因
        /// </summary>
        public string Source { get; set; }

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
        /// 解决的时间
        /// </summary>
        public DateTime? SettlementTime { get; set; }

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
        /// 是否有权限操作
        /// </summary>
        public bool HasPermission { get; set; }
    }
}
