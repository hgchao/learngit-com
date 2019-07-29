using App.Core.Common.EntityFramework;
using App.Core.Workflow;
using App.Projects;
using App.Safety.SafetyAccidents;
using App.Safety.SafetyProblems;
using PoorFff.Modules;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Safety
{
    [DependsOn(typeof(ProjectModule))]
    public class SafetyModule: PfModuleBase
    {
        public override void Initialize()
        {
            IocManager.RegisterByConvention(GetAssembly());
            ProfileManager.AddProfile<SafetyProfile>();
        }

        public override void PostInitialize()
        {
            var wfEngine = IocManager.Resolve<IWfEngine>();
            wfEngine.GetRuntimeProvider().AddProcessCompletedEventHandler(process => {
                if (process.ProcessDefinition.FormType == "fixed")
                {
                    switch (process.ProcessDefinition.FormName)
                    {
                        case SafetyContext.FormNameOfProjectSafetyAccident:
                            IocManager.Resolve<ISafetyAccidentService>().CompleteApproval(process.Id);
                            break;
                        case SafetyContext.FormNameOfProjectSafetyProblem:
                            IocManager.Resolve<ISafetyProblemService>().CompleteApproval(process.Id);
                            break;

                    }
                }
            });
            //AppCoreDbContext.BuilderEvent += modelBuilder =>
            //{
            //    modelBuilder.Entity<SafetyProblem>()
            //        .HasOne(u => u.ProblemPhotoSet)
            //        .WithOne().HasForeignKey(typeof(PmAttach), "PSP_ProblemPhotoSetId");
            //    modelBuilder.Entity<SafetyProblem>()
            //        .HasOne(u => u.CompletionPhotoSet)
            //        .WithOne().HasForeignKey(typeof(PmAttach), "PSP_CompletionPhotoSetId");
            //    modelBuilder.Entity<SafetyProblemRectification>()
            //        .HasOne(u => u.RectificationPhotoSet)
            //        .WithOne().HasForeignKey(typeof(PmAttach), "PSPR_RectificationPhotoSetId");

            //    modelBuilder.Entity<SafetyAccident>()
            //        .HasOne(u => u.AccidentPhotoSet)
            //        .WithOne().HasForeignKey(typeof(PmAttach), "PSA_AccidentPhotoSetId");
            //    modelBuilder.Entity<SafetyAccident>()
            //        .HasOne(u => u.SettlementPhotoSet)
            //        .WithOne().HasForeignKey(typeof(PmAttach), "PSA_SettlementPhotoSetId");
            //    modelBuilder.Entity<SafetyAccidentDisposal>()
            //        .HasOne(u => u.DisposalPhotoSet)
            //        .WithOne().HasForeignKey(typeof(PmAttach), "PSAD_DisposalPhotoSetId");
            //};
        }
    }
}
