using System;
using System.Collections.Generic;
using System.Text;

namespace App.ProjectProgress.PmMonthlyProgresses.Dto
{
    public class UpdateMonthlyProgressInput
    {
        public int Id { get; set; }

        /// <summary>
        /// 项目Id
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// 进度年份
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// 进度月份
        /// </summary>
        public int Month { get; set; }

        /// <summary>
        /// 本月完成征拆费
        /// </summary>
        public decimal CompletedDemolitionFee { get; set; }

        /// <summary>
        /// 本月完成工程费
        /// </summary>
        public decimal CompletedProjectCosts { get; set; }

        /// <summary>
        /// 累计形象进度
        /// </summary>
        public string AccumulatedImageProgress { get; set; }

        /// <summary>
        /// 形象进度
        /// </summary>
        public string ImageProgress { get; set; }

        /// <summary>
        /// 督办
        /// </summary>
        public string Supervision { get; set; }

        /// <summary>
        /// 下月计划形象进度
        /// </summary>
        public string NextMonthPlannedImageProgress { get; set; }
    }
}
