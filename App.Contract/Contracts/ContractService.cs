using App.Base;
using App.Base.EntityFramework;
using App.Base.Repositories;
using App.Contract.Contracts.Dto;
using App.Core.Authorization.Accounts;
using App.Core.Common;
using App.Core.Common.Entities;
using App.Core.Common.Exceptions;
using App.Core.Common.Repositories;
using App.Core.FileManagement.Files;
using App.Core.FileManagement.Repositories;
using App.Core.Workflow;
using App.Core.Workflow.Engine.Definition.Nodes.Tasks;
using App.Core.Workflow.Providers;
using App.Projects.ProjectBaseInfos;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Oa.Project;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using PoorFff;
using PoorFff.Dependencies;
using PoorFff.EventBus;
using PoorFff.Excel;
using PoorFff.Mapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace App.Contract.Contracts
{
    public class ContractService: IContractService
    {

       // private IAppRepositoryBase<Project> _projectRepository { get; set; }
        private IAuthInfoProvider _authInfoProvider;
        private IHistoryProvider _historyProvider;
        private IRuntimeProvider _runtimeProvider;
        private ITaskProvider _taskProvider;
        private IDefinitionProvider _definitionProvder;
        private IAppRepositoryBase<Contractt> _contractRepository;
        private IAppDbContextProvider _dbContextProvider;
        private IProjectHelper _projectHelper;

        public ContractService(
            IAuthInfoProvider authInfoProvider,
            IWfEngine wfEngine,
            IAppRepositoryBase<Contractt> contractRepository,
             // IAppRepositoryBase<Project> projectRepository,
            IAppDbContextProvider dbContextProvider,
            IProjectHelper projectHelper


            )
        {
            _projectHelper = projectHelper;
            _authInfoProvider = authInfoProvider;
            _contractRepository = contractRepository;
          //  _projectRepository = projectRepository;
            _dbContextProvider = dbContextProvider;
            _runtimeProvider = wfEngine.GetRuntimeProvider();
            _taskProvider = wfEngine.GetTaskProvider();
            _definitionProvder = wfEngine.GetDefinitionProvider();
            _historyProvider = wfEngine.GetHistoryProvider();
        }

        public int Add(StartProcessInput<AddContractInput> input)
        {
            if (input.Data.Name == null)
            {
                throw new AppCoreException("合同名称不能为空");
            }
            var variables = new Dictionary<string, object>();
            var userId = _authInfoProvider.GetCurrent().User.Id;
            if (!_projectHelper.HasPermission("合同信息负责人", input.Data.ProjectId))//权限设置
            {
                throw new AppCoreException("合同信息发布没有权限");
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
            var contractt = input.Data.MapTo<Contractt>();
             var count = ContractContext.Instance.GetContractNoCount(() => _contractRepository.Count(u => u.CreateTime.Value.Year == DateTime.Now.Year && (u.State == DataState.Stable || u.State == DataState.Creating)));
            contractt.ProcessInstanceId = processInstanceId;
            contractt.State = DataState.Creating;
            contractt.ContractNumber = $"{DateTime.Now.Year}{count.ToString("0000")}";
            contractt.Mid = null;
            _contractRepository.Add(contractt);
            var task = tasks[0];
            if (!input.PreventCommit)
                _taskProvider.Complete(task.Id);
            return contractt.Id;
        }
        /// <summary>
        /// 最后一个审批结束执行
        /// </summary>
        /// <param name="projectInstanceId"></param>
        public void CompleteApproval(int projectInstanceId)
        {
            var project = _contractRepository.Get().Include(u => u.Deposit).Include(u => u.Attachments).Where(u => u.ProcessInstanceId == projectInstanceId).FirstOrDefault();
            if (project == null)
            {
                throw new EntityException("ProjectInstanceId", projectInstanceId, "Contractt", "不存在");
            }
            if (project.State == DataState.Creating)
            {

                using (var transaction = _dbContextProvider.BeginTransaction())
                {

                    var temp = JsonConvert.DeserializeObject<Contractt>(JsonConvert.SerializeObject(project));
                    temp.State = DataState.Stable;
                    temp.ProcessInstanceId = null;
                    _contractRepository.Update(temp, new System.Linq.Expressions.Expression<Func<Contractt, object>>[] {
                        u => u.State,
                        u => u.ProcessInstanceId
                    }, true);
                    temp = JsonConvert.DeserializeObject<Contractt>(JsonConvert.SerializeObject(project));
                    temp.State = DataState.Created;
                    temp.Id = 0;
                    temp.Mid = project.Id;
                    //清空之前附件
                    temp.Attachments.ForEach(u =>
                    {
                        u.Id = 0;
                    });
                    if (temp.Deposit != null)
                    {
                        temp.Deposit.Id = 0;
                    }
                    _contractRepository.Add(temp);
                    transaction.Commit();
                }

            }
            else if (project.Mid != null && project.State == DataState.Updating)
            {
                using (var transaction = _dbContextProvider.BeginTransaction())
                {
                    var temp = JsonConvert.DeserializeObject<Contractt>(JsonConvert.SerializeObject(project));
                    temp.State = DataState.Updated;
                    _contractRepository.Update(temp, new System.Linq.Expressions.Expression<Func<Contractt, object>>[] {
                        u=>u.State
                    }, true);
                    temp = JsonConvert.DeserializeObject<Contractt>(JsonConvert.SerializeObject(project));
                    temp.Id = project.Mid.Value;
                    temp.Mid = null;
                    //清空之前附件
                    temp.Attachments.ForEach(u =>
                    {
                        u.Id = 0;
                    });
                    if (temp.Deposit != null)
                    {
                        temp.Deposit.Id = 0;
                    }
                    var existing = _contractRepository.Get().Include(u => u.Deposit).Include(u => u.Attachments).Where(u => u.Id == project.Mid).FirstOrDefault();
                    _contractRepository.Update(temp, existing, new System.Linq.Expressions.Expression<Func<Contractt, object>>[] {
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
        public void CompleteTask(CompleteTaskInput<UpdateContractInput> input)
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
                var project = input.Data.MapTo<Contractt>();
                if (editableFields == null)
                {
                    _contractRepository.Update(project, new System.Linq.Expressions.Expression<Func<Contractt, object>>[] {
                        u => u.State,
                        u=>u.Mid,
                        u=>u.ProcessInstanceId
                    }, false);
                }
                else
                {
                    _contractRepository.Update(project, editableFields.ToArray());
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
                return _contractRepository.Count(u => u.Name == name && u.Id != id && u.State == DataState.Stable);
            }
            else
            {
                return _contractRepository.Count(u => u.Name == name && u.State == DataState.Stable);
            }
        }

        public GetContractCountOutput GetTotalCount(string year)
        {
            
            var contractList = _contractRepository.Get().Select(u => new { u.State,u.ContractPrice,u.SigningDate }).Where(u => u.State == DataState.Stable && u.SigningDate.Value.Year == Convert.ToInt32(year)).ToList();
            var January = contractList.Count(u => u.State == DataState.Stable && u.SigningDate.Value.Month == 1);
            var February = contractList.Count(u => u.State == DataState.Stable && u.SigningDate.Value.Month == 2);
            var March = contractList.Count(u => u.State == DataState.Stable && u.SigningDate.Value.Month == 3);
            var April = contractList.Count(u => u.State == DataState.Stable && u.SigningDate.Value.Month == 4);
            var May = contractList.Count(u => u.State == DataState.Stable && u.SigningDate.Value.Month == 5);
            var June = contractList.Count(u => u.State == DataState.Stable && u.SigningDate.Value.Month == 6);
            var July = contractList.Count(u => u.State == DataState.Stable && u.SigningDate.Value.Month == 7);
            var August = contractList.Count(u => u.State == DataState.Stable && u.SigningDate.Value.Month == 8);
            var September = contractList.Count(u => u.State == DataState.Stable && u.SigningDate.Value.Month == 9);
            var October = contractList.Count(u => u.State == DataState.Stable && u.SigningDate.Value.Month == 10);
            var November = contractList.Count(u => u.State == DataState.Stable && u.SigningDate.Value.Month == 11);
            var December = contractList.Count(u => u.State == DataState.Stable && u.SigningDate.Value.Month == 12);
            var contractStr = January + "," + February + "," + March + "," + April + "," + May + "," + June + "," + July + "," + August + "," + September + "," + October + "," + November + "," + December;
            var JanuaryMoney = contractList.Where(u => u.State == DataState.Stable && u.SigningDate.Value.Month == 1).ToList().Sum(w => w.ContractPrice);
            var FebruaryMoney = contractList.Where(u => u.State == DataState.Stable && u.SigningDate.Value.Month == 2).ToList().Sum(w => w.ContractPrice);
            var MarchMoney = contractList.Where(u => u.State == DataState.Stable && u.SigningDate.Value.Month == 3).ToList().Sum(w => w.ContractPrice);
            var AprilMoney = contractList.Where(u => u.State == DataState.Stable && u.SigningDate.Value.Month == 4).ToList().Sum(w => w.ContractPrice);
            var MayMoney = contractList.Where(u => u.State == DataState.Stable && u.SigningDate.Value.Month == 5).ToList().Sum(w => w.ContractPrice);
            var JuneMoney = contractList.Where(u => u.State == DataState.Stable && u.SigningDate.Value.Month == 6).ToList().Sum(w => w.ContractPrice);
            var JulyMoney = contractList.Where(u => u.State == DataState.Stable && u.SigningDate.Value.Month == 7).ToList().Sum(w => w.ContractPrice);
            var AugustMoney = contractList.Where(u => u.State == DataState.Stable && u.SigningDate.Value.Month == 8).ToList().Sum(w => w.ContractPrice);
            var SeptemberMoney = contractList.Where(u => u.State == DataState.Stable && u.SigningDate.Value.Month == 9).ToList().Sum(w => w.ContractPrice);
            var OctoberMoney = contractList.Where(u => u.State == DataState.Stable && u.SigningDate.Value.Month == 10).ToList().Sum(w => w.ContractPrice);
            var NovemberMoney = contractList.Where(u => u.State == DataState.Stable && u.SigningDate.Value.Month == 11).ToList().Sum(w => w.ContractPrice);
            var DecemberMoney = contractList.Where(u => u.State == DataState.Stable && u.SigningDate.Value.Month == 12).ToList().Sum(w => w.ContractPrice);
            var contractStrMoney = JanuaryMoney + "," + FebruaryMoney + "," + MarchMoney + "," + AprilMoney + "," + MayMoney + "," + JuneMoney + "," + JulyMoney + "," + AugustMoney + "," + SeptemberMoney + "," + OctoberMoney + "," + NovemberMoney + "," + DecemberMoney;

            var countOutput = new GetContractCountOutput();
           
            countOutput.MonthCount = contractStr;//合同个数
            countOutput.MonthMoney = contractStrMoney;//合同金额
            return countOutput;
        }

        public PaginationData<GetContractListOutput> Get(int? projectId,
               int pageIndex, int pageSize,
               string keyword,
                  string sortField,
               string sortState
            )
        {
            var query = _contractRepository.Get()
                  .Include(u => u.Project)
                .Include(u => u.Category)
                 .Include(u => u.Deposit)
                .Include(u => u.Contractor)
                .Where(u => u.State == DataState.Stable);
            var userId = _authInfoProvider.GetCurrent()?.User?.Id ?? 0;

            #region query conditions
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(u =>
                    u.Name.Contains(keyword) ||
                    u.ContractNumber.Contains(keyword));
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
                    case "Name"://合同名称
                        query = sortState == "1" ? query.OrderByDescending(u => u.Name) : sortState == "2" ? query.OrderBy(u => u.Name) : query.OrderByDescending(u => u.CreateTime);
                        break;
                    case "ProjectId"://项目名称
                        query = sortState == "1" ? query.OrderByDescending(u => u.Name) : sortState == "2" ? query.OrderBy(u => u.Name) : query.OrderByDescending(u => u.CreateTime);
                        break;
                    case "CategoryId"://合同分类
                        query = sortState == "1" ? query.OrderByDescending(u => u.Name) : sortState == "2" ? query.OrderBy(u => u.Name) : query.OrderByDescending(u => u.CreateTime);
                        break;
                    case "Section"://标段
                        query = sortState == "1" ? query.OrderByDescending(u => u.Name) : sortState == "2" ? query.OrderBy(u => u.Name) : query.OrderByDescending(u => u.CreateTime);
                        break;
                    case "ContractorId"://承包方
                        query = sortState == "1" ? query.OrderByDescending(u => u.Name) : sortState == "2" ? query.OrderBy(u => u.Name) : query.OrderByDescending(u => u.CreateTime);
                        break;
                    case "ContractPrice"://合同价
                        query = sortState == "1" ? query.OrderByDescending(u => u.Name) : sortState == "2" ? query.OrderBy(u => u.Name) : query.OrderByDescending(u => u.CreateTime);
                        break;
                    case "Signature"://签署对象
                        query = sortState == "1" ? query.OrderByDescending(u => u.Name) : sortState == "2" ? query.OrderBy(u => u.Name) : query.OrderByDescending(u => u.CreateTime);
                        break;
                    case "CompletionDate"://竣工时间
                        query = sortState == "1" ? query.OrderByDescending(u => u.CompletionDate) : sortState == "2" ? query.OrderBy(u => u.CompletionDate) : query.OrderByDescending(u => u.CreateTime);
                        break;
                    case "Attachments"://附件
                        query = sortState == "1" ? query.OrderByDescending(u => u.Attachments.Any(v=>v.ContractId== userId)) : sortState == "2" ? query.OrderBy(u => u.Attachments.Any(v => v.ContractId == userId)) : query.OrderByDescending(u => u.CreateTime);
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
            var paging = PaginationDataHelper.WrapData<Contractt, T>(query, pageIndex, pageSize).TransferTo<GetContractListOutput>();
            paging.Data.ForEach(u => u.HasPermission = _projectHelper.HasPermission("合同信息负责人", u.ProjectId));
            return paging;
        }
        public void Delete(int id)
        {
            var problem = _contractRepository.Get().Where(u => u.Id == id).FirstOrDefault();
            if (problem != null)
            {
                var userId = _authInfoProvider.GetCurrent().User.Id;
                if (!_projectHelper.HasPermission("合同信息负责人", problem.ProjectId))//权限设置
                {
                    throw new AppCoreException("合同信息删除没有权限");
                }
                _contractRepository.Delete(new Contractt { Id = id });
            }

        }
        public MemoryStream Export(
             string title,
               Dictionary<string, string> comments,
               string keyword,
               List<int> contractApplicationIds

            )
        {
            // throw new NotImplementedException();
            var query = _contractRepository.Get()
                .Include(u => u.Project)
                .Include(u => u.Category)
                 .Include(u => u.Deposit)
                .Include(u => u.Contractor)
               .Where(u => u.State == DataState.Stable);
            var userId = _authInfoProvider.GetCurrent()?.User?.Id ?? 0;

            #region query conditions
            if (contractApplicationIds != null && contractApplicationIds.Count > 0)
            {
                query = query.Where(u => contractApplicationIds.Contains(u.Id));
            }
            else
            {
                if (!string.IsNullOrEmpty(keyword))
                {
                    query = query.Where(u =>
                     u.Name.Contains(keyword) ||
                     u.ContractNumber.Contains(keyword));
                }


            }

            #endregion
            query = query.OrderByDescending(u => u.CreateTime);
            // var userList = _userRepository.Get().Select(u => new { u.Id, u.Name }).ToList();
            //  var organizationUnitList = _organizationUnitRepository.Get().Select(u => new { u.Id, u.Name }).ToList();
            var resList = query.ToList();
            var outputs = new List<ExportContractOutput>();
            foreach (var res in resList)
            {
                var output = res.MapTo<ExportContractOutput>();
                // output.ProposerName = userList.Where(u => u.Id == res.ProposerId).Select(u => u.Name).FirstOrDefault();
                // output.DepartmentName = organizationUnitList.Where(u => u.Id == res.DepartmentId).Select(u => u.Name).FirstOrDefault();
                outputs.Add(output);
            }
            return outputs.Entity2Excel(comments, title);
        }

        public GetContractOutput Get(int id)
        {
            var project = _contractRepository.Get()
                 .Include(u => u.Project)
                .Include(u => u.Category)
                .Include(u => u.Deposit)
                .Include(u => u.Contractor)
                .Include(u => u.Attachments).ThenInclude(w => w.FileMeta).Where(u => u.Id == id).FirstOrDefault();
            if (project == null)
            {
                throw new EntityException("Id", id, "Contractt", "不存在");
            }
            else
            {

                var recordOutput = project.MapTo<GetContractOutput>();
                return recordOutput;
            }
        }
        /// <summary>
        /// 发起记录
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public GetContractOutput GetByTask(int taskId)
        {
            var authInfo = _authInfoProvider.GetCurrent();
            var userId = authInfo?.User?.Id;
            var task = _taskProvider.GetTask(taskId);
            if (task.Assignee != userId.Value.ToString())
            {
                throw new AppCoreException($"id:为{userId.Value}的用户不是任务【{task.NodeName}】的委托人");
            }
            var project = _contractRepository.Get()
                 .Include(u => u.Project)
                .Include(u => u.Category)
                .Include(u => u.Deposit)
                .Include(u => u.Contractor)
                .Include(u => u.Attachments).ThenInclude(w => w.FileMeta).Where(u => u.ProcessInstanceId == task.ProcessInstanceId).FirstOrDefault();
            if (project == null)
            {
                throw new EntityException("ProjectInstanceId", task.ProcessInstanceId, "Contractt", "不存在");
            }
            var processDefinition = _definitionProvder.Get(task.ProcessDefinitionId);
            var invisibleFields = ((UserTaskNode)processDefinition.GetNode(task.NodeUid)).GetInvisibleFields();
            if (invisibleFields != null)
                TypeDefaulter.SetDefault(project, invisibleFields.ToArray());

            return project.MapTo<GetContractOutput>();

        }
        /// <summary>
        /// 等待记录
        /// </summary>
        /// <param name="processId"></param>
        /// <returns></returns>
        public GetContractOutput GetByProcess(int processId)
        {
            var project = _contractRepository.Get()
                 .Include(u => u.Project)
                .Include(u => u.Category)
                .Include(u => u.Deposit)
                .Include(u => u.Contractor)
                .Include(u => u.Attachments).ThenInclude(w => w.FileMeta).Where(u => u.ProcessInstanceId == processId).FirstOrDefault();
            if (project == null)
            {
                throw new EntityException("ProjectInstanceId", processId, "Contractt", "不存在");
            }

            return project.MapTo<GetContractOutput>();

        }


        /// <summary>
        /// 处理记录
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public GetContractOutput GetByTaskHistory(int taskId)
        {
            var authInfo = _authInfoProvider.GetCurrent();
            var userId = authInfo?.User?.Id;
            var task = _historyProvider.GetTaskHistory(taskId);
            if (!task.AssigneeList.Contains($"{userId}"))
            {
                throw new AppCoreException($"id:为{userId.Value}的用户不是任务【{task.NodeName}】的委托人");
            }
            var project = _contractRepository.Get()
                 .Include(u => u.Project)
                .Include(u => u.Category)
               // .Include(u => u.ContractionMethod)
                .Include(u => u.Deposit)
                .Include(u => u.Contractor)
                .Include(u => u.Attachments).ThenInclude(w => w.FileMeta).Where(u => u.ProcessInstanceId == task.ProcessInstanceId).FirstOrDefault();
            if (project == null)
            {
                throw new EntityException("ProjectInstanceId", task.ProcessInstanceId, "Contractt", "不存在");
            }
            var processDefinition = _definitionProvder.Get(task.ProcessDefinitionId);
            var invisibleFields = ((UserTaskNode)processDefinition.GetNode(task.NodeUid)).GetInvisibleFields();
            if (invisibleFields != null)
                TypeDefaulter.SetDefault(project, invisibleFields.ToArray());

            return project.MapTo<GetContractOutput>();

        }

        public int Update(StartProcessInput<UpdateContractInput> input)
        {
            var variables = new Dictionary<string, object>();
            var userId = _authInfoProvider.GetCurrent().User.Id;
            if (!_projectHelper.HasPermission("合同信息负责人", input.Data.ProjectId))//权限设置
            {
                throw new AppCoreException("合同信息修改没有权限");
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
            var record = input.Data.MapTo<Contractt>();
            record.ProcessInstanceId = processInstanceId;
            record.State = DataState.Updating;
            record.Mid = record.Id;
            record.Id = 0;
            _contractRepository.Add(record);
            var task = tasks[0];
            _taskProvider.Complete(task.Id);
            return record.Id;
        }
    }
}