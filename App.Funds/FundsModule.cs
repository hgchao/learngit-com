
using App.Contract;
using App.Core.Workflow;
using App.Funds.ContractPayments;
using PoorFff.Modules;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Funds
{
    [DependsOn(typeof(ContractModule))]
    public class FundsModule : PfModuleBase
    {
        public override void Initialize()
        {
            IocManager.RegisterByConvention(GetAssembly());
            ProfileManager.AddProfile<FundsProfile>();
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
                        case FundsContext.FormNameOfProjectContractPayment:
                            IocManager.Resolve<IContractPaymentService>().CompleteApproval(process.Id);
                            break;
                        

                    }
                }
            });
        }
    }
}
