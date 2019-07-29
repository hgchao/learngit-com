using App.Core.Common.EntityFramework;
using App.Core.Workflow;
using App.Problems.Problems;
using App.Projects;
using PoorFff.Modules;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Problems
{
    [DependsOn(typeof(ProjectModule))]
    public class ProblemModule: PfModuleBase
    {
        public override void Initialize()
        {
            IocManager.RegisterByConvention(GetAssembly());
            ProfileManager.AddProfile<ProblemProfile>();
        }
        public override void PostInitialize()
        {
            var wfEngine = IocManager.Resolve<IWfEngine>();
            wfEngine.GetRuntimeProvider().AddProcessCompletedEventHandler(process => {
                if (process.ProcessDefinition.FormType == "fixed")
                {
                    switch (process.ProcessDefinition.FormName)
                    {
                        case ProblemContext.FormNameOfProjectProblem:
                            IocManager.Resolve<IProblemService>().CompleteApproval(process.Id);
                            break;
                      
                    }
                }
            });
            //AppCoreDbContext.BuilderEvent += modelBuilder =>
            //{
            //    modelBuilder.Entity<Problem>()
            //        .HasOne(u => u.ProblemPhotoSet)
            //        .WithOne().HasForeignKey(typeof(PmAttach), "PP_ProblemPhtotoSetId");

            //    modelBuilder.Entity<ProblemCoordination>()
            //        .HasOne(u => u.CoordinationPhotoSet)
            //        .WithOne().HasForeignKey(typeof(PmAttach), "PP_CoordinationPhtotoSetId");
            //};
        }
    }
}
