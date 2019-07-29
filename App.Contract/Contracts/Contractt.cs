using App.Core.Common.Entities;
using App.Core.Parameter.Selections;
using App.Projects.ProjectBaseInfos;

using App.Contract.ContractDeposits;
using App.Projects.Projects;
using PoorFff.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using App.Contract.ConstructionUnits;
using App.Core.Workflow.ProcessInstances;
using Oa.Project;

namespace App.Contract.Contracts
{
    public class Contractt: EntityWithAuditHaveTenant
    {

        public int ProjectId { get; set; }
        /// <summary>
        /// 所在项目
        /// </summary>
        [ForeignKey("ProjectId")]
        public Project Project { get; set; }

        /// <summary>
        /// 合同名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 合同编号
        /// </summary>
        public string ContractNumber { get; set; }

        public int? CategoryId { get; set; }
        /// <summary>
        /// 合同分类
        /// </summary>
        [ForeignKey("CategoryId")]
        public Option Category { get; set; }

        //public int? ContractionMethodId { get; set; }
        ///// <summary>
        ///// 发包模式
        ///// </summary>
        //[ForeignKey("ContractionMethodId")]
        //public Option ContractionMethod { get; set; }

        /// <summary>
        /// 标段
        /// </summary>
        public string Section { get; set; }

        /// <summary>
        /// 发包方
        /// </summary>
        public string Employer{ get; set; }

        public int? ContractorId { get; set; }
        /// <summary>
        /// 承包方
        /// </summary>
        [ForeignKey("ContractorId")]
        public ConstructionUnit Contractor { get; set; }

        /// <summary>
        /// 第三方
        /// </summary>
        public string ThirdParty { get; set; }

        /// <summary>
        /// 合同价
        /// </summary>
        [Column(TypeName = "decimal(15,2)")]
        public decimal ContractPrice { get; set; }

        /// <summary>
        /// 合同结算价
        /// </summary>
        [Column(TypeName = "decimal(15,2)")]
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
        public ContractDeposit Deposit { get; set; }

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
        /// 主体id
        /// </summary>
        public int? Mid { get; set; }
        [ForeignKey("Mid")]
        public Contractt MainObject { get; set; }
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
        /// <summary>
        /// 合同附件
        /// </summary>
        public List<ContractAttachment> Attachments { get; set; }

        public Contractt()
        {
            Attachments = new List<ContractAttachment>();
        }
    }

   
}
