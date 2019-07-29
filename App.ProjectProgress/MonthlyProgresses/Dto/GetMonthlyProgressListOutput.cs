using System;
using System.Collections.Generic;
using System.Text;

namespace App.ProjectProgress.PmMonthlyProgresses.Dto
{
    public class GetMonthlyProgressListOutput
    {
        public int Id { get; set; }

        /// <summary>
        /// 项目Id
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }

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
        /// 本月完成投资
        /// </summary>
        public decimal CompletedInvestment { get; set; }
    }
}
