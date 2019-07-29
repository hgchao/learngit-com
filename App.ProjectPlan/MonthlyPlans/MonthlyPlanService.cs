using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using App.Base.Repositories;
using App.Core.Common.Entities;
using App.Core.Common.Exceptions;
using App.ProjectPlan.MonthlyPlans.Dto;
using App.ProjectPlan.MonthlyPlans;
using Microsoft.EntityFrameworkCore;
using PoorFff.Mapper;
using App.Core.Authorization.Accounts;
using App.Projects.ProjectBaseInfos;

namespace App.ProjectPlan.MonthlyPlans
{
    public class MonthlyPlanService : IMonthlyPlanService
    {
        private IAuthInfoProvider _authInfoProvider;
        private IAppRepositoryBase<MonthlyPlan> _planRepository;
        // private IAppRepositoryBase<Project> _projectRepository;
        private IProjectHelper _projectHelper;
        public MonthlyPlanService(
              IAuthInfoProvider authInfoProvider,
            IAppRepositoryBase<MonthlyPlan> investmentMonthlyPlanRepository,
           // IAppRepositoryBase<Project> projectRepository
           IProjectHelper projectHelper
            )
        {
            _projectHelper = projectHelper;
            _authInfoProvider = authInfoProvider;
            _planRepository = investmentMonthlyPlanRepository;
           // _projectRepository = projectRepository;
        }
        public string MonthMoney(string year)
        {
            var planList = _planRepository.Get().Select(u => new { u.PlannedDemolitionFee, u.PlannedProjectCosts, u.RecordDate }).Where(u => u.RecordDate.Year == Convert.ToInt32(year)).ToList();
            var January = planList.Where(u => u.RecordDate.Month == 1).ToList().Sum(u=> u.PlannedDemolitionFee) + planList.Where(u => u.RecordDate.Month == 1).ToList().Sum(u => u.PlannedProjectCosts);
            var February = planList.Where(u => u.RecordDate.Month == 2).ToList().Sum(u => u.PlannedDemolitionFee) + planList.Where(u => u.RecordDate.Month == 2).ToList().Sum(u => u.PlannedProjectCosts);
            var March = planList.Where(u => u.RecordDate.Month == 3).ToList().Sum(u => u.PlannedDemolitionFee) + planList.Where(u => u.RecordDate.Month == 3).ToList().Sum(u => u.PlannedProjectCosts);
            var April = planList.Where(u => u.RecordDate.Month == 4).ToList().Sum(u => u.PlannedDemolitionFee) + planList.Where(u => u.RecordDate.Month == 4).ToList().Sum(u => u.PlannedProjectCosts);
            var May = planList.Where(u => u.RecordDate.Month == 5).ToList().Sum(u => u.PlannedDemolitionFee) + planList.Where(u => u.RecordDate.Month == 5).ToList().Sum(u => u.PlannedProjectCosts);
            var June = planList.Where(u => u.RecordDate.Month == 6).ToList().Sum(u => u.PlannedDemolitionFee) + planList.Where(u => u.RecordDate.Month == 6).ToList().Sum(u => u.PlannedProjectCosts);
            var July = planList.Where(u => u.RecordDate.Month == 7).ToList().Sum(u => u.PlannedDemolitionFee) + planList.Where(u => u.RecordDate.Month == 7).ToList().Sum(u => u.PlannedProjectCosts);
            var August = planList.Where(u => u.RecordDate.Month == 8).ToList().Sum(u => u.PlannedDemolitionFee) + planList.Where(u => u.RecordDate.Month == 8).ToList().Sum(u => u.PlannedProjectCosts);
            var September = planList.Where(u => u.RecordDate.Month == 9).ToList().Sum(u => u.PlannedDemolitionFee) + planList.Where(u => u.RecordDate.Month == 8).ToList().Sum(u => u.PlannedProjectCosts);
            var October = planList.Where(u => u.RecordDate.Month == 10).ToList().Sum(u => u.PlannedDemolitionFee) + planList.Where(u => u.RecordDate.Month == 10).ToList().Sum(u => u.PlannedProjectCosts);
            var November = planList.Where(u => u.RecordDate.Month == 11).ToList().Sum(u => u.PlannedDemolitionFee) + planList.Where(u => u.RecordDate.Month == 11).ToList().Sum(u => u.PlannedProjectCosts);
            var December = planList.Where(u => u.RecordDate.Month == 12).ToList().Sum(u => u.PlannedDemolitionFee) + planList.Where(u => u.RecordDate.Month == 12).ToList().Sum(u => u.PlannedProjectCosts);
            var contractStr = January + "," + February + "," + March + "," + April + "," + May + "," + June + "," + July + "," + August + "," + September + "," + October + "," + November + "," + December;
            return contractStr;
        }
        public int Add(AddMonthlyPlanInput input)
        {
            var userId = _authInfoProvider.GetCurrent().User.Id;
            if (!_projectHelper.HasPermission("项目计划负责人", input.ProjectId))//权限设置
            {
                throw new AppCoreException("月计划发布没有权限");
            }
            var plan = input.MapTo<MonthlyPlan>();
            if(plan.RecordDate == null)
            {
                throw new AppCoreException("传入数据的RecordTime不能为空");
            }
            if(plan.ProjectId == 0)
            {
                throw new AppCoreException("传入数据的ProjectId不能为空");
            }
            if(_planRepository.Count(u=>u.ProjectId == plan.ProjectId && u.RecordDate == plan.RecordDate) > 0)
            {
                throw new EntityException("RecordDate", plan.RecordDate.ToString("y"), "InvestmentMonthlyPlan", "已存在");
            }
            _planRepository.Add(plan);
            return plan.Id;
        }

        public GetMonthlyPlanOutput Get(int id)
        {
            var plan = _planRepository.GetIncluding(u => u.Id == id, new Expression<Func<MonthlyPlan, object>>[] { u => u.Project }).FirstOrDefault();
            if (plan == null)
                throw new EntityException("Id", id, "MonthlyPlan", "不存在");
            return plan.MapTo<GetMonthlyPlanOutput>();
        }

        public List<GetMonthlyPlanOutput> GetByProject(int projectId, int year)
        {
            var query = _planRepository.Get().Include(u=>u.Project).Where(u=>u.ProjectId == projectId  && u.RecordDate.Year == year);
            var paging = query.OrderBy(u=>u.RecordDate).ToList().MapToList<GetMonthlyPlanOutput>();
            paging.ForEach(u => u.HasPermission = _projectHelper.HasPermission("项目计划负责人", u.ProjectId));
            return paging;
        }

        public void Delete(int id)
        {
            var problem = _planRepository.Get().Where(u => u.Id == id).FirstOrDefault();
            if (problem != null)
            {
                var userId = _authInfoProvider.GetCurrent().User.Id;
                if (!_projectHelper.HasPermission("项目计划负责人", problem.ProjectId))//权限设置
                {
                    throw new AppCoreException("月计划删除没有权限");
                }
                _planRepository.Delete(new MonthlyPlan { Id = id });
            }

        }

        public void Update(UpdateMonthlyPlanInput input)
        {
            var userId = _authInfoProvider.GetCurrent().User.Id;
            if (!_projectHelper.HasPermission("项目计划负责人", input.ProjectId))//权限设置
            {
                throw new AppCoreException("月计划修改没有权限");
            }
            var plan = input.MapTo<MonthlyPlan>();
            if(plan.RecordDate == null)
            {
                throw new AppCoreException("传入数据的RecordTime不能为空");
            }
            if(plan.ProjectId == 0)
            {
                throw new AppCoreException("传入数据的ProjectId不能为空");
            }
            if(_planRepository.Count(u=>u.ProjectId == plan.ProjectId && u.RecordDate == plan.RecordDate && u.Id != plan.Id) > 0)
            {
                throw new EntityException("RecordDate", plan.RecordDate.ToString("y"), "InvestmentMonthlyPlan", "已存在");
            }
            _planRepository.Update(plan);
        }
    }
}
