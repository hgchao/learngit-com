using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using App.Base.Repositories;
using App.Core.Authorization;
using App.Core.Authorization.Accounts;
using App.Core.Authorization.Repositories;
using App.Core.Authorization.Users;
using App.Core.Common;
using App.Core.Common.Entities;
using App.Core.Workflow;
using App.Projects.ProjectBaseInfos;
using App.Projects.ProjectBaseInfos.Dto;
using App.Quality.QualityProblemRectifications;
using App.Quality.QualityProblems;
using App.Safety.SafetyProblems;
using App.Statistics.ProjectStatistics.Dto;
using Microsoft.EntityFrameworkCore;
using Oa.Project;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using PoorFff.Mapper;

namespace App.Statistics.ProjectStatistics
{
    public class ProjectStatisticsService : IProjectStatisticsService
    {
        private IAuthInfoProvider _authInfoProvider;
        //private IHistoryProvider _historyProvider;
        //private IRuntimeProvider _runtimeProvider;
        //private ITaskProvider _taskProvider;
        //private IDefinitionProvider _definitionProvder;
        private IAuthorizationRepositoryBase<User> _userRepository;
        private IAuthorizationRepositoryBase<UserUnit> _userUnitRepository;
        //private IAppDbContextProvider _dbContextProvider;
        //private IMessagingProvider _messagingProvider;
        private IAppRepositoryBase<Project> _projectRepository;
        private IAppRepositoryBase<QualityProblem> _qualityProblemRepository;
        private IAppRepositoryBase<SafetyProblem> _safetyProblemRepository;
        //private IAppRepositoryBase<ProjectUnit> _projectUnitRepository;
        //private IProjectHelper _projectHelper;
        public ProjectStatisticsService(
            IAppRepositoryBase<Project> projectRepository,
            IAppRepositoryBase<QualityProblem> qualityProblemRepository,
            IAppRepositoryBase<SafetyProblem> safetyProblemRepository,
            //IAppRepositoryBase<ProjectUnit> projectUnitRepository,
            IAuthInfoProvider authInfoProvider,
            IWfEngine wfEngine,
            IAuthorizationRepositoryBase<User> userRepository,
            IAuthorizationRepositoryBase<UserUnit> userUnitRepository,
            //IAppDbContextProvider dbContextProvider,
            //IMessagingProvider messagingProvider,
            IProjectHelper projectHelper
            )
        {
            //_projectHelper = projectHelper;
            //_messagingProvider = messagingProvider;
            _authInfoProvider = authInfoProvider;
            _userRepository = userRepository;
            _userUnitRepository = userUnitRepository;
            //_runtimeProvider = wfEngine.GetRuntimeProvider();
            //_taskProvider = wfEngine.GetTaskProvider();
            //_definitionProvder = wfEngine.GetDefinitionProvider();
            //_historyProvider = wfEngine.GetHistoryProvider();
            //_dbContextProvider = dbContextProvider;
            _projectRepository = projectRepository;
            _qualityProblemRepository = qualityProblemRepository;
            _safetyProblemRepository = safetyProblemRepository;
            //_projectUnitRepository = projectUnitRepository;
        }

        public PaginationData<GetProjectStatisticsListOutput> Get(int pageIndex, int pageSize, string keyword, string sortField, string sortState)
        {
            //var qualityProblem = _problemRepository.Get()//质量问题
            //  .Where(u => u.State == DataState.Stable && u.RectificationState != RectificationState.Completed);

            //var safetyProblem = _safetyProblemRepository.Get()//安全问题
            //    .Where(u => u.State == DataState.Stable && u.RectificationState != App.Safety.SafetyProblemRectifications.RectificationState.Completed);

            var currentUserId = _authInfoProvider.GetCurrent().User.Id;
            var currentUnitUserIds = _userUnitRepository.Get()
                .Where(v => _userUnitRepository.Get().Where(u => u.UserId == currentUserId).Any(u => u.OrganizationUnitId == v.OrganizationUnitId)).Select(u => u.UserId).ToList();
            var privilegedPersonIds = AuthorizationContext.Instance.GetPrivilegedPersonIds("项目信息");

            IQueryable<Project> query = _projectRepository.Get()
                .Include(u => u.Location)
                .Include(u => u.State)
                .Include(u => u.Type)
                .Include(u => u.ProjectNature)
                .Include(u => u.Members)
                .Where(u => u.DataState == DataState.Stable
                && (u.Members.Any(v => v.UserId == currentUserId) || u.CreatorId == currentUserId || currentUnitUserIds.Contains(u.CreatorId) || privilegedPersonIds.Contains(currentUserId))
                );
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(u => u.Name.Contains(keyword) || u.No.Contains(keyword));
            }

            ////sortState 1降序2升序0默认
            //if (!string.IsNullOrEmpty(sortField) && !string.IsNullOrEmpty(sortState))
            //{
            //    switch (sortField)
            //    {
            //        case "No"://项目编号
            //            query = sortState == "1" ? query.OrderByDescending(u => u.No) : sortState == "2" ? query.OrderBy(u => u.No) : query.OrderByDescending(u => u.CreateTime);
            //            break;
            //        case "Name"://项目名称
            //            query = sortState == "1" ? query.OrderByDescending(u => u.Name) : sortState == "2" ? query.OrderBy(u => u.Name) : query.OrderByDescending(u => u.CreateTime);
            //            break;
            //        case "TypeId"://项目类型
            //            query = sortState == "1" ? query.OrderByDescending(u => u.TypeId) : sortState == "2" ? query.OrderBy(u => u.TypeId) : query.OrderByDescending(u => u.CreateTime);
            //            break;
            //        case "ProjectNatureId"://项目性质
            //            query = sortState == "1" ? query.OrderByDescending(u => u.ProjectNatureId) : sortState == "2" ? query.OrderBy(u => u.ProjectNatureId) : query.OrderByDescending(u => u.CreateTime);
            //            break;
            //        case "StateId"://项目状态
            //            query = sortState == "1" ? query.OrderByDescending(u => u.StateId) : sortState == "2" ? query.OrderBy(u => u.StateId) : query.OrderByDescending(u => u.CreateTime);
            //            break;
            //        case "GeneralEstimate"://项目概算
            //            query = sortState == "1" ? query.OrderByDescending(u => u.GeneralEstimate) : sortState == "2" ? query.OrderBy(u => u.GeneralEstimate) : query.OrderByDescending(u => u.CreateTime);
            //            break;
            //        case "CommencementDate"://开工时间
            //            query = sortState == "1" ? query.OrderByDescending(u => u.CommencementDate) : sortState == "2" ? query.OrderBy(u => u.CommencementDate) : query.OrderByDescending(u => u.CreateTime);
            //            break;
            //        case "CreatorId"://发布人
            //            query = sortState == "1" ? query.OrderByDescending(u => u.CreatorId) : sortState == "2" ? query.OrderBy(u => u.CreatorId) : query.OrderByDescending(u => u.CreateTime);
            //            break;

            //        default:
            //            query = query.OrderByDescending(u => u.CreateTime);
            //            break;
            //    }

            //}
            //else
            //{
            //    query = query.OrderByDescending(u => u.CreateTime);
            //}
            //var paging = PaginationDataHelper.WrapData<Project, T>(query, pageIndex, pageSize).TransferTo<GetProjectStatisticsListOutput>();
            ////paging.Data.ForEach(u=>u.HasPermission = _projectHelper.HasPermission("项目负责人", u.Id));
            //paging.Data.ForEach(u => {
            //    u.HasPermission = u.CreatorId == _authInfoProvider.GetCurrent().User.Id;
            //    u.QualityProblemCount = qualityProblem.Count(s =>  s.ProjectId == u.Id);
            //    u.SafetyProblemCount = safetyProblem.Count(s => s.ProjectId == u.Id);
            //});
            //paging = paging.Data.Where(w=>w.QualityProblemCount!=0);//&& u.SafetyProblemCount != 0
            //return paging;
            var queryForProject = query;
            var queryForQuality = _qualityProblemRepository.Get().Where(u => u.State == DataState.Stable && u.RectificationState != RectificationState.Completed);
            var queryForSafety = _safetyProblemRepository.Get().Where(u => u.State == DataState.Stable && u.RectificationState != App.Safety.SafetyProblemRectifications.RectificationState.Completed);
            var queryForProjectAndQuality = queryForProject.GroupJoin(queryForQuality, u => u.Id, v => v.ProjectId, (u, v) => new { Project = u, QualityProblems = v });
            var queryForProjectAndQualityAndSafety = queryForProjectAndQuality.GroupJoin(queryForSafety, u => u.Project.Id, v => v.ProjectId, (u, v) => new { Project = u.Project, QualityProblems = u.QualityProblems, SafetyProblems = v });
            queryForProjectAndQualityAndSafety = queryForProjectAndQualityAndSafety.Where(u => u.QualityProblems.Count() > 0 || u.SafetyProblems.Count() > 0);
            var paging = PaginationDataHelper.WrapData(queryForProjectAndQualityAndSafety, pageIndex, pageSize, u=>u.Project.Id).TransferTo<GetProjectStatisticsListOutput>(u => {
                var project = u.Project;
                var output = project.MapTo<GetProjectStatisticsListOutput>();
                output.QualityProblemCount = u.QualityProblems.Count();//质量问题个数
                output.SafetyProblemCount = u.SafetyProblems.Count();//安全问题个数
                return output;
            });
            return paging;
        }
        public PaginationData<GetProjectWithProblemListOutput> Get(int pageIndex, int pageSize, string keyword,
            string address,
            int? typeId,
            string Name,
            string No,
            int? projectNatureId,
            int? stateId,
            int? projectLeaderId,
            int? generalEstimateL, int? generalEstimateR,
            DateTime? commencementDateL, DateTime? commencementDateR,
            string sortField,
            string sortState
            )
        {
            var currentUserId = _authInfoProvider.GetCurrent().User.Id;
            var currentUnitUserIds = _userUnitRepository.Get()
                .Where(v => _userUnitRepository.Get().Where(u => u.UserId == currentUserId).Any(u => u.OrganizationUnitId == v.OrganizationUnitId)).Select(u => u.UserId).ToList();
            var privilegedPersonIds = AuthorizationContext.Instance.GetPrivilegedPersonIds("项目信息");

            IQueryable<Project> queryForProject = _projectRepository.Get()
                .Where(u => u.DataState == DataState.Stable
                && (u.Members.Any(v => v.UserId == currentUserId) || u.CreatorId == currentUserId || currentUnitUserIds.Contains(u.CreatorId) || privilegedPersonIds.Contains(currentUserId))
                );
            if (!string.IsNullOrEmpty(keyword))
            {
                queryForProject = queryForProject.Where(u => u.Name.Contains(keyword) || u.No.Contains(keyword));
            }
            if (!string.IsNullOrEmpty(address))
            {
                queryForProject = queryForProject.Where(u => (string.Concat(u.Location.Province, u.Location.City, u.Location.District, u.Location.Town, u.Location.Street, u.Location.AddressDetail).Contains(address)));
            }
            if (!string.IsNullOrEmpty(No))
            {
                queryForProject = queryForProject.Where(u => u.No.Contains(No));
            }
            if (!string.IsNullOrEmpty(Name))
            {
                queryForProject = queryForProject.Where(u => u.Name.Contains(Name));
            }
            if (stateId != null)
            {
                queryForProject = queryForProject.Where(u => u.StateId == stateId);
            }
            if (typeId != null)
            {
                queryForProject = queryForProject.Where(u => u.TypeId == typeId);
            }
            if (projectNatureId != null)
            {
                queryForProject = queryForProject.Where(u => u.ProjectNatureId == projectNatureId);
            }
            if (projectLeaderId != null)
            {
                queryForProject = queryForProject.Where(u => u.Members.Any(v => v.ProjectRole == "项目负责人" && v.UserId == projectLeaderId));
            }
            if (generalEstimateL != null)
            {
                queryForProject = queryForProject.Where(u => u.GeneralEstimate >= generalEstimateL);
            }
            if (generalEstimateR != null)
            {
                queryForProject = queryForProject.Where(u => u.GeneralEstimate <= generalEstimateR);
            }
            if (commencementDateL != null)
            {
                queryForProject = queryForProject.Where(u => u.CommencementDate >= commencementDateL);
            }
            if (commencementDateR != null)
            {
                queryForProject = queryForProject.Where(u => u.CommencementDate <= commencementDateR);
            }
            var queryForQuality = _qualityProblemRepository.Get().Where(u => u.State == DataState.Stable && u.RectificationState != RectificationState.Completed);
            var queryForSafety = _safetyProblemRepository.Get().Where(u => u.State == DataState.Stable && u.RectificationState != App.Safety.SafetyProblemRectifications.RectificationState.Completed);
            var queryForProjectAndQuality = queryForProject.GroupJoin(queryForQuality, u => u.Id, v => v.ProjectId, (u, v) => new { Project = new Project {
                Id = u.Id,
                No = u.No,
                Name = u.Name,
                Type = u.Type,
                ProjectNature = u.ProjectNature,
                State = u.State,
                GeneralEstimate = u.GeneralEstimate,
                Location = u.Location,
                Members = u.Members,
                CommencementDate=u.CommencementDate,
                CreatorId = u.CreatorId
            }, QualityProblemCount = v.Count() });
            var queryForProjectAndQualityAndSafety = queryForProjectAndQuality.GroupJoin(queryForSafety, u => u.Project.Id, v => v.ProjectId, (u, v) => new { Project = u.Project, QualityProblemCount = u.QualityProblemCount, SafetyProblemCount = v.Count() });
            //sortState 1降序2升序0默认
            if (!string.IsNullOrEmpty(sortField))
            {
                switch (sortField)
                {
                    case "No"://项目编号
                        queryForProjectAndQualityAndSafety = string.IsNullOrEmpty(sortState) ? queryForProjectAndQualityAndSafety.OrderByDescending(u => u.Project.No) : queryForProjectAndQualityAndSafety.OrderBy(u => u.Project.No);
                        break;
                    case "Name"://项目名称
                        queryForProjectAndQualityAndSafety = string.IsNullOrEmpty(sortState) ? queryForProjectAndQualityAndSafety.OrderByDescending(u => u.Project.Name) : queryForProjectAndQualityAndSafety.OrderBy(u => u.Project.Name);
                        break;
                    case "quality":
                        queryForProjectAndQualityAndSafety = string.IsNullOrEmpty(sortState) ? queryForProjectAndQualityAndSafety.OrderByDescending(u => u.QualityProblemCount) : queryForProjectAndQualityAndSafety.OrderBy(u => u.QualityProblemCount);
                        break;
                    case "safety":
                        queryForProjectAndQualityAndSafety = string.IsNullOrEmpty(sortState) ? queryForProjectAndQualityAndSafety.OrderByDescending(u => u.SafetyProblemCount) : queryForProjectAndQualityAndSafety.OrderBy(u => u.SafetyProblemCount);
                        break;
                    default:
                        break;
                }

            }
            var paging = PaginationDataHelper.WrapData<object, T>(queryForProjectAndQualityAndSafety, pageIndex, pageSize).TransferTo<GetProjectWithProblemListOutput>(u => {
                dynamic du = u;
                var output = ((object)(du.Project)).MapTo<GetProjectWithProblemListOutput>();
                output.UncompletedQualityProblemCount = du.QualityProblemCount;
                output.UncompletedSafetyProblemCount = du.SafetyProblemCount;
                return output;
            });
            paging.Data.ForEach(u=>u.HasPermission = u.CreatorId == _authInfoProvider.GetCurrent().User.Id);
            return paging;
        }
    }
}
