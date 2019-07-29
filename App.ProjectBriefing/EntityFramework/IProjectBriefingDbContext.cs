using Microsoft.EntityFrameworkCore;
using App.ProjectBriefings.ProjectBriefings;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.ProjectBriefings.EntityFramework
{
    public interface IProjectBriefingDbContext
    {
        DbSet<ProjectBriefing> ProjectBriefings { get; set; }
    }
}
