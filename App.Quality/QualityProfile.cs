using App.Core.FileManagement.AttachmentFileMetas.Dto;
using App.Quality.PmQualityAccidentDisposals.Dto;
using App.Quality.QualityAccidentDisposals;
using App.Quality.QualityAccidentDisposals.Dto;
using App.Quality.QualityAccidents;
using App.Quality.QualityAccidents.Dto;
using App.Quality.QualityProblemRectifications;
using App.Quality.QualityProblemRectifications.Dto;
using App.Quality.QualityProblems;
using App.Quality.QualityProblems.Dto;
using App.Quality.QualityStandards;
using App.Quality.QualityStandards.Dto;
using Pm.Quality.PmQualityAccidents;
using PoorFff.Mapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Quality
{
    public class QualityProfile: PfProfile
    {
        public QualityProfile()
        {
            //质量管理标准
            base.CreateMap<AddQualityStandardInput, QualityStandard>();
            base.CreateMap<UpdateQualityStandardInput, QualityStandard>();
            base.CreateMap<QualityStandard, GetQualityStandardOutput>()
                .ForMember(u=>u.Category, expr=>expr.MapFrom(u=>u.Category.Name));
            CreateMap<AddAttachmentFileMetaInput, QualityStandardAttachment>();   //附件
            CreateMap<UpdateAttachmentFileMetaInput, QualityStandardAttachment>();
            CreateMap<QualityStandardAttachment, GetAttachmentFileMetaOutput>()
                .ForMember(u => u.Suffix, expr => expr.MapFrom(u => u.FileMeta.Suffix))
                .ForMember(u => u.FileMetaName, expr => expr.MapFrom(u => u.FileMeta.Name))
                .ForMember(u => u.Size, expr => expr.MapFrom(u => u.FileMeta.Size));

            //质量问题整改
            base.CreateMap<AddQualityProblemInput, QualityProblem>();
            base.CreateMap<CompleteQualityProblemInput, QualityProblem>();
            base.CreateMap<UpdateQualityProblemInput, QualityProblem>();
            base.CreateMap<QualityProblem, GetQualityProblemOutput>()
                .ForMember(u=>u.Category, expr=>expr.MapFrom(u=>u.Category.Name))
                .ForMember(u=>u.ProjectName, expr=>expr.MapFrom(u=>u.Project.Name))
                .ForMember(u=>u.ProjectNumber, expr=>expr.MapFrom(u=>u.Project.No));
            base.CreateMap<QualityProblem, GetQualityProblemListOutput>()
                .ForMember(u=>u.Category, expr=>expr.MapFrom(u=>u.Category.Name))
                .ForMember(u=>u.ProjectName, expr=>expr.MapFrom(u=>u.Project.Name))
                .ForMember(u=>u.ProjectNumber, expr=>expr.MapFrom(u=>u.Project.No));
            base.CreateMap<QualityProblem, ExportQualityProblemOutput>()
                .ForMember(u => u.Category, expr => expr.MapFrom(u => u.Category.Name))
                .ForMember(u => u.ProjectName, expr => expr.MapFrom(u => u.Project.Name))
                .ForMember(u => u.ProjectNumber, expr => expr.MapFrom(u => u.Project.No));
            CreateMap<AddAttachmentFileMetaInput, QualityProblemAttachment>();   //问题图片
            CreateMap<UpdateAttachmentFileMetaInput, QualityProblemAttachment>();
            CreateMap<QualityProblemAttachment, GetAttachmentFileMetaOutput>()
                .ForMember(u => u.Suffix, expr => expr.MapFrom(u => u.FileMeta.Suffix))
                .ForMember(u => u.FileMetaName, expr => expr.MapFrom(u => u.FileMeta.Name))
                .ForMember(u => u.Size, expr => expr.MapFrom(u => u.FileMeta.Size));
            CreateMap<AddAttachmentFileMetaInput, QualityCompletionAttachment>();  //整改完成图片
            CreateMap<QualityCompletionAttachment, GetAttachmentFileMetaOutput>()
                .ForMember(u => u.Suffix, expr => expr.MapFrom(u => u.FileMeta.Suffix))
                .ForMember(u => u.FileMetaName, expr => expr.MapFrom(u => u.FileMeta.Name))
                .ForMember(u => u.Size, expr => expr.MapFrom(u => u.FileMeta.Size));

            //整改进展
            base.CreateMap<AddQualityProblemRectificationInput, QualityProblemRectification>();
            base.CreateMap<QualityProblemRectification, GetQualityProblemRectificationOutput>();
            CreateMap<AddAttachmentFileMetaInput, QualityProblemRectificationAttachment>();  //整改图片
            CreateMap<QualityProblemRectificationAttachment, GetAttachmentFileMetaOutput>()
                .ForMember(u => u.Suffix, expr => expr.MapFrom(u => u.FileMeta.Suffix))
                .ForMember(u => u.FileMetaName, expr => expr.MapFrom(u => u.FileMeta.Name))
                .ForMember(u => u.Size, expr => expr.MapFrom(u => u.FileMeta.Size));

            //质量事故
            base.CreateMap<AddQualityAccidentInput, QualityAccident>();
            base.CreateMap<SettleQualityAccidentInput, QualityAccident>();
            base.CreateMap<UpdateQualityAccidentInput, QualityAccident>();
            base.CreateMap<QualityAccident, GetQualityAccidentOutput>()
                .ForMember(u => u.ProjectName, expr => expr.MapFrom(u => u.Project.Name))
                .ForMember(u => u.ProjectNumber, expr => expr.MapFrom(u => u.Project.No));
            base.CreateMap<QualityAccident, GetQualityAccidentListOutput>()
                .ForMember(u => u.ProjectName, expr => expr.MapFrom(u => u.Project.Name))
                .ForMember(u => u.ProjectNumber, expr => expr.MapFrom(u => u.Project.No));
            base.CreateMap<QualityAccident, ExportQualityAccidentOutput>()
               .ForMember(u => u.ProjectName, expr => expr.MapFrom(u => u.Project.Name))
               .ForMember(u => u.ProjectNumber, expr => expr.MapFrom(u => u.Project.No));
            CreateMap<AddAttachmentFileMetaInput, QualityAccidentAttachment>();   //质量事故图片
            CreateMap<UpdateAttachmentFileMetaInput, QualityAccidentAttachment>();
            CreateMap<QualityAccidentAttachment, GetAttachmentFileMetaOutput>()
                .ForMember(u => u.Suffix, expr => expr.MapFrom(u => u.FileMeta.Suffix))
                .ForMember(u => u.FileMetaName, expr => expr.MapFrom(u => u.FileMeta.Name))
                .ForMember(u => u.Size, expr => expr.MapFrom(u => u.FileMeta.Size));
            CreateMap<AddAttachmentFileMetaInput, QualitySettlementAttachment>();  //解决后图片
            CreateMap<QualitySettlementAttachment, GetAttachmentFileMetaOutput>()
                .ForMember(u => u.Suffix, expr => expr.MapFrom(u => u.FileMeta.Suffix))
                .ForMember(u => u.FileMetaName, expr => expr.MapFrom(u => u.FileMeta.Name))
                .ForMember(u => u.Size, expr => expr.MapFrom(u => u.FileMeta.Size));

            //处置进展
            base.CreateMap<AddQualityAccidentDisposalInput, QualityAccidentDisposal>();
            base.CreateMap<QualityAccidentDisposal, GetQualityAccidentDisposalOutput>();
            CreateMap<AddAttachmentFileMetaInput, QualityAccidentDisposalAttachment>();  //处置图片
            CreateMap<QualityAccidentDisposalAttachment, GetAttachmentFileMetaOutput>()
                .ForMember(u => u.Suffix, expr => expr.MapFrom(u => u.FileMeta.Suffix))
                .ForMember(u => u.FileMetaName, expr => expr.MapFrom(u => u.FileMeta.Name))
                .ForMember(u => u.Size, expr => expr.MapFrom(u => u.FileMeta.Size));
        }
    }
}
