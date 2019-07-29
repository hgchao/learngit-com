using App.Base;
using App.Contract;
using App.Core.Authorization.Accounts;
using App.Core.Common.Operators;
using App.Housekeeping;
using App.Problems;
using App.ProjectBriefings;
using App.ProjectBriefings;
using App.ProjectBriefings.ProjectBriefings;
using App.ProjectProgress;
using App.Projects;
using App.Quality;
using App.Safety;
using Newtonsoft.Json;
using PoorFff.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.ProjectBriefings
{
    [DependsOn(
        typeof(ContractModule),
        typeof(ProblemModule),
        typeof(ProjectModule),
        typeof(HousekeepingModule),
        typeof(QualityModule),
        typeof(SafetyModule),
        typeof(ProjectProgressModule)
        )]
    public class ProjectBriefingModule: PfModuleBase
    {
        public override void Initialize()
        {
            IocManager.RegisterByConvention(GetAssembly());
            ProfileManager.AddProfile<ProjectBriefingProfile>();
        }
        public override void PostInitialize()
        {
            AppBaseContext.Instance.Consume<string>("add-project-briefing", (channel, msg)=> {
                dynamic obj = JsonConvert.DeserializeObject<dynamic>(msg);
                var op = IocManager.Resolve<IDbOperator>();
                IocManager.Resolve<IProjectBriefingService>().AddNew((int)obj.tenantId, (int)obj.projectId);
            });
        }
    }
}
