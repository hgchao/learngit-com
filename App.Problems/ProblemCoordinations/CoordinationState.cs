using PoorFff.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Problems.ProblemRectifications
{
    public enum CoordinationState
    {
        /// <summary>
        /// 未解决
        /// </summary>
        [EnumDescription("未解决")]
        NotStarted = 1,
        /// <summary>
        /// 正在解决
        /// </summary>
        [EnumDescription("正在解决")]
        Underway = 2,
        /// <summary>
        /// 已解决
        /// </summary>
        [EnumDescription("已解决")]
        Completed = 3
    }
}
