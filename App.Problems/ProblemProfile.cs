
using App.Core.FileManagement.AttachmentFileMetas.Dto;
using App.Problems.ProblemCoordinations;
using App.Problems.ProblemRectifications.Dto;
using App.Problems.Problems;
using App.Problems.Problems.Dto;
using PoorFff.Mapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Problems
{
    public class ProblemProfile: PfProfile
    {
        public ProblemProfile()
        {

            CreateMap<AddProblemInput, Problem>();
            base.CreateMap<UpdateProblemInput, Problem>();
            base.CreateMap<CompleteProblemInput, Problem>();
            base.CreateMap<Problem, GetProblemOutput>()
                .ForMember(u => u.Category, expr => expr.MapFrom(u => u.Category.Name))
                .ForMember(u => u.ProjectName, expr => expr.MapFrom(u => u.Project.Name))
                .ForMember(u => u.ProjectNumber, expr => expr.MapFrom(u => u.Project.No));
            base.CreateMap<Problem, GetProblemListOutput>()
                .ForMember(u=>u.Category, expr=>expr.MapFrom(u=>u.Category.Name))
                .ForMember(u=>u.ProjectName, expr=>expr.MapFrom(u=>u.Project.Name))
                .ForMember(u=>u.ProjectNumber, expr=>expr.MapFrom(u=>u.Project.No));
            base.CreateMap<Problem, ExportProblemOutput>()
               .ForMember(u => u.Category, expr => expr.MapFrom(u => u.Category.Name))
               .ForMember(u => u.ProjectName, expr => expr.MapFrom(u => u.Project.Name))
               .ForMember(u => u.ProjectNumber, expr => expr.MapFrom(u => u.Project.No));
            CreateMap<AddAttachmentFileMetaInput, ProblemAttachment>();   //问题图片
            CreateMap<UpdateAttachmentFileMetaInput, ProblemAttachment>();
            CreateMap<ProblemAttachment, GetAttachmentFileMetaOutput>()
                .ForMember(u => u.Suffix, expr => expr.MapFrom(u => u.FileMeta.Suffix))
                .ForMember(u => u.FileMetaName, expr => expr.MapFrom(u => u.FileMeta.Name))
                .ForMember(u => u.Size, expr => expr.MapFrom(u => u.FileMeta.Size));

            base.CreateMap<AddProblemCoordinationInput, ProblemCoordination>();
            base.CreateMap<ProblemCoordination, GetProblemCoordinationOutput>();
            CreateMap<AddAttachmentFileMetaInput, ProblemCoordinationAttachment>();  //协调图片
            CreateMap<ProblemCoordinationAttachment, GetAttachmentFileMetaOutput>()
                .ForMember(u => u.Suffix, expr => expr.MapFrom(u => u.FileMeta.Suffix))
                .ForMember(u => u.FileMetaName, expr => expr.MapFrom(u => u.FileMeta.Name))
                .ForMember(u => u.Size, expr => expr.MapFrom(u => u.FileMeta.Size));
        }
    }
}
