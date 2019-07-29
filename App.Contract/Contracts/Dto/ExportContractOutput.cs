using System;
using System.Collections.Generic;
using System.Text;

namespace App.Contract.Contracts.Dto
{
  public  class ExportContractOutput
    {
        /// <summary>
        /// 合同名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 项目Id
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// 合同分类
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// 标段
        /// </summary>
        public string Section { get; set; }
        /// <summary>
        /// 承包方
        /// </summary>
        public string Contractor { get; set; }
        /// <summary>
        /// 合同履行起止时间
        /// </summary>
        public string PerformanceDatePeriods { get; set; }
        /// <summary>
        /// 合同价
        /// </summary>
        public decimal ContractPrice { get; set; }
        /// <summary>
        /// 合同结算价
        /// </summary>
        public decimal ContractSettlementPrice { get; set; }
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
    }
}
