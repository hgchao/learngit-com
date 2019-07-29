using App.Web.ApiExplorerService;
using PoorFff;
using PoorFff.Cache;
using PoorFff.Cache.Memory;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace App.Web
{
    public class AppWebContext
    {
        public static AppWebContext Instance;
        static AppWebContext()
        {
            Instance = new AppWebContext();
        }

        private AppWebContext()
        {
        }

        public int SystemIndex { get; set; }
        public string Domain { get; set; }
        public ICache Cache => GetCacheProvider();
        public bool EnabledSuperAdmin { get; set; }
        public Dictionary<string, string> Comments { get; set; }

        public void Initialize(IPfConfiguration configuration)
        {
            SystemIndex = configuration.Get<int>("AppWebContext.SystemIndex");
            Domain = configuration.Get<string>("AppWebContext.Domain");
            EnabledSuperAdmin = configuration.Get<bool>("AppWebContext.EnabledSuperAdmin");
            var xmlCommentTool = new XmlCommentTool(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
            Comments = xmlCommentTool.GetPropertySummaries();
        }

        public MemoryCacheProvider GetCacheProvider()
        {
            return new MemoryCacheProvider();
        }

    }
}
