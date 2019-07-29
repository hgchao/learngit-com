using App.Core.Authorization;
using App.Core.Authorization.Accounts;
using PoorFff;
using PoorFff.Dependencies;
using PoorFff.PubSub;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.ProjectBriefings
{
    public class ProjectBriefingContext
    {
        public readonly static ProjectBriefingContext Instance;
        static ProjectBriefingContext()
        {
            Instance = new ProjectBriefingContext();
        }

        private ProjectBriefingContext()
        {
        }
    }
}
