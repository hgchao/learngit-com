using App.Base;
using PoorFff.Modules;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Sample
{
    [DependsOn(typeof(AppBaseModule))]
    public class AppSampleModule: PfModuleBase
    {
        public override void Initialize()
        {
            IocManager.RegisterByConvention(GetAssembly());
            ProfileManager.AddProfile<AppSampleProfile>();
            AppSampleContext.Instance.Initialize(Configuration);
        }

        public override void PostInitialize()
        {
        }
    }
}
