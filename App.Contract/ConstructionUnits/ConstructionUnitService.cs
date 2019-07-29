using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using App.Base;
using App.Base.EntityFramework;
using App.Base.Repositories;
using App.Contract.ConstructionUnits;
using App.Contract.ConstructionUnits.Dto;
using App.Core.Authorization.Accounts;
using App.Core.Common;
using App.Core.Common.Entities;
using App.Core.Common.Exceptions;
using App.Core.Workflow;
using App.Core.Workflow.Engine.Definition.Nodes.Tasks;
using App.Core.Workflow.Providers;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Oa.Project;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using PoorFff;
using PoorFff.Excel;
using PoorFff.Mapper;

namespace Pm.ConstructionUnitAndStaff.PmConstructionUnits
{
    public class ConstructionUnitService : IConstructionUnitService
    {
        private IAuthInfoProvider _authInfoProvider;
        private IHistoryProvider _historyProvider;
        private IRuntimeProvider _runtimeProvider;
        private ITaskProvider _taskProvider;
        private IDefinitionProvider _definitionProvder;
        private IAppRepositoryBase<ConstructionUnit> _constructionUnitRepository;
        private IAppDbContextProvider _dbContextProvider;

        public ConstructionUnitService(
            IAuthInfoProvider authInfoProvider,
            IWfEngine wfEngine,
            IAppRepositoryBase<ConstructionUnit> constructionUnitRepository,
            IAppDbContextProvider dbContextProvider


            )
        {
            _authInfoProvider = authInfoProvider;
            _constructionUnitRepository = constructionUnitRepository;
            _dbContextProvider = dbContextProvider;
            _runtimeProvider = wfEngine.GetRuntimeProvider();
            _taskProvider = wfEngine.GetTaskProvider();
            _definitionProvder = wfEngine.GetDefinitionProvider();
            _historyProvider = wfEngine.GetHistoryProvider();
        }

        public int Add(StartProcessInput<AddConstructionUnitInput> input)
        {
            if (input.Data.Name == null)
            {
                throw new AppCoreException("单位名称不能为空");
            }
            var variables = new Dictionary<string, object>();
            var userId = _authInfoProvider.GetCurrent().User.Id;
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
            var constructionUnit = input.Data.MapTo<ConstructionUnit>();
            // var count = AppSampleContext.Instance.GetSupplierNoCount(() => _supplierRepository.Count(u => u.CreateTime.Value.Year == DateTime.Now.Year && (u.State == DataState.Stable || u.State == DataState.Creating)));
            constructionUnit.ProcessInstanceId = processInstanceId;
            constructionUnit.State = DataState.Creating;
            // constructionUnit.SupplierNo = $"{DateTime.Now.Year}{count.ToString("000000")}";
            constructionUnit.Mid = null;
            _constructionUnitRepository.Add(constructionUnit);
            var task = tasks[0];
            _taskProvider.Complete(task.Id);
            return constructionUnit.Id;
        }
        /// <summary>
        /// 最后一个审批结束执行
        /// </summary>
        /// <param name="projectInstanceId"></param>
        public void CompleteApproval(int projectInstanceId)
        {
            var project = _constructionUnitRepository.Get().Where(u => u.ProcessInstanceId == projectInstanceId).FirstOrDefault();
            if (project == null)
            {
                throw new EntityException("ProjectInstanceId", projectInstanceId, "ConstructionUnit", "不存在");
            }
            if (project.State == DataState.Creating)
            {
               
                using (var transaction = _dbContextProvider.BeginTransaction())
                {

                    var temp = JsonConvert.DeserializeObject<ConstructionUnit>(JsonConvert.SerializeObject(project));
                    temp.State = DataState.Stable;
                    temp.ProcessInstanceId = null;
                    _constructionUnitRepository.Update(temp, new System.Linq.Expressions.Expression<Func<ConstructionUnit, object>>[] {
                        u => u.State,
                        u => u.ProcessInstanceId
                    }, true);
                    temp = JsonConvert.DeserializeObject<ConstructionUnit>(JsonConvert.SerializeObject(project));
                    temp.State = DataState.Created;
                    temp.Id = 0;
                    temp.Mid = project.Id;
                    //清空之前附件
                    //temp.Attachments.ForEach(u =>
                    //{
                    //    u.Id = 0;
                    //});
                    _constructionUnitRepository.Add(temp);
                    transaction.Commit();
                }
              
            }
            else if (project.Mid != null && project.State == DataState.Updating)
            {
                using (var transaction = _dbContextProvider.BeginTransaction())
                {
                    var temp = JsonConvert.DeserializeObject<ConstructionUnit>(JsonConvert.SerializeObject(project));
                    temp.State = DataState.Updated;
                    _constructionUnitRepository.Update(temp, new System.Linq.Expressions.Expression<Func<ConstructionUnit, object>>[] {
                        u=>u.State
                    }, true);
                    temp = JsonConvert.DeserializeObject<ConstructionUnit>(JsonConvert.SerializeObject(project));
                    temp.Id = project.Mid.Value;
                    temp.Mid = null;
                    //清空之前附件
                    //temp.Attachments.ForEach(u =>
                    //{
                    //    u.Id = 0;
                    //});
                    var existing = _constructionUnitRepository.Get().Where(u => u.Id == project.Mid).FirstOrDefault();
                    _constructionUnitRepository.Update(temp, existing, new System.Linq.Expressions.Expression<Func<ConstructionUnit, object>>[] {
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
        public void CompleteTask(CompleteTaskInput<UpdateConstructionUnitInput> input)
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
                var project = input.Data.MapTo<ConstructionUnit>();
                if (editableFields == null)
                {
                    _constructionUnitRepository.Update(project, new System.Linq.Expressions.Expression<Func<ConstructionUnit, object>>[] {
                        u => u.State,
                        u=>u.Mid,
                        u=>u.ProcessInstanceId
                    }, false);
                }
                else
                {
                    _constructionUnitRepository.Update(project, editableFields.ToArray());
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
                return _constructionUnitRepository.Count(u => u.Name == name && u.Id != id && u.State == DataState.Stable);
            }
            else
            {
                return _constructionUnitRepository.Count(u => u.Name == name && u.State == DataState.Stable);
            }
        }

        public PaginationData<GetConstructionUnitListOutput> Get(
               int pageIndex, int pageSize,
               string keyword,
                  string sortField,
               string sortState
            )
        {
            var query = _constructionUnitRepository.Get()
                .Where(u => u.State == DataState.Stable);
            var userId = _authInfoProvider.GetCurrent()?.User?.Id ?? 0;

            #region query conditions
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(u =>
                    u.Name.Contains(keyword) ||
                    u.CompanyContact.Contains(keyword));
            }


            //sortState 1降序2升序0默认
            if (!string.IsNullOrEmpty(sortField) && !string.IsNullOrEmpty(sortState))
            {
                switch (sortField)
                {
                    case "Name"://单位名称
                        query = sortState == "1" ? query.OrderByDescending(u => u.Name) : sortState == "2" ? query.OrderBy(u => u.Name) : query.OrderByDescending(u => u.CreateTime);
                        break;
                    case "Type"://单位类型
                        query = sortState == "1" ? query.OrderByDescending(u => u.Type) : sortState == "2" ? query.OrderBy(u => u.Type) : query.OrderByDescending(u => u.CreateTime);
                        break;
                    case "CompanyContact"://企业联系人
                        query = sortState == "1" ? query.OrderByDescending(u => u.CompanyContact) : sortState == "2" ? query.OrderBy(u => u.CompanyContact) : query.OrderByDescending(u => u.CreateTime);
                        break;
                    case "Link"://联系方式
                        query = sortState == "1" ? query.OrderByDescending(u => u.Link) : sortState == "2" ? query.OrderBy(u => u.Link) : query.OrderByDescending(u => u.CreateTime);
                        break;
                    case "EvaluationLevel"://评价等级
                        query = sortState == "1" ? query.OrderByDescending(u => u.EvaluationLevel) : sortState == "2" ? query.OrderBy(u => u.EvaluationLevel) : query.OrderByDescending(u => u.CreateTime);
                        break;
                    case "Score"://得分
                        query = sortState == "1" ? query.OrderByDescending(u => u.Score) : sortState == "2" ? query.OrderBy(u => u.Score) : query.OrderByDescending(u => u.CreateTime);
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
            return PaginationDataHelper.WrapData<ConstructionUnit, T>(query, pageIndex, pageSize).TransferTo<GetConstructionUnitListOutput>();
        }
        public MemoryStream Export(
             string title,
               Dictionary<string, string> comments,
               string keyword,
               List<int> constructionUnitApplicationIds

            )
        {
            // throw new NotImplementedException();
            var query = _constructionUnitRepository.Get()
               .Where(u => u.State == DataState.Stable);
            var userId = _authInfoProvider.GetCurrent()?.User?.Id ?? 0;

            #region query conditions
            if (constructionUnitApplicationIds != null && constructionUnitApplicationIds.Count > 0)
            {
                query = query.Where(u => constructionUnitApplicationIds.Contains(u.Id));
            }
            else
            {
                if (!string.IsNullOrEmpty(keyword))
                {
                    query = query.Where(u =>
                     u.Name.Contains(keyword) ||
                     u.CompanyContact.Contains(keyword));
                }


            }

            #endregion
            query = query.OrderByDescending(u => u.CreateTime);
           // var userList = _userRepository.Get().Select(u => new { u.Id, u.Name }).ToList();
          //  var organizationUnitList = _organizationUnitRepository.Get().Select(u => new { u.Id, u.Name }).ToList();
            var resList = query.ToList();
            var outputs = new List<ExportConstructionUnitOutput>();
            foreach (var res in resList)
            {
                var output = res.MapTo<ExportConstructionUnitOutput>();
               // output.ProposerName = userList.Where(u => u.Id == res.ProposerId).Select(u => u.Name).FirstOrDefault();
               // output.DepartmentName = organizationUnitList.Where(u => u.Id == res.DepartmentId).Select(u => u.Name).FirstOrDefault();
                outputs.Add(output);
            }
            return outputs.Entity2Excel(comments, title);
        }

        public GetConstructionUnitOutput Get(int id)
        {
            var project = _constructionUnitRepository.Get().Where(u => u.Id == id).FirstOrDefault();
            if (project == null)
            {
                throw new EntityException("Id", id, "ConstructionUnit", "不存在");
            }
            else
            {

                var recordOutput = project.MapTo<GetConstructionUnitOutput>();
                return recordOutput;
            }
        }
        /// <summary>
        /// 发起记录
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public GetConstructionUnitOutput GetByTask(int taskId)
        {
            var authInfo = _authInfoProvider.GetCurrent();
            var userId = authInfo?.User?.Id;
            var task = _taskProvider.GetTask(taskId);
            if (task.Assignee != userId.Value.ToString())
            {
                throw new AppCoreException($"id:为{userId.Value}的用户不是任务【{task.NodeName}】的委托人");
            }
            var project = _constructionUnitRepository.Get().Where(u => u.ProcessInstanceId == task.ProcessInstanceId).FirstOrDefault();
            if (project == null)
            {
                throw new EntityException("ProjectInstanceId", task.ProcessInstanceId, "ConstructionUnit", "不存在");
            }
            var processDefinition = _definitionProvder.Get(task.ProcessDefinitionId);
            var invisibleFields = ((UserTaskNode)processDefinition.GetNode(task.NodeUid)).GetInvisibleFields();
            if (invisibleFields != null)
                TypeDefaulter.SetDefault(project, invisibleFields.ToArray());

            return project.MapTo<GetConstructionUnitOutput>();

        }
        /// <summary>
        /// 等待记录
        /// </summary>
        /// <param name="processId"></param>
        /// <returns></returns>
        public GetConstructionUnitOutput GetByProcess(int processId)
        {
            var project = _constructionUnitRepository.Get().Where(u => u.ProcessInstanceId == processId).FirstOrDefault();
            if (project == null)
            {
                throw new EntityException("ProjectInstanceId", processId, "MemorabiliaRecord", "不存在");
            }

            return project.MapTo<GetConstructionUnitOutput>();

        }


        /// <summary>
        /// 处理记录
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public GetConstructionUnitOutput GetByTaskHistory(int taskId)
        {
            var authInfo = _authInfoProvider.GetCurrent();
            var userId = authInfo?.User?.Id;
            var task = _historyProvider.GetTaskHistory(taskId);
            if (!task.AssigneeList.Contains($"{userId}"))
            {
                throw new AppCoreException($"id:为{userId.Value}的用户不是任务【{task.NodeName}】的委托人");
            }
            var project = _constructionUnitRepository.Get().Where(u => u.ProcessInstanceId == task.ProcessInstanceId).FirstOrDefault();
            if (project == null)
            {
                throw new EntityException("ProjectInstanceId", task.ProcessInstanceId, "MemorabiliaRecord", "不存在");
            }
            var processDefinition = _definitionProvder.Get(task.ProcessDefinitionId);
            var invisibleFields = ((UserTaskNode)processDefinition.GetNode(task.NodeUid)).GetInvisibleFields();
            if (invisibleFields != null)
                TypeDefaulter.SetDefault(project, invisibleFields.ToArray());

            return project.MapTo<GetConstructionUnitOutput>();

        }

        public int Update(StartProcessInput<UpdateConstructionUnitInput> input)
        {
            var variables = new Dictionary<string, object>();
            var userId = _authInfoProvider.GetCurrent().User.Id;
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
            var record = input.Data.MapTo<ConstructionUnit>();
            record.ProcessInstanceId = processInstanceId;
            record.State = DataState.Updating;
            record.Mid = record.Id;
            record.Id = 0;
            _constructionUnitRepository.Add(record);
            var task = tasks[0];
            _taskProvider.Complete(task.Id);
            return record.Id;
        }
    }
}
