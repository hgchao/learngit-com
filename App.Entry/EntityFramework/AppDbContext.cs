using App.Calendars.Calendars;
using App.Calendars.EntityFramework;
using App.Contract.ConstructionUnits;
using App.Contract.ContractDeposits;
using App.Contract.Contracts;
using App.Contract.EntityFramework;
using App.Core.Authorization.Apis;
using App.Core.Authorization.EntityFramework;
using App.Core.Authorization.Functions;
using App.Core.Authorization.Menus;
using App.Core.Authorization.Modules;
using App.Core.Authorization.OrganizationUnits;
using App.Core.Authorization.PriviledgedPersons;
using App.Core.Authorization.Roles;
using App.Core.Authorization.Tenants;
using App.Core.Authorization.Users;
using App.Core.Common.EntityFramework;
using App.Core.FileManagement.AttachmentFileMetas;
using App.Core.FileManagement.Attachments;
using App.Core.FileManagement.EntityFramework;
using App.Core.FileManagement.Files;
using App.Core.FileManagement.PublicFiles;
using App.Core.Form.FieldDefinitions;
using App.Core.Form.Fields;
using App.Core.Form.FormDefinitions;
using App.Core.Form.FormModels;
using App.Core.Messaging.DelayMessages;
using App.Core.Messaging.EntityFramework;
using App.Core.Messaging.Messages;
using App.Core.Messaging.Subscriptions;
using App.Core.Parameter.Configs;
using App.Core.Parameter.EntityFramework;
using App.Core.Parameter.Selections;
using App.Core.Workflow.Activities;
using App.Core.Workflow.Comments;
using App.Core.Workflow.EntityFramework;
using App.Core.Workflow.Executions;
using App.Core.Workflow.ProcessDefinitions;
using App.Core.Workflow.ProcessInstances;
using App.Core.Workflow.Tasks;
using App.Core.Workflow.Variables;
using App.Funds.ContractPayments;
using App.Funds.EntityFramework;
using App.Housekeeping.EntityFramework;
using App.Housekeeping.HousekeepingProblemRectifications;
using App.Housekeeping.HousekeepingProblems;
using App.Housekeeping.Housekeepings;
using App.Memorabilia.EntityFramework;
using App.Memorabilia.MemorabiliaRecords;
using App.Problems.EntityFramework;
using App.Problems.ProblemCoordinations;
using App.Problems.Problems;
using App.ProjectBriefings.EntityFramework;
using App.ProjectBriefings.ProjectBriefings;
using App.ProjectEarlyStage.EarlyStages;
using App.ProjectEarlyStage.EntityFramework;
using App.ProjectGantts.EntityFramework;
using App.ProjectGantts.Gantts;
using App.ProjectGantts.Links;
using App.ProjectGantts.Tasks;
using App.ProjectPlan.AnnualPlans;
using App.ProjectPlan.EntityFramework;
using App.ProjectPlan.MonthlyPlans;
using App.ProjectProgress.EntityFramework;
using App.ProjectProgress.PmMonthlyProgresses;
using App.ProjectProgress.WeeklyProgresses;
using App.Projects.EntityFramework;
using App.Projects.ProjectAttachments;
using App.Projects.ProjectBaseInfos;
using App.Projects.ProjectLocations;
using App.Projects.ProjectMembers;
using App.Quality.EntityFramework;
using App.Quality.QualityAccidentDisposals;
using App.Quality.QualityAccidents;
using App.Quality.QualityProblemRectifications;
using App.Quality.QualityProblems;
using App.Quality.QualityStandards;
using App.RecordMgmt.EntityFramework;
using App.RecordMgmt.Records;
using App.Safety.EntityFramework;
using App.Safety.SafetyAccidentDisposals;
using App.Safety.SafetyAccidents;
using App.Safety.SafetyProblemProgresses;
using App.Safety.SafetyProblemRectifications;
using App.Safety.SafetyProblems;
using App.Safety.SafetyStandards;
using App.Sample.EntityFramework;
using App.Statistics.EntityFramework;
using App.Workflow.EntityFramework;
using App.Workflow.ProcessDefinitions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace App.Entry.EntityFramework
{
    public class AppDbContext : AppCoreDbContext,
        IAuthorizationDbContext,
        IParameterDbContext,
        IFileManagementDbContext,
        IMessagingDbContext,
        IWorkflowDbContext,
        IAppSampleDbContext,
        IAppWorkflowDbContext,
        IProjectDbContext,
        IMemorabiliaDbContext,
        IContractDbContext,
        IProjectGanttDbContext,
        IQualityDbContext,
        ISafetyDbContext,
        IHousekeepingDbContext,
        IProjectPlanDbContext,
        IRecordMgmtDbContext,
        IProjectEarlyStageDbContext,
        IProblemDbContext,
        IProjectProgressDbContext,
        IProjectBriefingDbContext,
        IFundsDbContext,
        IAppStatisticsDbContext,
        ICalendarDbContext
    {
        public DbSet<UserWeixin> UserWeixins { get;set;}
        public DbSet<UserQyWeixin> UserQyWeixins { get;set;}
        public DbSet<Tenant> Tenants { get;set;}
        public DbSet<Role> Roles { get;set;}
        public DbSet<Menu> Menus { get;set;}
        public DbSet<RoleModule> RoleModules { get;set;}
        public DbSet<RoleFunction> RoleFunctions { get;set;}
        public DbSet<Module> Modules { get;set;}
        public DbSet<ModuleApi> ModuleApis { get;set;}
        public DbSet<Function> Functions { get;set;}
        public DbSet<FunctionApi> FunctionApis { get;set;}
        public DbSet<Api> Apis { get;set;}
        public DbSet<OrganizationUnit> OrganizationUnits { get;set;}
        public DbSet<User> Users { get;set;}
        public DbSet<UserRole> UserRoles { get;set;}
        public DbSet<UserUnit> UserUnits { get;set;}
        public DbSet<MenuShortcut> MenuShortcuts { get;set;}
        public DbSet<FileMeta> FileMetas { get;set;}
        public DbSet<PublicFile> PublicFiles { get;set;}
        public DbSet<Attachment> Attachments { get;set;}
        public DbSet<AttachmentFileMeta> AttachmentFileMetas { get;set;}
        public DbSet<MessageRecord> MessageRecords { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<DelayMessage> DelayMessages { get; set; }
        public DbSet<EventAction> EventActions { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<RoleProcessDefinition> RoleProcessDefinitions { get; set; }
        public DbSet<Wf_Hi_Activity> Wf_Hi_Activities { get;set;}
        public DbSet<Wf_Ru_Execution> Wf_Ru_Executions { get;set;}
        public DbSet<Wf_Re_ProcessDefinition> Wf_Re_ProcessDefinitions { get;set;}
        public DbSet<Wf_Re_ProcessModel> Wf_Re_ProcessModels { get;set;}
        public DbSet<Wf_Hi_ProcessInstance> Wf_Hi_ProcessInstances { get;set;}
        public DbSet<Wf_Hi_TaskInstance> Wf_Hi_TaskInstances { get;set;}
        public DbSet<Wf_Ru_Task> Wf_Ru_Tasks { get;set;}
        public DbSet<Wf_Hi_Variable> Wf_Hi_Variables { get;set;}
        public DbSet<Wf_Ru_Variable> Wf_Ru_Variables { get;set;}
        public DbSet<Wf_Hi_Comment> Wf_Hi_Comments { get;set;}
        public DbSet<Field> Fields { get;set;}
        public DbSet<FieldDefinition> FieldDefinitions { get;set;}
        public DbSet<FormModel> FormModels { get;set;}
        public DbSet<FormDefinition> FormDefinitions { get;set;}
        public DbSet<PrivilegedPerson> PrivilegedPeople { get; set; }
        public DbSet<Config> Configs { get; set; }
        public DbSet<Selection> Selections { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<ProjectMember> ProjectMembers { get; set; }
        public DbSet<ProjectAttachment> ProjectAttachments { get; set; }
        public DbSet<ProjectLocation> ProjectLocations { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<MemorabiliaRecord> MemorabiliaRecords { get; set; }
        public DbSet<MemorabiliaAttachment> MemorabiliaAttachments { get; set; }
        public DbSet<ContractDeposit> ContractDeposits { get; set; }
        public DbSet<Contractt> Contracts { get; set; }
        public DbSet<ConstructionUnit> ConstructionUnits { get; set; }
        public DbSet<ContractAttachment> ContractAttachments { get; set; }
        public DbSet<ProjectGantt> ProjectGantts { get; set; }
        public DbSet<ProjectTask> ProjectTasks { get; set; }
        public DbSet<ProjectLink> ProjectLinks { get; set; }
        public DbSet<QualityStandard> QualityStandards { get; set; }
        public DbSet<QualityStandardAttachment> QualityStandardAttachments { get; set; }
        public DbSet<QualityProblem> QualityProblems { get; set; }
        public DbSet<QualityProblemAttachment> QualityProblemAttachments { get; set; }
        public DbSet<QualityCompletionAttachment> QualityCompletionAttachments { get; set; }
        public DbSet<QualityProblemRectification> QualityProblemRectifications { get; set; }
        public DbSet<QualityProblemRectificationAttachment> QualityProblemRectificationAttachments { get; set; }
        public DbSet<QualityAccident> QualityAccidents { get; set; }
        public DbSet<QualityAccidentAttachment> QualityAccidentAttachments { get; set; }
        public DbSet<QualitySettlementAttachment> QualitySettlementAttachments { get; set; }
        public DbSet<QualityAccidentDisposal> QualityAccidentDisposals { get; set; }
        public DbSet<QualityAccidentDisposalAttachment> QualityAccidentDisposalAttachments { get; set; }
        public DbSet<SafetyStandard> SafetyStandards { get; set; }
        public DbSet<SafetyStandardAttachment> SafetyStandardAttachments { get; set; }
        public DbSet<SafetyProblem> SafetyProblems { get; set; }
        public DbSet<SafetyProblemAttachment> SafetyProblemAttachments { get; set; }
        public DbSet<SafetyCompletionAttachment> SafetyCompletionAttachments { get; set; }
        public DbSet<SafetyProblemRectification> SafetyProblemRectifications { get; set; }
        public DbSet<SafetyProblemRectificationAttachment> SafetyProblemRectificationAttachments { get; set; }
        public DbSet<SafetyAccident> SafetyAccidents { get; set; }
        public DbSet<SafetyAccidentAttachment> SafetyAccidentAttachments { get; set; }
        public DbSet<SafetySettlementAttachment> SafetySettlementAttachments { get; set; }
        public DbSet<SafetyAccidentDisposal> SafetyAccidentDisposals { get; set; }
        public DbSet<SafetyAccidentDisposalAttachment> SafetyAccidentDisposalAttachments { get; set; }
        public DbSet<HousekeepingProblem> HousekeepingProblems { get; set; }
        public DbSet<HousekeepingProblemAttachment> HousekeepingProblemAttachments { get; set; }
        public DbSet<HousekeepingCompletionAttachment> HousekeepingCompletionAttachments { get; set; }
        public DbSet<HousekeepingProblemRectification> HousekeepingProblemRectifications { get; set; }
        public DbSet<HousekeepingProblemRectificationAttachment> HousekeepingProblemRectificationAttachments { get; set; }
        public DbSet<AnnualPlan> AnnualPlans { get; set; }
        public DbSet<MonthlyPlan> MonthlyPlans { get; set; }
        public DbSet<Record> Records { get; set; }
        public DbSet<RecordAttachment> RecordAttachments { get; set; }
        public DbSet<EarlyStage> EarlyStages { get; set; }
        public DbSet<EarlyStageAttachment> EarlyStageAttachments { get; set; }
        public DbSet<Problem> Problems { get; set; }
        public DbSet<ProblemAttachment> ProblemAttachments { get; set; }
        public DbSet<ProblemCoordination> ProblemCoordinations { get; set; }
        public DbSet<ProblemCoordinationAttachment> ProblemCoordinationAttachments { get; set; }
        public DbSet<WeeklyProgress> WeeklyProgresses { get; set; }
        public DbSet<ProjectBriefing> ProjectBriefings { get; set; }
        public DbSet<ContractPayment> ContractPayments { get; set; }
        public DbSet<Calendar> Calendars { get; set; }
        public DbSet<MonthlyProgress> PmMonthlyProgresses { get; set; }
        public DbSet<WeeklyProgressAttachment> WeeklyProgressAttachments { get; set; }

        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public AppDbContext() : base(GetDbContexOptions())
        {
        }

        public static DbContextOptions GetDbContexOptions()
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseMySql(AppEntryContext.Instance.ConnectionString);
            return optionsBuilder.Options;
        }

    }
}
