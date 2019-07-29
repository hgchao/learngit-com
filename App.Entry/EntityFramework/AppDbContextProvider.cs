using App.Base.EntityFramework;
using App.Core.Authorization.EntityFramework;
using App.Core.Common.EntityFramework;
using App.Core.Common.Operators;
using App.Core.FileManagement.EntityFramework;
using App.Core.Messaging.EntityFramework;
using App.Core.Parameter.EntityFramework;
using App.Core.Workflow.EntityFramework;
using Microsoft.EntityFrameworkCore;
using PoorFff.Session;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace App.Entry.EntityFramework
{
    public class AppDbContextProvider : AppCoreDbContextProvider,
        IAuthorizationDbContextProvider,
        IParameterDbContextProvider,
        IFileManagementDbContextProvider,
        IWorkflowDbContextProvider,
        IMessagingDbContextProvider,
        IAppDbContextProvider
    {

        private IDbOperator _op;
        private IPfSession _session;
        public AppDbContextProvider(IDbOperator op, IPfSession session)
        {
            _op = op;
            _session = session;
        }


        public override AppCoreDbContext GetDbContext()
        {

            var currentThreadId = Thread.CurrentThread.ManagedThreadId;
            var dbContext = _session.Get2<AppDbContext>("AppEntryModule.DbContext");
            if (dbContext == null)
            {
                dbContext = new AppDbContext();
                dbContext.Operator = _op;
                _session.Set2("AppEntryModule.DbContext", dbContext);
            }
            return dbContext;
        }

    }
}
