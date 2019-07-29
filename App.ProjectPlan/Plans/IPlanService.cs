using App.Core.Common.Entities;
using App.ProjectPlan.Plans;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.ProjectPlan.Plans
{
    public interface IPlanService
    {
        PaginationData<ProjectInvestmentPlan> Get(int? projectId, int pageIndex, int pageSize, string keyword,string sortField,string sortState);
    }
}
