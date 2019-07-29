using PoorFff.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Quality.QualityAccidentDisposals
{
    public enum DisposalState
    {
        /// <summary>
        /// 未处置
        /// </summary>
        [EnumDescription("未处置")]
        NotStarted = 1,
        /// <summary>
        /// 正在处置
        /// </summary>
        [EnumDescription("正在处置")]
        Underway = 2,
        /// <summary>
        /// 已处置
        /// </summary>
        [EnumDescription("已处置")]
        Completed = 3
    }
}
