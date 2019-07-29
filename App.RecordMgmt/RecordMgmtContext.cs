using System;
using System.Collections.Generic;
using System.Text;

namespace App.RecordMgmt
{
    public class RecordMgmtContext
    {
        public static RecordMgmtContext Instance;
        static RecordMgmtContext()
        {
            Instance = new RecordMgmtContext();
        }

        public const string FormName = "档案信息";
        private RecordMgmtContext() { }
    }
}
