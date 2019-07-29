using App.Base;
using App.Core.Common.Entities;
using App.RecordMgmt.Records.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.RecordMgmt.Records
{
   public interface IRecordService
    {
        int Add(StartProcessInput<AddRecordInput> input);
        int Update(StartProcessInput<UpdateRecordInput> input);
        void Delete(int id);
        void CompleteProcess(int processInstanceId);
        void CompleteTask(CompleteTaskInput<UpdateRecordInput> input);
        GetRecordOutput Get(int id);
        GetRecordOutput GetByTask(int taskId);
        GetRecordOutput GetByProcess(int processId);
        GetRecordOutput GetByTaskHistory(int taskHistoryId);
        PaginationData<GetRecordListOutput> Get(int pageIndex, int pageSize, int projectId, int? typeId, string keyword,
              string sortField,
              string sortState);
    }
}
