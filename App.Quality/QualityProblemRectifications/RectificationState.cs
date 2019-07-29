using PoorFff.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Quality.QualityProblemRectifications
{
    public enum RectificationState
    {
        /// <summary>
        /// 未整改
        /// </summary>
        [EnumDescription("未整改")]
        NotStarted = 1,
        /// <summary>
        /// 正在整改
        /// </summary>
        [EnumDescription("正在整改")]
        Underway = 2,
        /// <summary>
        /// 已整改
        /// </summary>
        [EnumDescription("已整改")]
        Completed = 3
    }
}
