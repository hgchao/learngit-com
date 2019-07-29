using Microsoft.EntityFrameworkCore;
using App.Workflow.ProcessDefinitions;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Workflow.EntityFramework
{
    public interface IAppWorkflowDbContext
    {
        DbSet<RoleProcessDefinition> RoleProcessDefinitions { get; set; }
    }
}
