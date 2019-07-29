using PoorFff;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Sample
{
    /// <summary>
    /// 模块上下文
    /// </summary>
    public class AppSampleContext
    {
        public static readonly AppSampleContext Instance;
        static AppSampleContext()
        {
            Instance = new AppSampleContext();
        }

        public string Field1 { get; set; }
        public void Initialize(IPfConfiguration configuration)
        {
            Field1 = configuration.Get<string>("AppSampleContext.Field1", ()=>"");
        }
        private AppSampleContext() { }
    }
}
