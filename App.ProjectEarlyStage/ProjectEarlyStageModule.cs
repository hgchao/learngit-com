using App.Core.Workflow;
using App.ProjectEarlyStage.EarlyStages;
using App.Projects;
using PoorFff.Modules;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.ProjectEarlyStage
{
    [DependsOn(typeof(ProjectModule))]
    public class ProjectEarlyStageModule:PfModuleBase
    {
        public override void Initialize()
        {
            IocManager.RegisterByConvention(GetAssembly());
            ProfileManager.AddProfile<ProjectEarlyStageProfile>();
        }
        public override void PostInitialize()
        {
            var wfEngine = IocManager.Resolve<IWfEngine>();
            wfEngine.GetRuntimeProvider().AddProcessCompletedEventHandler(process => {
                if (process.ProcessDefinition.FormType == "fixed")
                {
                    switch (process.ProcessDefinition.FormName)
                    {
                        case ProjectEarlyStageContext.FormNameOfProjectEarlyStage:
                            IocManager.Resolve<IEarlyStageService>().CompleteApproval(process.Id);
                            break;
                       

                    }
                }
            });
            
        }
    }
}
