using App.Core.FileManagement.AttachmentFileMetas.Dto;
using App.ProjectEarlyStage.EarlyStages;
using App.ProjectEarlyStage.EarlyStages.Dto;
using App.ProjectEarlyStage.PmEarlyStages.Dto;
using Pm.ProjectEarlyStage.PmEarlyStages;
using PoorFff.Mapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.ProjectEarlyStage
{
    public class ProjectEarlyStageProfile: PfProfile
    {
        public ProjectEarlyStageProfile()
        {


            base.CreateMap<AddEarlyStageInput, EarlyStage>();
            base.CreateMap<UpdateEarlyStageInput, EarlyStage>();
            base.CreateMap<EarlyStage, GetEarlyStageOutput>()
                .ForMember(u => u.Node, expr => expr.MapFrom(u => u.Node.Name))
                .ForMember(u => u.ProjectName, expr => expr.MapFrom(u => u.Project.Name));
            CreateMap<AddAttachmentFileMetaInput, EarlyStageAttachment>();   
            CreateMap<UpdateAttachmentFileMetaInput, EarlyStageAttachment>();
            CreateMap<EarlyStageAttachment, GetAttachmentFileMetaOutput>()
                .ForMember(u => u.Suffix, expr => expr.MapFrom(u => u.FileMeta.Suffix))
                .ForMember(u => u.FileMetaName, expr => expr.MapFrom(u => u.FileMeta.Name))
                .ForMember(u => u.Size, expr => expr.MapFrom(u => u.FileMeta.Size));
        }
    }
}
