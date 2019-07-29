
using App.Core.Workflow;
using App.Memorabilia.MemorabiliaRecords;
using App.Projects;
using PoorFff.Modules;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Memorabilia
{
    [DependsOn(typeof(ProjectModule))]
    public class MemorabiliaModule: PfModuleBase
    {
        public override void Initialize()
        {
            IocManager.RegisterByConvention(GetAssembly());
            ProfileManager.AddProfile<MemorabiliaProfile>();
        }
        public override void PostInitialize()
        {

       

            var wfEngine = IocManager.Resolve<IWfEngine>();
            wfEngine.GetRuntimeProvider().AddProcessCompletedEventHandler(process => {
                if (process.ProcessDefinition.FormType == "fixed")
                {
                    switch (process.ProcessDefinition.FormName)
                    {
                        case MemorabiliaContext.FormNameOfProjectMemorabilia:
                            IocManager.Resolve<IMemorabiliaRecordService>().CompleteApproval(process.Id);
                            break;
                        

                    }
                }
            });
        }
    }
}
