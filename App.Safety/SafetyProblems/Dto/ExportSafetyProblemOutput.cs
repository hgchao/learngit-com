using App.Safety.SafetyProblemRectifications;
using Newtonsoft.Json;
using PoorFff.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Safety.SafetyProblems.Dto
{
   public class ExportSafetyProblemOutput
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
        /// 安全问题分类
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// 安全问题来源
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// 安全问题创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 整改情况
        /// </summary>
        [JsonConverter(typeof(EnumJsonConverter))]
        public RectificationState RectificationState { get; set; }

        /// <summary>
        /// 限期整改时间
        /// </summary>
        public DateTime Deadline { get; set; }

        /// <summary>
        /// 整改完成时间
        /// </summary>
        public DateTime? CompletionDate { get; set; }
    }
}
