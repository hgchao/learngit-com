using App.Base;
using App.Core.Common.Entities;
using App.Memorabilia.MemorabiliaRecords.Dto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace App.Memorabilia.MemorabiliaRecords
{
    public interface IMemorabiliaRecordService
    {

        int Count(string name, int? id);

        void Delete(int id);
        int Add(StartProcessInput<AddMemorabiliaRecordInput> input);
        int Update(StartProcessInput<UpdateMemorabiliaRecordInput> input);
        /// <summary>
        /// 最后一个审批结束执行
        /// </summary>
        /// <param name="projectInstanceId"></param>
        void CompleteApproval(int projectInstanceId);
        /// <summary>
        /// 每次审核办理执行
        /// </summary>
        /// <param name="input"></param>
        void CompleteTask(CompleteTaskInput<UpdateMemorabiliaRecordInput> input);
        /// <summary>
        /// 发起记录
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        GetMemorabiliaRecordOutput GetByTask(int taskId);
        /// <summary>
        /// 等待记录
        /// </summary>
        /// <param name="processId"></param>
        /// <returns></returns>
        GetMemorabiliaRecordOutput GetByProcess(int processId);
        /// <summary>
        /// 处理记录
        /// </summary>
        /// <param name="taskHistoryId"></param>
        /// <returns></returns>
        GetMemorabiliaRecordOutput GetByTaskHistory(int taskHistoryId);
        GetMemorabiliaRecordOutput Get(int id);
      
        PaginationData<GetMemorabiliaRecordListOutput> Get(int? projectId,
               int pageIndex, int pageSize,
               string keyword,
               string sortField,
               string sortState


            );
        MemoryStream Export(
               string title,
               Dictionary<string, string> comments,
               string keyword,
               List<int> memorabiliaApplicationIds
            );
    }

}
