using App.Core.Common.EntityFramework;
using App.Safety.SafetyAccidentDisposals;
using App.Safety.SafetyAccidents;
using App.Safety.SafetyProblemProgresses;
using App.Safety.SafetyProblemRectifications;
using App.Safety.SafetyProblems;
using App.Safety.SafetyStandards;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Safety.EntityFramework
{
    public interface ISafetyDbContext: IAppCoreDbContext
    {    // 安全管理标准
        DbSet<SafetyStandard> SafetyStandards { get; set; }
        DbSet<SafetyStandardAttachment> SafetyStandardAttachments { get; set; } //附件
        //安全问题整改
        DbSet<SafetyProblem> SafetyProblems { get; set; }
        DbSet<SafetyProblemAttachment> SafetyProblemAttachments { get; set; } //问题图片
        DbSet<SafetyCompletionAttachment> SafetyCompletionAttachments { get; set; }  //整改完成图片
        // 整改进展
        DbSet<SafetyProblemRectification> SafetyProblemRectifications { get; set; }
        DbSet<SafetyProblemRectificationAttachment> SafetyProblemRectificationAttachments { get; set; } //整改图片
        //安全事故
        DbSet<SafetyAccident> SafetyAccidents{ get; set; }
        DbSet<SafetyAccidentAttachment> SafetyAccidentAttachments { get; set; } //事故图片
        DbSet<SafetySettlementAttachment> SafetySettlementAttachments { get; set; }  //解决后图片
        //处置进展
        DbSet<SafetyAccidentDisposal> SafetyAccidentDisposals { get; set; }
        DbSet<SafetyAccidentDisposalAttachment> SafetyAccidentDisposalAttachments { get; set; }
    }
}
