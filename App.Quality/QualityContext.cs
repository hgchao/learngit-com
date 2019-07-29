using System;
using System.Collections.Generic;
using System.Text;

namespace App.Quality
{
    public class QualityContext
    {
        public static QualityContext Instance;
        static QualityContext()
        {
            Instance = new QualityContext();
        }
        private QualityContext() { }
        public const string FormNameOfProjectQualityAccident = "质量事故";
        public const string FormNameOfProjectQualityProblem = "质量问题";
    }
}
