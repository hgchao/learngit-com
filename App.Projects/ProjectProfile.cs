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
using App.Core.FileManagement.AttachmentFileMetas.Dto;
using App.Projects.ProjectAttachments;
using System.Linq;
using App.Projects.ProjectUnits;
using App.Projects.ProjectUnitMembers.Dto;

namespace App.Projects
{
  public  class ProjectProfile : PfProfile
    {
        public ProjectProfile()
        {
            base.CreateMap<AddProjectLocationInput, ProjectLocation>();
            base.CreateMap<UpdateProjectLocationInput, ProjectLocation>();
            base.CreateMap<ProjectLocation, GetProjectLocationOutput>();

            base.CreateMap<AddProjectMemberInput, ProjectMember>();
            base.CreateMap<UpdateProjectMemberInput, ProjectMember>();
            base.CreateMap<ProjectMember, GetProjectMemberOutput>();

            base.CreateMap<AddProjectUnitMemberInput, ProjectUnitMember>();
            base.CreateMap<UpdateProjectUnitMemberInput, ProjectUnitMember>();
            base.CreateMap<ProjectUnitMember, GetProjectUnitMemberOutput>();

            base.CreateMap<AddProjectUnitInput, ProjectUnit>();
            base.CreateMap<UpdateProjectUnitInput, ProjectUnit>();
            base.CreateMap<ProjectUnit, GetProjectUnitOutput>();

            base.CreateMap<AddProjectInput, Project>();
            base.CreateMap<UpdateProjectInput, Project>();
            base.CreateMap<Project, GetProjectOutput>();
            base.CreateMap<Project, GetProjectStatisticsOutput>();
            base.CreateMap<Project, GetProjectListOutput>()
                .ForMember(u => u.State, expr => expr.MapFrom(s => s.State.Name))
                .ForMember(u => u.Type, expr => expr.MapFrom(s => s.Type.Name))
                .ForMember(u => u.ProjectNature, expr => expr.MapFrom(s => s.ProjectNature.Name))
                .ForMember(u=>u.ProjectLeader, expr=>expr.MapFrom(s=>s.Members.Where(u=>u.ProjectRole == "项目负责人").Select(u=>u.UserId).FirstOrDefault()));

            CreateMap<AddAttachmentFileMetaInput, ProjectAttachment>();
            CreateMap<UpdateAttachmentFileMetaInput, ProjectAttachment>();
            CreateMap<ProjectAttachment, GetAttachmentFileMetaOutput>()
                .ForMember(u => u.Size, expr => expr.MapFrom(u => u.FileMeta.Size))
                .ForMember(u => u.Suffix, expr => expr.MapFrom(u => u.FileMeta.Suffix))
                .ForMember(u => u.FileMetaName, expr => expr.MapFrom(u => u.FileMeta.Name));

        }
    }
}
