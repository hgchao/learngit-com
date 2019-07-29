using App.Core.Common.EntityFramework;
using App.Housekeeping.HousekeepingProblemRectifications;
using App.Housekeeping.HousekeepingProblems;
using App.Housekeeping.Housekeepings;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Housekeeping.EntityFramework
{
    public interface IHousekeepingDbContext: IAppCoreDbContext
    {
        DbSet<HousekeepingProblem> HousekeepingProblems { get; set; }
        DbSet<HousekeepingProblemAttachment> HousekeepingProblemAttachments { get; set; }
        DbSet<HousekeepingCompletionAttachment> HousekeepingCompletionAttachments { get; set; }
        DbSet<HousekeepingProblemRectification> HousekeepingProblemRectifications { get; set; }
        DbSet<HousekeepingProblemRectificationAttachment> HousekeepingProblemRectificationAttachments { get; set; }
    }
}
