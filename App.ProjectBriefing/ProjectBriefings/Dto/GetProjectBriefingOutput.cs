
using System;
using System.Collections.Generic;
using System.Text;

namespace App.ProjectBriefings.ProjectBriefings.Dto
{
    public class GetProjectBriefingOutput
    {
        public GetProjectForBriefingOutput Project { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// 进度截至日期
        /// </summary>
        public string ProgressLimitDate { get; set; }

        /// <summary>
        /// 本周进展(无效)
        /// </summary>
        public string ThisWeekProgress { get; set; }

        /// <summary>
        /// 累计至本周形象进度(无效)
        /// </summary>
        public string CumulativeImageProgress { get; set; }

        /// <summary>
        /// 下周计划(无效)
        /// </summary>
        public string NextWeekProgressPlan { get; set; }

        /// <summary>
        /// 详细信息
        /// </summary>
        public string Information { get; set; }

        /// <summary>
        /// 存在问题及建议
        /// </summary>
        public string ProblemAndSolution { get; set; }

        /// <summary>
        /// 督办(无效)
        /// </summary>
        public string Supervision { get; set; }

        /// <summary>
        /// 质量问题来源及描述
        /// </summary>
        public string QualitySourceAndDescription { get; set; }

        /// <summary>
        /// 质量事故标题和内容
        /// </summary>
        public string QualityAccidentAndDescription { get; set; }

        /// <summary>
        /// 安全问题来源及描述
        /// </summary>
        public string SafetySourceAndDescription { get; set; }

        /// <summary>
        /// 安全事故标题合同内容
        /// </summary>
        public string SafetyAccidentAndDescription { get; set; }

        /// <summary>
        /// 文明施工问题内容
        /// </summary>
        public string HousekeepingConetent { get; set; }
    }
}
