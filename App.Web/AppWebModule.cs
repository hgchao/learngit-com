using App.Core.Messaging.Qywx;
using App.Core.Messaging.WebSocket;
using App.Core.Web;
using App.Entry;
using App.Web.FileUrl;
using PoorFff.Modules;
using System;
using System.Collections.Generic;
using System.Text;
using PoorFff.Logger;
using App.Web.LogImpl;

namespace App.Web
{
    [DependsOn(
    typeof(MessagingWebSocketModule),
    typeof(MessagingQywxModule),
    typeof(WebModule),
    typeof(AppEntryModule)
    )]
    public class AppWebModule: PfModuleBase
    {
        public override void Initialize()
        {
            IocManager.RegisterByConvention(GetAssembly());
            IocManager.Register<IFileUrlBuilder, FileUrlBuilder>();
            IocManager.Register<ILogger, SerilogImpl>();
            AppWebContext.Instance.Initialize(Configuration);
        }

        public override void PostInitialize()
        {
        }

    }
}
