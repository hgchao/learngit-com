using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;

namespace App.Web.LogImpl
{
    public class SerilogImpl : PoorFff.Logger.ILogger
    {
        public void Debug(string msg)
        {
            Log.Debug(msg);
        }

        public void Error(string msg)
        {
            Log.Error(msg);
        }

        public void Fatal(string msg)
        {
            Log.Fatal(msg);
        }

        public void Information(string msg)
        {
            Log.Information(msg);
        }

        public void Verbose(string msg)
        {
            Log.Verbose(msg);
        }

        public void Warning(string msg)
        {
            Log.Warning(msg);
        }
    }
}
