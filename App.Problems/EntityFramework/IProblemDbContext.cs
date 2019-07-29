using App.Core.Common.EntityFramework;
using App.Problems.ProblemCoordinations;
using App.Problems.Problems;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Problems.EntityFramework
{
    public interface IProblemDbContext: IAppCoreDbContext
    {
        DbSet<Problem> Problems { get; set; }
        DbSet<ProblemAttachment> ProblemAttachments { get; set; }
        DbSet<ProblemCoordination> ProblemCoordinations { get; set; }
        DbSet<ProblemCoordinationAttachment> ProblemCoordinationAttachments { get; set; }
    }
}
