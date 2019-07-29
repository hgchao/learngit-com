using App.Base;
using App.Core.Common.Entities;
using App.Quality.QualityAccidentDisposals;
using App.Quality.QualityAccidentDisposals.Dto;
using App.Quality.QualityAccidents.Dto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace App.Quality.QualityAccidents
{
    public interface IQualityAccidentService
    {

        int Count(string name, int? id);

        void Delete(int id);
        int Add(StartProcessInput<AddQualityAccidentInput> input);
        int Update(StartProcessInput<UpdateQualityAccidentInput> input);
        /// <summary>
        /// 最后一个审批结束执行
        /// </summary>
        /// <param name="projectInstanceId"></param>
        void CompleteApproval(int projectInstanceId);
        /// <summary>
        /// 每次审核办理执行
        /// </summary>
        /// <param name="input"></param>
        void CompleteTask(CompleteTaskInput<UpdateQualityAccidentInput> input);
        /// <summary>
        /// 发起记录
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        GetQualityAccidentOutput GetByTask(int taskId);
        /// <summary>
        /// 等待记录
        /// </summary>
        /// <param name="processId"></param>
        /// <returns></returns>
        GetQualityAccidentOutput GetByProcess(int processId);
        /// <summary>
        /// 处理记录
        /// </summary>
        /// <param name="taskHistoryId"></param>
        /// <returns></returns>
        GetQualityAccidentOutput GetByTaskHistory(int taskHistoryId);
        GetQualityAccidentOutput Get(int id);
        PaginationData<GetQualityAccidentListOutput> Get(
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
              List<int> qualityAccidentApplicationIds
           );
        int AddDisposal(int accidentId, AddQualityAccidentDisposalInput disposalInput);
        void SettleAccident(SettleQualityAccidentInput input);
    }
}
