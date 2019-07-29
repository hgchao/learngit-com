using App.Projects.ProjectAttachments;
using App.Projects.ProjectBaseInfos;
using App.Projects.ProjectLocations;
using App.Projects.ProjectMembers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Projects.EntityFramework
{
    public interface IProjectDbContext
    {
        DbSet<Project> Projects { get; set; }
        DbSet<ProjectMember> ProjectMembers { get; set; }
        DbSet<ProjectAttachment> ProjectAttachments { get; set; }
        DbSet<ProjectLocation> ProjectLocations { get; set; }
    }
}
