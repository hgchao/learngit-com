using App.Base;
using App.Core.Common.Entities;
using App.ProjectProgress.PmMonthlyProgresses.Dto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace App.ProjectProgress.PmMonthlyProgresses
{
    public interface IMonthlyProgressService
    {
        int Add(StartProcessInput<AddMonthlyProgressInput> input);
        int Update(StartProcessInput<UpdateMonthlyProgressInput> input);
        void Delete(int id);
        void CompleteProcess(int processInstanceId);
        void CompleteTask(CompleteTaskInput<UpdateMonthlyProgressInput> input);
        GetMonthlyProgressOutput GetByTask(int taskId);
        GetMonthlyProgressOutput GetByProcess(int processId);
        GetMonthlyProgressOutput GetByTaskHistory(int taskHistoryId);
        GetMonthlyProgressOutput Get(int id);
        PaginationData<GetMonthlyProgressListOutput> Get(int pageIndex, int pageSize, int? projectId, string keyword,
               string sortField,
               string sortState);
        MemoryStream Export(int pageIndex, int pageSize, string keyword);
        float GetCurrentProgress(int projectId);
    }
}
