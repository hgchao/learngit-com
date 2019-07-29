using System;
using System.Collections.Generic;
using System.Text;

namespace App.Projects.ProjectBaseInfos.Dto
{
 public   class GetProjectCountOutput
    {
        /// <summary>
        /// 总概算金额(万元)
        /// </summary>
        public decimal GeneralMoney { get; set; }
        /// <summary>
        /// 总合同金额(万元)
        /// </summary>
        public decimal MacrocontractMoney { get; set; }

        /// <summary>
        /// 总预留金(万元)
        /// </summary>
        public decimal ReserveMoney { get; set; }

        /// <summary>
        /// 项目个数
        /// </summary>
        public int ProjectCount { get; set; }

    


    }
}
