using App.Projects;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace App.Contract
{
    public class ContractContext
    {
        public readonly static ContractContext Instance;
        static ContractContext()
        {
            Instance = new ContractContext();
        }

        public string FileCategory { get; }
        private ContractContext()
        {
            FileCategory = "合同";
        }
        private ConcurrentDictionary<int, Counter> ProjectNoCount = new ConcurrentDictionary<int, Counter>();
        public int GetContractNoCount(Func<int> GetCount)
        {
            var count = ProjectNoCount.GetOrAdd(DateTime.Now.Year, (key) => new Counter { Count = GetCount() });
            return ++count.Count;
        }
        public const string FormNameOfProjectConstructionUnit = "参建单位";
        public const string FormNameOfProjectContract = "合同信息";
    }
}
