using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using App.Base;
using App.Base.EntityFramework;
using App.Base.Repositories;
using App.Core.Authorization.Accounts;
using App.Core.Authorization.Repositories;
using App.Core.Authorization.Users;
using App.Core.Common.Exceptions;
using App.Core.Common.Operators;
using App.Core.Messaging;
using App.Core.Workflow;
using App.Core.Workflow.Engine.Definition.Nodes.Tasks;
using App.Core.Workflow.Providers;
using App.ProjectGantts.Gantts;
using App.ProjectGantts.Tasks.Dto;
using App.Projects;
using App.Projects.ProjectBaseInfos;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Oa.Project;
using PoorFff.Mapper;

namespace App.ProjectGantts.Tasks
{
    public class ProjectTaskService : IProjectTaskService
    {
        private IAppRepositoryBase<ProjectTask> _projectTaskRepository;
        private IAuthInfoProvider _authInfoProvider;
        private IHistoryProvider _historyProvider;
        private IRuntimeProvider _runtimeProvider;
        private ITaskProvider _taskProvider;
        private IDefinitionProvider _definitionProvder;
        private IAuthorizationRepositoryBase<User> _userRepository;
        private IAppDbContextProvider _dbContextProvider;
        private IAppRepositoryBase<ProjectGantt> _ganttRepository;
        private IAppRepositoryBase<Project> _projectRepository;
        private IProjectHelper _projectHelper;
        public ProjectTaskService(
            IAppRepositoryBase<ProjectTask> taskRepository,
            IAppRepositoryBase<ProjectGantt> ganttRepository,
            IAuthInfoProvider authInfoProvider,
            IWfEngine wfEngine,
            IAuthorizationRepositoryBase<User> userRepository,
            IAppDbContextProvider dbContextProvider,
            IProjectHelper projectHelper
            )
        {
            _ganttRepository = ganttRepository;
            _projectHelper = projectHelper;
            _authInfoProvider = authInfoProvider;
            _userRepository = userRepository;
            _runtimeProvider = wfEngine.GetRuntimeProvider();
            _taskProvider = wfEngine.GetTaskProvider();
            _definitionProvder = wfEngine.GetDefinitionProvider();
            _historyProvider = wfEngine.GetHistoryProvider();
            _dbContextProvider = dbContextProvider;
            _projectTaskRepository = taskRepository;
        }

        public int Add(AddProjectTaskInput input)
        {
            var projectId = _ganttRepository.Get().Where(u => u.Id == input.GanttId).Select(u => u.ProjectId).FirstOrDefault();
            if (!_projectHelper.HasPermission("项目进度负责人", projectId))
            {
                throw new AppCoreException("项目进度没有权限");
            }

            var task = input.MapTo<ProjectTask>();
            int maxSortId = _projectTaskRepository.Get().Where(u => u.GanttId == task.GanttId).Select(u=>u.SortNo).DefaultIfEmpty().Max();
            task.SortNo = maxSortId + 1;
            _projectTaskRepository.Add(task);
            return task.Id;
        }

        public void Delete(int id)
        {
            var projectId = _ganttRepository.Get().Where(u => u.Tasks.Any(v => v.Id == id)).Select(u => u.ProjectId).FirstOrDefault();
            if (!_projectHelper.HasPermission("项目进度负责人", projectId))
            {
                throw new AppCoreException("项目进度没有权限");
            }
            _projectTaskRepository.Delete(new ProjectTask { Id = id});
        }

        public GetProjectTaskOutput Get(int id)
        {
            return _projectTaskRepository.Get()
                .Include(u=>u.Attachments).ThenInclude(u=>u.FileMeta)
                .Where(u=>u.Id == id)
                .MapTo<GetProjectTaskOutput>();
        }

        public void Update(UpdateProjectTaskInput input)
        {
            var projectId = _ganttRepository.Get().Where(u => u.Tasks.Any(v => v.Id == input.Id)).Select(u => u.ProjectId).FirstOrDefault();
            if (!_projectHelper.HasPermission("项目进度负责人", projectId))
            {
                throw new AppCoreException("项目进度没有权限");
            }
            var task = input.MapTo<ProjectTask>();
            var existing = _projectTaskRepository.Get().Where(u => u.Id == task.Id).FirstOrDefault();
            int adjacentTaskId;
            var nextSibling = false;
            var targetId = input.Target;
            _projectTaskRepository.Update(task, new System.Linq.Expressions.Expression<Func<ProjectTask, object>>[] { u=>u.GanttId, u=>u.SortNo}, false);
            if (!string.IsNullOrEmpty(targetId))
            {
                if (targetId.StartsWith("next:"))
                {
                    targetId = targetId.Replace("next:", "");
                    nextSibling = true;
                }
                if (!int.TryParse(targetId, out adjacentTaskId))
                {
                    return;
                }
                var adjacentTask = _projectTaskRepository.Get().Where(u => u.Id == adjacentTaskId).Select(u=>new ProjectTask { Id=u.Id, SortNo = u.SortNo}).FirstOrDefault();
                var sortedOrder = adjacentTask.SortNo;
                if (nextSibling)
                    sortedOrder++;
                List<ProjectTask> updatedList = new List<ProjectTask>();
                updatedList.Add(new ProjectTask { Id = task.Id, SortNo = sortedOrder});
                var otherUpdatedList = _projectTaskRepository.Get().Where(u => u.SortNo >= sortedOrder && u.Id != task.Id).Select(u => new ProjectTask { Id = u.Id, SortNo = u.SortNo }).ToList();
                otherUpdatedList.ForEach(u => u.SortNo++);
                updatedList.AddRange(otherUpdatedList);
                _projectTaskRepository.BatchUpdate(updatedList, new System.Linq.Expressions.Expression<Func<ProjectTask, object>>[] {u=>u.SortNo}, true);
            }
        }

        public void CompleteTask(CompleteProjectTaskInput input)
        {
            NullableHelper.SetNull(input);
            var projectTask = input.MapTo<ProjectTask>();
            var existing = _projectTaskRepository.Get().Include(u => u.Attachments).Where(u=>u.Id == input.Id).FirstOrDefault();
            if (existing.Assignee != _authInfoProvider.GetCurrent().User.Id)
            {
                throw new AppCoreException("当前用户不是节点负责人");
            }
            _projectTaskRepository.Update(projectTask, existing, new System.Linq.Expressions.Expression<Func<ProjectTask, object>>[] {
                u=>u.SortNo,
                u=>u.GanttId,
                u=>u.Assignee,
                u=>u.Name,
                u=>u.StartDate,
                u=>u.Content,
                u=>u.Duration,
                u=>u.Pid,
                u=>u.Type,
            }, false);
        }
    }
}
