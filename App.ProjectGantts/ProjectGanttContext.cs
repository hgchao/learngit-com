using PoorFff;
using PoorFff.Cache;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.ProjectGantts
{
    public class ProjectGanttContext
    {
        public static ProjectGanttContext Instance;
        static ProjectGanttContext()
        {
            Instance = new ProjectGanttContext();
        }
        private ProjectGanttContext() { }
    }
}
