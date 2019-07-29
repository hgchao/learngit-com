using App.Base;
using App.Base.EntityFramework;
using App.Base.Repositories;
using App.Core.Authorization.Accounts;
using App.Core.Common;
using App.Core.Common.Entities;
using App.Core.Common.Exceptions;
using App.Core.Common.Operators;
using App.Core.Workflow;
using App.Core.Workflow.Engine.Definition.Nodes.Tasks;
using App.Core.Workflow.Providers;
using App.Problems;
using App.Problems.Problems;
using App.Projects.ProjectBaseInfos;
using App.Quality.QualityProblemRectifications;
using App.Quality.QualityProblemRectifications.Dto;
using App.Quality.QualityProblems.Dto;
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

namespace App.Quality.QualityProblems
{
    public class QualityProblemService: IQualityProblemService
    {
        private IAuthInfoProvider _authInfoProvider;
        private IHistoryProvider _historyProvider;
        private IRuntimeProvider _runtimeProvider;
        private ITaskProvider _taskProvider;
        private IDefinitionProvider _definitionProvder;
        private IAppRepositoryBase<QualityProblem> _problemRepository;
        private IAppRepositoryBase<QualityProblemRectification> _rectificationRepository;
        // private IAppRepositoryBase<Project> _projectRepository;
        private IAppDbContextProvider _dbContextProvider;
        private IEventBus _eventBus;
        private IDbOperator _op;
        private IProjectHelper _projectHelper;
        public QualityProblemService(
             IAuthInfoProvider authInfoProvider,
            IAppRepositoryBase<QualityProblem> problemRepository,
            IAppRepositoryBase<QualityProblemRectification> rectificationRepository,
              // IAppRepositoryBase<Project> projectRepository,
            IAppDbContextProvider dbContextProvider,
            IDbOperator op,
            IEventBus eventBus,
             IWfEngine wfEngine,
             IProjectHelper projectHelper
            )
        {
            _projectHelper = projectHelper;
            _authInfoProvider = authInfoProvider;
            _problemRepository = problemRepository;
            _rectificationRepository = rectificationRepository;
            //_projectRepository = projectRepository;
            _dbContextProvider = dbContextProvider;
            _eventBus = eventBus;
            _op = op;
            _runtimeProvider = wfEngine.GetRuntimeProvider();
            _taskProvider = wfEngine.GetTaskProvider();
            _definitionProvder = wfEngine.GetDefinitionProvider();
            _historyProvider = wfEngine.GetHistoryProvider();
        }
        public int Add(StartProcessInput<AddQualityProblemInput> input)
        {
            if (input.Data.Source == null)
            {
                throw new AppCoreException("质量问题来源不能为空");
            }
            var variables = new Dictionary<string, object>();
            var userId = _authInfoProvider.GetCurrent().User.Id;
            if (!_projectHelper.HasPermission("质量信息负责人", input.Data.ProjectId))//权限设置
            {
                throw new AppCoreException("质量问题发布没有权限");
            }
            variables.Add("starter", userId.ToString());
            int processInstanceId = _runtimeProvider.StartProcessInstanceByName(
                input.ProcessDefinitionName,
                $"{input.ProcessDefinitionName}-{input.Data.Source}",
                variables);
            var tasks = _taskProvider.GetByProcessInstance(processInstanceId);
            if (tasks.Count == 0)
            {
                throw new AppCoreException("任务未创建成功！");
            }
            var qualityProblem = input.Data.MapTo<QualityProblem>();
            // var count = AppSampleContext.Instance.GetSupplierNoCount(() => _supplierRepository.Count(u => u.CreateTime.Value.Year == DateTime.Now.Year && (u.State == DataState.Stable || u.State == DataState.Creating)));
            qualityProblem.ProcessInstanceId = processInstanceId;
            qualityProblem.State = DataState.Creating;
            // memorabiliaRecord.SupplierNo = $"{DateTime.Now.Year}{count.ToString("000000")}";
            qualityProblem.Mid = null;
            _problemRepository.Add(qualityProblem);
            var task = tasks[0];
            if (!input.PreventCommit)
                _taskProvider.Complete(task.Id);
            return qualityProblem.Id;
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
                throw new EntityException("ProjectInstanceId", projectInstanceId, "QualityProblem", "不存在");
            }
            if (project.State == DataState.Creating)
            {

                using (var transaction = _dbContextProvider.BeginTransaction())
                {

                    var temp = JsonConvert.DeserializeObject<QualityProblem>(JsonConvert.SerializeObject(project));
                    temp.State = DataState.Stable;
                    temp.ProcessInstanceId = null;
                    _problemRepository.Update(temp, new System.Linq.Expressions.Expression<Func<QualityProblem, object>>[] {
                        u => u.State,
                        u => u.ProcessInstanceId
                    }, true);
                    temp = JsonConvert.DeserializeObject<QualityProblem>(JsonConvert.SerializeObject(project));
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
                    var temp = JsonConvert.DeserializeObject<QualityProblem>(JsonConvert.SerializeObject(project));
                    temp.State = DataState.Updated;
                    _problemRepository.Update(temp, new System.Linq.Expressions.Expression<Func<QualityProblem, object>>[] {
                        u=>u.State
                    }, true);
                    temp = JsonConvert.DeserializeObject<QualityProblem>(JsonConvert.SerializeObject(project));
                    temp.Id = project.Mid.Value;
                    temp.Mid = null;
                    //清空之前附件
                    temp.ProblemPhotoSets.ForEach(u =>
                    {
                        u.Id = 0;
                    });
                    var existing = _problemRepository.Get().Include(u => u.ProblemPhotoSets).Where(u => u.Id == project.Mid).FirstOrDefault();
                    _problemRepository.Update(temp, existing, new System.Linq.Expressions.Expression<Func<QualityProblem, object>>[] {
                        u=>u.ProcessInstanceId,
                        u=>u.State,
                        u=>u.CompletionTime, // 整改完成时间
                        u=>u.CompletionPhotoSets, // 整改完成图片
                         u=>u.RectificationState, //整改情况
                        u=>u.Rectifications // 整改进展
                    }, false);
                    transaction.Commit();
                }
                AppBaseContext.Instance.Produce("add-project-briefing", JsonConvert.SerializeObject(new
                {
                    TenantId = _op.TenantId,
                    ProjectId = project.ProjectId
                }));
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
        public void CompleteTask(CompleteTaskInput<UpdateQualityProblemInput> input)
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
                var project = input.Data.MapTo<QualityProblem>();
                if (editableFields == null)
                {
                    _problemRepository.Update(project, new System.Linq.Expressions.Expression<Func<QualityProblem, object>>[] {
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
                return _problemRepository.Count(u => u.Source == name && u.Id != id && u.State == DataState.Stable);
            }
            else
            {
                return _problemRepository.Count(u => u.Source == name && u.State == DataState.Stable);
            }
        }

        public PaginationData<GetQualityProblemListOutput> Get(int? projectId,
               int pageIndex, int pageSize,
               RectificationState? rectificationState,
               string keyword,
                 int? categoryId,
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
            if (categoryId != null)
            {
                query = query.Where(u => u.CategoryId == categoryId);
            }
            if (rectificationState != null)
                query = query.Where(u => u.RectificationState == rectificationState);
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(u =>
                                    u.Project.Name.Contains(keyword) ||
                                    u.Project.No.Contains(keyword) ||
                                    u.Source.Contains(keyword) ||
                                    u.Description.Contains(keyword)
                                    );
            }
            if (!string.IsNullOrEmpty(projectId.ToString()))
            {
                query = query.Where(u => u.ProjectId == projectId);
            }

            //sortState 1降序2升序0默认
            if (!string.IsNullOrEmpty(sortField) && !string.IsNullOrEmpty(sortState))
            {
                switch (sortField)
                {
                    case "CategoryId"://质量问题分类
                        query = sortState == "1" ? query.OrderByDescending(u => u.CategoryId) : sortState == "2" ? query.OrderBy(u => u.CategoryId) : query.OrderByDescending(u => u.CreateTime);
                        break;
                    case "Source"://质量问题来源
                        query = sortState == "1" ? query.OrderByDescending(u => u.Source) : sortState == "2" ? query.OrderBy(u => u.Source) : query.OrderByDescending(u => u.CreateTime);
                        break;
                    case "CreateTime"://创建时间
                        query = sortState == "1" ? query.OrderByDescending(u => u.CreateTime) : sortState == "2" ? query.OrderBy(u => u.CreateTime) : query.OrderByDescending(u => u.CreateTime);
                        break;
                    case "RectificationState"://整改情况
                        query = sortState == "1" ? query.OrderByDescending(u => u.RectificationState) : sortState == "2" ? query.OrderBy(u => u.RectificationState) : query.OrderByDescending(u => u.CreateTime);
                        break;
                    case "CompletionTime"://整改完成时间
                        query = sortState == "1" ? query.OrderByDescending(u => u.CompletionTime) : sortState == "2" ? query.OrderBy(u => u.CompletionTime) : query.OrderByDescending(u => u.CreateTime);
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
            var paging = PaginationDataHelper.WrapData<QualityProblem, T>(query, pageIndex, pageSize).TransferTo<GetQualityProblemListOutput>();
            paging.Data.ForEach(u => u.HasPermission = _projectHelper.HasPermission("质量信息负责人", u.ProjectId));
            return paging;
        }
        public void Delete(int id)
        {
            var problem = _problemRepository.Get().Where(u => u.Id == id).FirstOrDefault();
            if (problem != null)
            {
                var userId = _authInfoProvider.GetCurrent().User.Id;
                if (!_projectHelper.HasPermission("质量信息负责人", problem.ProjectId))//权限设置
                {
                    throw new AppCoreException("质量问题删除没有权限");
                }
                _problemRepository.Delete(new QualityProblem { Id = id });
            }

        }
        public MemoryStream Export(int? projectId,
             string title,
               Dictionary<string, string> comments,
                RectificationState? rectificationState,
               string keyword,
               int? categoryId,
               List<int> memorabiliaApplicationIds

            )
        {
            // throw new NotImplementedException();
            var query = _problemRepository.Get()
               .Include(u => u.Project)
                .Include(u => u.Category)
               .Where(u => u.State == DataState.Stable);
            var userId = _authInfoProvider.GetCurrent()?.User?.Id ?? 0;

            #region query conditions
            if (memorabiliaApplicationIds != null && memorabiliaApplicationIds.Count > 0)
            {
                query = query.Where(u => memorabiliaApplicationIds.Contains(u.Id));
            }
            else
            {
                if (categoryId != null)
                {
                    query = query.Where(u => u.CategoryId == categoryId);
                }
                if (rectificationState != null)
                    query = query.Where(u => u.RectificationState == rectificationState);
                if (!string.IsNullOrEmpty(keyword))
                {
                    query = query.Where(u =>
                                 u.Project.Name.Contains(keyword) ||
                                 u.Project.No.Contains(keyword) ||
                                 u.Source.Contains(keyword) ||
                                 u.Description.Contains(keyword)
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
            var outputs = new List<ExportQualityProblemOutput>();
            foreach (var res in resList)
            {
                var output = res.MapTo<ExportQualityProblemOutput>();
                //output.ProposerName = userList.Where(u => u.Id == res.ProposerId).Select(u => u.Name).FirstOrDefault();
                //output.DepartmentName = organizationUnitList.Where(u => u.Id == res.DepartmentId).Select(u => u.Name).FirstOrDefault();
                outputs.Add(output);
            }
            return outputs.Entity2Excel(comments, title);
        }

        public GetQualityProblemOutput Get(int id)
        {
            var problem = _problemRepository.Get().Where(u => u.Id == id)
                      .Include(u => u.Rectifications)
                       .ThenInclude(u => u.RectificationPhotoSets).ThenInclude(w => w.FileMeta)
                        .Include(u => u.Project)
                        .Include(u => u.Category)
                        .Include(u => u.ProblemPhotoSets).ThenInclude(w => w.FileMeta)
                        .Include(u => u.CompletionPhotoSets).ThenInclude(w => w.FileMeta)
                        .FirstOrDefault();
            if (problem == null)
            {
                throw new EntityException("Id", id, "QualityProblem", "不存在");
            }
            else
            {

                var recordOutput = problem.MapTo<GetQualityProblemOutput>();
                return recordOutput;
            }
        }
        /// <summary>
        /// 发起记录
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public GetQualityProblemOutput GetByTask(int taskId)
        {
            var authInfo = _authInfoProvider.GetCurrent();
            var userId = authInfo?.User?.Id;
            var task = _taskProvider.GetTask(taskId);
            if (task.Assignee != userId.Value.ToString())
            {
                throw new AppCoreException($"id:为{userId.Value}的用户不是任务【{task.NodeName}】的委托人");
            }
            var problem = _problemRepository.Get()
                .Include(u => u.Rectifications)
                       .ThenInclude(u => u.RectificationPhotoSets).ThenInclude(w => w.FileMeta)
                        .Include(u => u.Project)
                        .Include(u => u.Category)
                        .Include(u => u.ProblemPhotoSets).ThenInclude(w => w.FileMeta)
                        .Include(u => u.CompletionPhotoSets).ThenInclude(w => w.FileMeta).Where(u => u.ProcessInstanceId == task.ProcessInstanceId).FirstOrDefault();
            if (problem == null)
            {
                throw new EntityException("ProjectInstanceId", task.ProcessInstanceId, "QualityProblem", "不存在");
            }
            var processDefinition = _definitionProvder.Get(task.ProcessDefinitionId);
            var invisibleFields = ((UserTaskNode)processDefinition.GetNode(task.NodeUid)).GetInvisibleFields();
            if (invisibleFields != null)
                TypeDefaulter.SetDefault(problem, invisibleFields.ToArray());

            return problem.MapTo<GetQualityProblemOutput>();

        }
        /// <summary>
        /// 等待记录
        /// </summary>
        /// <param name="processId"></param>
        /// <returns></returns>
        public GetQualityProblemOutput GetByProcess(int processId)
        {
            var problem = _problemRepository.Get()
                  .Include(u => u.Rectifications)
                       .ThenInclude(u => u.RectificationPhotoSets).ThenInclude(w => w.FileMeta)
                        .Include(u => u.Project)
                        .Include(u => u.Category)
                        .Include(u => u.ProblemPhotoSets).ThenInclude(w => w.FileMeta)
                        .Include(u => u.CompletionPhotoSets).ThenInclude(w => w.FileMeta).Where(u => u.ProcessInstanceId == processId).FirstOrDefault();
            if (problem == null)
            {
                throw new EntityException("ProjectInstanceId", processId, "QualityProblem", "不存在");
            }

            return problem.MapTo<GetQualityProblemOutput>();

        }


        /// <summary>
        /// 处理记录
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public GetQualityProblemOutput GetByTaskHistory(int taskId)
        {
            var authInfo = _authInfoProvider.GetCurrent();
            var userId = authInfo?.User?.Id;
            var task = _historyProvider.GetTaskHistory(taskId);
            if (!task.AssigneeList.Contains($"{userId}"))
            {
                throw new AppCoreException($"id:为{userId.Value}的用户不是任务【{task.NodeName}】的委托人");
            }
            var problem = _problemRepository.Get()
                  .Include(u => u.Rectifications)
                       .ThenInclude(u => u.RectificationPhotoSets).ThenInclude(w => w.FileMeta)
                        .Include(u => u.Project)
                        .Include(u => u.Category)
                        .Include(u => u.ProblemPhotoSets).ThenInclude(w => w.FileMeta)
                        .Include(u => u.CompletionPhotoSets).ThenInclude(w => w.FileMeta).Where(u => u.ProcessInstanceId == task.ProcessInstanceId).FirstOrDefault();
            if (problem == null)
            {
                throw new EntityException("ProjectInstanceId", task.ProcessInstanceId, "QualityProblem", "不存在");
            }
            var processDefinition = _definitionProvder.Get(task.ProcessDefinitionId);
            var invisibleFields = ((UserTaskNode)processDefinition.GetNode(task.NodeUid)).GetInvisibleFields();
            if (invisibleFields != null)
                TypeDefaulter.SetDefault(problem, invisibleFields.ToArray());

            return problem.MapTo<GetQualityProblemOutput>();

        }

        public int Update(StartProcessInput<UpdateQualityProblemInput> input)
        {
            var variables = new Dictionary<string, object>();
            var userId = _authInfoProvider.GetCurrent().User.Id;
            if (!_projectHelper.HasPermission("质量信息负责人", input.Data.ProjectId))//权限设置
            {
                throw new AppCoreException("质量问题修改没有权限");
            }
            variables.Add("starter", userId.ToString());
            int processInstanceId = _runtimeProvider.StartProcessInstanceByName(
                input.ProcessDefinitionName,
                $"{input.ProcessDefinitionName}-{input.Data.Source}",
                variables);
            var tasks = _taskProvider.GetByProcessInstance(processInstanceId);
            if (tasks.Count == 0)
            {
                throw new AppCoreException("任务未创建成功！");
            }
            var record = input.Data.MapTo<QualityProblem>();
            record.ProcessInstanceId = processInstanceId;
            record.State = DataState.Updating;
            record.Mid = record.Id;
            record.Id = 0;
            _problemRepository.Add(record);
            var task = tasks[0];
            _taskProvider.Complete(task.Id);
            return record.Id;
        }

       

        public int AddRectification(int problemId, AddQualityProblemRectificationInput input)
        {
            var existing = _problemRepository.Get().Where(u => u.Id == problemId).FirstOrDefault();
            var rectification = input.MapTo<QualityProblemRectification>();
            rectification.QualityProblemId = problemId;
            using (var transaction = _dbContextProvider.BeginTransaction())
            {
                _problemRepository.Update(
                    new QualityProblem
                    {
                        Id = problemId,
                        RectificationState = RectificationState.Underway,
                        CompletionTime = null
                    },
                    new System.Linq.Expressions.Expression<Func<QualityProblem, object>>[] {
                        u => u.RectificationState,
                        u => u.CompletionTime
                    });
                _rectificationRepository.Add(rectification);
                transaction.Commit();
            }
            AppBaseContext.Instance.Produce("add-project-briefing", JsonConvert.SerializeObject(new
            {
                TenantId = _op.TenantId,
                ProjectId = existing.ProjectId
            }));
            return rectification.Id;
        }

        public void CompleteRetification(CompleteQualityProblemInput input)
        {
            var existing = _problemRepository.Get().Include(u => u.CompletionPhotoSets).Where(u => u.Id == input.Id).FirstOrDefault();
            var problem = input.MapTo<QualityProblem>();
            problem.RectificationState = RectificationState.Completed;
            _problemRepository.Update(problem, existing, new System.Linq.Expressions.Expression<Func<QualityProblem, object>>[] {
                        u=>u.CompletionTime,
                        u=>u.RectificationState,
                        u=>u.CompletionPhotoSets
                    });
            AppBaseContext.Instance.Produce("add-project-briefing", JsonConvert.SerializeObject(new
            {
                TenantId = _op.TenantId,
                ProjectId = existing.ProjectId
            }));
            //_problemRepository.UpdateWithRelatedEntity(problem, true,
            //    new System.Linq.Expressions.Expression<Func<QualityProblem, object>>[] {
            //        u =>u.CompletionTime,
            //        u=>u.RectificationState
            //    });
        }

        public GetQProblemCountOutput GetCount(DateTime? createDateL, DateTime? createDateR)
        {
            var query = _problemRepository.Get().Where(u => u.State == DataState.Stable&&u.RectificationState != RectificationState.Completed);

            if (createDateL != null)
            {
                query = query.Where(u => u.CreateTime >= createDateL);
            }
            if (createDateR != null)
            {
                query = query.Where(u => u.CreateTime <= createDateR);
            }

            var countOutput = new GetQProblemCountOutput();
            countOutput.QualityProblemCount = query.ToList().Count();//质量问题个数
          
            return countOutput;
        }
    }
}
