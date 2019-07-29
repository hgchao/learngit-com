using System;
using System.Collections.Generic;
using System.Text;

namespace App.Contract.Contracts.Dto
{
   public class GetContractCountOutput
    {
        /// <summary>
        /// 合同月个数
        /// </summary>
        public string MonthCount { get; set; }
        /// <summary>
        /// 合同月金额
        /// </summary>
        public string MonthMoney { get; set; }
    }
}
