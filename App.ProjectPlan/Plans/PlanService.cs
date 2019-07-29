using App.Base.Repositories;
using App.Core.Authorization;
using App.Core.Authorization.Accounts;
using App.Core.Authorization.Repositories;
using App.Core.Authorization.Users;
using App.Core.Common;
using App.Core.Common.Entities;
using App.ProjectPlan.AnnualPlans;
using App.ProjectPlan.MonthlyPlans;
using App.ProjectPlan.Plans;
using App.Projects.ProjectBaseInfos;
using Microsoft.EntityFrameworkCore;
using Oa.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.ProjectPlan.Plans
{
    public class PlanService: IPlanService
    {
        private IAuthInfoProvider _authInfoProvider;
        private IAuthorizationRepositoryBase<UserUnit> _userUnitRepository;
        private IAppRepositoryBase<Project> _projectRepository;
        private IAppRepositoryBase<AnnualPlan> _annualPlanRepository;
        private IAppRepositoryBase<MonthlyPlan> _monthlyPlanRepository;
        private IProjectHelper _projectHelper;
        public PlanService(
            IAuthInfoProvider authInfoProvider,
            IAuthorizationRepositoryBase<UserUnit> userUnitRepository,
            IAppRepositoryBase<Project> projectRepository,
            IAppRepositoryBase<AnnualPlan> annualPlanRepository,
            IAppRepositoryBase<MonthlyPlan> monthlyPlanRepository,
            IProjectHelper projectHelper
            )
        {
            _authInfoProvider = authInfoProvider;
            _userUnitRepository = userUnitRepository;
            _projectHelper = projectHelper;
            _projectRepository = projectRepository;
            _annualPlanRepository = annualPlanRepository;
            _monthlyPlanRepository = monthlyPlanRepository;
        }

        public PaginationData<ProjectInvestmentPlan> Get(int? projectId, int pageIndex, int pageSize, string keyword,
               string sortField,
               string sortState)
        {
            var currentUserId = _authInfoProvider.GetCurrent().User.Id;
            var currentUnitUserIds = _userUnitRepository.Get()
                .Where(v =>_userUnitRepository.Get().Where(u => u.UserId == currentUserId).Any(u => u.OrganizationUnitId == v.OrganizationUnitId)).Select(u => u.UserId).ToList();
            var privilegedPersonIds = AuthorizationContext.Instance.GetPrivilegedPersonIds("项目信息");
            IQueryable<Project> queryForProject = _projectRepository.Get().Where(u=>u.DataState == DataState.Stable 
                && (u.Members.Any(v=>v.UserId == currentUserId) || u.CreatorId == currentUserId || currentUnitUserIds.Contains(u.CreatorId) || privilegedPersonIds.Contains(currentUserId))
            );
            if (!string.IsNullOrEmpty(projectId.ToString()))
            {
                queryForProject = queryForProject.Where(u => u.Id == projectId);
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                queryForProject = queryForProject.Where(u=>u.Name.Contains(keyword));
            }
            IQueryable<AnnualPlan> queryForAnnual = _annualPlanRepository.Get().OrderBy(u=>u.RecordDate);
            IQueryable<MonthlyPlan> queryForMonthly = _monthlyPlanRepository.Get().OrderBy(u=>u.RecordDate);
            //sortState 1降序2升序0默认
            if (!string.IsNullOrEmpty(sortField) && !string.IsNullOrEmpty(sortState))
            {
                switch (sortField)
                {
                    case "RecordDate"://年份
                        queryForAnnual = sortState == "1" ? queryForAnnual.OrderByDescending(u => u.RecordDate) : sortState == "2" ? queryForAnnual.OrderBy(u => u.RecordDate) : queryForAnnual.OrderByDescending(u => u.CreateTime);
                        break;
                    case "Plann"://年投资
                        queryForAnnual = sortState == "1" ? queryForAnnual.OrderByDescending(u => u.PlannedDemolitionFee) : sortState == "2" ? queryForAnnual.OrderBy(u => u.PlannedDemolitionFee) : queryForAnnual.OrderByDescending(u => u.CreateTime);
                        queryForAnnual = sortState == "1" ? queryForAnnual.OrderByDescending(u => u.PlannedProjectCosts) : sortState == "2" ? queryForAnnual.OrderBy(u => u.PlannedProjectCosts) : queryForAnnual.OrderByDescending(u => u.CreateTime);
                        break;
                    case "Monthly"://月投资
                        queryForMonthly = sortState == "1" ? queryForMonthly.OrderByDescending(u => u.PlannedDemolitionFee) : sortState == "2" ? queryForMonthly.OrderBy(u => u.PlannedDemolitionFee) : queryForMonthly.OrderByDescending(u => u.CreateTime);
                        queryForMonthly = sortState == "1" ? queryForMonthly.OrderByDescending(u => u.PlannedProjectCosts) : sortState == "2" ? queryForMonthly.OrderBy(u => u.PlannedProjectCosts) : queryForMonthly.OrderByDescending(u => u.CreateTime);
                        break;
                 
              
                    default:
                        queryForAnnual = queryForAnnual.OrderByDescending(u => u.CreateTime);
                        break;
                }

            }
            else
            {
                queryForAnnual = queryForAnnual.OrderByDescending(u => u.CreateTime);
            }

            var query = queryForAnnual.GroupJoin(
                queryForMonthly, 
                u => new { u.ProjectId, u.RecordDate.Year }, 
                v => new { v.ProjectId, v.RecordDate.Year }, 
                (u, v) => new AnnualPlanWrapper{ AnnualPlan = u, MonthlyPlans = v.ToList() });
            var query2 = queryForProject.GroupJoin(query, u => u.Id, v => v.AnnualPlan.ProjectId, (u, v) => new ProjectPlanWrapper
            {
                Project = u,
                AnnnualPlans = v.ToList()
            });
            var paging = PaginationDataHelper.WrapData(query2, pageIndex, pageSize, u => u.Project.CreateTime).TransferTo<ProjectInvestmentPlan>(u=> {
                var projectPlan = new ProjectInvestmentPlan() {
                    ProjectId = u.Project.Id,
                    ProjectName = u.Project.Name,
                    HasPermission = _projectHelper.HasPermission("项目计划负责人", u.Project.Id)
                };
                u.AnnnualPlans.ForEach(annualPlan => {
                    var annualInvestmentPlan = new AnnualInvestmentPlan()
                    {
                        Id = annualPlan.AnnualPlan.Id,
                        Year = annualPlan.AnnualPlan.RecordDate.Year,
                        Investment = annualPlan.AnnualPlan.PlannedDemolitionFee + annualPlan.AnnualPlan.PlannedProjectCosts,
                        PlannedDemolitionFee = annualPlan.AnnualPlan.PlannedDemolitionFee,
                        PlannedProjectCosts= annualPlan.AnnualPlan.PlannedProjectCosts
                    };
                    
                    annualPlan.MonthlyPlans.ForEach(monthlyPlan=> {
                        annualInvestmentPlan.MonthlyInvestmentPlans.Add(new MonthlyInvestmentPlan {
                            Id = monthlyPlan.Id,
                            Month = monthlyPlan.RecordDate.Month,
                            Investment = monthlyPlan.PlannedDemolitionFee + monthlyPlan.PlannedProjectCosts,
                            PlannedDemolitionFee = monthlyPlan.PlannedDemolitionFee,
                            PlannedProjectCosts = monthlyPlan.PlannedProjectCosts
                        });
                    });
                   
                    projectPlan.AnnualInvestmentPlans.Add(annualInvestmentPlan);
                });
                return projectPlan;
            });
           
           // var paging2 = paging;
            return paging;
        }

        public class ProjectPlanWrapper
        {
            public Project Project { get; set; }
            public List<AnnualPlanWrapper> AnnnualPlans { get; set; }

            public ProjectPlanWrapper()
            {
                AnnnualPlans = new List<AnnualPlanWrapper>();
            }
        }

        public class AnnualPlanWrapper
        {
            public AnnualPlan AnnualPlan { get; set; }
            public List<MonthlyPlan> MonthlyPlans { get; set; }
            public AnnualPlanWrapper()
            {
                MonthlyPlans = new List<MonthlyPlan>();
            }
        }
    }
}
