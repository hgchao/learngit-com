using App.Core.Authorization;
using App.Core.FileManagement;
using App.Base;
using PoorFff.Modules;
using System;
using System.Collections.Generic;
using System.Text;
using App.Projects;
using App.Core.Common.EntityFramework;
using App.RecordMgmt.Records;
using App.Core.Workflow;

namespace App.RecordMgmt
{
    [DependsOn(
        typeof(ProjectModule)
        )]
    public class RecordMgmtModule : PfModuleBase
    {
        public override void Initialize()
        {
            IocManager.RegisterByConvention(GetAssembly());
            ProfileManager.AddProfile<RecordMgmtProfile>();
        }

        public override void PostInitialize()
        {
            AppCoreDbContext.BuilderEvent += builder =>
            {
                builder.Entity<Record>().HasMany(u => u.Attachments).WithOne(u => u.Record).OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.Cascade);
            };

            var wfEngine = IocManager.Resolve<IWfEngine>();
            wfEngine.GetRuntimeProvider().AddProcessCompletedEventHandler(process =>
            {
                if (process.ProcessDefinition.FormType == "fixed")
                {
                    switch (process.ProcessDefinition.FormName)
                    {
                        case RecordMgmtContext.FormName:
                            IocManager.Resolve<IRecordService>().CompleteProcess(process.Id);
                            break;
                    }
                }
            });
        }
    }
}
