using App.Quality.QualityProblemRectifications;
using Newtonsoft.Json;
using PoorFff.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Quality.QualityProblems.Dto
{
  public  class ExportQualityProblemOutput
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
        /// 质量问题分类
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// 质量问题来源
        /// </summary>
        public string Source { get; set; }

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
        /// 整改完成时间
        /// </summary>
        public DateTime? CompletionDate { get; set; }
    }
}
