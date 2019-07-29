using App.Housekeeping.HousekeepingProblemRectifications;
using Newtonsoft.Json;
using PoorFff.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Housekeeping.Housekeepings.Dto
{
   public class ExportHousekeepingProblemOutput
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
        /// 文明施工问题内容
        /// </summary>
        public string Content { get; set; }

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
