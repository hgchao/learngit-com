using App.Core.Common.EntityFramework;
using App.Core.Parameter;
using App.Core.Workflow;
using App.Projects;
using App.Quality.QualityAccidentDisposals;
using App.Quality.QualityAccidents;
using App.Quality.QualityProblems;

using PoorFff.Modules;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Quality
{
    [DependsOn(
        typeof(ParameterModule),
        typeof(ProjectModule)
        )]
    public class QualityModule: PfModuleBase
    {
        public override void Initialize()
        {
            IocManager.RegisterByConvention(GetAssembly());
            ProfileManager.AddProfile<QualityProfile>();
        }

        public override void PostInitialize()
        {
            var wfEngine = IocManager.Resolve<IWfEngine>();
            wfEngine.GetRuntimeProvider().AddProcessCompletedEventHandler(process => {
                if (process.ProcessDefinition.FormType == "fixed")
                {
                    switch (process.ProcessDefinition.FormName)
                    {
                        case QualityContext.FormNameOfProjectQualityAccident:
                            IocManager.Resolve<IQualityAccidentService>().CompleteApproval(process.Id);
                            break;
                        case QualityContext.FormNameOfProjectQualityProblem:
                            IocManager.Resolve<IQualityProblemService>().CompleteApproval(process.Id);
                            break;

                    }
                }
            });
            //AppCoreDbContext.BuilderEvent += modelBuilder =>
            //{
            //    modelBuilder.Entity<QualityProblem>()
            //        .HasOne(u => u.ProblemPhotoSet)
            //        .WithOne().HasForeignKey(typeof(PmAttach), "PQP_ProblemPhotoSetId");
            //    modelBuilder.Entity<QualityProblem>()
            //        .HasOne(u => u.CompletionPhotoSet)
            //        .WithOne().HasForeignKey(typeof(PmAttach), "PQP_CompletionPhotoSetId");
            //    modelBuilder.Entity<QualityProblemRectification>()
            //        .HasOne(u => u.RectificationPhotoSet)
            //        .WithOne().HasForeignKey(typeof(PmAttach), "PQPR_RectificationPhotoSetId");

            //    modelBuilder.Entity<QualityAccident>()
            //        .HasOne(u => u.AccidentPhotoSet)
            //        .WithOne().HasForeignKey(typeof(PmAttach), "PQA_AccidentPhotoSetId");
            //    modelBuilder.Entity<QualityAccident>()
            //        .HasOne(u => u.SettlementPhotoSet)
            //        .WithOne().HasForeignKey(typeof(PmAttach), "PQA_SettlementPhotoSetId");
            //    modelBuilder.Entity<QualityAccidentDisposal>()
            //        .HasOne(u => u.DisposalPhotoSet)
            //        .WithOne().HasForeignKey(typeof(PmAttach), "PQAD_DisposalPhotoSetId");
            //};
        }
    }
}
