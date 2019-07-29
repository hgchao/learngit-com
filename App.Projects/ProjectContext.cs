using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace App.Projects
{
    public class Counter
    {
        public int Count { get; set; }
    }
    public class ProjectContext
    {
        public static ProjectContext Instance;
        static ProjectContext()
        {
            Instance = new ProjectContext();
        }

        public const string FormName = "项目信息";
        private ProjectContext() { }
        private ConcurrentDictionary<int, Counter> ProjectNoCount = new ConcurrentDictionary<int, Counter>();
        public int GetProjectBaseInfoNoCount(Func<int> GetCount)
        {
            var count = ProjectNoCount.GetOrAdd(DateTime.Now.Year, (key) => new Counter { Count = GetCount() });
            return ++count.Count;
        }

    }
}
