using App.Quality.QualityAccidentDisposals;
using Newtonsoft.Json;
using PoorFff.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Quality.QualityAccidents.Dto
{
  public  class ExportQualityAccidentOutput
    {
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
    }
}
