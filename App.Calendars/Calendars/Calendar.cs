using App.Core.Common.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Calendars.Calendars
{
   public class Calendar : EntityWithAuditHaveTenant
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? StopTime { get; set; }
        /// <summary>
        /// 状态（0待办1完成）
        /// </summary>
        public CalendarState CalendarState { get; set; }
        /// <summary>
        /// 完成时间
        /// </summary>
        public DateTime? FinishTime { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 是否全天（1是，0否）
        /// </summary>
        public bool AllDay { get; set; }

    }
}
