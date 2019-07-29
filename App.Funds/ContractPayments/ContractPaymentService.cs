using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using App.Base;
using App.Base.EntityFramework;
using App.Base.Repositories;
using App.Core.Authorization.Accounts;
using App.Core.Common;
using App.Core.Common.Entities;
using App.Core.Common.Exceptions;
using App.Core.Workflow;
using App.Core.Workflow.Engine.Definition.Nodes.Tasks;
using App.Core.Workflow.Providers;
using App.Funds.ContractPayments.Dto;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Oa.Project;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using PoorFff;
using PoorFff.Mapper;

namespace App.Funds.ContractPayments
{
    public class ContractPaymentService : IContractPaymentService
    {
        private IAuthInfoProvider _authInfoProvider;
        private IHistoryProvider _historyProvider;
        private IRuntimeProvider _runtimeProvider;
        private ITaskProvider _taskProvider;
        private IDefinitionProvider _definitionProvder;
        private IAppRepositoryBase<ContractPayment> _contractPaymentRepository;
        private IAppDbContextProvider _dbContextProvider;
        public ContractPaymentService(
             IAuthInfoProvider authInfoProvider,
            IAppRepositoryBase<ContractPayment> contractPaymentRepository,
            IAppDbContextProvider dbContextProvider,
             IWfEngine wfEngine
            )
        {
            _authInfoProvider = authInfoProvider;
            _contractPaymentRepository = contractPaymentRepository;
            _dbContextProvider = dbContextProvider;
            _runtimeProvider = wfEngine.GetRuntimeProvider();
            _taskProvider = wfEngine.GetTaskProvider();
            _definitionProvder = wfEngine.GetDefinitionProvider();
            _historyProvider = wfEngine.GetHistoryProvider();
        }
        public int Add(StartProcessInput<AddContractPaymentInput> input)
        {
            if (input.Data.ContractId == 0)
            {
                throw new AppCoreException("合同id不能为0");
            }
            var variables = new Dictionary<string, object>();
            var userId = _authInfoProvider.GetCurrent().User.Id;
            variables.Add("starter", userId.ToString());
            int processInstanceId = _runtimeProvider.StartProcessInstanceByName(
                input.ProcessDefinitionName,
                $"{input.ProcessDefinitionName}",//-{input.Data.Title}
                variables);
            var tasks = _taskProvider.GetByProcessInstance(processInstanceId);
            if (tasks.Count == 0)
            {
                throw new AppCoreException("任务未创建成功！");
            }
           
            var contractPayment = input.Data.MapTo<ContractPayment>();
            // var count = AppSampleContext.Instance.GetSupplierNoCount(() => _supplierRepository.Count(u => u.CreateTime.Value.Year == DateTime.Now.Year && (u.State == DataState.Stable || u.State == DataState.Creating)));
            contractPayment.ProcessInstanceId = processInstanceId;
            contractPayment.State = DataState.Creating;
            // memorabiliaRecord.SupplierNo = $"{DateTime.Now.Year}{count.ToString("000000")}";
            var paymentNumberPrefix = $"CP{DateTime.Now.ToString("yyyyMMdd")}";
            var count = _contractPaymentRepository.Count(u => u.PaymentNumber.Contains(paymentNumberPrefix) && (u.State == DataState.Stable || u.State == DataState.Creating));
            contractPayment.PaymentNumber= $"{paymentNumberPrefix}{count.ToString("00")}";
            contractPayment.Mid = null;
            _contractPaymentRepository.Add(contractPayment);
            var task = tasks[0];
            if (!input.PreventCommit)
                _taskProvider.Complete(task.Id);
            return contractPayment.Id;
        }
        /// <summary>
        /// 最后一个审批结束执行
        /// </summary>
        /// <param name="projectInstanceId"></param>
        public void CompleteApproval(int projectInstanceId)
        {
            var project = _contractPaymentRepository.Get().Where(u => u.ProcessInstanceId == projectInstanceId).FirstOrDefault();
            if (project == null)
            {
                throw new EntityException("ProjectInstanceId", projectInstanceId, "ContractPayment", "不存在");
            }
            if (project.State == DataState.Creating)
            {

                using (var transaction = _dbContextProvider.BeginTransaction())
                {

                    var temp = JsonConvert.DeserializeObject<ContractPayment>(JsonConvert.SerializeObject(project));
                    temp.State = DataState.Stable;
                    temp.ProcessInstanceId = null;
                    _contractPaymentRepository.Update(temp, new System.Linq.Expressions.Expression<Func<ContractPayment, object>>[] {
                        u => u.State,
                        u => u.ProcessInstanceId
                    }, true);
                    temp = JsonConvert.DeserializeObject<ContractPayment>(JsonConvert.SerializeObject(project));
                    temp.State = DataState.Created;
                    temp.Id = 0;
                    temp.Mid = project.Id;
                    ////清空之前附件
                    //temp.AccidentPhotoSets.ForEach(u =>
                    //{
                    //    u.Id = 0;
                    //});
                    _contractPaymentRepository.Add(temp);
                    transaction.Commit();
                }
              
            }
            else if (project.Mid != null && project.State == DataState.Updating)
            {
                using (var transaction = _dbContextProvider.BeginTransaction())
                {
                    var temp = JsonConvert.DeserializeObject<ContractPayment>(JsonConvert.SerializeObject(project));
                    temp.State = DataState.Updated;
                    _contractPaymentRepository.Update(temp, new System.Linq.Expressions.Expression<Func<ContractPayment, object>>[] {
                        u=>u.State
                    }, true);
                    temp = JsonConvert.DeserializeObject<ContractPayment>(JsonConvert.SerializeObject(project));
                    temp.Id = project.Mid.Value;
                    temp.Mid = null;
                    ////清空之前附件
                    //temp.AccidentPhotoSets.ForEach(u =>
                    //{
                    //    u.Id = 0;
                    //});
                    var existing = _contractPaymentRepository.Get().Where(u => u.Id == project.Mid).FirstOrDefault();
                    _contractPaymentRepository.Update(temp, existing, new System.Linq.Expressions.Expression<Func<ContractPayment, object>>[] {
                        u=>u.ProcessInstanceId,
                        u=>u.State,
                        u=>u.PaymentNumber
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
        public void CompleteTask(CompleteTaskInput<UpdateContractPaymentInput> input)
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
                var project = input.Data.MapTo<ContractPayment>();
                if (editableFields == null)
                {
                    _contractPaymentRepository.Update(project, new System.Linq.Expressions.Expression<Func<ContractPayment, object>>[] {
                        u => u.State,
                        u=>u.Mid,
                        u=>u.ProcessInstanceId,
                        u=>u.PaymentNumber
                    }, false);
                }
                else
                {
                    _contractPaymentRepository.Update(project, editableFields.ToArray());
                }
            }
            if (!input.PreventCommit)
            {
                _taskProvider.Complete(input.Id, input.Comment);
            }
        }


        public PaginationData<GetContractPaymentOutput> Get(int? contractId,
               int pageIndex, int pageSize,
               string keyword,
                  string sortField,
               string sortState
            )
        {
            var query = _contractPaymentRepository.Get()
                .Include(u => u.Contract)
                .Where(u => u.State == DataState.Stable);
           // var userId = _authInfoProvider.GetCurrent()?.User?.Id ?? 0;

            #region query conditions
            if (!string.IsNullOrEmpty(contractId.ToString()))
            {
                query = query.Where(u => u.ContractId == contractId);
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(u =>
                                    u.Contract.Name.Contains(keyword) ||
                                    u.Contract.ContractNumber.Contains(keyword) ||
                                    u.PaymentNumber.Contains(keyword) ||
                                    u.PaymentType.Contains(keyword)
                                    );
            }
          

            //sortState 1降序2升序0默认
            if (!string.IsNullOrEmpty(sortField) && !string.IsNullOrEmpty(sortState))
            {
                switch (sortField)
                {
                    case "PaidAmount"://支付金额
                        query = sortState == "1" ? query.OrderByDescending(u => u.PaidAmount) : sortState == "2" ? query.OrderBy(u => u.PaidAmount) : query.OrderByDescending(u => u.CreateTime);
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
            return PaginationDataHelper.WrapData<ContractPayment, T>(query, pageIndex, pageSize).TransferTo<GetContractPaymentOutput>();
        }
        

        public GetContractPaymentOutput Get(int id)
        {
            var accident = _contractPaymentRepository.Get().Where(u => u.Id == id)
                .Include(u => u.Contract)
                .FirstOrDefault();
            if (accident == null)
            {
                throw new EntityException("Id", id, "ContractPayment", "不存在");
            }
            else
            {

                var recordOutput = accident.MapTo<GetContractPaymentOutput>();
                return recordOutput;
            }
        }
        /// <summary>
        /// 发起记录
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public GetContractPaymentOutput GetByTask(int taskId)
        {
            var authInfo = _authInfoProvider.GetCurrent();
            var userId = authInfo?.User?.Id;
            var task = _taskProvider.GetTask(taskId);
            if (task.Assignee != userId.Value.ToString())
            {
                throw new AppCoreException($"id:为{userId.Value}的用户不是任务【{task.NodeName}】的委托人");
            }
            var accident = _contractPaymentRepository.Get()
                 .Include(u => u.Contract).Where(u => u.ProcessInstanceId == task.ProcessInstanceId).FirstOrDefault();
            if (accident == null)
            {
                throw new EntityException("ProjectInstanceId", task.ProcessInstanceId, "ContractPayment", "不存在");
            }
            var processDefinition = _definitionProvder.Get(task.ProcessDefinitionId);
            var invisibleFields = ((UserTaskNode)processDefinition.GetNode(task.NodeUid)).GetInvisibleFields();
            if (invisibleFields != null)
                TypeDefaulter.SetDefault(accident, invisibleFields.ToArray());

            return accident.MapTo<GetContractPaymentOutput>();

        }
        /// <summary>
        /// 等待记录
        /// </summary>
        /// <param name="processId"></param>
        /// <returns></returns>
        public GetContractPaymentOutput GetByProcess(int processId)
        {
            var accident = _contractPaymentRepository.Get()
                 .Include(u => u.Contract).Where(u => u.ProcessInstanceId == processId).FirstOrDefault();
            if (accident == null)
            {
                throw new EntityException("ProjectInstanceId", processId, "ContractPayment", "不存在");
            }

            return accident.MapTo<GetContractPaymentOutput>();

        }


        /// <summary>
        /// 处理记录
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public GetContractPaymentOutput GetByTaskHistory(int taskId)
        {
            var authInfo = _authInfoProvider.GetCurrent();
            var userId = authInfo?.User?.Id;
            var task = _historyProvider.GetTaskHistory(taskId);
            if (!task.AssigneeList.Contains($"{userId}"))
            {
                throw new AppCoreException($"id:为{userId.Value}的用户不是任务【{task.NodeName}】的委托人");
            }
            var accident = _contractPaymentRepository.Get()
                 .Include(u => u.Contract).Where(u => u.ProcessInstanceId == task.ProcessInstanceId).FirstOrDefault();
            if (accident == null)
            {
                throw new EntityException("ProjectInstanceId", task.ProcessInstanceId, "ContractPayment", "不存在");
            }
            var processDefinition = _definitionProvder.Get(task.ProcessDefinitionId);
            var invisibleFields = ((UserTaskNode)processDefinition.GetNode(task.NodeUid)).GetInvisibleFields();
            if (invisibleFields != null)
                TypeDefaulter.SetDefault(accident, invisibleFields.ToArray());

            return accident.MapTo<GetContractPaymentOutput>();

        }

        public int Update(StartProcessInput<UpdateContractPaymentInput> input)
        {
            var variables = new Dictionary<string, object>();
            var userId = _authInfoProvider.GetCurrent().User.Id;
            variables.Add("starter", userId.ToString());
            int processInstanceId = _runtimeProvider.StartProcessInstanceByName(
                input.ProcessDefinitionName,
                $"{input.ProcessDefinitionName}",//-{input.Data.Title}
                variables);
            var tasks = _taskProvider.GetByProcessInstance(processInstanceId);
            if (tasks.Count == 0)
            {
                throw new AppCoreException("任务未创建成功！");
            }
            var record = input.Data.MapTo<ContractPayment>();
            record.ProcessInstanceId = processInstanceId;
            record.State = DataState.Updating;
            record.Mid = record.Id;
            record.Id = 0;
            _contractPaymentRepository.Add(record);
            var task = tasks[0];
            _taskProvider.Complete(task.Id);
            return record.Id;
        }

        public void Delete(int id)
        {
            _contractPaymentRepository.Delete(new ContractPayment { Id = id });
        }

        public GetContractPaymentMoneyOutput GetTotalMoney(DateTime? paymentTimeDateL, DateTime? paymentTimeDateR)
        {
            var query = _contractPaymentRepository.Get().Select(u => new { u.State, u.CompleteAmount, u.PaidAmount, u.PaymentTime, u.CreateTime }).Where(u => u.State == DataState.Stable);

            if (paymentTimeDateL != null)
            {
                query = query.Where(u => u.PaymentTime >= paymentTimeDateL);
            }
            if (paymentTimeDateR != null)
            {
                query = query.Where(u => u.PaymentTime <= paymentTimeDateR);
            }



            var countOutput = new GetContractPaymentMoneyOutput();
            foreach (var res in query.ToList())
            {
                countOutput.CompletionMoney += res.CompleteAmount;//累计完成金额
                countOutput.ShouldMoney += res.PaidAmount;//累计应支付金额
            }
            return countOutput;
        }
    }
}
