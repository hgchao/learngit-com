using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using App.Core.Common.Entities;
using App.Core.Common.Exceptions;
using Microsoft.EntityFrameworkCore;

using App.Base.EntityFramework;
using App.Base.Repositories;
using App.Memorabilia.MemorabiliaRecords.Dto;

using PoorFff.Mapper;
using App.Core.Common;
using App.Base;
using App.Core.Authorization.Accounts;
using App.Core.Workflow.Providers;
using App.Core.Workflow;
using Oa.Project;
using Newtonsoft.Json;
using App.Core.Workflow.Engine.Definition.Nodes.Tasks;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System.IO;
using PoorFff.Excel;
using PoorFff;
using App.Projects.ProjectBaseInfos;

namespace App.Memorabilia.MemorabiliaRecords
{
    public class MemorabiliaRecordService : IMemorabiliaRecordService
    {
       
        private IAuthInfoProvider _authInfoProvider;
        private IHistoryProvider _historyProvider;
        private IRuntimeProvider _runtimeProvider;
        private ITaskProvider _taskProvider;
        private IDefinitionProvider _definitionProvder;
        private IAppRepositoryBase<MemorabiliaRecord> _recordRepository;
        //private IAppRepositoryBase<Project> _projectRepository;
        private IAppDbContextProvider _dbContextProvider;
        private IProjectHelper _projectHelper;

        public MemorabiliaRecordService(
            IAuthInfoProvider authInfoProvider,
            IWfEngine wfEngine,
            IAppRepositoryBase<MemorabiliaRecord> recordRepository,
             //IAppRepositoryBase<Project> projectRepository,
            IAppDbContextProvider dbContextProvider,
            IProjectHelper projectHelper


            )
        {
            _projectHelper = projectHelper;
            _authInfoProvider = authInfoProvider;
            _recordRepository = recordRepository;
           // _projectRepository = projectRepository;
            _dbContextProvider = dbContextProvider;
            _runtimeProvider = wfEngine.GetRuntimeProvider();
            _taskProvider = wfEngine.GetTaskProvider();
            _definitionProvder = wfEngine.GetDefinitionProvider();
            _historyProvider = wfEngine.GetHistoryProvider();
        }

        public int Add(StartProcessInput<AddMemorabiliaRecordInput> input)
        {
         
            if (input.Data.Name == null)
            {
                throw new AppCoreException("大事记名称不能为空");
            }
            var variables = new Dictionary<string, object>();
            var userId = _authInfoProvider.GetCurrent().User.Id;
            if (!_projectHelper.HasPermission("大事记负责人", input.Data.ProjectId))//权限设置
            {
                throw new AppCoreException("大事记发布没有权限");
            }
            variables.Add("starter", userId.ToString());
            int processInstanceId = _runtimeProvider.StartProcessInstanceByName(
                input.ProcessDefinitionName,
                $"{input.ProcessDefinitionName}-{input.Data.Name}",
                variables);
            var tasks = _taskProvider.GetByProcessInstance(processInstanceId);
            if (tasks.Count == 0)
            {
                throw new AppCoreException("任务未创建成功！");
            }
            var memorabiliaRecord = input.Data.MapTo<MemorabiliaRecord>();
           // var count = AppSampleContext.Instance.GetSupplierNoCount(() => _supplierRepository.Count(u => u.CreateTime.Value.Year == DateTime.Now.Year && (u.State == DataState.Stable || u.State == DataState.Creating)));
            memorabiliaRecord.ProcessInstanceId = processInstanceId;
            memorabiliaRecord.State = DataState.Creating;
           // memorabiliaRecord.SupplierNo = $"{DateTime.Now.Year}{count.ToString("000000")}";
            memorabiliaRecord.Mid = null;
            _recordRepository.Add(memorabiliaRecord);
            var task = tasks[0];
            if (!input.PreventCommit)
                _taskProvider.Complete(task.Id);
            return memorabiliaRecord.Id;
        }
        /// <summary>
        /// 最后一个审批结束执行
        /// </summary>
        /// <param name="projectInstanceId"></param>
        public void CompleteApproval(int projectInstanceId)
        {
            var project = _recordRepository.Get().Include(u => u.Attachments).Where(u => u.ProcessInstanceId == projectInstanceId).FirstOrDefault();
            if (project == null)
            {
                throw new EntityException("ProjectInstanceId", projectInstanceId, "MemorabiliaRecord", "不存在");
            }
            if (project.State == DataState.Creating)
            {
                //project.State = DataState.Stable;
                //_supplierRepository.Update(project, new System.Linq.Expressions.Expression<Func<Supplier, object>>[] {
                //    u => u.State
                //}, true);
                using (var transaction = _dbContextProvider.BeginTransaction())
                {

                    var temp = JsonConvert.DeserializeObject<MemorabiliaRecord>(JsonConvert.SerializeObject(project));
                    temp.State = DataState.Stable;
                    temp.ProcessInstanceId = null;
                    _recordRepository.Update(temp, new System.Linq.Expressions.Expression<Func<MemorabiliaRecord, object>>[] {
                        u => u.State,
                        u => u.ProcessInstanceId
                    }, true);
                    temp = JsonConvert.DeserializeObject<MemorabiliaRecord>(JsonConvert.SerializeObject(project));
                    temp.State = DataState.Created;
                    temp.Id = 0;
                    temp.Mid = project.Id;
                    //清空之前附件
                    temp.Attachments.ForEach(u =>
                    {
                        u.Id = 0;
                    });
                    _recordRepository.Add(temp);
                    transaction.Commit();
                }
                //foreach (var resposiblePersonId in project.ResposiblePersonIdList)
                //{
                //    _messagingProvider.CreateNotice(resposiblePersonId.ToString(), $"与您有关的项目【{project.Name}已经创建完成");
                //}
            }
            else if (project.Mid != null && project.State == DataState.Updating)
            {
                using (var transaction = _dbContextProvider.BeginTransaction())
                {
                    var temp = JsonConvert.DeserializeObject<MemorabiliaRecord>(JsonConvert.SerializeObject(project));
                    temp.State = DataState.Updated;
                    _recordRepository.Update(temp, new System.Linq.Expressions.Expression<Func<MemorabiliaRecord, object>>[] {
                        u=>u.State
                    }, true);
                    temp = JsonConvert.DeserializeObject<MemorabiliaRecord>(JsonConvert.SerializeObject(project));
                    temp.Id = project.Mid.Value;
                    temp.Mid = null;
                    //清空之前附件
                    temp.Attachments.ForEach(u =>
                    {
                        u.Id = 0;
                    });
                    var existing = _recordRepository.Get().Include(u => u.Attachments).Where(u => u.Id == project.Mid).FirstOrDefault();
                    _recordRepository.Update(temp, existing, new System.Linq.Expressions.Expression<Func<MemorabiliaRecord, object>>[] {
                        u=>u.ProcessInstanceId,
                        u=>u.State
                    }, false);
                    transaction.Commit();
                }
                //foreach (var resposiblePersonId in project.ResposiblePersonIdList)
                //{
                //    _messagingProvider.CreateNotice(resposiblePersonId.ToString(), $"与您有关的项目【{project.Name}已经更新完成");
                //}
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
        public void CompleteTask(CompleteTaskInput<UpdateMemorabiliaRecordInput> input)
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
                var project = input.Data.MapTo<MemorabiliaRecord>();
                if (editableFields == null)
                {
                    _recordRepository.Update(project, new System.Linq.Expressions.Expression<Func<MemorabiliaRecord, object>>[] {
                        u => u.State,
                        u=>u.Mid,
                        u=>u.ProcessInstanceId
                    }, false);
                }
                else
                {
                    _recordRepository.Update(project, editableFields.ToArray());
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
                return _recordRepository.Count(u => u.Name == name && u.Id != id && u.State == DataState.Stable);
            }
            else
            {
                return _recordRepository.Count(u => u.Name == name && u.State == DataState.Stable);
            }
        }
    
        public PaginationData<GetMemorabiliaRecordListOutput> Get(int? projectId,
               int pageIndex, int pageSize,
               string keyword,
                  string sortField,
               string sortState
            )
        {
            var query = _recordRepository.Get()
                .Include(u => u.Category)
                .Include(u => u.Project)
                .Where(u => u.State == DataState.Stable);
            var userId = _authInfoProvider.GetCurrent()?.User?.Id ?? 0;
         
            #region query conditions
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(u =>
                    u.Name.Contains(keyword) ||
                    u.Description.Contains(keyword));
            }
            if (!string.IsNullOrEmpty(projectId.ToString()))
            {
                query = query.Where(u => u.ProjectId== projectId);
            }

            //sortState 1降序2升序0默认
            if (!string.IsNullOrEmpty(sortField) && !string.IsNullOrEmpty(sortState))
            {
                switch (sortField)
                {
                    case "CategoryId"://事项类型
                        query = sortState == "1" ? query.OrderByDescending(u => u.CategoryId) : sortState == "2" ? query.OrderBy(u => u.CategoryId) : query.OrderByDescending(u => u.CreateTime);
                        break;
                    case "Name"://事项名称
                        query = sortState == "1" ? query.OrderByDescending(u => u.Name) : sortState == "2" ? query.OrderBy(u => u.Name) : query.OrderByDescending(u => u.CreateTime);
                        break;
                    case "Participant"://参与人员
                        query = sortState == "1" ? query.OrderByDescending(u => u.Participant) : sortState == "2" ? query.OrderBy(u => u.Participant) : query.OrderByDescending(u => u.CreateTime);
                        break;
                    case "CreateTime"://创建时间
                        query = sortState == "1" ? query.OrderByDescending(u => u.CreateTime) : sortState == "2" ? query.OrderBy(u => u.CreateTime) : query.OrderByDescending(u => u.CreateTime);
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
            var paging = PaginationDataHelper.WrapData<MemorabiliaRecord, T>(query, pageIndex, pageSize).TransferTo<GetMemorabiliaRecordListOutput>();
            paging.Data.ForEach(u => u.HasPermission = _projectHelper.HasPermission("大事记负责人", u.ProjectId));
            return paging;
        }
        public void Delete(int id)
        {
            var problem = _recordRepository.Get().Where(u => u.Id == id).FirstOrDefault();
            if (problem != null)
            {
                var userId = _authInfoProvider.GetCurrent().User.Id;
                if (!_projectHelper.HasPermission("大事记负责人", problem.ProjectId))//权限设置
                {
                    throw new AppCoreException("大事记删除没有权限");
                }
                _recordRepository.Delete(new MemorabiliaRecord { Id = id });
            }
            
        }
        public MemoryStream Export(
             string title,
               Dictionary<string, string> comments,
               string keyword,
               List<int> memorabiliaApplicationIds

            )
        {
            // throw new NotImplementedException();
            var query = _recordRepository.Get()
               .Include(u => u.Category)
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
                if (!string.IsNullOrEmpty(keyword))
                {
                    query = query.Where(u =>
                     u.Name.Contains(keyword) ||
                     u.Description.Contains(keyword));
                }


            }

            #endregion
            query = query.OrderByDescending(u => u.CreateTime);
            //var userList = _userRepository.Get().Select(u => new { u.Id, u.Name }).ToList();
            //var organizationUnitList = _organizationUnitRepository.Get().Select(u => new { u.Id, u.Name }).ToList();
            var resList = query.ToList();
            var outputs = new List<ExportMemorabiliaRecordOutput>();
            foreach (var res in resList)
            {
                var output = res.MapTo<ExportMemorabiliaRecordOutput>();
                //output.ProposerName = userList.Where(u => u.Id == res.ProposerId).Select(u => u.Name).FirstOrDefault();
                //output.DepartmentName = organizationUnitList.Where(u => u.Id == res.DepartmentId).Select(u => u.Name).FirstOrDefault();
                outputs.Add(output);
            }
            return outputs.Entity2Excel(comments, title);
        }

        public GetMemorabiliaRecordOutput Get(int id)
        {
            var project = _recordRepository.Get()
                .Include(u=>u.Category)
                .Include(u => u.Project)
                .Include(u => u.Attachments).ThenInclude(w => w.FileMeta).Where(u => u.Id == id).FirstOrDefault();
            if (project == null)
            {
                throw new EntityException("Id", id, "MemorabiliaRecord", "不存在");
            }
            else
            {
                
                var recordOutput = project.MapTo<GetMemorabiliaRecordOutput>();
                return recordOutput;
            }
        }
        /// <summary>
        /// 发起记录
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public GetMemorabiliaRecordOutput GetByTask(int taskId)
        {
            var authInfo = _authInfoProvider.GetCurrent();
            var userId = authInfo?.User?.Id;
            var task = _taskProvider.GetTask(taskId);
            if (task.Assignee != userId.Value.ToString())
            {
                throw new AppCoreException($"id:为{userId.Value}的用户不是任务【{task.NodeName}】的委托人");
            }
            var project = _recordRepository.Get()
                 .Include(u => u.Category)
                .Include(u => u.Project)
                .Include(u => u.Attachments).ThenInclude(w => w.FileMeta).Where(u => u.ProcessInstanceId == task.ProcessInstanceId).FirstOrDefault();
            if (project == null)
            {
                throw new EntityException("ProjectInstanceId", task.ProcessInstanceId, "MemorabiliaRecord", "不存在");
            }
            var processDefinition = _definitionProvder.Get(task.ProcessDefinitionId);
            var invisibleFields = ((UserTaskNode)processDefinition.GetNode(task.NodeUid)).GetInvisibleFields();
            if (invisibleFields != null)
                TypeDefaulter.SetDefault(project, invisibleFields.ToArray());

            return  project.MapTo<GetMemorabiliaRecordOutput>();
            
        }
        /// <summary>
        /// 等待记录
        /// </summary>
        /// <param name="processId"></param>
        /// <returns></returns>
        public GetMemorabiliaRecordOutput GetByProcess(int processId)
        {
            var project = _recordRepository.Get()
                 .Include(u => u.Category)
                .Include(u => u.Project)
                .Include(u => u.Attachments).ThenInclude(w => w.FileMeta).Where(u => u.ProcessInstanceId == processId).FirstOrDefault();
            if (project == null)
            {
                throw new EntityException("ProjectInstanceId", processId, "MemorabiliaRecord", "不存在");
            }

            return project.MapTo<GetMemorabiliaRecordOutput>();
           
        }


        /// <summary>
        /// 处理记录
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public GetMemorabiliaRecordOutput GetByTaskHistory(int taskId)
        {
            var authInfo = _authInfoProvider.GetCurrent();
            var userId = authInfo?.User?.Id;

            var task = _historyProvider.GetTaskHistory(taskId);
            if (!task.AssigneeList.Contains($"{userId}"))
            {
                throw new AppCoreException($"id:为{userId.Value}的用户不是任务【{task.NodeName}】的委托人");
            }
            var project = _recordRepository.Get()
                 .Include(u => u.Category)
                .Include(u => u.Project)
                .Include(u => u.Attachments).ThenInclude(w => w.FileMeta).Where(u => u.ProcessInstanceId == task.ProcessInstanceId).FirstOrDefault();
            if (project == null)
            {
                throw new EntityException("ProjectInstanceId", task.ProcessInstanceId, "MemorabiliaRecord", "不存在");
            }
            var processDefinition = _definitionProvder.Get(task.ProcessDefinitionId);
            var invisibleFields = ((UserTaskNode)processDefinition.GetNode(task.NodeUid)).GetInvisibleFields();
            if (invisibleFields != null)
                TypeDefaulter.SetDefault(project, invisibleFields.ToArray());

            return project.MapTo<GetMemorabiliaRecordOutput>();
           
        }

        public int Update(StartProcessInput<UpdateMemorabiliaRecordInput> input)
        {
            var variables = new Dictionary<string, object>();
            var userId = _authInfoProvider.GetCurrent().User.Id;
            if (!_projectHelper.HasPermission("大事记负责人", input.Data.ProjectId))//权限设置
            {
                throw new AppCoreException("大事记修改没有权限");
            }
            variables.Add("starter", userId.ToString());
            int processInstanceId = _runtimeProvider.StartProcessInstanceByName(
                input.ProcessDefinitionName,
                $"{input.ProcessDefinitionName}-{input.Data.Name}",
                variables);
            var tasks = _taskProvider.GetByProcessInstance(processInstanceId);
            if (tasks.Count == 0)
            {
                throw new AppCoreException("任务未创建成功！");
            }
            var record = input.Data.MapTo<MemorabiliaRecord>();
            record.ProcessInstanceId = processInstanceId;
            record.State = DataState.Updating;
            record.Mid = record.Id;
            record.Id = 0;
            _recordRepository.Add(record);
            var task = tasks[0];
            _taskProvider.Complete(task.Id);
            return record.Id;
        }
    }
}
