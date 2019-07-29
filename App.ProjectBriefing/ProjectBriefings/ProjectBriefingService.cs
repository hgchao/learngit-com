using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using App.Base.Repositories;
using App.Contract.ConstructionUnits.Dto;
using App.Contract.Contracts;
using App.Core.Common.EntityFramework;
using App.Core.Common.Exceptions;
using App.Housekeeping.Housekeepings;
using App.Problems.ProblemRectifications;
using App.Problems.Problems;
using App.ProjectBriefings.ProjectBriefings.Dto;
using App.ProjectProgress.WeeklyProgresses;
using App.Projects.ProjectBaseInfos;
using App.Quality.QualityAccidents;
using App.Quality.QualityProblemRectifications;
using App.Quality.QualityProblems;
using App.Safety.SafetyAccidents;
using App.Safety.SafetyProblems;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Oa.Project;
using PoorFff.Enums;
using PoorFff.Mapper;

namespace App.ProjectBriefings.ProjectBriefings
{
    public class ProjectBriefingService : IProjectBriefingService
    {
        private IAppRepositoryBase<ProjectBriefing> _briefingRepository;
        private IAppRepositoryBase<Project> _projectRepository;
        private IAppRepositoryBase<Contractt> _contractRepository;
        private IAppRepositoryBase<WeeklyProgress> _progressRepository;
        private IAppRepositoryBase<Problem> _problemRepository;
        private IAppRepositoryBase<QualityProblem> _qualityProblemRepository;
        private IAppRepositoryBase<QualityAccident> _qualityAccidentRepository;
        private IAppRepositoryBase<SafetyProblem> _safetyProblemRepository;
        private IAppRepositoryBase<SafetyAccident> _safetyAccidentRepository;
        private IAppRepositoryBase<HousekeepingProblem> _housekeepingRepository;
        public ProjectBriefingService(
            IAppRepositoryBase<ProjectBriefing> briefingRepository,
            IAppRepositoryBase<Project> projectRepository,
            IAppRepositoryBase<Contractt> contractRepository,
            IAppRepositoryBase<WeeklyProgress> progressRepository,
            IAppRepositoryBase<Problem> problemRepository,
            IAppRepositoryBase<QualityProblem> qualityProblemRepository,
             IAppRepositoryBase<QualityAccident> qualityAccidentRepository,
             IAppRepositoryBase<SafetyProblem> safetyProblemRepository,
              IAppRepositoryBase<SafetyAccident> safetyAccidentRepository,
             IAppRepositoryBase<HousekeepingProblem> housekeepingRepository
            )
        {
            _briefingRepository = briefingRepository;
            _projectRepository = projectRepository;
            _contractRepository = contractRepository;
            _progressRepository = progressRepository;
            _problemRepository = problemRepository;
            _qualityProblemRepository = qualityProblemRepository;
            _qualityAccidentRepository = qualityAccidentRepository;
            _safetyProblemRepository = safetyProblemRepository;
            _safetyAccidentRepository = safetyAccidentRepository;
            _housekeepingRepository = housekeepingRepository;
        }
        public int AddNew(int tenantId, int projectId)
        {
            if (projectId == 0)
            {
                throw new EntityException("Id", projectId, "Project", "不存在");
            }
            var count = _briefingRepository.Count(u => u.ProjectId == projectId);
            ProjectBriefing briefing = new ProjectBriefing();
            var weeklyProgress = _progressRepository.Get().Where(u => u.ProjectId == projectId && u.DataState == DataState.Stable).OrderByDescending(u => u.CreateTime).FirstOrDefault();
            if (weeklyProgress != null)
            {
                briefing.CumulativeImageProgress = weeklyProgress.AccumulatedImageProgress;
                briefing.NextWeekProgressPlan = weeklyProgress.NextMonthPlannedImageProgress;
                briefing.Supervision = weeklyProgress.Supervision;
                briefing.ThisWeekProgress = weeklyProgress.ImageProgress;
                briefing.Information = weeklyProgress.Information;
                briefing.ProgressLimitDate = $"{weeklyProgress.AddDate}";
            }
            //存在问题统计
            var problems = _problemRepository.Get()
                .Where(u => u.ProjectId == projectId && u.State == DataState.Stable && u.CoordinationState != CoordinationState.Completed)
                .OrderBy(u => u.CreateTime).ToList(); 
            var list = problems.Select(u => new { problem = u.Content, Solution = u.ProposalSolution }).ToList();
            briefing.ProblemAndSolution = JsonConvert.SerializeObject(list);
            //质量问题统计
            var qualityProblem = _qualityProblemRepository.Get()
                 .Include(u => u.Rectifications)
                .Where(u => u.ProjectId == projectId && u.State == DataState.Stable)// && u.RectificationState != RectificationState.Completed
                .OrderBy(u => u.CreateTime).ToList();
            var listProblem = qualityProblem.Select(u => new { QualityId = u.Id, Source = u.Source, RectificationState=u.RectificationState.GetDesc(), Description = u.Description, Scheme=u.Rectifications.Count==0?null: u.Rectifications.Select(s=>s.Description).ToList() }).ToList();
            briefing.QualitySourceAndDescription = JsonConvert.SerializeObject(listProblem);
            //质量事故统计
            var qualityAccident = _qualityAccidentRepository.Get()
                 .Include(u => u.Disposals)
                .Where(u => u.ProjectId == projectId && u.State == DataState.Stable)// && u.RectificationState != RectificationState.Completed
                .OrderBy(u => u.CreateTime).ToList();
            var listAccident = qualityAccident.Select(u => new { AccidentId = u.Id, Title = u.Title, DisposalState = u.DisposalState.GetDesc(), Content = u.Content, Scheme = u.Disposals.Count == 0 ? null : u.Disposals.Select(s => s.Plan).ToList() }).ToList();
            briefing.QualityAccidentAndDescription = JsonConvert.SerializeObject(listAccident);
            //安全问题统计
            var safetyProblem = _safetyProblemRepository.Get()
                .Include(u=>u.Source)
                .Include(u=>u.Rectifications)
                .Where(u => u.ProjectId == projectId && u.State == DataState.Stable)// && u.RectificationState != Safety.SafetyProblemRectifications.RectificationState.Completed
                .OrderBy(u => u.CreateTime).ToList();
            var listSafety = safetyProblem.Select(u => new { SafetyId = u.Id, Sources = u.Source?.Name, RectificationStates = u.RectificationState.GetDesc(), Descriptions = u.Description, Scheme = u.Rectifications.Count == 0 ? null : u.Rectifications.Select(s => s.Description).ToList() }).ToList();
            briefing.SafetySourceAndDescription = JsonConvert.SerializeObject(listSafety);
            //安全事故统计
            var safetyAccident = _safetyAccidentRepository.Get()
                .Include(u => u.Source)
                 .Include(u => u.Disposals)
                .Where(u => u.ProjectId == projectId && u.State== DataState.Stable)// && u.RectificationState != Safety.SafetyProblemRectifications.RectificationState.Completed
                .OrderBy(u => u.CreateTime).ToList();
            var listSafetyAccident = safetyAccident.Select(u => new { SafetyAccidentId = u.Id, Titles = u.Title, DisposalStates = u.DisposalState.GetDesc(), Contents = u.Content, Scheme = u.Disposals.Count == 0 ? null : u.Disposals.Select(s => s.Solution).ToList() }).ToList();
            briefing.SafetyAccidentAndDescription = JsonConvert.SerializeObject(listSafetyAccident);
            //文明施工问题统计

            var housekeeping = _housekeepingRepository.Get()
                .Where(u => u.ProjectId == projectId && u.State == DataState.Stable && u.RectificationState != Housekeeping.HousekeepingProblemRectifications.RectificationState.Completed)
                .OrderBy(u => u.CreateTime).ToList();
            var listHousekeeping = housekeeping.Select(u => new { Content = u.Content }).ToList();
            briefing.HousekeepingConetent = JsonConvert.SerializeObject(listHousekeeping);
            briefing.Version = ++count;
            briefing.ProjectId = projectId;
            briefing.TenantId = tenantId;
            _briefingRepository.Add(briefing);
            return briefing.Id;
        }

        public GetProjectBriefingOutput GetLatest(int projectId)
        {
            var briefing = _briefingRepository.Get().Where(u => u.ProjectId == projectId).OrderByDescending(u => u.Version).FirstOrDefault();
            if (briefing == null)
                return null;
            var output = briefing.MapTo<GetProjectBriefingOutput>();
            output.Project = GetProject(projectId);
            return output;
        }

        private GetProjectForBriefingOutput GetProject(int projectId)
        {
            Project project = _projectRepository.Get().Where(u => u.Id == projectId)
                .Include(u => u.Attachments).ThenInclude(w => w.FileMeta)
                .FirstOrDefault();
            if (project == null)
            {
                throw new EntityException("Id", projectId, "Project", "不存在");
            }
            var constructionUnits = _contractRepository.Get().Where(u => u.ProjectId == projectId).Include(u => u.Contractor).Select(u => u.Contractor).Distinct().ToList();
            var output = project.MapTo<GetProjectForBriefingOutput>();
            output.ConstructionUnits = constructionUnits.MapToList<GetConstructionUnitListOutput>();
            return output;
        }
    }
}
