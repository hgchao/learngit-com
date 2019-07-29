using System;
using System.Collections.Generic;
using System.Text;

namespace App.Contract.ContractDeposits.Dto
{
    public class GetContractDepositOutput
    {
        public int Id { get; set; }
        
        /// <summary>
        /// 保证金金额
        /// </summary>
        public decimal DepositAmount { get; set; }

        /// <summary>
        /// 约定退还金额
        /// </summary>
        public decimal ConventionalRefundAmount { get; set; }
    }
}
