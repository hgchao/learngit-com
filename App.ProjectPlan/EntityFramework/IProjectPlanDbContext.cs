using App.ProjectPlan.AnnualPlans;
using App.ProjectPlan.MonthlyPlans;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.ProjectPlan.EntityFramework
{
    public interface IProjectPlanDbContext
    {
        DbSet<AnnualPlan> AnnualPlans { get; set; }
        DbSet<MonthlyPlan> MonthlyPlans { get; set; }
    }
}
