using App.Base;
using App.Core.Common.Entities;
using App.Safety.SafetyAccidentDisposals;
using App.Safety.SafetyAccidentDisposals.Dto;
using App.Safety.SafetyAccidents.Dto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace App.Safety.SafetyAccidents
{
    public interface ISafetyAccidentService
    {
      
        int Count(string name, int? id);


        int Add(StartProcessInput<AddSafetyAccidentInput> input);
        void Delete(int id);
        int Update(StartProcessInput<UpdateSafetyAccidentInput> input);
        /// <summary>
        /// 最后一个审批结束执行
        /// </summary>
        /// <param name="projectInstanceId"></param>
        void CompleteApproval(int projectInstanceId);
        /// <summary>
        /// 每次审核办理执行
        /// </summary>
        /// <param name="input"></param>
        void CompleteTask(CompleteTaskInput<UpdateSafetyAccidentInput> input);
        /// <summary>
        /// 发起记录
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        GetSafetyAccidentOutput GetByTask(int taskId);
        /// <summary>
        /// 等待记录
        /// </summary>
        /// <param name="processId"></param>
        /// <returns></returns>
        GetSafetyAccidentOutput GetByProcess(int processId);
        /// <summary>
        /// 处理记录
        /// </summary>
        /// <param name="taskHistoryId"></param>
        /// <returns></returns>
        GetSafetyAccidentOutput GetByTaskHistory(int taskHistoryId);
        GetSafetyAccidentOutput Get(int id);
        PaginationData<GetSafetyAccidentListOutput> Get(
              int? projectId,
              int pageIndex, int pageSize,
              DisposalState? state,
              string keyword,
              string sortField,
              string sortState


           );
        MemoryStream Export(
              int? projectId,
              string title,
              Dictionary<string, string> comments,
              DisposalState? state,
              string keyword,
              List<int> safetyAccidentApplicationIds
           );
        int AddDisposal(int accidentId, AddSafetyAccidentDisposalInput disposalInput);
        void SettleAccident(SettleSafetyAccidentInput input);
    }
}
