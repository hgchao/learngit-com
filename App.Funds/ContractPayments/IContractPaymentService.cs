using App.Base;
using App.Core.Common.Entities;
using App.Funds.ContractPayments.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Funds.ContractPayments
{
    public interface IContractPaymentService
    {
        //首页统计
        GetContractPaymentMoneyOutput GetTotalMoney(DateTime? paymentTimeDateL, DateTime? paymentTimeDateR);

        void Delete(int id);
        int Add(StartProcessInput<AddContractPaymentInput> input);
        int Update(StartProcessInput<UpdateContractPaymentInput> input);
        /// <summary>
        /// 最后一个审批结束执行
        /// </summary>
        /// <param name="projectInstanceId"></param>
        void CompleteApproval(int projectInstanceId);
        /// <summary>
        /// 每次审核办理执行
        /// </summary>
        /// <param name="input"></param>
        void CompleteTask(CompleteTaskInput<UpdateContractPaymentInput> input);
        /// <summary>
        /// 发起记录
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        GetContractPaymentOutput GetByTask(int taskId);
        /// <summary>
        /// 等待记录
        /// </summary>
        /// <param name="processId"></param>
        /// <returns></returns>
        GetContractPaymentOutput GetByProcess(int processId);
        /// <summary>
        /// 处理记录
        /// </summary>
        /// <param name="taskHistoryId"></param>
        /// <returns></returns>
        GetContractPaymentOutput GetByTaskHistory(int taskHistoryId);
        GetContractPaymentOutput Get(int id);
        PaginationData<GetContractPaymentOutput> Get(
              int? contractId,
              int pageIndex, int pageSize,
              string keyword,
              string sortField,
              string sortState


           );
    }
}
