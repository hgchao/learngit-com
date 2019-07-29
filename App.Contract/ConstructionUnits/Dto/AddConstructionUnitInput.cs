using System;
using System.Collections.Generic;
using System.Text;

namespace App.Contract.ConstructionUnits.Dto
{
    public class AddConstructionUnitInput
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
    }
}
