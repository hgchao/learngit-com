using App.Core.FileManagement.AttachmentFileMetas.Dto;
using App.ProjectBriefings.ProjectBriefings;
using App.ProjectBriefings.ProjectBriefings.Dto;
using App.Projects.ProjectAttachments;
using App.Projects.ProjectBaseInfos;
using PoorFff.Mapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.ProjectBriefings
{
    public class ProjectBriefingProfile: PfProfile
    {
        public ProjectBriefingProfile()
        {
          base.CreateMap<Project, GetProjectForBriefingOutput>();
          CreateMap<ProjectAttachment, GetAttachmentFileMetaOutput>()
                .ForMember(u => u.Suffix, expr => expr.MapFrom(u => u.FileMeta.Suffix))
                .ForMember(u => u.FileMetaName, expr => expr.MapFrom(u => u.FileMeta.Name))
                .ForMember(u => u.Size, expr => expr.MapFrom(u => u.FileMeta.Size));

            base.CreateMap<ProjectBriefing, GetProjectBriefingOutput>();
                //.ForMember(u=>u.Project, expr=>expr.Ignore());
        }
    }
}
