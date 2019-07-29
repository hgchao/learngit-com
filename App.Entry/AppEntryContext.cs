using PoorFff;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Entry
{
    public class AppEntryContext
    {
        public static readonly AppEntryContext Instance;
        static AppEntryContext()
        {
            Instance = new AppEntryContext();
        }

        public int MainThreadId { get; set; }
        public string ConnectionString { get; set; }

        public void Initialize(IPfConfiguration configuration)
        {
            MainThreadId = configuration.Get<int>("AppEntryContext.MainThreadId");
            ConnectionString = configuration.Get<string>("AppEntryContext.ConnectionString");
        }
    }
}
