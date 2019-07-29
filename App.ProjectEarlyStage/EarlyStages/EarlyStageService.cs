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
using App.Core.Common.Repositories;
using App.Core.Workflow;
using App.Core.Workflow.Engine.Definition.Nodes.Tasks;
using App.Core.Workflow.Providers;
using App.ProjectEarlyStage.EarlyStages;
using App.ProjectEarlyStage.EarlyStages.Dto;
using App.ProjectEarlyStage.PmEarlyStages.Dto;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Oa.Project;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using PoorFff;
using PoorFff.EventBus;
using PoorFff.Excel;
using PoorFff.Mapper;

namespace Pm.ProjectEarlyStage.PmEarlyStages
{
    public class EarlyStageService: IEarlyStageService
    {
        private IAuthInfoProvider _authInfoProvider;
        private IHistoryProvider _historyProvider;
        private IRuntimeProvider _runtimeProvider;
        private ITaskProvider _taskProvider;
        private IDefinitionProvider _definitionProvder;
        private IAppRepositoryBase<EarlyStage> _earlyStageRepository;
        private IAppDbContextProvider _dbContextProvider;
        private IEventBus _eventBus;
        public EarlyStageService(
              IAuthInfoProvider authInfoProvider,
            IAppRepositoryBase<EarlyStage> earlyStageRepository,
            IAppDbContextProvider dbContextProvider,
            IEventBus eventBus,
             IWfEngine wfEngine
            )
        {
            _authInfoProvider = authInfoProvider;
            _earlyStageRepository = earlyStageRepository;
            _dbContextProvider = dbContextProvider;
            _eventBus = eventBus;
            _runtimeProvider = wfEngine.GetRuntimeProvider();
            _taskProvider = wfEngine.GetTaskProvider();
            _definitionProvder = wfEngine.GetDefinitionProvider();
            _historyProvider = wfEngine.GetHistoryProvider();
        }
        public int Add(StartProcessInput<AddEarlyStageInput> input)
        {
            if (input.Data.ReplyNumber == null)
            {
                throw new AppCoreException("批复文号不能为空");
            }
            var variables = new Dictionary<string, object>();
            var userId = _authInfoProvider.GetCurrent().User.Id;
            variables.Add("starter", userId.ToString());
            int processInstanceId = _runtimeProvider.StartProcessInstanceByName(
                input.ProcessDefinitionName,
                $"{input.ProcessDefinitionName}-{input.Data.ReplyNumber}",
                variables);
            var tasks = _taskProvider.GetByProcessInstance(processInstanceId);
            if (tasks.Count == 0)
            {
                throw new AppCoreException("任务未创建成功！");
            }
            var earlyStage = input.Data.MapTo<EarlyStage>();
            // var count = AppSampleContext.Instance.GetSupplierNoCount(() => _supplierRepository.Count(u => u.CreateTime.Value.Year == DateTime.Now.Year && (u.State == DataState.Stable || u.State == DataState.Creating)));
            earlyStage.ProcessInstanceId = processInstanceId;
            earlyStage.State = DataState.Creating;
            // memorabiliaRecord.SupplierNo = $"{DateTime.Now.Year}{count.ToString("000000")}";
            earlyStage.Mid = null;
            _earlyStageRepository.Add(earlyStage);
            var task = tasks[0];
            if (!input.PreventCommit)
                _taskProvider.Complete(task.Id);
            return earlyStage.Id;
        }
        /// <summary>
        /// 最后一个审批结束执行
        /// </summary>
        /// <param name="projectInstanceId"></param>
        public void CompleteApproval(int projectInstanceId)
        {
            var project = _earlyStageRepository.Get().Include(u => u.Attachments).Where(u => u.ProcessInstanceId == projectInstanceId).FirstOrDefault();
            if (project == null)
            {
                throw new EntityException("ProjectInstanceId", projectInstanceId, "EarlyStage", "不存在");
            }
            if (project.State == DataState.Creating)
            {

                using (var transaction = _dbContextProvider.BeginTransaction())
                {

                    var temp = JsonConvert.DeserializeObject<EarlyStage>(JsonConvert.SerializeObject(project));
                    temp.State = DataState.Stable;
                    temp.ProcessInstanceId = null;
                    _earlyStageRepository.Update(temp, new System.Linq.Expressions.Expression<Func<EarlyStage, object>>[] {
                        u => u.State,
                        u => u.ProcessInstanceId
                    }, true);
                    temp = JsonConvert.DeserializeObject<EarlyStage>(JsonConvert.SerializeObject(project));
                    temp.State = DataState.Created;
                    temp.Id = 0;
                    temp.Mid = project.Id;
                    //清空之前附件
                    temp.Attachments.ForEach(u =>
                    {
                        u.Id = 0;
                    });
                    _earlyStageRepository.Add(temp);
                    transaction.Commit();
                }
            }
            else if (project.Mid != null && project.State == DataState.Updating)
            {
                using (var transaction = _dbContextProvider.BeginTransaction())
                {
                    var temp = JsonConvert.DeserializeObject<EarlyStage>(JsonConvert.SerializeObject(project));
                    temp.State = DataState.Updated;
                    _earlyStageRepository.Update(temp, new System.Linq.Expressions.Expression<Func<EarlyStage, object>>[] {
                        u=>u.State
                    }, true);
                    temp = JsonConvert.DeserializeObject<EarlyStage>(JsonConvert.SerializeObject(project));
                    temp.Id = project.Mid.Value;
                    temp.Mid = null;
                    //清空之前附件
                    temp.Attachments.ForEach(u =>
                    {
                        u.Id = 0;
                    });
                    var existing = _earlyStageRepository.Get().Include(u => u.Attachments).Where(u => u.Id == project.Mid).FirstOrDefault();
                    _earlyStageRepository.Update(temp, existing, new System.Linq.Expressions.Expression<Func<EarlyStage, object>>[] {
                        u=>u.ProcessInstanceId,
                        u=>u.State
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
        public void CompleteTask(CompleteTaskInput<UpdateEarlyStageInput> input)
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
                var project = input.Data.MapTo<EarlyStage>();
                if (editableFields == null)
                {
                    _earlyStageRepository.Update(project, new System.Linq.Expressions.Expression<Func<EarlyStage, object>>[] {
                        u => u.State,
                        u=>u.Mid,
                        u=>u.ProcessInstanceId
                    }, false);
                }
                else
                {
                    _earlyStageRepository.Update(project, editableFields.ToArray());
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
                return _earlyStageRepository.Count(u => u.ReplyNumber == name && u.Id != id && u.State == DataState.Stable);
            }
            else
            {
                return _earlyStageRepository.Count(u => u.ReplyNumber == name && u.State == DataState.Stable);
            }
        }

        public List<GetEarlyStageOutput> Get(int? projectId,string typeName, string sortField,
              string sortState)
        {
            var query = _earlyStageRepository.Get()
                  .Include(u => u.Project)
                  .Include(u => u.Node)
                  .Include(u => u.Attachments).ThenInclude(w => w.FileMeta)
                .Where(u => u.State == DataState.Stable);
           // var userId = _authInfoProvider.GetCurrent()?.User?.Id ?? 0;

            #region query conditions
           
            if (!string.IsNullOrEmpty(typeName))
                query = query.Where(u => u.TypeName == typeName);
          
            if (!string.IsNullOrEmpty(projectId.ToString()))
            {
                query = query.Where(u => u.ProjectId == projectId);
            }

            //sortState 1降序2升序0默认
            if (!string.IsNullOrEmpty(sortField) && !string.IsNullOrEmpty(sortState))
            {
                switch (sortField)
                {
                    case "ReplyNumber"://批次编号
                        query = sortState == "1" ? query.OrderByDescending(u => u.ReplyNumber) : sortState == "2" ? query.OrderBy(u => u.ReplyNumber) : query.OrderByDescending(u => u.CreateTime);
                        break;
                    default:
                        query = query.OrderByDescending(u => u.CreateTime);
                        break;
                }

            }
            else
            {
                query = query.OrderBy(u => u.Node.SortNo);
                
            }
            #endregion
            var outputs = query.MapToList<GetEarlyStageOutput>();
            return outputs;

        }
        public MemoryStream Export(int? projectId,
             string title,
               Dictionary<string, string> comments,
               string typeName,
               List<int> earlyStageApplicationIds

            )
        {
            // throw new NotImplementedException();
            var query = _earlyStageRepository.Get()
                .Include(u => u.Project)
                .Include(u => u.Node)
                .Include(u => u.Attachments).ThenInclude(w => w.FileMeta)
                .Where(u => u.State == DataState.Stable);
            var userId = _authInfoProvider.GetCurrent()?.User?.Id ?? 0;

            #region query conditions
            if (earlyStageApplicationIds != null && earlyStageApplicationIds.Count > 0)
            {
                query = query.Where(u => earlyStageApplicationIds.Contains(u.Id));
            }
            else
            {
                if (!string.IsNullOrEmpty(typeName))
                {
                    query = query.Where(u => u.ProjectId == projectId);
                }
            }

            #endregion
            query = query.OrderBy(u => u.Node.SortNo);
            //var userList = _userRepository.Get().Select(u => new { u.Id, u.Name }).ToList();
            //var organizationUnitList = _organizationUnitRepository.Get().Select(u => new { u.Id, u.Name }).ToList();
            var resList = query.ToList();
            var outputs = new List<ExportEarlyStageOutput>();
            foreach (var res in resList)
            {
                var output = res.MapTo<ExportEarlyStageOutput>();
                //output.ProposerName = userList.Where(u => u.Id == res.ProposerId).Select(u => u.Name).FirstOrDefault();
                //output.DepartmentName = organizationUnitList.Where(u => u.Id == res.DepartmentId).Select(u => u.Name).FirstOrDefault();
                outputs.Add(output);
            }
            return outputs.Entity2Excel(comments, title);
        }

        public GetEarlyStageOutput Get(int id)
        {
            var problem = _earlyStageRepository.Get().Where(u => u.Id == id)
                       .Include(u => u.Project)
                       .Include(u => u.Node)
                       .Include(u => u.Attachments).ThenInclude(w => w.FileMeta)
                        .FirstOrDefault();
            if (problem == null)
            {
                throw new EntityException("Id", id, "EarlyStage", "不存在");
            }
            else
            {

                var recordOutput = problem.MapTo<GetEarlyStageOutput>();
                return recordOutput;
            }
        }
        /// <summary>
        /// 发起记录
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public GetEarlyStageOutput GetByTask(int taskId)
        {
            var authInfo = _authInfoProvider.GetCurrent();
            var userId = authInfo?.User?.Id;
            var task = _taskProvider.GetTask(taskId);
            if (task.Assignee != userId.Value.ToString())
            {
                throw new AppCoreException($"id:为{userId.Value}的用户不是任务【{task.NodeName}】的委托人");
            }
            var problem = _earlyStageRepository.Get()
                       .Include(u => u.Project)
                       .Include(u => u.Node)
                       .Include(u => u.Attachments).ThenInclude(w => w.FileMeta).Where(u => u.ProcessInstanceId == task.ProcessInstanceId).FirstOrDefault();
            if (problem == null)
            {
                throw new EntityException("ProjectInstanceId", task.ProcessInstanceId, "EarlyStage", "不存在");
            }
            var processDefinition = _definitionProvder.Get(task.ProcessDefinitionId);
            var invisibleFields = ((UserTaskNode)processDefinition.GetNode(task.NodeUid)).GetInvisibleFields();
            if (invisibleFields != null)
                TypeDefaulter.SetDefault(problem, invisibleFields.ToArray());

            return problem.MapTo<GetEarlyStageOutput>();

        }
        /// <summary>
        /// 等待记录
        /// </summary>
        /// <param name="processId"></param>
        /// <returns></returns>
        public GetEarlyStageOutput GetByProcess(int processId)
        {
            var problem = _earlyStageRepository.Get()
                       .Include(u => u.Project)
                       .Include(u => u.Node)
                       .Include(u => u.Attachments).ThenInclude(w => w.FileMeta).Where(u => u.ProcessInstanceId == processId).FirstOrDefault();
            if (problem == null)
            {
                throw new EntityException("ProjectInstanceId", processId, "EarlyStage", "不存在");
            }

            return problem.MapTo<GetEarlyStageOutput>();

        }


        /// <summary>
        /// 处理记录
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public GetEarlyStageOutput GetByTaskHistory(int taskId)
        {
            var authInfo = _authInfoProvider.GetCurrent();
            var userId = authInfo?.User?.Id;
            var task = _historyProvider.GetTaskHistory(taskId);
            if (!task.AssigneeList.Contains($"{userId}"))
            {
                throw new AppCoreException($"id:为{userId.Value}的用户不是任务【{task.NodeName}】的委托人");
            }
            var problem = _earlyStageRepository.Get()
                       .Include(u => u.Project)
                       .Include(u => u.Node)
                       .Include(u => u.Attachments).ThenInclude(w => w.FileMeta).Where(u => u.ProcessInstanceId == task.ProcessInstanceId).FirstOrDefault();
            if (problem == null)
            {
                throw new EntityException("ProjectInstanceId", task.ProcessInstanceId, "EarlyStage", "不存在");
            }
            var processDefinition = _definitionProvder.Get(task.ProcessDefinitionId);
            var invisibleFields = ((UserTaskNode)processDefinition.GetNode(task.NodeUid)).GetInvisibleFields();
            if (invisibleFields != null)
                TypeDefaulter.SetDefault(problem, invisibleFields.ToArray());

            return problem.MapTo<GetEarlyStageOutput>();

        }

        public int Update(StartProcessInput<UpdateEarlyStageInput> input)
        {
            var variables = new Dictionary<string, object>();
            var userId = _authInfoProvider.GetCurrent().User.Id;
            variables.Add("starter", userId.ToString());
            int processInstanceId = _runtimeProvider.StartProcessInstanceByName(
                input.ProcessDefinitionName,
                $"{input.ProcessDefinitionName}-{input.Data.ReplyNumber}",
                variables);
            var tasks = _taskProvider.GetByProcessInstance(processInstanceId);
            if (tasks.Count == 0)
            {
                throw new AppCoreException("任务未创建成功！");
            }
            var record = input.Data.MapTo<EarlyStage>();
            record.ProcessInstanceId = processInstanceId;
            record.State = DataState.Updating;
            record.Mid = record.Id;
            record.Id = 0;
            _earlyStageRepository.Add(record);
            var task = tasks[0];
            _taskProvider.Complete(task.Id);
            return record.Id;
        }
        
    }
}
