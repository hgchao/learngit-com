using App.Core.Common.EntityFramework;
using App.Quality.QualityAccidentDisposals;
using App.Quality.QualityAccidents;
using App.Quality.QualityProblemRectifications;
using App.Quality.QualityProblems;
using App.Quality.QualityStandards;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Quality.EntityFramework
{
    public interface IQualityDbContext: IAppCoreDbContext
    {
        //质量管理标准
        DbSet<QualityStandard> QualityStandards { get; set; }
        DbSet<QualityStandardAttachment> QualityStandardAttachments { get; set; } //附件
        //质量问题整改
        DbSet<QualityProblem> QualityProblems { get; set; }
        DbSet<QualityProblemAttachment> QualityProblemAttachments { get; set; }   //问题图片
        DbSet<QualityCompletionAttachment> QualityCompletionAttachments { get; set; } //整改完成图片
        //整改进展
        DbSet<QualityProblemRectification> QualityProblemRectifications { get; set; }
        DbSet<QualityProblemRectificationAttachment> QualityProblemRectificationAttachments { get; set; } //整改图片
        //质量事故
        DbSet<QualityAccident> QualityAccidents{ get; set; }
        DbSet<QualityAccidentAttachment> QualityAccidentAttachments { get; set; }  //质量事故图片
        DbSet<QualitySettlementAttachment> QualitySettlementAttachments { get; set; } //解决后图片
        //处置进展
        DbSet<QualityAccidentDisposal> QualityAccidentDisposals { get; set; }
        DbSet<QualityAccidentDisposalAttachment> QualityAccidentDisposalAttachments { get; set; } //处置图片
    }
}
