using App.Base;
using App.Base.EntityFramework;
using App.Base.Repositories;
using App.Core.Authorization;
using App.Core.Authorization.Accounts;
using App.Core.Common;
using App.Core.Common.Entities;
using App.Core.Common.Exceptions;
using App.Core.Common.Operators;
using App.Core.Workflow;
using App.Core.Workflow.Engine.Definition.Nodes.Tasks;
using App.Core.Workflow.Providers;
using App.Problems.ProblemCoordinations;
using App.Problems.ProblemRectifications;
using App.Problems.ProblemRectifications.Dto;
using App.Problems.Problems;
using App.Problems.Problems.Dto;
using App.Projects.ProjectBaseInfos;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Oa.Project;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using PoorFff;
using PoorFff.EventBus;
using PoorFff.Excel;
using PoorFff.Mapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace App.Problems.PmProblems
{
    public class ProblemService: IProblemService
    {
        private IAuthInfoProvider _authInfoProvider;
        private IHistoryProvider _historyProvider;
        private IRuntimeProvider _runtimeProvider;
        private ITaskProvider _taskProvider;
        private IDefinitionProvider _definitionProvder;
        private IAppRepositoryBase<Problem> _problemRepository;
        private IAppRepositoryBase<ProblemCoordination> _coordinationRepository;
       // private IAppRepositoryBase<Project> _projectRepository;
        private IAppDbContextProvider _dbContextProvider;
        private IEventBus _eventBus;
        private IDbOperator _op;
        private IProjectHelper _projectHelper;
        public ProblemService(
             IAuthInfoProvider authInfoProvider,
            IAppRepositoryBase<Problem> problemRepository,
            IAppRepositoryBase<ProblemCoordination> coordinationRepository,
           //  IAppRepositoryBase<Project> projectRepository,
            IAppDbContextProvider dbContextProvider,
            IDbOperator op,
            IEventBus eventBus,
             IWfEngine wfEngine,
            IProjectHelper projectHelper
            )
        {
            _projectHelper = projectHelper;
            _authInfoProvider  = authInfoProvider;
            _problemRepository = problemRepository;
            _coordinationRepository = coordinationRepository;
           // _projectRepository = projectRepository;
            _dbContextProvider = dbContextProvider;
            _eventBus = eventBus;
            _op = op;
            _runtimeProvider = wfEngine.GetRuntimeProvider();
            _taskProvider = wfEngine.GetTaskProvider();
            _definitionProvder = wfEngine.GetDefinitionProvider();
            _historyProvider = wfEngine.GetHistoryProvider();
        }
        public int Add(StartProcessInput<AddProblemInput> input)
        {
            if (input.Data.ProjectId == 0)
            {
                throw new AppCoreException("项目id不能为0");
            }
            var variables = new Dictionary<string, object>();
            var userId = _authInfoProvider.GetCurrent().User.Id;
            if (!_projectHelper.HasPermission("存在问题负责人", input.Data.ProjectId))//权限设置
            {
                throw new AppCoreException("存在问题发布没有权限");
            }
            variables.Add("starter", userId.ToString());
            int processInstanceId = _runtimeProvider.StartProcessInstanceByName(
                input.ProcessDefinitionName,
                $"{input.ProcessDefinitionName}-{input.Data.Content}",
                variables);
            var tasks = _taskProvider.GetByProcessInstance(processInstanceId);
            if (tasks.Count == 0)
            {
                throw new AppCoreException("任务未创建成功！");
            }
            var problem = input.Data.MapTo<Problem>();
            // var count = AppSampleContext.Instance.GetSupplierNoCount(() => _supplierRepository.Count(u => u.CreateTime.Value.Year == DateTime.Now.Year && (u.State == DataState.Stable || u.State == DataState.Creating)));
            problem.ProcessInstanceId = processInstanceId;
            problem.State = DataState.Creating;
            // memorabiliaRecord.SupplierNo = $"{DateTime.Now.Year}{count.ToString("000000")}";
            problem.Mid = null;
            _problemRepository.Add(problem);
            var task = tasks[0];
            if (!input.PreventCommit)
                _taskProvider.Complete(task.Id);
            return problem.Id;
        }
        /// <summary>
        /// 最后一个审批结束执行
        /// </summary>
        /// <param name="projectInstanceId"></param>
        public void CompleteApproval(int projectInstanceId)
        {
            var project = _problemRepository.Get().Include(u => u.ProblemPhotoSets).Where(u => u.ProcessInstanceId == projectInstanceId).FirstOrDefault();
            if (project == null)
            {
                throw new EntityException("ProjectInstanceId", projectInstanceId, "Problem", "不存在");
            }
            if (project.State == DataState.Creating)
            {

                using (var transaction = _dbContextProvider.BeginTransaction())
                {

                    var temp = JsonConvert.DeserializeObject<Problem>(JsonConvert.SerializeObject(project));
                    temp.State = DataState.Stable;
                    temp.ProcessInstanceId = null;
                    _problemRepository.Update(temp, new System.Linq.Expressions.Expression<Func<Problem, object>>[] {
                        u => u.State,
                        u => u.ProcessInstanceId
                    }, true);
                    temp = JsonConvert.DeserializeObject<Problem>(JsonConvert.SerializeObject(project));
                    temp.State = DataState.Created;
                    temp.Id = 0;
                    temp.Mid = project.Id;
                    //清空之前附件
                    temp.ProblemPhotoSets.ForEach(u =>
                    {
                        u.Id = 0;
                    });
                    _problemRepository.Add(temp);
                    transaction.Commit();
                }
                AppBaseContext.Instance.Produce("add-project-briefing", JsonConvert.SerializeObject(new
                {
                    TenantId = _op.TenantId,
                    ProjectId = project.ProjectId
                }));

            }
            else if (project.Mid != null && project.State == DataState.Updating)
            {
                using (var transaction = _dbContextProvider.BeginTransaction())
                {
                    var temp = JsonConvert.DeserializeObject<Problem>(JsonConvert.SerializeObject(project));
                    temp.State = DataState.Updated;
                    _problemRepository.Update(temp, new System.Linq.Expressions.Expression<Func<Problem, object>>[] {
                        u=>u.State
                    }, true);
                    temp = JsonConvert.DeserializeObject<Problem>(JsonConvert.SerializeObject(project));
                    temp.Id = project.Mid.Value;
                    temp.Mid = null;
                    //清空之前附件
                    temp.ProblemPhotoSets.ForEach(u =>
                    {
                        u.Id = 0;
                    });
                    var existing = _problemRepository.Get().Include(u => u.ProblemPhotoSets).Where(u => u.Id == project.Mid).FirstOrDefault();
                    _problemRepository.Update(temp, existing, new System.Linq.Expressions.Expression<Func<Problem, object>>[] {
                        u=>u.ProcessInstanceId,
                        u=>u.State,
                        u=>u.ActualCompletionTime, // 实际完成时间
                         u=>u.CoordinationState//协调情况
                    }, false);
                    transaction.Commit();
                }
            }
            else
            {

                throw new AppCoreException("当前数据不再更新状态");
            }
        }
        /// <summary>
        /// 每次审核办理执行
        /// </summary>
        /// <param name="input"></param>
        public void CompleteTask(CompleteTaskInput<UpdateProblemInput> input)
        {
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
                var project = input.Data.MapTo<Problem>();
                if (editableFields == null)
                {
                    _problemRepository.Update(project, new System.Linq.Expressions.Expression<Func<Problem, object>>[] {
                        u => u.State,
                        u=>u.Mid,
                        u=>u.ProcessInstanceId
                    }, false);
                }
                else
                {
                    _problemRepository.Update(project, editableFields.ToArray());
                }
            }
            if (!input.PreventCommit)
            {
                _taskProvider.Complete(input.Id, input.Comment);
            }
        }

        public int Count(string name, int? id)
        {
            name = name.Trim();
            if (!string.IsNullOrEmpty(id.ToString()))
            {
                return _problemRepository.Count(u => u.Content == name && u.Id != id && u.State == DataState.Stable);
            }
            else
            {
                return _problemRepository.Count(u => u.Content == name && u.State == DataState.Stable);
            }
        }

        public PaginationData<GetProblemListOutput> Get(int? projectId,
               int pageIndex, int pageSize,
               int? categoryId,
              CoordinationState? coordinationState,
               string keyword,
                  string sortField,
               string sortState
            )
        {
            var query = _problemRepository.Get()
                .Include(u => u.Project)
                 .Include(u => u.Category)
                .Where(u => u.State == DataState.Stable);
            var userId = _authInfoProvider.GetCurrent()?.User?.Id ?? 0;

            #region query conditions
            if (!string.IsNullOrEmpty(projectId.ToString()))
            {
                query = query.Where(u => u.ProjectId == projectId);
            }
            if (categoryId != null)
                query = query.Where(u => u.CategoryId == categoryId);
            if (coordinationState != null)
                query = query.Where(u => u.CoordinationState == coordinationState);
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(u =>
                                    u.Project.Name.Contains(keyword) ||
                                    u.Project.No.Contains(keyword) ||
                                    u.Content.Contains(keyword)
                                    );
            }
        

            //sortState 1降序2升序0默认
            if (!string.IsNullOrEmpty(sortField) && !string.IsNullOrEmpty(sortState))
            {
                switch (sortField)
                {
                    case "CategoryId"://存在问题分类
                        query = sortState == "1" ? query.OrderByDescending(u => u.CategoryId) : sortState == "2" ? query.OrderBy(u => u.CategoryId) : query.OrderByDescending(u => u.CreateTime);
                        break;
                    case "Content"://问题内容
                        query = sortState == "1" ? query.OrderByDescending(u => u.Content) : sortState == "2" ? query.OrderBy(u => u.Content) : query.OrderByDescending(u => u.CreateTime);
                        break;
                    case "PlannedCompletionTime"://计划完成时间
                        query = sortState == "1" ? query.OrderByDescending(u => u.PlannedCompletionTime) : sortState == "2" ? query.OrderBy(u => u.PlannedCompletionTime) : query.OrderByDescending(u => u.CreateTime);
                        break;
                    case "ActualCompletionTime"://实际完成时间
                        query = sortState == "1" ? query.OrderByDescending(u => u.ActualCompletionTime) : sortState == "2" ? query.OrderBy(u => u.ActualCompletionTime) : query.OrderByDescending(u => u.CreateTime);
                        break;
                    case "CoordinationState"://协调情况
                        query = sortState == "1" ? query.OrderByDescending(u => u.CoordinationState) : sortState == "2" ? query.OrderBy(u => u.CoordinationState) : query.OrderByDescending(u => u.CreateTime);
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
            #endregion
            var paging = PaginationDataHelper.WrapData<Problem, T>(query, pageIndex, pageSize).TransferTo<GetProblemListOutput>();
            paging.Data.ForEach(u => u.HasPermission = _projectHelper.HasPermission("存在问题负责人", u.ProjectId));
            return paging;
        }
        public void Delete(int id)
        {
            var problem = _problemRepository.Get().Where(u => u.Id == id).FirstOrDefault();
            if (problem != null)
            {
                var userId = _authInfoProvider.GetCurrent().User.Id;
                if (!_projectHelper.HasPermission("存在问题负责人", problem.ProjectId))//权限设置
                {
                    throw new AppCoreException("存在问题删除没有权限");
                }
                _problemRepository.Delete(new Problem { Id = id });
            }

        }
        public MemoryStream Export(int? projectId,
             string title,
               Dictionary<string, string> comments,
               int? categoryId,
              CoordinationState? coordinationState,
               string keyword,
               List<int> memorabiliaApplicationIds

            )
        {
            // throw new NotImplementedException();
            var query = _problemRepository.Get()
               .Include(u => u.Project)
               .Where(u => u.State == DataState.Stable);
            var userId = _authInfoProvider.GetCurrent()?.User?.Id ?? 0;

            #region query conditions
            if (memorabiliaApplicationIds != null && memorabiliaApplicationIds.Count > 0)
            {
                query = query.Where(u => memorabiliaApplicationIds.Contains(u.Id));
            }
            else
            {
                if (!string.IsNullOrEmpty(projectId.ToString()))
                {
                    query = query.Where(u => u.ProjectId == projectId);
                }
                if (categoryId != null)
                    query = query.Where(u => u.CategoryId == categoryId);
                if (coordinationState != null)
                    query = query.Where(u => u.CoordinationState == coordinationState);
                if (!string.IsNullOrEmpty(keyword))
                {
                    query = query.Where(u =>
                                 u.Project.Name.Contains(keyword) ||
                                 u.Project.No.Contains(keyword) ||
                                 u.Content.Contains(keyword)
                                 );
                }
                if (!string.IsNullOrEmpty(projectId.ToString()))
                {
                    query = query.Where(u => u.ProjectId == projectId);
                }

            }

            #endregion
            query = query.OrderByDescending(u => u.CreateTime);
            //var userList = _userRepository.Get().Select(u => new { u.Id, u.Name }).ToList();
            //var organizationUnitList = _organizationUnitRepository.Get().Select(u => new { u.Id, u.Name }).ToList();
            var resList = query.ToList();
            var outputs = new List<ExportProblemOutput>();
            foreach (var res in resList)
            {
                var output = res.MapTo<ExportProblemOutput>();
                //output.ProposerName = userList.Where(u => u.Id == res.ProposerId).Select(u => u.Name).FirstOrDefault();
                //output.DepartmentName = organizationUnitList.Where(u => u.Id == res.DepartmentId).Select(u => u.Name).FirstOrDefault();
                outputs.Add(output);
            }
            return outputs.Entity2Excel(comments, title);
        }

        public GetProblemOutput Get(int id)
        {
            var accident = _problemRepository.Get().Where(u => u.Id == id)
                .Include(u => u.Coordinations)
                    .ThenInclude(u => u.CoordinationPhotoSets).ThenInclude(w => w.FileMeta)
                .Include(u => u.Project)
                 .Include(u => u.Category)
                .Include(u => u.ProblemPhotoSets).ThenInclude(w => w.FileMeta)
                .FirstOrDefault();
            if (accident == null)
            {
                throw new EntityException("Id", id, "Problem", "不存在");
            }
            else
            {

                var recordOutput = accident.MapTo<GetProblemOutput>();
                return recordOutput;
            }
        }
        /// <summary>
        /// 发起记录
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public GetProblemOutput GetByTask(int taskId)
        {
            var authInfo = _authInfoProvider.GetCurrent();
            var userId = authInfo?.User?.Id;
            var task = _taskProvider.GetTask(taskId);
            if (task.Assignee != userId.Value.ToString())
            {
                throw new AppCoreException($"id:为{userId.Value}的用户不是任务【{task.NodeName}】的委托人");
            }
            var accident = _problemRepository.Get()
               .Include(u => u.Coordinations)
                .ThenInclude(u => u.CoordinationPhotoSets).ThenInclude(w => w.FileMeta)
                .Include(u => u.Project)
                 .Include(u => u.Category)
                .Include(u => u.ProblemPhotoSets).ThenInclude(w => w.FileMeta).Where(u => u.ProcessInstanceId == task.ProcessInstanceId).FirstOrDefault();
            if (accident == null)
            {
                throw new EntityException("ProjectInstanceId", task.ProcessInstanceId, "Problem", "不存在");
            }
            var processDefinition = _definitionProvder.Get(task.ProcessDefinitionId);
            var invisibleFields = ((UserTaskNode)processDefinition.GetNode(task.NodeUid)).GetInvisibleFields();
            if (invisibleFields != null)
                TypeDefaulter.SetDefault(accident, invisibleFields.ToArray());

            return accident.MapTo<GetProblemOutput>();

        }
        /// <summary>
        /// 等待记录
        /// </summary>
        /// <param name="processId"></param>
        /// <returns></returns>
        public GetProblemOutput GetByProcess(int processId)
        {
            var accident = _problemRepository.Get()
                  .Include(u => u.Coordinations)
                .ThenInclude(u => u.CoordinationPhotoSets).ThenInclude(w => w.FileMeta)
                .Include(u => u.Project)
                 .Include(u => u.Category)
                .Include(u => u.ProblemPhotoSets).ThenInclude(w => w.FileMeta).Where(u => u.ProcessInstanceId == processId).FirstOrDefault();
            if (accident == null)
            {
                throw new EntityException("ProjectInstanceId", processId, "Problem", "不存在");
            }

            return accident.MapTo<GetProblemOutput>();

        }


        /// <summary>
        /// 处理记录
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public GetProblemOutput GetByTaskHistory(int taskId)
        {
            var authInfo = _authInfoProvider.GetCurrent();
            var userId = authInfo?.User?.Id;
            var task = _historyProvider.GetTaskHistory(taskId);
            if (!task.AssigneeList.Contains($"{userId}"))
            {
                throw new AppCoreException($"id:为{userId.Value}的用户不是任务【{task.NodeName}】的委托人");
            }
            var accident = _problemRepository.Get()
                 .Include(u => u.Coordinations)
                .ThenInclude(u => u.CoordinationPhotoSets).ThenInclude(w => w.FileMeta)
                .Include(u => u.Project)
                 .Include(u => u.Category)
                .Include(u => u.ProblemPhotoSets).ThenInclude(w => w.FileMeta).Where(u => u.ProcessInstanceId == task.ProcessInstanceId).FirstOrDefault();
            if (accident == null)
            {
                throw new EntityException("ProjectInstanceId", task.ProcessInstanceId, "Problem", "不存在");
            }
            var processDefinition = _definitionProvder.Get(task.ProcessDefinitionId);
            var invisibleFields = ((UserTaskNode)processDefinition.GetNode(task.NodeUid)).GetInvisibleFields();
            if (invisibleFields != null)
                TypeDefaulter.SetDefault(accident, invisibleFields.ToArray());

            return accident.MapTo<GetProblemOutput>();

        }

        public int Update(StartProcessInput<UpdateProblemInput> input)
        {
            var variables = new Dictionary<string, object>();
            var userId = _authInfoProvider.GetCurrent().User.Id;
            if (!_projectHelper.HasPermission("存在问题负责人", input.Data.ProjectId))//权限设置
            {
                throw new AppCoreException("存在问题修改没有权限");
            }
            variables.Add("starter", userId.ToString());
            int processInstanceId = _runtimeProvider.StartProcessInstanceByName(
                input.ProcessDefinitionName,
                $"{input.ProcessDefinitionName}-{input.Data.Content}",
                variables);
            var tasks = _taskProvider.GetByProcessInstance(processInstanceId);
            if (tasks.Count == 0)
            {
                throw new AppCoreException("任务未创建成功！");
            }
            var record = input.Data.MapTo<Problem>();
            record.ProcessInstanceId = processInstanceId;
            record.State = DataState.Updating;
            record.Mid = record.Id;
            record.Id = 0;
            _problemRepository.Add(record);
            var task = tasks[0];
            _taskProvider.Complete(task.Id);
            return record.Id;
        }
       

        public int AddCoordination(int problemId, AddProblemCoordinationInput input)
        {
            var rectification = input.MapTo<ProblemCoordination>();
            rectification.ProblemId = problemId;
            using (var transaction = _dbContextProvider.BeginTransaction())
            {
                _problemRepository.Update(
                    new Problem
                    {
                        Id = problemId,
                        CoordinationState = CoordinationState.Underway,
                        ActualCompletionTime = null
                    },
                    new System.Linq.Expressions.Expression<Func<Problem, object>>[] { u => u.CoordinationState, u => u.ActualCompletionTime });
                _coordinationRepository.Add(rectification);
                transaction.Commit();
            }
            return rectification.Id;
        }

        public void CompleteCoordination(CompleteProblemInput input)
        {
            var problem = input.MapTo<Problem>();
            problem.CoordinationState = CoordinationState.Completed;
            _problemRepository.Update(problem,
                new System.Linq.Expressions.Expression<Func<Problem, object>>[] {
                    u =>u.ActualCompletionTime,
                    u=>u.CoordinationState
                });
            AppBaseContext.Instance.Produce("add-project-briefing", JsonConvert.SerializeObject(new {
                TenantId = _op.TenantId,
                ProjectId = problem.ProjectId
            }));
        }
    }
}
