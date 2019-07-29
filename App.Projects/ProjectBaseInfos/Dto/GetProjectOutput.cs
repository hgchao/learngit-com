using App.Core.FileManagement.AttachmentFileMetas.Dto;
using App.Projects.ProjectLocations.Dto;
using App.Projects.ProjectMembers.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Projects.ProjectBaseInfos.Dto
{
    public class GetProjectOutput
    {
        public int Id { get; set; }

        /// <summary>
        /// 项目编号
        /// </summary>
        public string No { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 项目类型OptionId
        /// </summary>
        public int? TypeId { get; set; }

        /// <summary>
        /// 当前阶段OptionId
        /// </summary>
        public int? StageId { get; set; }

        /// <summary>
        /// 项目状态OptionId
        /// </summary>
        public int? StateId { get; set; }

        /// <summary>
        /// 项目性质OptionId
        /// </summary>
        public int? ProjectNatureId { get; set; }

        /// <summary>
        /// 财务投资
        /// </summary>
        public decimal FinancialInvestment { get; set; }

        /// <summary>
        /// 非财务投资
        /// </summary>
        public decimal NonFinancialInvestment { get; set; }

        /// <summary>
        /// 项目概算
        /// </summary>
        public decimal GeneralEstimate { get; set; }

        /// <summary>
        /// 暂估总投资
        /// </summary>
        public decimal TentativeEstimatedTotalInvestment { get; set; }

        /// <summary>
        /// 开工时间
        /// </summary>
        public DateTime? CommencementDate { get; set; }

        /// <summary>
        /// 竣工时间
        /// </summary>
        public DateTime? FinishDate { get; set; }

        /// <summary>
        /// 项目地址
        /// </summary>
        public GetProjectLocationOutput Location { get; set; }

        /// <summary>
        /// 项目单位
        /// </summary>
        public List<GetProjectUnitOutput> Units { get; set; }

        /// <summary>
        /// 项目成员
        /// </summary>
        public List<GetProjectMemberOutput> Members { get; set; }

        /// <summary>
        /// 项目概述
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// 附件
        /// </summary>
        public List<GetAttachmentFileMetaOutput> Attachments { get; set; }

        public GetProjectOutput()
        {
            Members = new List<GetProjectMemberOutput>();
            Attachments = new List<GetAttachmentFileMetaOutput>();
            Units = new List<GetProjectUnitOutput>();
        }
    }
}
