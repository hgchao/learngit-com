using App.Core.FileManagement.AttachmentFileMetas.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.ProjectProgress.WeeklyProgresses.Dto
{
    public class GetWeeklyProgressListOutput
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
        /// 新增时间
        /// </summary>
        public DateTime AddDate { get; set; }

        ///// <summary>
        ///// 年份
        ///// </summary>
        //public int Year { get; set; }

        ///// <summary>
        ///// 周数
        ///// </summary>
        //public int Week { get; set; }

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
        /// 下周计划形象进度
        /// </summary>
        public string NextMonthPlannedImageProgress { get; set; }

        /// <summary>
        /// 详细信息
        /// </summary>
        public string Information { get; set; }

        /// <summary>
        /// 是否有权限操作
        /// </summary>
        public bool HasPermission { get; set; }

        /// <summary>
        /// 附件
        /// </summary>
        public List<GetAttachmentFileMetaOutput> Attachments { get; set; }

        public GetWeeklyProgressListOutput()
        {
            Attachments = new List<GetAttachmentFileMetaOutput>();
        }

    }
}
