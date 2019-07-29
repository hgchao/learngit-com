
using App.Core.FileManagement.AttachmentFileMetas.Dto;
using App.Housekeeping.HousekeepingProblemRectifications;
using App.Housekeeping.HousekeepingProblemRectifications.Dto;
using App.Housekeeping.HousekeepingProblems;
using App.Housekeeping.Housekeepings;
using App.Housekeeping.Housekeepings.Dto;
using PoorFff.Mapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Housekeeping
{
    public class HousekeepingProfile: PfProfile
    {
        public HousekeepingProfile()
        {
            //文明施工问题内容
            base.CreateMap<AddHousekeepingProblemInput, HousekeepingProblem>();
            base.CreateMap<CompleteHousekeepingProblemInput, HousekeepingProblem>();
            base.CreateMap<UpdateHousekeepingProblemInput, HousekeepingProblem>();
            base.CreateMap<HousekeepingProblem, GetHousekeepingProblemOutput>()
                .ForMember(u=>u.ProjectName, expr=>expr.MapFrom(u=>u.Project.Name))
                .ForMember(u=>u.ProjectNumber, expr=>expr.MapFrom(u=>u.Project.No));
            base.CreateMap<HousekeepingProblem, GetHousekeepingProblemListOutput>()
                .ForMember(u=>u.ProjectName, expr=>expr.MapFrom(u=>u.Project.Name))
                .ForMember(u=>u.ProjectNumber, expr=>expr.MapFrom(u=>u.Project.No));
            base.CreateMap<HousekeepingProblem, ExportHousekeepingProblemOutput>()
             .ForMember(u => u.ProjectName, expr => expr.MapFrom(u => u.Project.Name))
             .ForMember(u => u.ProjectNumber, expr => expr.MapFrom(u => u.Project.No));
            CreateMap<AddAttachmentFileMetaInput, HousekeepingProblemAttachment>();   //问题图片
            CreateMap<UpdateAttachmentFileMetaInput, HousekeepingProblemAttachment>();
            CreateMap<HousekeepingProblemAttachment, GetAttachmentFileMetaOutput>()
                .ForMember(u => u.Suffix, expr => expr.MapFrom(u => u.FileMeta.Suffix))
                .ForMember(u => u.FileMetaName, expr => expr.MapFrom(u => u.FileMeta.Name))
                .ForMember(u => u.Size, expr => expr.MapFrom(u => u.FileMeta.Size));
            CreateMap<AddAttachmentFileMetaInput, HousekeepingCompletionAttachment>();   //整改完成图片
            CreateMap<UpdateAttachmentFileMetaInput, HousekeepingCompletionAttachment>();
            CreateMap<HousekeepingCompletionAttachment, GetAttachmentFileMetaOutput>()
                .ForMember(u => u.Suffix, expr => expr.MapFrom(u => u.FileMeta.Suffix))
                .ForMember(u => u.FileMetaName, expr => expr.MapFrom(u => u.FileMeta.Name))
                .ForMember(u => u.Size, expr => expr.MapFrom(u => u.FileMeta.Size));

            //整改进展
            base.CreateMap<AddHousekeepingProblemRectificationInput, HousekeepingProblemRectification>();
            base.CreateMap<HousekeepingProblemRectification, GetHousekeepingProblemRectificationOutput>();
            CreateMap<AddAttachmentFileMetaInput, HousekeepingProblemRectificationAttachment>();  //整改图片
            CreateMap<HousekeepingProblemRectificationAttachment, GetAttachmentFileMetaOutput>()
                .ForMember(u => u.Suffix, expr => expr.MapFrom(u => u.FileMeta.Suffix))
                .ForMember(u => u.FileMetaName, expr => expr.MapFrom(u => u.FileMeta.Name))
                .ForMember(u => u.Size, expr => expr.MapFrom(u => u.FileMeta.Size));
        }
    }
}
