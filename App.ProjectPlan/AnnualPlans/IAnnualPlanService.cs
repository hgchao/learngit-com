using App.Core.Common.Entities;
using App.ProjectPlan.AnnualPlans.Dto;
using App.ProjectPlan.PmAnnualPlans.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.ProjectPlan.AnnualPlans
{
    public interface IAnnualPlanService
    {
        void Delete(int id);
        int Add(AddAnnualPlanInput input);
        void Update(UpdateAnnualPlanInput input);


        GetAnnualPlanOutput Get(int id);
        List<GetAnnualPlanOutput> GetByProject(int projectId, DateTime? startDate, DateTime? endDate);
        GetAnnualPlanOutput GetByProject(int projectId, int year);
    }
}
