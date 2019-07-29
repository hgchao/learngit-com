using System;
using System.Collections.Generic;
using System.Text;

namespace App.Projects.ProjectBaseInfos.Dto
{
   public class GetProjectStatisticsOutput
    {
        /// <summary>
        /// 房屋建筑
        /// </summary>
        public string Housing { get; set; }

        /// <summary>
        /// 市政道路
        /// </summary>
        public string Municipal { get; set; }

        /// <summary>
        /// 市政绿化
        /// </summary>
        public string Greening { get; set; }

        /// <summary>
        /// 土方平场
        /// </summary>
        public string Earth { get; set; }

        /// <summary>
        /// 市政安装工程
        /// </summary>
        public string Works { get; set; }
    }
}
