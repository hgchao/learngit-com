using App.Core.Common.Entities;
using App.Contract.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace App.Contract.ContractDeposits
{
    public class ContractDeposit: EntityWithAuditHaveTenant
    {
        /// <summary>
        /// 合同管理id
        /// </summary>
        public int ContractId { get; set; }
        [ForeignKey("ContractId")]
        public Contractt contract { get; set; }
        /// <summary>
        /// 保证金金额
        /// </summary>
        [Column(TypeName = "decimal(15,2)")]
        public decimal DepositAmount { get; set; }

        /// <summary>
        /// 约定退还金额
        /// </summary>
        [Column(TypeName = "decimal(15,2)")]
        public decimal ConventionalRefundAmount { get; set; }
    }
}
