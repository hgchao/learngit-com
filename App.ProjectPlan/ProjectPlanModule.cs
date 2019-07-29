
using App.Projects;
using PoorFff.Modules;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.ProjectPlan
{
    [DependsOn(typeof(ProjectModule))]
    public class ProjectPlanModule: PfModuleBase
    {
        public override void Initialize()
        {
            IocManager.RegisterByConvention(GetAssembly());
            ProfileManager.AddProfile<ProjectPlanProfile>();
        }
    }
}
