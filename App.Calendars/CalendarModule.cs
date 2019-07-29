using App.Projects;
using PoorFff.Modules;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Calendars
{
    [DependsOn(typeof(ProjectModule))]
    public class CalendarModule : PfModuleBase
    {
        public override void Initialize()
        {
            IocManager.RegisterByConvention(GetAssembly());
            ProfileManager.AddProfile<CalendarProfile>();
        }
    }
}
