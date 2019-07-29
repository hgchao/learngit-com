using App.Core.Authorization.Accounts;
using App.Core.Common.Entities;
using App.Core.Common.Exceptions;
using App.Core.Form.Fields.Dto;
using App.Core.Workflow;
using App.Core.Workflow.Providers;
using App.Core.Workflow.Tasks;
using App.Core.Workflow.Tasks.Dto;
using App.Workflow.Tasks.Dto;
using PoorFff.PfExpression.Extensions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace App.Workflow.Tasks
{
    public class TaskService: ITaskService
    {
        private ITaskProvider _taskProvider;
        private IHistoryProvider _historyProvider;
        private IAuthInfoProvider _authInfoProvider;
        public TaskService(
            IWfEngine wfEgine,
            IAuthInfoProvider authInfoProvider)
        {
            _taskProvider = wfEgine.GetTaskProvider();
            _historyProvider = wfEgine.GetHistoryProvider();
            _authInfoProvider = authInfoProvider;
        }

        public void CompleteTask(CompleteTaskInput input)
        {
            var authInfo = _authInfoProvider.GetCurrent();
            var userId = authInfo?.User?.Id;
            var task = _taskProvider.GetTask(input.Id);
            if (task.Assignee != userId.Value.ToString())
            {
                throw new AppCoreException($"id:为{userId.Value}的用户不是任务【{task.NodeName}】的委托人");
            }
            if (input.FormContents.Count > 0)
            {
                _taskProvider.SetFields(input.Id, input.FormContents);
            }
            if(!input.PreventCommit)
            {
                _taskProvider.Complete(input.Id, input.Comment);
            }
        }

        public bool Exist(int id)
        {
            return _taskProvider.IsTaskExist(id);
        }

        public PaginationData<GetTaskOutput> Get(int pageIndex, int pageSize, string keyword, DateTime? createTimeL, DateTime? createTimeR, string processDefinitionName = null, bool include = true)
        {
            var authInfo = _authInfoProvider.GetCurrent();
            var userId = authInfo?.User?.Id;
            Expression<Func<Wf_Ru_Task, bool>> expr = u=>true;
            if (createTimeL != null)
            {
                expr = expr.AndAlso(u => u.CreateTime >= createTimeL);
            }
            if (createTimeR != null)
            {
                expr = expr.AndAlso(u => u.CreateTime <= createTimeR);
            }
            if(!string.IsNullOrEmpty(processDefinitionName))
            {
                if (include)
                {
                    expr = expr.AndAlso(u=>u.ProcessInstance.ProcessDefinition.Name == processDefinitionName);
                }
                else
                {
                    expr = expr.AndAlso(u=>u.ProcessInstance.ProcessDefinition.Name != processDefinitionName);
                }
            }
            return _taskProvider.GetTasks(pageIndex, pageSize, keyword, userId == null ? "-1" : userId.Value.ToString(), expr);
        }

        public GetTaskOutput Get(int id)
        {
            var authInfo = _authInfoProvider.GetCurrent();
            var userId = authInfo?.User?.Id;
            var task = _taskProvider.GetTask(id);
            if (task.Assignee != userId.Value.ToString())
            {
                throw new AppCoreException($"id:为{userId.Value}的用户不是任务：{task.NodeName}的委托人");
            }
            return task;
        }

        public GetTaskOutput GetByProcessInstance(int processInstanceId)
        {
            var authInfo = _authInfoProvider.GetCurrent();
            var userId = authInfo?.User?.Id;
            return _taskProvider.GetTask(processInstanceId, userId.Value.ToString());
        }

        public List<FieldDto> GetFields(int taskId)
        {
            var authInfo = _authInfoProvider.GetCurrent();
            var userId = authInfo?.User?.Id;
            var task = _taskProvider.GetTask(taskId);
            if (task.Assignee != userId.Value.ToString())
            {
                throw new AppCoreException($"id:为{userId.Value}的用户不是任务：{task.NodeName}的委托人");
            }
            return _taskProvider.GetFieldsByTask(taskId);
        }

        public List<FieldDto> GetFieldsByHistory(int taskId)
        {
            var authInfo = _authInfoProvider.GetCurrent();
            var userId = authInfo?.User?.Id;
            var task = _historyProvider.GetTaskHistory(taskId);
            if (!task.AssigneeList.Contains($"{userId}"))
            {
                throw new AppCoreException($"id:为{userId.Value}的用户不是任务【{task.NodeName}】的委托人");
            }
            return _historyProvider.GetFieldsByTaskHistory(taskId);
        }

        public PaginationData<GetTaskHistoryOutput> GetHistory(int pageIndex, int pageSize, string keyword, DateTime? createTimeL, DateTime? createTimeR)
        {
            var authInfo = _authInfoProvider.GetCurrent();
            var userId = authInfo?.User?.Id;
            Expression<Func<Wf_Hi_TaskInstance, bool>> expr = null;
            if (createTimeL != null)
            {
                expr = u => u.CreateTime >= createTimeL;
            }
            if (createTimeR != null)
            {
                expr = u => u.CreateTime <= createTimeR;
            }
            return _historyProvider.GetTaskHistory(pageIndex, pageSize, keyword, userId == null ? "-1" : userId.Value.ToString(), expr);
        }

        public void Redirect(RedirectTaskInput input)
        {
            var authInfo = _authInfoProvider.GetCurrent();
            var userId = authInfo?.User?.Id;
            var task = _taskProvider.GetTask(input.Id);
            if (task.Assignee != userId.Value.ToString())
            {
                throw new AppCoreException($"id:为{userId.Value}的用户不是任务：{task.NodeName}的委托人");
            }
            _taskProvider.Redirect(input.Id, input.NodeUid, input.Comment);
        }

        public void Withdraw(int taskInstanceId)
        {
            var authInfo = _authInfoProvider.GetCurrent();
            var userId = authInfo?.User?.Id;
            _taskProvider.Withdraw(taskInstanceId, userId.Value.ToString());
        }

    }
}
