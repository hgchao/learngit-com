using App.Calendars;
using App.Contract;
using App.Core.Authorization;
using App.Core.FileManagement;
using App.Funds;
using App.Housekeeping;
using App.Memorabilia;
using App.Problems;
using App.ProjectBriefings;
using App.ProjectEarlyStage;
using App.ProjectGantts;
using App.ProjectPlan;
using App.ProjectProgress;
using App.Projects;
using App.Quality;
using App.RecordMgmt;
using App.Safety;
using App.Statistics;
using App.Workflow;
using PoorFff.Modules;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Entry
{
    [DependsOn(typeof(AuthorizationModule),
        typeof(FileManagementModule),
        typeof(AppWorkflowModule),
        typeof(ProjectModule),
        typeof(ProjectGanttModule),
        typeof(MemorabiliaModule),
        typeof(ContractModule),
        typeof(QualityModule),
        typeof(SafetyModule),
        typeof(HousekeepingModule),
        typeof(ProjectPlanModule),
        typeof(RecordMgmtModule),
        typeof(ProjectEarlyStageModule),
        typeof(ProjectProgressModule),
        typeof(ProblemModule),
        typeof(ProblemModule),
        typeof(ProjectBriefingModule),
        typeof(FundsModule),
        typeof(StatisticsModule),
        typeof(CalendarModule)
        
        )]
    public class AppEntryModule: PfModuleBase
    {
        public override void Initialize()
        {
            IocManager.RegisterByConvention(GetAssembly());
            AppEntryContext.Instance.Initialize(Configuration);
        }

        public override void PostInitialize()
        {
        }
    }
}
