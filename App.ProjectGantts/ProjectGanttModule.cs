using App.Base;
using App.Core.Authorization;
using App.Core.Common.EntityFramework;
using App.Core.Common.Operators;
using App.Core.FileManagement;
using App.Core.Messaging;
using App.Core.Workflow;
using App.ProjectGantts.Tasks;
using App.Projects;
using App.Projects.Projects;
using Newtonsoft.Json;
using PoorFff.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.ProjectGantts
{
    [DependsOn(typeof(ProjectModule))]
    public class ProjectGanttModule: PfModuleBase
    {
        public override void Initialize()
        {
            IocManager.RegisterByConvention(GetAssembly());
            ProfileManager.AddProfile<ProjectGanttProfile>();
        }

    }
}
