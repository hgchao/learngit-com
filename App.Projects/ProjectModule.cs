using App.Base;
using App.Core.Authorization;
using App.Core.FileManagement;
using App.Core.Messaging;
using App.Core.Workflow;
using App.Projects.ProjectBaseInfos;
using App.Projects.Projects;
using PoorFff.Modules;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Projects
{
    [DependsOn(typeof(AppBaseModule))]
    public class ProjectModule: PfModuleBase
    {
        public override void Initialize()
        {
            IocManager.RegisterByConvention(GetAssembly());
            IocManager.Register<IProjectHelper, ProjectHelper>();
            ProfileManager.AddProfile<ProjectProfile>();
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
                        case ProjectContext.FormName:
                            IocManager.Resolve<IProjectService>().CompleteProcess(process.Id);
                            break;
                    }
                }
            });

        }
    }
}
