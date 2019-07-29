using App.Problems.ProblemRectifications;
using Newtonsoft.Json;
using PoorFff.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Problems.Problems.Dto
{
   public  class ExportProblemOutput
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
        /// 存在问题分类
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// 问题内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 计划完成时间
        /// </summary>
        public DateTime? PlannedCompletionTime { get; set; }

        /// <summary>
        /// 实际完成时间
        /// </summary>
        public DateTime? ActualCompletionTime { get; set; }

        /// <summary>
        /// 协调情况
        /// </summary>
        [JsonConverter(typeof(EnumJsonConverter))]
        public CoordinationState CoordinationState { get; set; }
    }
}
