using System;
using System.Collections.Generic;
using System.Text;

namespace App.Calendars.Calendars.Dto
{
  public  class ExportCalendarOutput
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
        ///// <summary>
        ///// 备注
        ///// </summary>
        //public string Remark { get; set; }
        /// <summary>
        /// 是否全天
        /// </summary>
        public string AllDay { get; set; }
        /// <summary>
        /// 申请人
        /// </summary>
        public string ProposerName { get; set; }
    }
}
