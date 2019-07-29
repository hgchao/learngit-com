
using App.ProjectPlan.AnnualPlans;
using App.ProjectPlan.AnnualPlans.Dto;
using App.ProjectPlan.MonthlyPlans;
using App.ProjectPlan.MonthlyPlans.Dto;
using App.ProjectPlan.PmAnnualPlans.Dto;
using PoorFff.Mapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.ProjectPlan
{
    public class ProjectPlanProfile: PfProfile
    {
        public ProjectPlanProfile()
        {
            base.CreateMap<AddMonthlyPlanInput, MonthlyPlan>()
            .ForMember(u => u.RecordDate, expr => expr.MapFrom(u => new DateTime(u.Year, u.Month, 1)));
            base.CreateMap<UpdateMonthlyPlanInput, MonthlyPlan>()
            .ForMember(u => u.RecordDate, expr => expr.MapFrom(u => new DateTime(u.Year, u.Month, 1)));
            base.CreateMap<MonthlyPlan, GetMonthlyPlanOutput>()
                .ForMember(u => u.PlannedInvestment, expr => expr.MapFrom(u => u.PlannedDemolitionFee + u.PlannedProjectCosts))
                .ForMember(u=>u.Year, expr=>expr.MapFrom(u=>u.RecordDate.Year))
                .ForMember(u=>u.Month, expr=>expr.MapFrom(u=>u.RecordDate.Month))
                .ForMember(u=>u.ProjectName, expr=>expr.MapFrom(u=>u.Project.Name));

            base.CreateMap<AddAnnualPlanInput, AnnualPlan>()
            .ForMember(u => u.RecordDate, expr => expr.MapFrom(u => new DateTime(u.Year, 1, 1)));
            base.CreateMap<UpdateAnnualPlanInput, AnnualPlan>()
            .ForMember(u => u.RecordDate, expr => expr.MapFrom(u => new DateTime(u.Year, 1, 1)));
            base.CreateMap<AnnualPlan, GetAnnualPlanOutput>()
                .ForMember(u => u.PlannedInvestment, expr => expr.MapFrom(u => u.PlannedDemolitionFee + u.PlannedProjectCosts))
                .ForMember(u=>u.Year, expr=>expr.MapFrom(u=>u.RecordDate.Year))
                .ForMember(u=>u.ProjectName, expr=>expr.MapFrom(u=>u.Project.Name));
        }
    }
}
