using System;
using System.Collections.Generic;
using System.Text;

namespace App.Calendars.Calendars.Dto
{
   public class AddCalendarInput
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
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 是否全天（1是，0否）
        /// </summary>
        public bool AllDay { get; set; }
    }
}
