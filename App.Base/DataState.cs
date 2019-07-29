using System;
using System.Collections.Generic;
using System.Text;

namespace Oa.Project
{
    public enum DataState
    {
        /// <summary>
        /// 更新中
        /// </summary>
        Updating = 1,
        /// <summary>
        /// 创建中
        /// </summary>
        Creating = 2,
        /// <summary>
        /// 更新完成
        /// </summary>
        Updated = 3,
        /// <summary>
        /// 创建完成
        /// </summary>
        Created = 4,
        /// <summary>
        /// 最终数据
        /// </summary>
        Stable = 5,
        /// <summary>
        /// 丢弃的
        /// </summary>
        Discard = 6
    }
}
