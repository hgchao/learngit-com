using System;
using System.Collections.Generic;
using System.Text;

namespace App.Funds.ContractPayments.Dto
{
  public  class GetContractPaymentMoneyOutput
    {
        /// <summary>
        /// 累计完成金额(万元)
        /// </summary>
        public decimal CompletionMoney { get; set; }

        /// <summary>
        /// 累计应支付金额(万元)
        /// </summary>
        public decimal ShouldMoney { get; set; }
    }
}
