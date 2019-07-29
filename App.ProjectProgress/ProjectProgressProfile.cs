using App.Core.FileManagement.AttachmentFileMetas.Dto;
using App.ProjectProgress.PmMonthlyProgresses;
using App.ProjectProgress.PmMonthlyProgresses.Dto;
using App.ProjectProgress.WeeklyProgresses;
using App.ProjectProgress.WeeklyProgresses.Dto;
using PoorFff.Mapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.ProjectProgress
{
    public class ProjectProgressProfile: PfProfile
    {
        public ProjectProgressProfile()
        {
            base.CreateMap<AddMonthlyProgressInput, MonthlyProgress>()
                .ForMember(u => u.RecordDate, expr => expr.MapFrom(u => new DateTime(u.Year, u.Month, 1)));
            base.CreateMap<UpdateMonthlyProgressInput, MonthlyProgress>()
                .ForMember(u => u.RecordDate, expr => expr.MapFrom(u => new DateTime(u.Year, u.Month, 1)));

            base.CreateMap<MonthlyProgress, GetMonthlyProgressOutput>()
                .ForMember(u => u.CompletedInvestment, expr => expr.MapFrom(u => u.CompletedDemolitionFee + u.CompletedProjectCosts))
                .ForMember(u => u.Year, expr => expr.MapFrom(u => u.RecordDate.Year))
                .ForMember(u => u.Month, expr => expr.MapFrom(u => u.RecordDate.Month))
                .ForMember(u => u.ProjectName, expr => expr.MapFrom(u => u.Project.Name));
            base.CreateMap<MonthlyProgress, GetMonthlyProgressListOutput>()
                .ForMember(u => u.CompletedInvestment, expr => expr.MapFrom(u => u.CompletedDemolitionFee + u.CompletedProjectCosts))
                .ForMember(u => u.Year, expr => expr.MapFrom(u => u.RecordDate.Year))
                .ForMember(u => u.Month, expr => expr.MapFrom(u => u.RecordDate.Month))
                .ForMember(u => u.ProjectName, expr => expr.MapFrom(u => u.Project.Name));

            base.CreateMap<AddWeeklyProgressInput, WeeklyProgress>();
            base.CreateMap<UpdateWeeklyProgressInput, WeeklyProgress>();
            base.CreateMap<WeeklyProgress, GetWeeklyProgressOutput>()
                .ForMember(u => u.ProjectName, expr => expr.MapFrom(u => u.Project.Name));
            base.CreateMap<WeeklyProgress, GetWeeklyProgressListOutput>()
                .ForMember(u => u.ProjectName, expr => expr.MapFrom(u => u.Project.Name));
            //周进度附件
            CreateMap<AddAttachmentFileMetaInput, WeeklyProgressAttachment>();
            CreateMap<WeeklyProgressAttachment, GetAttachmentFileMetaOutput>()
                .ForMember(u => u.Suffix, expr => expr.MapFrom(u => u.FileMeta.Suffix))
                .ForMember(u => u.FileMetaName, expr => expr.MapFrom(u => u.FileMeta.Name))
                .ForMember(u => u.Size, expr => expr.MapFrom(u => u.FileMeta.Size));
        }
    }
}
