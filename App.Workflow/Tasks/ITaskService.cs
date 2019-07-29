using App.Core.Common.Entities;
using App.Core.Form.Fields.Dto;
using App.Core.Workflow.Tasks.Dto;
using App.Workflow.Tasks.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Workflow.Tasks
{
    public interface ITaskService
    {
        GetTaskOutput Get(int id);
        GetTaskOutput GetByProcessInstance(int processInstanceId);
        bool Exist(int id);
        PaginationData<GetTaskOutput> Get(int pageIndex, int pageSize, string keyword, DateTime? createTimeL, DateTime? createTimeR, string processDefinitionName = null, bool include = true);
        PaginationData<GetTaskHistoryOutput> GetHistory(int pageIndex, int pageSize, string keyword, DateTime? createTimeL, DateTime? createTimeR);
        List<FieldDto> GetFields(int taskId);
        List<FieldDto> GetFieldsByHistory(int taskId);
        void CompleteTask(CompleteTaskInput input);
        void Redirect(RedirectTaskInput input);
        void Withdraw(int taskInstanceId);

    }
}
