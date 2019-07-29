using App.Core.Common.Entities;
using App.ProjectPlan.MonthlyPlans.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.ProjectPlan.MonthlyPlans
{
    public interface IMonthlyPlanService
    { /// <summary>
      /// 首页折线图，数据为不同月份项目计划金额
      /// </summary>
      /// <param name="year"></param>
      /// <returns></returns>
        string MonthMoney(string year);
        void Delete(int id);
        int Add(AddMonthlyPlanInput input);
        void Update(UpdateMonthlyPlanInput input);


        GetMonthlyPlanOutput Get(int id);
        List<GetMonthlyPlanOutput> GetByProject(int projectId, int year);
    }
}
