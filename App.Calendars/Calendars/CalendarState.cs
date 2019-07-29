using PoorFff.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Calendars.Calendars
{
 public enum CalendarState
    {
        /// <summary>
        /// 待办
        /// </summary>
        [EnumDescription("待办")]
        WaitApproval = 0,
        /// <summary>
        /// 完成
        /// </summary>
        [EnumDescription("通过")]
        Passed = 1
    }
}
