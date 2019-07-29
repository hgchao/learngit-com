using PoorFff;
using PoorFff.PubSub;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.ProjectProgress
{
    public class ProjectProgressContext
    {
        public static ProjectProgressContext Instance;
        static ProjectProgressContext()
        {
            Instance = new ProjectProgressContext();
        }
        public const string WeeklyProgressFormName = "进度周报";
        public const string MonthlyProgressFormName = "投资信息";

        public string RedisConnectionString { get; set; }
        public IPubSub Sub => GetProvider();
        private ProjectProgressContext()
        {
        }

        public void Initialize(IPfConfiguration configuration)
        {
            RedisConnectionString = configuration.Get<string>("AppProjectProgressContext.RedisConnectionString");
        }

        private PubSubRedisProvider GetProvider()
        {
            return new PubSubRedisProvider(RedisConnectionString);
        }
    }
}
