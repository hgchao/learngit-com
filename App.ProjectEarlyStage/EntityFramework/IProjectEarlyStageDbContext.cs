using App.Core.Common.EntityFramework;
using Microsoft.EntityFrameworkCore;
using App.ProjectEarlyStage.EarlyStages;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.ProjectEarlyStage.EntityFramework
{
    public interface IProjectEarlyStageDbContext: IAppCoreDbContext
    {
        DbSet<EarlyStage> EarlyStages{ get; set; }
        DbSet<EarlyStageAttachment> EarlyStageAttachments { get; set; }
    }
}
