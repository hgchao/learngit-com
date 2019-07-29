using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Web
{
    public class RedisInfo
    {
        public string ConnectionString { get; set; }
        public int AuthDbIndex { get; set; }
        public int MsDbIndex { get; set; }
        public int TimingDbIndex { get; set; }
    }
}
