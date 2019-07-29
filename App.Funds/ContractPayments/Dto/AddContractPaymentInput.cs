using System;
using System.Collections.Generic;
using System.Text;

namespace App.Funds.ContractPayments.Dto
{
    public class AddContractPaymentInput
    {
        /// <summary>
        /// 合同Id
        /// </summary>
        public int ContractId { get; set; }

        /// <summary>
        /// 完成金额
        /// </summary>
        public decimal CompleteAmount { get; set; }

        /// <summary>
        /// 支付金额
        /// </summary>
        public decimal PaidAmount { get; set; }

        /// <summary>
        /// 支付时间
        /// </summary>
        public DateTime PaymentTime { get; set; }

        /// <summary>
        /// 支付类型
        /// </summary>
        public string PaymentType { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
