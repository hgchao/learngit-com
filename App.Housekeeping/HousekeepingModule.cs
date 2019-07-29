
using App.Core.Workflow;
using App.Housekeeping.Housekeepings;
using App.Projects;
using PoorFff.Modules;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Housekeeping
{
    [DependsOn(typeof(ProjectModule))]
    public class HousekeepingModule: PfModuleBase
    {
        public override void Initialize()
        {
            IocManager.RegisterByConvention(GetAssembly());
            ProfileManager.AddProfile<HousekeepingProfile>();
        }

        public override void PostInitialize()
        {
            var wfEngine = IocManager.Resolve<IWfEngine>();
            wfEngine.GetRuntimeProvider().AddProcessCompletedEventHandler(process => {
                if (process.ProcessDefinition.FormType == "fixed")
                {
                    switch (process.ProcessDefinition.FormName)
                    {
                        case HousekeepingContext.FormNameOfProjectHousekeeping:
                            IocManager.Resolve<IHousekeepingProblemService>().CompleteApproval(process.Id);
                            break;
                       

                    }
                }
            });
            //AppCoreDbContext.BuilderEvent += modelBuilder =>
            //{
            //    modelBuilder.Entity<HousekeepingProblem>()
            //        .HasOne<PmAttach>(u => u.CompletionPhotoSet)
            //        .WithOne().HasForeignKey(typeof(PmAttach), "PHP_CompletionPhotoSetId");
            //    modelBuilder.Entity<HousekeepingProblem>()
            //        .HasOne<PmAttach>(u => u.ProblemPhotoSet)
            //        .WithOne().HasForeignKey(typeof(PmAttach), "PHP_ProblemPhotoSetId");
            //    modelBuilder.Entity<HousekeepingProblemRectification>()
            //        .HasOne<PmAttach>(u => u.RectificationPhotoSet)
            //        .WithOne().HasForeignKey(typeof(PmAttach), "PHPR_RectificationPhotoSetId");
            //};
        }
    }
}
