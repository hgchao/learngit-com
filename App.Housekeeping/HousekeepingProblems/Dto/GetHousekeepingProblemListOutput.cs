using App.Housekeeping.HousekeepingProblemRectifications;
using Newtonsoft.Json;
using PoorFff.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Housekeeping.Housekeepings.Dto
{
    public class GetHousekeepingProblemListOutput
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
        public DateTime? CompletionTime { get; set; }

        /// <summary>
        /// 是否有权限操作
        /// </summary>
        public bool HasPermission { get; set; }

        public GetHousekeepingProblemListOutput()
        {
        }
    }
}
