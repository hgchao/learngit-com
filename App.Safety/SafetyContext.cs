using System;
using System.Collections.Generic;
using System.Text;

namespace App.Safety
{
   public class SafetyContext
    {
        public static SafetyContext Instance;
        static SafetyContext()
        {
            Instance = new SafetyContext();
        }
        private SafetyContext() { }
        public const string FormNameOfProjectSafetyAccident = "安全事故";
        public const string FormNameOfProjectSafetyProblem = "安全问题";
    }
}
