using App.Base;
using App.Contract.Contracts.Dto;
using App.Core.Common.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace App.Contract.Contracts
{
    public interface IContractService
    {
        int Count(string name, int? id);
        void Delete(int id);
        //首页月度咨询量
        GetContractCountOutput GetTotalCount(string year);

        int Add(StartProcessInput<AddContractInput> input);
        int Update(StartProcessInput<UpdateContractInput> input);
        /// <summary>
        /// 最后一个审批结束执行
        /// </summary>
        /// <param name="projectInstanceId"></param>
        void CompleteApproval(int projectInstanceId);
        /// <summary>
        /// 每次审核办理执行
        /// </summary>
        /// <param name="input"></param>
        void CompleteTask(CompleteTaskInput<UpdateContractInput> input);
        /// <summary>
        /// 发起记录
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        GetContractOutput GetByTask(int taskId);
        /// <summary>
        /// 等待记录
        /// </summary>
        /// <param name="processId"></param>
        /// <returns></returns>
        GetContractOutput GetByProcess(int processId);
        /// <summary>
        /// 处理记录
        /// </summary>
        /// <param name="taskHistoryId"></param>
        /// <returns></returns>
        GetContractOutput GetByTaskHistory(int taskHistoryId);
        GetContractOutput Get(int id);

        PaginationData<GetContractListOutput> Get(int? projectId,
               int pageIndex, int pageSize,
               string keyword,
               string sortField,
               string sortState


            );
        MemoryStream Export(
               string title,
               Dictionary<string, string> comments,
               string keyword,
               List<int> contractApplicationIds
            );
    }
}

