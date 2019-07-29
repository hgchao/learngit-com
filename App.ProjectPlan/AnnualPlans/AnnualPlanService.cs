using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using App.Base.Repositories;
using App.Core.Authorization.Accounts;
using App.Core.Common.Entities;
using App.Core.Common.Exceptions;
using App.ProjectPlan.AnnualPlans.Dto;
using App.ProjectPlan.PmAnnualPlans.Dto;
using App.Projects.ProjectBaseInfos;
using Microsoft.EntityFrameworkCore;
using PoorFff.Mapper;

namespace App.ProjectPlan.AnnualPlans
{
    public class AnnualPlanService : IAnnualPlanService
    {
        private IAuthInfoProvider _authInfoProvider;
        private IAppRepositoryBase<AnnualPlan> _planRepository;
        // private IAppRepositoryBase<Project> _projectRepository;
        private IProjectHelper _projectHelper;
        public AnnualPlanService(
             IAuthInfoProvider authInfoProvider,
            IAppRepositoryBase<AnnualPlan> investmentAnnualPlanRepository,
            // IAppRepositoryBase<Project> projectRepository
            IProjectHelper projectHelper
            )
        {
            _projectHelper = projectHelper;
            _authInfoProvider = authInfoProvider;
            _planRepository = investmentAnnualPlanRepository;
           // _projectRepository = projectRepository;
        }

        public int Add(AddAnnualPlanInput input)
        {
            var userId = _authInfoProvider.GetCurrent().User.Id;
            if (!_projectHelper.HasPermission("项目计划负责人", input.ProjectId))//权限设置
            {
                throw new AppCoreException("年计划发布没有权限");
            }
            var plan = input.MapTo<AnnualPlan>();
            if(plan.RecordDate == null)
            {
                throw new AppCoreException("传入数据的Year不能为空");
            }
            if(plan.ProjectId == 0)
            {
                throw new AppCoreException("传入数据的ProjectId不能为空");
            }
            if(_planRepository.Count(u=>u.ProjectId == plan.ProjectId && u.RecordDate == plan.RecordDate) > 0)
            {
                throw new EntityException("RecordDate", plan.RecordDate.ToString("yyyy"), "InvestmentAnnualPlan", "已存在");
            }
            _planRepository.Add(plan);
            return plan.Id;
        }

        public GetAnnualPlanOutput Get(int id)
        {
            return _planRepository.Get(id).MapTo<GetAnnualPlanOutput>();
        }

        public List<GetAnnualPlanOutput> GetByProject(int projectId, DateTime? startDate, DateTime? endDate)
        {
            var query = _planRepository.Get().Include(u=>u.Project).Where(u=>u.ProjectId == projectId );
            if (startDate != null)
                query = query.Where(u=>u.RecordDate>=startDate);
            if (endDate != null)
                query = query.Where(u=>u.RecordDate<=endDate);
            var paging = query.OrderBy(u=>u.RecordDate).ToList().MapToList<GetAnnualPlanOutput>();
            paging.ForEach(u => u.HasPermission = _projectHelper.HasPermission("项目计划负责人", u.ProjectId));
            return paging;
        }

        public GetAnnualPlanOutput GetByProject(int projectId, int year)
        {
            var query = _planRepository.Get().Include(u => u.Project).Where(u => u.RecordDate.Year == year && u.ProjectId == projectId);
            return query.OrderBy(u => u.RecordDate).ToList().MapToList<GetAnnualPlanOutput>().FirstOrDefault();
        }
        public void Delete(int id)
        {
            var problem = _planRepository.Get().Where(u => u.Id == id).FirstOrDefault();
            if (problem != null)
            {
                var userId = _authInfoProvider.GetCurrent().User.Id;
                if (!_projectHelper.HasPermission("项目计划负责人", problem.ProjectId))//权限设置
                {
                    throw new AppCoreException("年计划删除没有权限");
                }
                _planRepository.Delete(new AnnualPlan { Id = id });
            }

        }
        public void Update(UpdateAnnualPlanInput input)
        {
            var userId = _authInfoProvider.GetCurrent().User.Id;
            if (!_projectHelper.HasPermission("项目计划负责人", input.ProjectId))//权限设置
            {
                throw new AppCoreException("年计划修改没有权限");
            }
            var plan = input.MapTo<AnnualPlan>();
            if(plan.RecordDate == null)
            {
                throw new AppCoreException("传入数据的Year不能为空");
            }
            if(plan.ProjectId == 0)
            {
                throw new AppCoreException("传入数据的ProjectId不能为空");
            }
            if(_planRepository.Count(u=>u.ProjectId == plan.ProjectId && u.RecordDate == plan.RecordDate && u.Id != plan.Id) > 0)
            {
                throw new EntityException("RecordDate", plan.RecordDate.ToString("yyyy"), "InvestmentAnnualPlan", "已存在");
            }
            _planRepository.Update(plan);
        }
    }
}
