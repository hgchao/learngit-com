using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using App.Core.Common.Entities;
using Microsoft.EntityFrameworkCore;
using App.Base.Repositories;
using App.RecordMgmt.Records.Dto;
using PoorFff.Mapper;
using App.Core.Common;
using App.Core.Authorization.Accounts;
using App.Core.Workflow.Providers;
using App.Core.Authorization.Repositories;
using App.Base.EntityFramework;
using App.Core.Authorization.Users;
using App.Core.Messaging;
using App.Core.Workflow;
using App.Base;
using App.Projects;
using App.Core.Common.Exceptions;
using Oa.Project;
using Newtonsoft.Json;
using App.Core.Workflow.Engine.Definition.Nodes.Tasks;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;

namespace App.RecordMgmt.Records
{
    public class RecordService : IRecordService
    {
        private IAuthInfoProvider _authInfoProvider;
        private IHistoryProvider _historyProvider;
        private IRuntimeProvider _runtimeProvider;
        private ITaskProvider _taskProvider;
        private IDefinitionProvider _definitionProvder;
        private IAuthorizationRepositoryBase<User> _userRepository;
        private IAppDbContextProvider _dbContextProvider;
        private IMessagingProvider _messagingProvider;
        private IAppRepositoryBase<Record> _recordRepository;
        public RecordService(
            IAppRepositoryBase<Record> recordRepository,
            IAuthInfoProvider authInfoProvider,
            IWfEngine wfEngine,
            IAuthorizationRepositoryBase<User> userRepository,
            IAppDbContextProvider dbContextProvider,
            IMessagingProvider messagingProvider)
        {
            _recordRepository = recordRepository;
            _messagingProvider = messagingProvider;
            _authInfoProvider = authInfoProvider;
            _userRepository = userRepository;
            _runtimeProvider = wfEngine.GetRuntimeProvider();
            _taskProvider = wfEngine.GetTaskProvider();
            _definitionProvder = wfEngine.GetDefinitionProvider();
            _historyProvider = wfEngine.GetHistoryProvider();
            _dbContextProvider = dbContextProvider;
        }
        public int Add(StartProcessInput<AddRecordInput> input)
        {
            NullableHelper.SetNull(input.Data);
            var variables = new Dictionary<string, object>();
            var userId = _authInfoProvider.GetCurrent().User.Id;
            variables.Add("starter", userId.ToString());
            int processInstanceId = _runtimeProvider.StartProcessInstanceByName(
                input.ProcessDefinitionName,
                $"{input.ProcessDefinitionName}-{input.Data.RecordName}",
                variables);
            var tasks = _taskProvider.GetByProcessInstance(processInstanceId);
            if (tasks.Count == 0)
            {
                throw new AppCoreException("任务未创建成功！");
            }
            var record = input.Data.MapTo<Record>();
            record.ProcessInstanceId = processInstanceId;
            record.DataState = DataState.Creating;
            record.Mid = null;
            _recordRepository.Add(record);
            var task = tasks[0];
            if (!input.PreventCommit)
                _taskProvider.Complete(task.Id);
            return record.Id;
        }

        public void CompleteProcess(int processInstanceId)
        {
            var record = _recordRepository.Get()
                .Include(u => u.Attachments)
                .Where(u => u.ProcessInstanceId == processInstanceId).FirstOrDefault();
            if (record == null)
            {
                throw new EntityException("RecordInstanceId", processInstanceId, "Record", "不存在");
            }
            if (record.DataState == DataState.Creating)
            {
                //record.State = DataState.Stable;
                //_recordBaseInfoRepository.Update(record, new System.Linq.Expressions.Expression<Func<RecordBaseInfo, object>>[] {
                //    u => u.State
                //}, true);
                using (var transaction = _dbContextProvider.BeginTransaction())
                {

                    var temp = JsonConvert.DeserializeObject<Record>(JsonConvert.SerializeObject(record));
                    temp.DataState = DataState.Stable;
                    temp.ProcessInstanceId = null;
                    _recordRepository.Update(temp, new System.Linq.Expressions.Expression<Func<Record, object>>[] {
                        u => u.DataState,
                        u => u.ProcessInstanceId
                    }, true);
                    temp = JsonConvert.DeserializeObject<Record>(JsonConvert.SerializeObject(record));
                    temp.Attachments.ForEach(u => u.Id = 0);
                    temp.DataState = DataState.Created;
                    temp.Id = 0;
                    temp.Mid = record.Id;
                    _recordRepository.Add(temp);
                    transaction.Commit();
                }
            }
            else if (record.Mid != null && record.DataState == DataState.Updating)
            {
                using (var transaction = _dbContextProvider.BeginTransaction())
                {
                    var temp = JsonConvert.DeserializeObject<Record>(JsonConvert.SerializeObject(record));
                    temp.DataState = DataState.Updated;
                    _recordRepository.Update(temp, new System.Linq.Expressions.Expression<Func<Record, object>>[] {
                        u=>u.DataState
                    }, true);
                    temp = JsonConvert.DeserializeObject<Record>(JsonConvert.SerializeObject(record));
                    temp.Id = record.Mid.Value;
                    temp.Attachments.ForEach(u => u.Id = 0);
                    temp.Mid = null;
                    var existing = _recordRepository.Get()
                        .Include(u => u.Attachments)
                        .Where(u => u.Id == temp.Id).FirstOrDefault();
                    _recordRepository.Update(temp, existing, new System.Linq.Expressions.Expression<Func<Record, object>>[] {
                        u=>u.RecordTypeId,
                        u=>u.RecordName,
                        u=>u.Description,
                        u=>u.Attachments
                    }, true);
                    transaction.Commit();
                }
            }
            else
            {

                throw new AppCoreException("当前数据不再更新状态");
            }
        }

        public void CompleteTask(CompleteTaskInput<UpdateRecordInput> input)
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
                var record = input.Data.MapTo<Record>();
                var existing = _recordRepository.Get()
                    .Include(u => u.Attachments)
                    .Where(u => u.ProcessInstanceId == task.ProcessInstanceId).FirstOrDefault();
                record.Id = existing.Id;
                if (existing == null)
                {
                    throw new EntityException("RecordInstanceId", task.ProcessInstanceId, "Record", "不存在");
                }
                if (editableFields == null)
                {
                    _recordRepository.Update(record, existing, new System.Linq.Expressions.Expression<Func<Record, object>>[] {
                        u => u.DataState,
                        u=>u.Mid,
                        u=>u.ProcessInstanceId,
                        u=>u.ProjectId
                    }, false);
                }
                else
                {
                    _recordRepository.Update(record, editableFields.ToArray());
                }
            }
            if (!input.PreventCommit)
            {
                _taskProvider.Complete(input.Id, input.Comment);
            }
        }

        public int Update(StartProcessInput<UpdateRecordInput> input)
        {
            NullableHelper.SetNull(input.Data);
            var variables = new Dictionary<string, object>();
            var userId = _authInfoProvider.GetCurrent().User.Id;
            variables.Add("starter", userId.ToString());
            int processInstanceId = _runtimeProvider.StartProcessInstanceByName(
                input.ProcessDefinitionName,
                $"{input.ProcessDefinitionName}-{input.Data.RecordName}",
                variables);
            var tasks = _taskProvider.GetByProcessInstance(processInstanceId);
            if (tasks.Count == 0)
            {
                throw new AppCoreException("任务未创建成功！");
            }
            var record = _recordRepository.Get()
                .Include(u => u.Attachments)
                .Where(u => u.Id == input.Data.Id).FirstOrDefault();
            var updated = input.Data.MapTo<Record>();
            record.ProcessInstanceId = processInstanceId;
            record.RecordTypeId = updated.RecordTypeId;
            record.Attachments = updated.Attachments;
            record.Description = updated.Description;
            record.RecordName = updated.RecordName;
            record.Attachments.ForEach(u => u.Id = 0);
            record.DataState = DataState.Updating;
            record.Mid = record.Id;
            record.Id = 0;
            _recordRepository.Add(record);
            var task = tasks[0];
            _taskProvider.Complete(task.Id);
            return record.Id;
        }

        public void Delete(int id)
        {
            _recordRepository.Delete(new Record { Id = id });
        }

        public GetRecordOutput Get(int id)
        {
            var record = _recordRepository.Get()
                .Include(u => u.Attachments).ThenInclude(u => u.FileMeta)
                .Where(u => u.Id == id).FirstOrDefault();
            if (record == null)
            {
                throw new EntityException("Id", id, "Record", "不存在");
            }
            return record.MapTo<GetRecordOutput>();
        }

        public PaginationData<GetRecordListOutput> Get(int pageIndex, int pageSize, int projectId, int? typeId, string keyword,
              string sortField,
              string sortState)
        {
            IQueryable<Record> query = _recordRepository.Get()
                .Include(u => u.RecordType)
                .Include(u => u.Attachments).ThenInclude(u=>u.FileMeta)
                .Where(u => u.DataState == DataState.Stable && u.ProjectId == projectId);
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(u => u.RecordName.Contains(keyword));
            }
            if (typeId != null)
            {
                query = query.Where(u => u.RecordTypeId == typeId);
            }
            //sortState 1降序2升序0默认
            if (!string.IsNullOrEmpty(sortField) && !string.IsNullOrEmpty(sortState))
            {
                switch (sortField)
                {
                    case "RecordTypeId"://档案类型
                        query = sortState == "1" ? query.OrderByDescending(u => u.RecordTypeId) : sortState == "2" ? query.OrderBy(u => u.RecordTypeId) : query.OrderByDescending(u => u.CreateTime);
                        break;
                    case "RecordName"://档案名称
                        query = sortState == "1" ? query.OrderByDescending(u => u.RecordName) : sortState == "2" ? query.OrderBy(u => u.RecordName) : query.OrderByDescending(u => u.CreateTime);
                        break;
                    case "Description"://描述
                        query = sortState == "1" ? query.OrderByDescending(u => u.Description) : sortState == "2" ? query.OrderBy(u => u.Description) : query.OrderByDescending(u => u.CreateTime);
                        break;
                    case "CreateTime"://创建时间
                        query = sortState == "1" ? query.OrderByDescending(u => u.CreateTime) : sortState == "2" ? query.OrderBy(u => u.CreateTime) : query.OrderByDescending(u => u.CreateTime);
                        break;
                    case "Attachments"://附件
                        query = sortState == "1" ? query.OrderByDescending(u => u.Attachments.Any(v => v.FileMetaId > 0)) : sortState == "2" ? query.OrderBy(u => u.Attachments.Any(v => v.FileMetaId > 0)) : query.OrderByDescending(u => u.CreateTime);
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
            return PaginationDataHelper.WrapData<Record, T>(query, pageIndex, pageSize).TransferTo<GetRecordListOutput>();
        }

        public GetRecordOutput GetByProcess(int processId)
        {
            var record = _recordRepository.Get()
                .Include(u => u.Attachments).ThenInclude(u => u.FileMeta)
                .Where(u => u.ProcessInstanceId == processId).FirstOrDefault();
            if (record == null)
            {
                throw new EntityException("ProcessInstanceId", processId, "Record", "不存在");
            }
            return record.MapTo<GetRecordOutput>();
        }

        public GetRecordOutput GetByTask(int taskId)
        {
            var authInfo = _authInfoProvider.GetCurrent();
            var userId = authInfo?.User?.Id;
            var task = _taskProvider.GetTask(taskId);
            if (task.Assignee != userId.Value.ToString())
            {
                throw new AppCoreException($"id:为{userId.Value}的用户不是任务【{task.NodeName}】的委托人");
            }
            var record = _recordRepository.Get()
                .Include(u => u.Attachments).ThenInclude(u => u.FileMeta)
                .Where(u => u.ProcessInstanceId == task.ProcessInstanceId).FirstOrDefault();
            if (record == null)
            {
                throw new EntityException("ProcessInstanceId", task.ProcessInstanceId, "Record", "不存在");
            }
            return record.MapTo<GetRecordOutput>();
        }

        public GetRecordOutput GetByTaskHistory(int taskHistoryId)
        {
            var authInfo = _authInfoProvider.GetCurrent();
            var userId = authInfo?.User?.Id;
            var task = _historyProvider.GetTaskHistory(taskHistoryId);
            if (!task.AssigneeList.Contains($"{userId}"))
            {
                throw new AppCoreException($"id:为{userId.Value}的用户不是任务【{task.NodeName}】的委托人");
            }
            var record = _recordRepository.Get()
                .Include(u => u.Attachments).ThenInclude(u => u.FileMeta)
                .Where(u => u.ProcessInstanceId == task.ProcessInstanceId).FirstOrDefault();
            if (record == null)
            {
                throw new EntityException("ProcessInstanceId", task.ProcessInstanceId, "Record", "不存在");
            }
            return record.MapTo<GetRecordOutput>();
        }


    }
}
