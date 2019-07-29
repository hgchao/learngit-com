using App.ProjectGantts.Gantts;
using App.ProjectGantts.Links;
using App.ProjectGantts.Tasks;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.ProjectGantts.EntityFramework
{
    public interface IProjectGanttDbContext
    {
        DbSet<ProjectGantt> ProjectGantts { get; set; }
        DbSet<ProjectTask> ProjectTasks { get; set; }
        DbSet<ProjectLink> ProjectLinks { get; set; }
    }
}
