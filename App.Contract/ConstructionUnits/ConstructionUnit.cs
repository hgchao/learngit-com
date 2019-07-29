using App.Core.Common.Entities;
using App.Core.Workflow.ProcessInstances;
using Oa.Project;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace App.Contract.ConstructionUnits
{
    public class ConstructionUnit: EntityWithAuditHaveTenant
    {
        /// <summary>
        /// 参建单位名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 参建单位类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 公司规模
        /// </summary>
        public string CompanySize { get; set; }

        /// <summary>
        /// 注册资金
        /// </summary>
        [Column(TypeName = "decimal(15,2)")]
        public decimal RegisteredCapital { get; set; }

        /// <summary>
        /// 公司法人
        /// </summary>
        public string LegalPerson { get; set; }

        /// <summary>
        /// 统一信用代码
        /// </summary>
        public string UniformCreditCode { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 企业联系人
        /// </summary>
        public string CompanyContact { get; set; }

        /// <summary>
        /// 联系方式
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        /// 评价等级
        /// </summary>
        public string EvaluationLevel { get; set; }

        /// <summary>
        /// 得分
        /// </summary>
        public float Score { get; set; }

        /// <summary>
        /// 主体id
        /// </summary>
        public int? Mid { get; set; }
        [ForeignKey("Mid")]
        public ConstructionUnit MainObject { get; set; }
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
