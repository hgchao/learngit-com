using App.Core.Common.EntityFramework;
using Microsoft.EntityFrameworkCore;
using App.ProjectProgress.PmMonthlyProgresses;
using App.ProjectProgress.WeeklyProgresses;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.ProjectProgress.EntityFramework
{
    public interface IProjectProgressDbContext: IAppCoreDbContext
    {
        DbSet<MonthlyProgress> PmMonthlyProgresses { get; set; }
        DbSet<WeeklyProgress> WeeklyProgresses { get; set; }
        DbSet<WeeklyProgressAttachment> WeeklyProgressAttachments { get; set; }
    }
}
