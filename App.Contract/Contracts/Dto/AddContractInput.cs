
using App.Contract.ContractDeposits.Dto;
using App.Core.FileManagement.AttachmentFileMetas.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Contract.Contracts.Dto
{
    public class AddContractInput
    {
        /// <summary>
        /// 项目id
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// 合同名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 合同编号
        /// </summary>
        public string ContractNumber { get; set; }

        /// <summary>
        /// 合同分类OptionId
        /// </summary>
        public int? CategoryId { get; set; }

        ///// <summary>
        ///// 发包模式OptionId
        ///// </summary>
        //public int? ContractionMethodId { get; set; }

        /// <summary>
        /// 标段
        /// </summary>
        public string Section { get; set; }

        /// <summary>
        /// 发包方
        /// </summary>
        public string Employer { get; set; }

        /// <summary>
        /// 承包方Id
        /// </summary>
        public int? ContractorId { get; set; }

        /// <summary>
        /// 第三方
        /// </summary>
        public string ThirdParty { get; set; }

        /// <summary>
        /// 合同价
        /// </summary>
        public decimal ContractPrice { get; set; }

        /// <summary>
        /// 合同结算价
        /// </summary>
        public decimal ContractSettlementPrice { get; set; }

        /// <summary>
        /// 合同内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 支付方式
        /// </summary>
        public string PaymentTerms { get; set; }

        /// <summary>
        /// 合同签订日期
        /// </summary>
        public DateTime? SigningDate { get; set; }

        /// <summary>
        /// 工期
        /// </summary>
        public int Period { get; set; }

        /// <summary>
        /// 保证金
        /// </summary>
        public AddContractDepositInput Deposit { get; set; }

        /// <summary>
        /// 合同履行开始时间
        /// </summary>
        public DateTime? PerformanceStartDate { get; set; }

        /// <summary>
        /// 合同履行开始时间
        /// </summary>
        public DateTime? PerformanceEndDate { get; set; }

        /// <summary>
        /// 自保信息
        /// </summary>
        public string Information { get; set; }

        /// <summary>
        /// 竣工时间
        /// </summary>
        public DateTime? CompletionDate { get; set; }

        /// <summary>
        /// 签署对象
        /// </summary>
        public string Signature { get; set; }
        /// <summary>
        /// 附件
        /// </summary>
        public List<AddAttachmentFileMetaInput> Attachments { get; set; }
        public AddContractInput()
        {
            Attachments = new List<AddAttachmentFileMetaInput>();
        }
      
    }
}
