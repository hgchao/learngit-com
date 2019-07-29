using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using App.Core.Common.Entities;
using App.Core.Common.Exceptions;
using App.Core.Common.Repositories;
using Microsoft.EntityFrameworkCore;
using App.ProjectProgress.PmMonthlyProgresses.Dto;
using PoorFff.Mapper;
using App.Base.Repositories;
using App.ProjectPlan.AnnualPlans;
using App.Core.Common;
using App.Projects.ProjectBaseInfos;
using App.Core.Authorization.Accounts;
using App.Core.Workflow.Providers;
using App.Core.Authorization.Repositories;
using App.Base.EntityFramework;
using App.Core.Messaging;
using App.Core.Common.Operators;
using App.Core.Workflow;
using App.Core.Authorization.Users;
using App.Base;
using App.Projects;
using Oa.Project;
using Newtonsoft.Json;
using App.Core.Workflow.Engine.Definition.Nodes.Tasks;
using System.IO;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;

namespace App.ProjectProgress.PmMonthlyProgresses
{
    public class MonthlyProgressService: IMonthlyProgressService
    {
        private IAuthInfoProvider _authInfoProvider;
        private IHistoryProvider _historyProvider;
        private IRuntimeProvider _runtimeProvider;
        private ITaskProvider _taskProvider;
        private IDefinitionProvider _definitionProvder;
        private IAuthorizationRepositoryBase<User> _userRepository;
        private IAppDbContextProvider _dbContextProvider;
        private IMessagingProvider _messagingProvider;
        private IAppRepositoryBase<MonthlyProgress> _progressRepository;
        private IAppRepositoryBase<Project> _projectRepository;
        private IDbOperator _op;
        private IProjectHelper _projectHelper;
        public MonthlyProgressService(
            IAppRepositoryBase<Project> projectRepository,
            IAppRepositoryBase<MonthlyProgress> progressRepository,
            IDbOperator op,
            IAuthInfoProvider authInfoProvider,
            IWfEngine wfEngine,
            IAuthorizationRepositoryBase<User> userRepository,
            IAppDbContextProvider dbContextProvider,
            IMessagingProvider messagingProvider,
            IProjectHelper projectHelper
            )
        {
            _projectHelper = projectHelper;
            _messagingProvider = messagingProvider;
            _authInfoProvider = authInfoProvider;
            _userRepository = userRepository;
            _runtimeProvider = wfEngine.GetRuntimeProvider();
            _taskProvider = wfEngine.GetTaskProvider();
            _definitionProvder = wfEngine.GetDefinitionProvider();
            _historyProvider = wfEngine.GetHistoryProvider();
            _dbContextProvider = dbContextProvider;
            _progressRepository = progressRepository;
            _projectRepository = projectRepository;
            _op = op;
        }

        public int Add(StartProcessInput<AddMonthlyProgressInput> input)
        {
            NullableHelper.SetNull(input.Data);
            var variables = new Dictionary<string, object>();
            var userId = _authInfoProvider.GetCurrent().User.Id;

            if (!_projectHelper.HasPermission("投资信息负责人", input.Data.ProjectId))
            {
                throw new AppCoreException("月投资填报没有权限");
            }

            variables.Add("starter", userId.ToString());
            int processInstanceId = _runtimeProvider.StartProcessInstanceByName(
                input.ProcessDefinitionName,
                $"{input.ProcessDefinitionName}-{input.Data.Year}-{input.Data.Month}",
                variables);
            var tasks = _taskProvider.GetByProcessInstance(processInstanceId);
            if (tasks.Count == 0)
            {
                throw new AppCoreException("任务未创建成功！");
            }
            var progress = input.Data.MapTo<MonthlyProgress>();
            if (progress.ProjectId == 0)
            {
                throw new AppCoreException("传入数据的ProjectId不能为空");
            }
            if (_progressRepository.Count(u => u.ProjectId == progress.ProjectId && u.RecordDate == progress.RecordDate && u.DataState == DataState.Stable) > 0)
            {
                throw new EntityException("Date", progress.RecordDate, "PmMonthlyReport", "已存在");
            }
            progress.ProcessInstanceId = processInstanceId;
            progress.DataState = DataState.Creating;
            progress.Mid = null;
            _progressRepository.Add(progress);
            var task = tasks[0];
            if (!input.PreventCommit)
                _taskProvider.Complete(task.Id);
            return progress.Id;
        }

        public void CompleteProcess(int processInstanceId)
        {
            var progress = _progressRepository.Get().Where(u => u.ProcessInstanceId == processInstanceId).FirstOrDefault();
            if (progress == null)
            {
                throw new EntityException("MonthlyProgressInstanceId", processInstanceId, "MonthlyProgress", "不存在");
            }
            if (progress.DataState == DataState.Creating)
            {
                //progress.State = DataState.Stable;
                //_progressBaseInfoRepository.Update(progress, new System.Linq.Expressions.Expression<Func<MonthlyProgressBaseInfo, object>>[] {
                //    u => u.State
                //}, true);
                using (var transaction = _dbContextProvider.BeginTransaction())
                {

                    var temp = JsonConvert.DeserializeObject<MonthlyProgress>(JsonConvert.SerializeObject(progress));
                    temp.DataState = DataState.Stable;
                    temp.ProcessInstanceId = null;
                    _progressRepository.Update(temp, new System.Linq.Expressions.Expression<Func<MonthlyProgress, object>>[] {
                        u => u.DataState,
                        u => u.ProcessInstanceId
                    }, true);
                    temp = JsonConvert.DeserializeObject<MonthlyProgress>(JsonConvert.SerializeObject(progress));
                    temp.DataState = DataState.Created;
                    temp.Id = 0;
                    temp.Mid = progress.Id;
                    _progressRepository.Add(temp);
                    transaction.Commit();
                }

                AppBaseContext.Instance.Produce("add-project-briefing", JsonConvert.SerializeObject(new
                {
                    TenantId = _op.TenantId,
                    ProjectId = progress.ProjectId
                }));
            }
            else if (progress.Mid != null && progress.DataState == DataState.Updating)
            {
                using (var transaction = _dbContextProvider.BeginTransaction())
                {
                    var temp = JsonConvert.DeserializeObject<MonthlyProgress>(JsonConvert.SerializeObject(progress));
                    temp.DataState = DataState.Updated;
                    _progressRepository.Update(temp, new System.Linq.Expressions.Expression<Func<MonthlyProgress, object>>[] {
                        u=>u.DataState
                    }, true);
                    temp = JsonConvert.DeserializeObject<MonthlyProgress>(JsonConvert.SerializeObject(progress));
                    temp.Id = progress.Mid.Value;
                    temp.Mid = null;
                    var existing = _progressRepository.Get()
                        .Where(u => u.Id == temp.Id).FirstOrDefault();
                    _progressRepository.Update(temp, existing, new System.Linq.Expressions.Expression<Func<MonthlyProgress, object>>[] {
                        u=>u.ProcessInstanceId,
                        u=>u.DataState,
                    }, false);
                    transaction.Commit();
                }
            }
            else
            {

                throw new AppCoreException("当前数据不再更新状态");
            }
        }

        public void CompleteTask(CompleteTaskInput<UpdateMonthlyProgressInput> input)
        {
            NullableHelper.SetNull(input.Data);
            var authInfo = _authInfoProvider.GetCurrent();
            var userId = authInfo?.User?.Id;
            var task = _taskProvider.GetTask(input.Id);
            if (task.Assignee != userId.Value.ToString())
            {
                throw new AppCoreException($"id:为{userId.Value}的用户不是任务【{task.NodeName}】的委托人");
            }
            var processDefinition = _definitionProvder.Get(task.ProcessDefinitionId);
            var editableFields = ((UserTaskNode)processDefinition.GetNode(task.NodeUid)).GetEditableFields();
            if (input.Data != null)
            {
                var progress = input.Data.MapTo<MonthlyProgress>();
                var existing = _progressRepository.Get()
                    .Where(u => u.ProcessInstanceId == task.ProcessInstanceId).FirstOrDefault();
                progress.Id = existing.Id;
                if (existing == null)
                {
                    throw new EntityException("MonthlyProgressInstanceId", task.ProcessInstanceId, "MonthlyProgress", "不存在");
                }
                if (editableFields == null)
                {
                    _progressRepository.Update(progress, existing, new System.Linq.Expressions.Expression<Func<MonthlyProgress, object>>[] {
                        u => u.DataState,
                        u=>u.Mid,
                        u=>u.ProcessInstanceId,
                    }, false);
                }
                else
                {
                    _progressRepository.Update(progress, editableFields.ToArray());
                }
            }
            if (!input.PreventCommit)
            {
                _taskProvider.Complete(input.Id, input.Comment);
            }
        }

        public int Update(StartProcessInput<UpdateMonthlyProgressInput> input)
        {
            NullableHelper.SetNull(input.Data);
            var variables = new Dictionary<string, object>();
            var userId = _authInfoProvider.GetCurrent().User.Id;

            var projectId = _progressRepository.Get().Where(u => u.Id == input.Data.Id).Select(u => u.ProjectId).FirstOrDefault();
            var list = _projectRepository.Get().Where(u => u.Id == input.Data.ProjectId).SelectMany(u => u.Members).Where(u => u.ProjectRole == "投资信息负责人").Select(u => u.UserId).ToList();
            if (!_projectHelper.HasPermission("投资信息负责人", projectId))
            {
                throw new AppCoreException("月投资更新没有权限");
            }

            var progress = input.Data.MapTo<MonthlyProgress>();
            if (_progressRepository.Count(u => u.ProjectId == projectId && u.RecordDate == progress.RecordDate && u.DataState == DataState.Stable && u.Id != progress.Id) > 0)
            {
                throw new EntityException("Date", progress.RecordDate, "PmMonthlyReport", "已存在");
            }

            variables.Add("starter", userId.ToString());
            int processInstanceId = _runtimeProvider.StartProcessInstanceByName(
                input.ProcessDefinitionName,
                $"{input.ProcessDefinitionName}-{input.Data.Year}-{input.Data.Month}",
                variables);
            var tasks = _taskProvider.GetByProcessInstance(processInstanceId);
            if (tasks.Count == 0)
            {
                throw new AppCoreException("任务未创建成功！");
            }
            progress.ProcessInstanceId = processInstanceId;
            progress.DataState = DataState.Updating;
            progress.Mid = progress.Id;
            progress.Id = 0;
            _progressRepository.Add(progress);
            var task = tasks[0];
            _taskProvider.Complete(task.Id);
            return progress.Id;
        }

        public void Delete(int id)
        {
            var projectId = _progressRepository.Get().Where(u => u.Id == id).Select(u => u.ProjectId).FirstOrDefault();
            if (!_projectHelper.HasPermission("投资信息负责人", projectId))
            {
                throw new AppCoreException("月投资删除没有权限");
            }
            _progressRepository.Delete(new MonthlyProgress { Id = id });
        }

        public MemoryStream Export(int pageIndex, int pageSize, string keyword)
        {
            throw new NotImplementedException();
        }

        public GetMonthlyProgressOutput Get(int id)
        {
            var progress = _progressRepository.Get()
                .Where(u => u.Id == id).FirstOrDefault();
            if (progress == null)
            {
                throw new EntityException("Id", id, "MonthlyProgress", "不存在");
            }
            return progress.MapTo<GetMonthlyProgressOutput>();
        }

        public PaginationData<GetMonthlyProgressListOutput> Get(int pageIndex, int pageSize, int? projectId, string keyword,
               string sortField,
               string sortState)
        {
            IQueryable<MonthlyProgress> query = _progressRepository.Get()
                .Where(u => u.DataState == DataState.Stable);
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(u => u.Project.Name.Contains(keyword));
            }
            if (projectId != null)
            {
                query = query.Where(u => u.ProjectId == projectId);
            }
            //sortState 1降序2升序0默认
            if (!string.IsNullOrEmpty(sortField) && !string.IsNullOrEmpty(sortState))
            {
                switch (sortField)
                {
                    case "Year"://年份
                        query = sortState == "1" ? query.OrderByDescending(u => u.RecordDate) : sortState == "2" ? query.OrderBy(u => u.RecordDate) : query.OrderByDescending(u => u.CreateTime);
                        break;
                    case "Month"://月份
                        query = sortState == "1" ? query.OrderByDescending(u => u.RecordDate) : sortState == "2" ? query.OrderBy(u => u.RecordDate) : query.OrderByDescending(u => u.CreateTime);
                        break;
                    case "CompletedDemolitionFee"://本月完成征拆费
                        query = sortState == "1" ? query.OrderByDescending(u => u.CompletedDemolitionFee) : sortState == "2" ? query.OrderBy(u => u.CompletedDemolitionFee) : query.OrderByDescending(u => u.CreateTime);
                        break;
                    case "CompletedProjectCosts"://本月完成工程费
                        query = sortState == "1" ? query.OrderByDescending(u => u.CompletedProjectCosts) : sortState == "2" ? query.OrderBy(u => u.CompletedProjectCosts) : query.OrderByDescending(u => u.CreateTime);
                        break;
                    case "Zhong"://本月完成投资
                        query = sortState == "1" ? query.OrderByDescending(u => u.CompletedDemolitionFee) : sortState == "2" ? query.OrderBy(u => u.CompletedDemolitionFee) : query.OrderByDescending(u => u.CreateTime);
                        query = sortState == "1" ? query.OrderByDescending(u => u.CompletedProjectCosts) : sortState == "2" ? query.OrderBy(u => u.CompletedProjectCosts) : query.OrderByDescending(u => u.CreateTime);
                        break;
                    default:
                        query = query.OrderByDescending(u => u.CreateTime);
                        break;
                }

            }
            else
            {
                query = query.OrderByDescending(u => u.CreateTime);
            }
            var paging = PaginationDataHelper.WrapData<MonthlyProgress, T>(query, pageIndex, pageSize).TransferTo<GetMonthlyProgressListOutput>();
            paging.Data.ForEach(u => _projectHelper.HasPermission("投资信息负责人", u.ProjectId));
            return paging;
        }

        public GetMonthlyProgressOutput GetByProcess(int processId)
        {
            var progress = _progressRepository.Get()
                .Where(u => u.ProcessInstanceId == processId).FirstOrDefault();
            if (progress == null)
            {
                throw new EntityException("ProcessInstanceId", processId, "MonthlyProgress", "不存在");
            }
            return progress.MapTo<GetMonthlyProgressOutput>();
        }

        public GetMonthlyProgressOutput GetByTask(int taskId)
        {
            var authInfo = _authInfoProvider.GetCurrent();
            var userId = authInfo?.User?.Id;
            var task = _taskProvider.GetTask(taskId);
            if (task.Assignee != userId.Value.ToString())
            {
                throw new AppCoreException($"id:为{userId.Value}的用户不是任务【{task.NodeName}】的委托人");
            }
            var progress = _progressRepository.Get()
                .Where(u => u.ProcessInstanceId == task.ProcessInstanceId).FirstOrDefault();
            if (progress == null)
            {
                throw new EntityException("ProcessInstanceId", task.ProcessInstanceId, "MonthlyProgress", "不存在");
            }
            return progress.MapTo<GetMonthlyProgressOutput>();
        }

        public GetMonthlyProgressOutput GetByTaskHistory(int taskHistoryId)
        {
            var authInfo = _authInfoProvider.GetCurrent();
            var userId = authInfo?.User?.Id;
            var task = _historyProvider.GetTaskHistory(taskHistoryId);
            if (!task.AssigneeList.Contains($"{userId}"))
            {
                throw new AppCoreException($"id:为{userId.Value}的用户不是任务【{task.NodeName}】的委托人");
            }
            var progress = _progressRepository.Get()
                .Where(u => u.ProcessInstanceId == task.ProcessInstanceId).FirstOrDefault();
            if (progress == null)
            {
                throw new EntityException("ProcessInstanceId", task.ProcessInstanceId, "MonthlyProgress", "不存在");
            }
            return progress.MapTo<GetMonthlyProgressOutput>();
        }

        public float GetCurrentProgress(int projectId)
        {
            var accumulateInvestment = _progressRepository.Get().Where(u => u.ProjectId == projectId && u.Mid == null).Sum(u => u.CompletedDemolitionFee + u.CompletedProjectCosts);
            var cost = _projectRepository.Get().Where(u => u.Id == projectId).Select(u => u.GeneralEstimate).FirstOrDefault();
            return cost == 0 ? 0 : 100 * (Convert.ToSingle(accumulateInvestment) / Convert.ToSingle(cost));
        }
    }
}
