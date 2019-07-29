using App.Core.FileManagement.AttachmentFileMetas.Dto;
using App.Projects.ProjectLocations.Dto;
using App.Projects.ProjectMembers.Dto;
using App.Projects.ProjectUnitMembers.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Projects.ProjectBaseInfos.Dto
{
    public class UpdateProjectInput
    {
        public int Id { get; set; }

        ///// <summary>
        ///// 项目编号
        ///// </summary>
        //public string No { get; set; }

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
        public UpdateProjectLocationInput Location { get; set; }

        /// <summary>
        /// 项目成员
        /// </summary>
        public List<UpdateProjectMemberInput> Members { get; set; }

        /// <summary>
        /// 项目参与单位
        /// </summary>
        public List<UpdateProjectUnitInput> Units { get; set; }

        /// <summary>
        /// 项目概述
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// 附件
        /// </summary>
        public List<UpdateAttachmentFileMetaInput> Attachments { get; set; }

        public UpdateProjectInput()
        {
            Members = new List<UpdateProjectMemberInput>();
            Attachments = new List<UpdateAttachmentFileMetaInput>();
            Units = new List<UpdateProjectUnitInput>();
        }
    }
}
