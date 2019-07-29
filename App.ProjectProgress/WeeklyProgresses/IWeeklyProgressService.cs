using App.Base;
using App.Core.Common.Entities;
using App.ProjectProgress.PmMonthlyProgresses.Dto;
using App.ProjectProgress.WeeklyProgresses.Dto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace App.ProjectProgress.WeeklyProgresses
{
    public interface IWeeklyProgressService
    {
        //int Add(AddWeeklyProgressInput input);
        //void Update(UpdateWeeklyProgressInput input);
        //GetWeeklyProgressOutput Get(int id);

        //PaginationData<GetWeeklyProgressOutput> Get(int pageIndex, int pageSize, string keyword);
        //PaginationData<GetWeeklyProgressOutput> GetByProjectId(int projectId, int pageIndex, int pageSize, string keyword);

        int Add(StartProcessInput<AddWeeklyProgressInput> input);
        int Update(StartProcessInput<UpdateWeeklyProgressInput> input);
        void Delete(int id);
        void CompleteProcess(int processInstanceId);
        void CompleteTask(CompleteTaskInput<UpdateWeeklyProgressInput> input);
        GetWeeklyProgressOutput GetByTask(int taskId);
        GetWeeklyProgressOutput GetByProcess(int processId);
        GetWeeklyProgressOutput GetByTaskHistory(int taskHistoryId);
        GetWeeklyProgressOutput Get(int id);
        PaginationData<GetWeeklyProgressListOutput> Get(int pageIndex, int pageSize, int? projectId, string keyword,
               string sortField,
               string sortState);
        MemoryStream Export(int pageIndex, int pageSize, string keyword);
    }
}
