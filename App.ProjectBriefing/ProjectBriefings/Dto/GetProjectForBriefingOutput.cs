
using App.Contract.ConstructionUnits.Dto;
using App.Core.FileManagement.AttachmentFileMetas.Dto;
using App.Projects.ProjectAttachments;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.ProjectBriefings.ProjectBriefings.Dto
{
    public class GetProjectForBriefingOutput
    {
        public int Id { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 业主单位
        /// </summary>
        public string ProprietorUnit { get; set; }

        /// <summary>
        /// 代建单位
        /// </summary>
        public string ConstructionAgentUnit { get; set; }

        /// <summary>
        /// 行业主管单位
        /// </summary>
        public string SupervisorUnit { get; set; }

        /// <summary>
        /// 责任单位
        /// </summary>
        public string ResponsibleUnit { get; set; }

        /// <summary>
        /// 项目概述
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// 参建单位
        /// </summary>
        public List<GetConstructionUnitListOutput> ConstructionUnits { get; set; }

        /// <summary>
        /// 附件
        /// </summary>
        public List<GetAttachmentFileMetaOutput> Attachments { get; set; }

        public GetProjectForBriefingOutput()
        {
            ConstructionUnits = new List<GetConstructionUnitListOutput>();
            Attachments = new List<GetAttachmentFileMetaOutput>();
        }
    }
}
