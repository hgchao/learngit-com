using System;
using System.Collections.Generic;
using System.Text;

namespace App.ProjectEarlyStage
{
  public  class ProjectEarlyStageContext
    {
        public static ProjectEarlyStageContext Instance;
        static ProjectEarlyStageContext()
        {
            Instance = new ProjectEarlyStageContext();
        }
        private ProjectEarlyStageContext() { }
        public const string FormNameOfProjectEarlyStage = "项目流程";
    }
}
