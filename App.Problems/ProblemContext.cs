using PoorFff;
using PoorFff.PubSub;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Problems
{
    public class ProblemContext
    {
        public static ProblemContext Instance;
        static ProblemContext()
        {
            Instance = new ProblemContext();
        }
        private ProblemContext()
        {
        }
        public const string FormNameOfProjectProblem = "存在问题";
    }
}
