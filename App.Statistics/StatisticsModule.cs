using App.Base;
using PoorFff.Modules;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Statistics
{
    [DependsOn(typeof(AppBaseModule))]
    public class StatisticsModule : PfModuleBase
    {
      
        public override void Initialize()
        {
            IocManager.RegisterByConvention(GetAssembly());
            ProfileManager.AddProfile<StatisticsProfile>();
        }

    }
}
