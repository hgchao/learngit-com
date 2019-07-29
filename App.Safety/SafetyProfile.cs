using App.Core.FileManagement.AttachmentFileMetas.Dto;
using App.Safety.SafetyAccidentDisposals;
using App.Safety.SafetyAccidentDisposals.Dto;
using App.Safety.SafetyAccidents;
using App.Safety.SafetyAccidents.Dto;
using App.Safety.SafetyProblemProgresses;
using App.Safety.SafetyProblemRectifications;
using App.Safety.SafetyProblemRectifications.Dto;
using App.Safety.SafetyProblems;
using App.Safety.SafetyProblems.Dto;
using App.Safety.SafetyStandards;
using App.Safety.SafetyStandards.Dto;
using PoorFff.Mapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Safety
{
    public class SafetyProfile: PfProfile
    {
        public SafetyProfile()
        {
            // 安全管理标准
            base.CreateMap<AddSafetyStandardInput, SafetyStandard>();
            base.CreateMap<UpdateSafetyStandardInput, SafetyStandard>();
            base.CreateMap<SafetyStandard, GetSafetyStandardOutput>()
                .ForMember(u=>u.Category, expr=>expr.MapFrom(u=>u.Category.Name));
            CreateMap<AddAttachmentFileMetaInput, SafetyStandardAttachment>();   //附件
            CreateMap<UpdateAttachmentFileMetaInput, SafetyStandardAttachment>();
            CreateMap<SafetyStandardAttachment, GetAttachmentFileMetaOutput>()
                .ForMember(u => u.Suffix, expr => expr.MapFrom(u => u.FileMeta.Suffix))
                .ForMember(u => u.FileMetaName, expr => expr.MapFrom(u => u.FileMeta.Name))
                .ForMember(u => u.Size, expr => expr.MapFrom(u => u.FileMeta.Size));

            //安全问题整改
            base.CreateMap<AddSafetyProblemInput, SafetyProblem>();
            base.CreateMap<CompleteSafetyProblemInput, SafetyProblem>();
            base.CreateMap<UpdateSafetyProblemInput, SafetyProblem>();
            base.CreateMap<SafetyProblem, GetSafetyProblemOutput>()
                .ForMember(u=>u.Category, expr=>expr.MapFrom(u=>u.Category.Name))
                .ForMember(u=>u.Source, expr=>expr.MapFrom(u=>u.Source.Name))
                .ForMember(u=>u.ProjectName, expr=>expr.MapFrom(u=>u.Project.Name))
                .ForMember(u=>u.ProjectNumber, expr=>expr.MapFrom(u=>u.Project.No));
            base.CreateMap<SafetyProblem, GetSafetyProblemListOutput>()
                .ForMember(u=>u.Category, expr=>expr.MapFrom(u=>u.Category.Name))
                .ForMember(u=>u.Source, expr=>expr.MapFrom(u=>u.Source.Name))
                .ForMember(u=>u.ProjectName, expr=>expr.MapFrom(u=>u.Project.Name))
                .ForMember(u=>u.ProjectNumber, expr=>expr.MapFrom(u=>u.Project.No));
            CreateMap<SafetyProblem, ExportSafetyProblemOutput>()
                .ForMember(u => u.Category, expr => expr.MapFrom(u => u.Category.Name))
                .ForMember(u => u.Source, expr => expr.MapFrom(u => u.Source.Name))
                .ForMember(u => u.ProjectName, expr => expr.MapFrom(u => u.Project.Name))
                .ForMember(u => u.ProjectNumber, expr => expr.MapFrom(u => u.Project.No));
            CreateMap<AddAttachmentFileMetaInput, SafetyProblemAttachment>();   //问题图片
            CreateMap<UpdateAttachmentFileMetaInput, SafetyProblemAttachment>();
            CreateMap<SafetyProblemAttachment, GetAttachmentFileMetaOutput>()
                .ForMember(u => u.Suffix, expr => expr.MapFrom(u => u.FileMeta.Suffix))
                .ForMember(u => u.FileMetaName, expr => expr.MapFrom(u => u.FileMeta.Name))
                .ForMember(u => u.Size, expr => expr.MapFrom(u => u.FileMeta.Size));
            CreateMap<AddAttachmentFileMetaInput, SafetyCompletionAttachment>();   //整改完成图片
            CreateMap<UpdateAttachmentFileMetaInput, SafetyCompletionAttachment>();
            CreateMap<SafetyCompletionAttachment, GetAttachmentFileMetaOutput>()
                .ForMember(u => u.Suffix, expr => expr.MapFrom(u => u.FileMeta.Suffix))
                .ForMember(u => u.FileMetaName, expr => expr.MapFrom(u => u.FileMeta.Name))
                .ForMember(u => u.Size, expr => expr.MapFrom(u => u.FileMeta.Size));

            // 整改进展
            base.CreateMap<AddSafetyProblemRectificationInput, SafetyProblemRectification>();
            base.CreateMap<SafetyProblemRectification, GetSafetyProblemRectificationOutput>();
            CreateMap<AddAttachmentFileMetaInput, SafetyProblemRectificationAttachment>();  //整改图片
            CreateMap<SafetyProblemRectificationAttachment, GetAttachmentFileMetaOutput>()
                .ForMember(u => u.Suffix, expr => expr.MapFrom(u => u.FileMeta.Suffix))
                .ForMember(u => u.FileMetaName, expr => expr.MapFrom(u => u.FileMeta.Name))
                .ForMember(u => u.Size, expr => expr.MapFrom(u => u.FileMeta.Size));

            //安全事故
            base.CreateMap<AddSafetyAccidentInput, SafetyAccident>();
            base.CreateMap<SettleSafetyAccidentInput, SafetyAccident>();
            base.CreateMap<UpdateSafetyAccidentInput, SafetyAccident>();
            base.CreateMap<SafetyAccident, GetSafetyAccidentOutput>()
                .ForMember(u => u.Severity, expr => expr.MapFrom(u => u.Severity.Name))
                .ForMember(u => u.Source, expr => expr.MapFrom(u => u.Source.Name))
                .ForMember(u => u.Category, expr => expr.MapFrom(u => u.Category.Name))
                .ForMember(u => u.ProjectName, expr => expr.MapFrom(u => u.Project.Name))
                .ForMember(u => u.ProjectNumber, expr => expr.MapFrom(u => u.Project.No));
            base.CreateMap<SafetyAccident, GetSafetyAccidentListOutput>()
                .ForMember(u => u.Source, expr => expr.MapFrom(u => u.Source.Name))
                .ForMember(u => u.Category, expr => expr.MapFrom(u => u.Category.Name))
                .ForMember(u => u.Severity, expr => expr.MapFrom(u => u.Severity.Name))
                .ForMember(u => u.ProjectName, expr => expr.MapFrom(u => u.Project.Name))
                .ForMember(u => u.ProjectNumber, expr => expr.MapFrom(u => u.Project.No));
            CreateMap<SafetyAccident, ExportSafetyAccidentOutput>()
                .ForMember(u => u.Source, expr => expr.MapFrom(u => u.Source.Name))
                .ForMember(u => u.Category, expr => expr.MapFrom(u => u.Category.Name))
                .ForMember(u => u.Severity, expr => expr.MapFrom(u => u.Severity.Name))
                .ForMember(u => u.ProjectName, expr => expr.MapFrom(u => u.Project.Name))
                .ForMember(u => u.ProjectNumber, expr => expr.MapFrom(u => u.Project.No));
            CreateMap<AddAttachmentFileMetaInput, SafetyAccidentAttachment>();   //事故图片
            CreateMap<UpdateAttachmentFileMetaInput, SafetyAccidentAttachment>();
            CreateMap<SafetyAccidentAttachment, GetAttachmentFileMetaOutput>()
                .ForMember(u => u.Suffix, expr => expr.MapFrom(u => u.FileMeta.Suffix))
                .ForMember(u => u.FileMetaName, expr => expr.MapFrom(u => u.FileMeta.Name))
                .ForMember(u => u.Size, expr => expr.MapFrom(u => u.FileMeta.Size));
            CreateMap<AddAttachmentFileMetaInput, SafetySettlementAttachment>();   //解决后图片
            CreateMap<UpdateAttachmentFileMetaInput, SafetySettlementAttachment>();
            CreateMap<SafetySettlementAttachment, GetAttachmentFileMetaOutput>()
                .ForMember(u => u.Suffix, expr => expr.MapFrom(u => u.FileMeta.Suffix))
                .ForMember(u => u.FileMetaName, expr => expr.MapFrom(u => u.FileMeta.Name))
                .ForMember(u => u.Size, expr => expr.MapFrom(u => u.FileMeta.Size));

            //处置进展
            base.CreateMap<AddSafetyAccidentDisposalInput, SafetyAccidentDisposal>();
            base.CreateMap<SafetyAccidentDisposal, GetSafetyAccidentDisposalOutput>();
            CreateMap<AddAttachmentFileMetaInput, SafetyAccidentDisposalAttachment>();  //处置图片
            CreateMap<SafetyAccidentDisposalAttachment, GetAttachmentFileMetaOutput>()
                .ForMember(u => u.Suffix, expr => expr.MapFrom(u => u.FileMeta.Suffix))
                .ForMember(u => u.FileMetaName, expr => expr.MapFrom(u => u.FileMeta.Name))
                .ForMember(u => u.Size, expr => expr.MapFrom(u => u.FileMeta.Size));
        }
    }
}
