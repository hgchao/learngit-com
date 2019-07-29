using App.Contract.Contracts;
using App.Core.Common.Entities;
using App.Core.Common.Repositories;
using App.Core.Workflow.ProcessInstances;
using Oa.Project;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace App.Funds.ContractPayments
{
    public class ContractPayment: EntityWithAuditHaveTenant
    {
        public int ContractId { get; set; }
        [ForeignKey("ContractId")]
        public Contractt Contract { get; set; }

        /// <summary>
        /// 支付单号
        /// </summary>
        public string PaymentNumber { get; set; }

        /// <summary>
        /// 完成金额
        /// </summary>
        [Column(TypeName = "decimal(15,2)")]
        public decimal CompleteAmount { get; set; }

        /// <summary>
        /// 支付金额
        /// </summary>
        [Column(TypeName = "decimal(15,2)")]
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

        /// <summary>
        /// 主体id
        /// </summary>
        public int? Mid { get; set; }
        [ForeignKey("Mid")]
        public ContractPayment MainObject { get; set; }
        /// <summary>
        /// 当前状态
        /// </summary>
        public DataState State { get; set; }
        /// <summary>
        /// 流程实例id 
        /// </summary>
        public int? ProcessInstanceId { get; set; }
        [ForeignKey("ProcessInstanceId")]
        public Wf_Hi_ProcessInstance ProcessInstance { get; set; }
    }
}
