using App.Projects.ProjectLocations.Dto;
using App.Projects.ProjectMembers.Dto;
using App.Projects.ProjectBaseInfos;
using App.Projects.ProjectLocations;
using App.Projects.ProjectMembers;
using PoorFff.Mapper;
using System;
using System.Collections.Generic;
using System.Text;
using App.Projects.ProjectBaseInfos.Dto;
using App.ProjectGantts.Tasks;
using App.ProjectGantts.Tasks.Dto;
using App.ProjectGantts.Links;
using App.ProjectGantts.Links.Dto;
using App.ProjectGantts.Gantts;
using App.ProjectGantts.Gantts.Dto;
using App.Core.FileManagement.AttachmentFileMetas.Dto;
using App.ProjectGantts.TaskAttachments;

namespace App.ProjectGantts
{
  public  class ProjectGanttProfile : PfProfile
    {
        public ProjectGanttProfile()
        {
            CreateMap<CompleteProjectTaskInput, ProjectTask>();
            CreateMap<UpdateProjectTaskInput, ProjectTask>()
                .ForMember(u => u.Name, expr => expr.MapFrom(u => u.Text))
                .ForMember(u => u.StartDate, expr => expr.MapFrom(u => u.Start_date))
                .ForMember(u => u.Pid, expr => expr.MapFrom(u => u.Parent));
            CreateMap<AddProjectTaskInput, ProjectTask>()
                .ForMember(u => u.Name, expr => expr.MapFrom(u => u.Text))
                .ForMember(u => u.StartDate, expr => expr.MapFrom(u => u.Start_date))
                .ForMember(u => u.Pid, expr => expr.MapFrom(u => u.Parent));
            CreateMap<ProjectTask, GetProjectTaskOutput>()
                .ForMember(u => u.Text, expr => expr.MapFrom(u => u.Name))
                .ForMember(u => u.Start_date, expr => expr.MapFrom(u => u.StartDate))
                .ForMember(u => u.Parent, expr => expr.MapFrom(u => u.Pid));

            CreateMap<AddProjectLinkInput, ProjectLink>()
                .ForMember(u => u.SourceTaskId, expr => expr.MapFrom(u => u.Source))
                .ForMember(u => u.TargetTaskId, expr => expr.MapFrom(u => u.Target));
            CreateMap<UpdateProjectLinkInput, ProjectLink>()
                .ForMember(u => u.SourceTaskId, expr => expr.MapFrom(u => u.Source))
                .ForMember(u => u.TargetTaskId, expr => expr.MapFrom(u => u.Target));
            CreateMap<ProjectLink, GetProjectLinkOutput>()
                .ForMember(u => u.Source, expr => expr.MapFrom(u => u.SourceTaskId))
                .ForMember(u => u.Target, expr => expr.MapFrom(u => u.TargetTaskId));

            CreateMap<ProjectGantt, GetProjectGanttOutput>()
                .ForMember(u => u.Data, expr => expr.MapFrom(u => u.Tasks.MapToList<GetProjectTaskOutput>()));

            CreateMap<AddAttachmentFileMetaInput, ProjectTaskAttachment>();
            CreateMap<UpdateAttachmentFileMetaInput, ProjectTaskAttachment>();
            CreateMap<ProjectTaskAttachment, GetAttachmentFileMetaOutput>()
                .ForMember(u => u.Size, expr => expr.MapFrom(u => u.FileMeta.Size))
                .ForMember(u => u.Suffix, expr => expr.MapFrom(u => u.FileMeta.Suffix))
                .ForMember(u => u.FileMetaName, expr => expr.MapFrom(u => u.FileMeta.Name));
        }
    }
}
