using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
using App.Projects.ProjectBaseInfos;
using App.Safety.SafetyAccidentDisposals;
using App.Safety.SafetyAccidentDisposals.Dto;
using App.Safety.SafetyAccidents.Dto;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Oa.Project;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using PoorFff;
using PoorFff.Enums;
using PoorFff.EventBus;
using PoorFff.Excel;
using PoorFff.Mapper;

namespace App.Safety.SafetyAccidents
{
    public class SafetyAccidentService : ISafetyAccidentService
    {
        private IAuthInfoProvider _authInfoProvider;
        private IHistoryProvider _historyProvider;
        private IRuntimeProvider _runtimeProvider;
        private ITaskProvider _taskProvider;
        private IDefinitionProvider _definitionProvder;
        private IAppRepositoryBase<SafetyAccident> _accidentRepository;
        private IAppRepositoryBase<SafetyAccidentDisposal> _disposalRepository;
       // private IAppRepositoryBase<Project> _projectRepository;
        private IAppDbContextProvider _dbContextProvider;
        private IDbOperator _op;
        private IEventBus _eventBus;
        private IProjectHelper _projectHelper;
        public SafetyAccidentService(
            IAuthInfoProvider authInfoProvider,
            IAppRepositoryBase<SafetyAccident> accidentRepository,
            IAppRepositoryBase<SafetyAccidentDisposal> disposalRepository,
            //IAppRepositoryBase<Project> projectRepository,
            IAppDbContextProvider dbContextProvider,
                IDbOperator op,
            IEventBus eventBus,
            IWfEngine wfEngine,
             IProjectHelper projectHelper
            )
        {
            _projectHelper = projectHelper;
            _authInfoProvider = authInfoProvider;
            _accidentRepository = accidentRepository;
            _disposalRepository = disposalRepository;
            //_projectRepository = projectRepository;
            _dbContextProvider = dbContextProvider;
            _eventBus = eventBus;
            _op = op;
            _runtimeProvider = wfEngine.GetRuntimeProvider();
            _taskProvider = wfEngine.GetTaskProvider();
            _definitionProvder = wfEngine.GetDefinitionProvider();
            _historyProvider = wfEngine.GetHistoryProvider();
        }
        public int Add(StartProcessInput<AddSafetyAccidentInput> input)
        {
            if (input.Data.Title == null)
            {
                throw new AppCoreException("安全事故标题不能为空");
            }
            var variables = new Dictionary<string, object>();
            var userId = _authInfoProvider.GetCurrent().User.Id;
            if (!_projectHelper.HasPermission("安全信息负责人", input.Data.ProjectId))//权限设置
            {
                throw new AppCoreException("安全事故发布没有权限");
            }
            variables.Add("starter", userId.ToString());
            int processInstanceId = _runtimeProvider.StartProcessInstanceByName(
                input.ProcessDefinitionName,
                $"{input.ProcessDefinitionName}-{input.Data.Title}",
                variables);
            var tasks = _taskProvider.GetByProcessInstance(processInstanceId);
            if (tasks.Count == 0)
            {
                throw new AppCoreException("任务未创建成功！");
            }
            var accident = input.Data.MapTo<SafetyAccident>();
            // var count = AppSampleContext.Instance.GetSupplierNoCount(() => _supplierRepository.Count(u => u.CreateTime.Value.Year == DateTime.Now.Year && (u.State == DataState.Stable || u.State == DataState.Creating)));
            accident.ProcessInstanceId = processInstanceId;
            accident.State = DataState.Creating;
            // memorabiliaRecord.SupplierNo = $"{DateTime.Now.Year}{count.ToString("000000")}";
            accident.Mid = null;
            _accidentRepository.Add(accident);
            var task = tasks[0];
            if (!input.PreventCommit)
                _taskProvider.Complete(task.Id);
            return accident.Id;
        }
        /// <summary>
        /// 最后一个审批结束执行
        /// </summary>
        /// <param name="projectInstanceId"></param>
        public void CompleteApproval(int projectInstanceId)
        {
            var project = _accidentRepository.Get().Include(u => u.AccidentPhotoSets).Where(u => u.ProcessInstanceId == projectInstanceId).FirstOrDefault();
            if (project == null)
            {
                throw new EntityException("ProjectInstanceId", projectInstanceId, "SafetyAccident", "不存在");
            }
            if (project.State == DataState.Creating)
            {

                using (var transaction = _dbContextProvider.BeginTransaction())
                {

                    var temp = JsonConvert.DeserializeObject<SafetyAccident>(JsonConvert.SerializeObject(project));
                    temp.State = DataState.Stable;
                    temp.ProcessInstanceId = null;
                    _accidentRepository.Update(temp, new System.Linq.Expressions.Expression<Func<SafetyAccident, object>>[] {
                        u => u.State,
                        u => u.ProcessInstanceId
                    }, true);
                    temp = JsonConvert.DeserializeObject<SafetyAccident>(JsonConvert.SerializeObject(project));
                    temp.State = DataState.Created;
                    temp.Id = 0;
                    temp.Mid = project.Id;
                    //清空之前附件
                    temp.AccidentPhotoSets.ForEach(u =>
                    {
                        u.Id = 0;
                    });
                    _accidentRepository.Add(temp);
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
                    var temp = JsonConvert.DeserializeObject<SafetyAccident>(JsonConvert.SerializeObject(project));
                    temp.State = DataState.Updated;
                    _accidentRepository.Update(temp, new System.Linq.Expressions.Expression<Func<SafetyAccident, object>>[] {
                        u=>u.State
                    }, true);
                    temp = JsonConvert.DeserializeObject<SafetyAccident>(JsonConvert.SerializeObject(project));
                    temp.Id = project.Mid.Value;
                    temp.Mid = null;
                    //清空之前附件
                    temp.AccidentPhotoSets.ForEach(u =>
                    {
                        u.Id = 0;
                    });
                    var existing = _accidentRepository.Get().Include(u => u.AccidentPhotoSets).Where(u => u.Id == project.Mid).FirstOrDefault();
                    _accidentRepository.Update(temp, existing, new System.Linq.Expressions.Expression<Func<SafetyAccident, object>>[] {
                        u=>u.ProcessInstanceId,
                        u=>u.State,
                        u=>u.SettlementTime, // 解决的时间
                        u=>u.SettlementPhotoSets, // 解决后图片
                         u=>u.DisposalState, //处置状态
                        u=>u.Disposals // 处置进展
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
        public void CompleteTask(CompleteTaskInput<UpdateSafetyAccidentInput> input)
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
                var project = input.Data.MapTo<SafetyAccident>();
                if (editableFields == null)
                {
                    _accidentRepository.Update(project, new System.Linq.Expressions.Expression<Func<SafetyAccident, object>>[] {
                        u => u.State,
                        u=>u.Mid,
                        u=>u.ProcessInstanceId
                    }, false);
                }
                else
                {
                    _accidentRepository.Update(project, editableFields.ToArray());
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
                return _accidentRepository.Count(u => u.Title == name && u.Id != id && u.State == DataState.Stable);
            }
            else
            {
                return _accidentRepository.Count(u => u.Title == name && u.State == DataState.Stable);
            }
        }

        public PaginationData<GetSafetyAccidentListOutput> Get(int? projectId,
               int pageIndex, int pageSize,
               DisposalState? state,
               string keyword,
                  string sortField,
               string sortState
            )
        {
            var query = _accidentRepository.Get()
                .Include(u => u.Project)
                .Include(u => u.Source)
                .Include(u => u.Category)
                .Include(u => u.Severity)
                .Where(u => u.State == DataState.Stable);
            var userId = _authInfoProvider.GetCurrent()?.User?.Id ?? 0;

            #region query conditions
            if (state != null)
                query = query.Where(u => u.DisposalState == state);
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(u =>
                                    u.Project.Name.Contains(keyword) ||
                                    u.Project.No.Contains(keyword) ||
                                    u.Title.Contains(keyword) ||
                                    u.Content.Contains(keyword)
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
                    case "Title"://安全事故标题
                        query = sortState == "1" ? query.OrderByDescending(u => u.Title) : sortState == "2" ? query.OrderBy(u => u.Title) : query.OrderByDescending(u => u.CreateTime);
                        break;
                    case "Content"://事故内容
                        query = sortState == "1" ? query.OrderByDescending(u => u.Content) : sortState == "2" ? query.OrderBy(u => u.Content) : query.OrderByDescending(u => u.CreateTime);
                        break;
                    case "DiscoveryTime"://事故发现时间
                        query = sortState == "1" ? query.OrderByDescending(u => u.DiscoveryTime) : sortState == "2" ? query.OrderBy(u => u.DiscoveryTime) : query.OrderByDescending(u => u.CreateTime);
                        break;
                    case "DisposalState"://处置状态
                        query = sortState == "1" ? query.OrderByDescending(u => u.DisposalState) : sortState == "2" ? query.OrderBy(u => u.DisposalState) : query.OrderByDescending(u => u.CreateTime);
                        break;
                    case "SettlementTime"://解决的时间
                        query = sortState == "1" ? query.OrderByDescending(u => u.SettlementTime) : sortState == "2" ? query.OrderBy(u => u.SettlementTime) : query.OrderByDescending(u => u.CreateTime);
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
            var paging = PaginationDataHelper.WrapData<SafetyAccident, T>(query, pageIndex, pageSize).TransferTo<GetSafetyAccidentListOutput>();
            paging.Data.ForEach(u => u.HasPermission = _projectHelper.HasPermission("安全信息负责人", u.ProjectId));
            return paging;
        }
        public void Delete(int id)
        {
            var problem = _accidentRepository.Get().Where(u => u.Id == id).FirstOrDefault();
            if (problem != null)
            {
                var userId = _authInfoProvider.GetCurrent().User.Id;
                if (!_projectHelper.HasPermission("安全信息负责人", problem.ProjectId))//权限设置
                {
                    throw new AppCoreException("安全事故删除没有权限");
                }
                _accidentRepository.Delete(new SafetyAccident { Id = id });
            }

        }
        public MemoryStream Export(
               int? projectId,
               string title,
               Dictionary<string, string> comments,
               DisposalState? state,
               string keyword,
               List<int> memorabiliaApplicationIds

            )
        {
            var query = _accidentRepository.Get()
               .Include(u => u.Project)
               .Include(u => u.Source)
               .Include(u => u.Category)
               .Include(u => u.Severity)
               .Where(u => u.State == DataState.Stable);
            var userId = _authInfoProvider.GetCurrent()?.User?.Id ?? 0;

            #region query conditions
            if (memorabiliaApplicationIds != null && memorabiliaApplicationIds.Count > 0)
            {
                query = query.Where(u => memorabiliaApplicationIds.Contains(u.Id));
            }
            else
            {
                if (state != null)
                    query = query.Where(u => u.DisposalState == state);
                if (!string.IsNullOrEmpty(keyword))
                {
                    query = query.Where(u =>
                                 u.Project.Name.Contains(keyword) ||
                                 u.Project.No.Contains(keyword) ||
                                 u.Title.Contains(keyword) ||
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
            var outputs = new List<ExportSafetyAccidentOutput>();
            foreach (var res in resList)
            {
                var output = res.MapTo<ExportSafetyAccidentOutput>();
                //output.ProposerName = userList.Where(u => u.Id == res.ProposerId).Select(u => u.Name).FirstOrDefault();
                //output.DepartmentName = organizationUnitList.Where(u => u.Id == res.DepartmentId).Select(u => u.Name).FirstOrDefault();
                outputs.Add(output);
            }
            return outputs.Entity2Excel(comments, title);
        }

        public GetSafetyAccidentOutput Get(int id)
        {
         

            var accident = _accidentRepository.Get().Where(u => u.Id == id)
                .Include(u => u.Disposals)
                .ThenInclude(u => u.DisposalPhotoSets).ThenInclude(w => w.FileMeta)
                .Include(u => u.Project)
                .Include(u => u.Source)
                .Include(u => u.Category)
                .Include(u => u.Severity)
                .Include(u => u.AccidentPhotoSets).ThenInclude(w => w.FileMeta)
                .Include(u => u.SettlementPhotoSets).ThenInclude(w => w.FileMeta)
                .FirstOrDefault();
            if (accident == null)
            {
                throw new EntityException("Id", id, "SafetyAccident", "不存在");
            }
            else
            {

                var recordOutput = accident.MapTo<GetSafetyAccidentOutput>();
                return recordOutput;
            }
        }
        /// <summary>
        /// 发起记录
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public GetSafetyAccidentOutput GetByTask(int taskId)
        {
            var authInfo = _authInfoProvider.GetCurrent();
            var userId = authInfo?.User?.Id;
            var task = _taskProvider.GetTask(taskId);
            if (task.Assignee != userId.Value.ToString())
            {
                throw new AppCoreException($"id:为{userId.Value}的用户不是任务【{task.NodeName}】的委托人");
            }
            var accident = _accidentRepository.Get()
                .Include(u => u.Disposals)
                .ThenInclude(u => u.DisposalPhotoSets).ThenInclude(w => w.FileMeta)
                .Include(u => u.Project)
                .Include(u => u.Source)
                .Include(u => u.Category)
                .Include(u => u.Severity)
                .Include(u => u.AccidentPhotoSets).ThenInclude(w => w.FileMeta)
                .Include(u => u.SettlementPhotoSets).ThenInclude(w => w.FileMeta).Where(u => u.ProcessInstanceId == task.ProcessInstanceId).FirstOrDefault();
            if (accident == null)
            {
                throw new EntityException("ProjectInstanceId", task.ProcessInstanceId, "SafetyAccident", "不存在");
            }
            var processDefinition = _definitionProvder.Get(task.ProcessDefinitionId);
            var invisibleFields = ((UserTaskNode)processDefinition.GetNode(task.NodeUid)).GetInvisibleFields();
            if (invisibleFields != null)
                TypeDefaulter.SetDefault(accident, invisibleFields.ToArray());

            return accident.MapTo<GetSafetyAccidentOutput>();

        }
        /// <summary>
        /// 等待记录
        /// </summary>
        /// <param name="processId"></param>
        /// <returns></returns>
        public GetSafetyAccidentOutput GetByProcess(int processId)
        {
            var accident = _accidentRepository.Get()
                 .Include(u => u.Disposals)
                .ThenInclude(u => u.DisposalPhotoSets).ThenInclude(w => w.FileMeta)
                .Include(u => u.Project)
                .Include(u => u.Source)
                .Include(u => u.Category)
                .Include(u => u.Severity)
                .Include(u => u.AccidentPhotoSets).ThenInclude(w => w.FileMeta)
                .Include(u => u.SettlementPhotoSets).ThenInclude(w => w.FileMeta).Where(u => u.ProcessInstanceId == processId).FirstOrDefault();
            if (accident == null)
            {
                throw new EntityException("ProjectInstanceId", processId, "SafetyAccident", "不存在");
            }

            return accident.MapTo<GetSafetyAccidentOutput>();

        }


        /// <summary>
        /// 处理记录
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public GetSafetyAccidentOutput GetByTaskHistory(int taskId)
        {
            var authInfo = _authInfoProvider.GetCurrent();
            var userId = authInfo?.User?.Id;
            var task = _historyProvider.GetTaskHistory(taskId);
            if (!task.AssigneeList.Contains($"{userId}"))
            {
                throw new AppCoreException($"id:为{userId.Value}的用户不是任务【{task.NodeName}】的委托人");
            }
            var accident = _accidentRepository.Get()
                 .Include(u => u.Disposals)
                .ThenInclude(u => u.DisposalPhotoSets).ThenInclude(w => w.FileMeta)
                .Include(u => u.Project)
                .Include(u => u.Source)
                .Include(u => u.Category)
                .Include(u => u.Severity)
                .Include(u => u.AccidentPhotoSets).ThenInclude(w => w.FileMeta)
                .Include(u => u.SettlementPhotoSets).ThenInclude(w => w.FileMeta).Where(u => u.ProcessInstanceId == task.ProcessInstanceId).FirstOrDefault();
            if (accident == null)
            {
                throw new EntityException("ProjectInstanceId", task.ProcessInstanceId, "SafetyAccident", "不存在");
            }
            var processDefinition = _definitionProvder.Get(task.ProcessDefinitionId);
            var invisibleFields = ((UserTaskNode)processDefinition.GetNode(task.NodeUid)).GetInvisibleFields();
            if (invisibleFields != null)
                TypeDefaulter.SetDefault(accident, invisibleFields.ToArray());

            return accident.MapTo<GetSafetyAccidentOutput>();

        }

        public int Update(StartProcessInput<UpdateSafetyAccidentInput> input)
        {
            var variables = new Dictionary<string, object>();
            var userId = _authInfoProvider.GetCurrent().User.Id;
            if (!_projectHelper.HasPermission("安全信息负责人", input.Data.ProjectId))//权限设置
            {
                throw new AppCoreException("安全事故修改没有权限");
            }
            variables.Add("starter", userId.ToString());
            int processInstanceId = _runtimeProvider.StartProcessInstanceByName(
                input.ProcessDefinitionName,
                $"{input.ProcessDefinitionName}-{input.Data.Title}",
                variables);
            var tasks = _taskProvider.GetByProcessInstance(processInstanceId);
            if (tasks.Count == 0)
            {
                throw new AppCoreException("任务未创建成功！");
            }
            var record = input.Data.MapTo<SafetyAccident>();
            record.ProcessInstanceId = processInstanceId;
            record.State = DataState.Updating;
            record.Mid = record.Id;
            record.Id = 0;
            _accidentRepository.Add(record);
            var task = tasks[0];
            _taskProvider.Complete(task.Id);
            return record.Id;
        }
        

        public int AddDisposal(int accidentId, AddSafetyAccidentDisposalInput disposalInput)
        {
            var existing = _accidentRepository.Get().Where(u => u.Id == accidentId).FirstOrDefault();
            var disposal = disposalInput.MapTo<SafetyAccidentDisposal>();
            disposal.SafetyAccidentId = accidentId;
            using (var transaction = _dbContextProvider.BeginTransaction())
            {
                _accidentRepository.Update(new SafetyAccident
                {
                    Id = accidentId,
                    DisposalState = DisposalState.Underway,
                    SettlementTime = null
                },
                    new System.Linq.Expressions.Expression<Func<SafetyAccident, object>>[] {
                        u=>u.DisposalState,
                        u=>u.SettlementTime
                    });
                _disposalRepository.Add(disposal);
                transaction.Commit();
            }
            AppBaseContext.Instance.Produce("add-project-briefing", JsonConvert.SerializeObject(new
            {
                TenantId = _op.TenantId,
                ProjectId = existing.ProjectId
            }));
            return disposal.Id;
        }

        public void SettleAccident(SettleSafetyAccidentInput input)
        {
            var existing = _accidentRepository.Get().Include(u => u.SettlementPhotoSets).Where(u => u.Id == input.Id).FirstOrDefault();
            var accident = input.MapTo<SafetyAccident>();
            accident.DisposalState = DisposalState.Completed;
            _accidentRepository.Update(accident, existing, new System.Linq.Expressions.Expression<Func<SafetyAccident, object>>[] {
                        u=>u.SettlementTime,
                        u=>u.DisposalState,
                        u=>u.SettlementPhotoSets
                    });
            AppBaseContext.Instance.Produce("add-project-briefing", JsonConvert.SerializeObject(new
            {
                TenantId = _op.TenantId,
                ProjectId = existing.ProjectId
            }));
            //_accidentRepository.UpdateWithRelatedEntity(accident,true, 
            //    new System.Linq.Expressions.Expression<Func<SafetyAccident, object>>[] {
            //    u =>u.SettlementTime,
            //    u=>u.DisposalState
            //});
        }
    }
}
