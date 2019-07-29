
using App.Contract.ContractDeposits.Dto;
using App.Core.FileManagement.AttachmentFileMetas.Dto;

using System;
using System.Collections.Generic;
using System.Text;

namespace App.Contract.Contracts.Dto
{
    public class GetContractOutput
    {
        public int Id { get; set; }

        /// <summary>
        /// 项目id
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// 合同名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 合同编号
        /// </summary>
        public string ContractNumber { get; set; }
        /// <summary>
        /// 合同分类id
        /// </summary>
        public int? CategoryId { get; set; }
        /// <summary>
        /// 合同分类
        /// </summary>
        public string Category { get; set; }

        //public int? ContractionMethodId { get; set; }
        ///// <summary>
        ///// 发包模式
        ///// </summary>
        //public string ContractionMethod { get; set; }

        /// <summary>
        /// 标段
        /// </summary>
        public string Section { get; set; }

        /// <summary>
        /// 发包方
        /// </summary>
        public string Employer { get; set; }
        /// <summary>
        /// 承包方id
        /// </summary>
        public int? ContractorId { get; set; }
        /// <summary>
        /// 承包方
        /// </summary>
        public string Contractor { get; set; }

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
        public GetContractDepositOutput Deposit { get; set; }

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
        /// 合同附件
        /// </summary>
        public List<GetAttachmentFileMetaOutput> Attachments { get; set; }

        public GetContractOutput()
        {
            Attachments = new List<GetAttachmentFileMetaOutput>();
        }
    }
}
