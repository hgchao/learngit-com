using App.Core.Workflow;
using App.ProjectPlan;
using App.ProjectProgress.PmMonthlyProgresses;
using App.ProjectProgress.WeeklyProgresses;
using App.Projects;
using PoorFff.Modules;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.ProjectProgress
{
    [DependsOn(
        typeof(ProjectModule),
        typeof(ProjectPlanModule)
        )]
    public class ProjectProgressModule: PfModuleBase
    {
        public override void Initialize()
        {
            IocManager.RegisterByConvention(GetAssembly());
            ProfileManager.AddProfile<ProjectProgressProfile>();
            ProjectProgressContext.Instance.Initialize(Configuration);
        }

        public override void PostInitialize()
        {
            var wfEngine = IocManager.Resolve<IWfEngine>();
            wfEngine.GetRuntimeProvider().AddProcessCompletedEventHandler(process =>
            {
                if (process.ProcessDefinition.FormType == "fixed")
                {
                    switch (process.ProcessDefinition.FormName)
                    {
                        case ProjectProgressContext.WeeklyProgressFormName:
                            IocManager.Resolve<IWeeklyProgressService>().CompleteProcess(process.Id);
                            break;
                        case ProjectProgressContext.MonthlyProgressFormName:
                            IocManager.Resolve<IMonthlyProgressService>().CompleteProcess(process.Id);
                            break;
                    }
                }
            });
        }
    }
}
