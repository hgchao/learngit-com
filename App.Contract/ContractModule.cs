using App.Contract.ConstructionUnits;
using App.Contract.Contracts;
using App.Core.Workflow;

using App.Projects;
using PoorFff.Modules;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Contract
{

    [DependsOn(typeof(ProjectModule))]

    public class ContractModule : PfModuleBase
    {
        public override void Initialize()
        {
            IocManager.RegisterByConvention(GetAssembly());
            ProfileManager.AddProfile<ContractProfile>();
        }
        public override void PostInitialize()
        {

            var wfEngine = IocManager.Resolve<IWfEngine>();
            wfEngine.GetRuntimeProvider().AddProcessCompletedEventHandler(process => {
                if (process.ProcessDefinition.FormType == "fixed")
                {
                    switch (process.ProcessDefinition.FormName)
                    {
                        case ContractContext.FormNameOfProjectContract:  //合同
                            IocManager.Resolve<IContractService>().CompleteApproval(process.Id);
                            break;
                        case ContractContext.FormNameOfProjectConstructionUnit: //参建单位 
                            IocManager.Resolve<IConstructionUnitService>().CompleteApproval(process.Id);
                            break;

                    }
                }
            });
        }
    }
   
}

