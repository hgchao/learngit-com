using App.Core.Authorization;
using App.Core.Common;
using App.Core.Messaging;
using App.Core.Parameter;
using App.Core.Workflow;
using PoorFff.Modules;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Base
{
    [DependsOn(
        typeof(AppCoreModule),
        typeof(AuthorizationModule),
        typeof(WorkflowModule),
        typeof(ParameterModule),
        typeof(MessagingModule))]
    public class AppBaseModule: PfModuleBase
    {
        public override void Initialize()
        {
            IocManager.RegisterByConvention(GetAssembly());
            AppBaseContext.Instance.Initialize(Configuration);
        }

        public override void PostInitialize()
        {
        }
    }
}
