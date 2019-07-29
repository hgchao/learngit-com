using App.Base;
using App.Core.Common.Entities;
using App.Housekeeping.HousekeepingProblemRectifications;
using App.Housekeeping.HousekeepingProblemRectifications.Dto;
using App.Housekeeping.Housekeepings.Dto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace App.Housekeeping.Housekeepings
{
    public interface IHousekeepingProblemService
    {
      
        int Count(string name, int? id);


        int Add(StartProcessInput<AddHousekeepingProblemInput> input);
        void Delete(int id);
        int Update(StartProcessInput<UpdateHousekeepingProblemInput> input);
        /// <summary>
        /// 最后一个审批结束执行
        /// </summary>
        /// <param name="projectInstanceId"></param>
        void CompleteApproval(int projectInstanceId);
        /// <summary>
        /// 每次审核办理执行
        /// </summary>
        /// <param name="input"></param>
        void CompleteTask(CompleteTaskInput<UpdateHousekeepingProblemInput> input);
        /// <summary>
        /// 发起记录
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        GetHousekeepingProblemOutput GetByTask(int taskId);
        /// <summary>
        /// 等待记录
        /// </summary>
        /// <param name="processId"></param>
        /// <returns></returns>
        GetHousekeepingProblemOutput GetByProcess(int processId);
        /// <summary>
        /// 处理记录
        /// </summary>
        /// <param name="taskHistoryId"></param>
        /// <returns></returns>
        GetHousekeepingProblemOutput GetByTaskHistory(int taskHistoryId);
        GetHousekeepingProblemOutput Get(int id);
        PaginationData<GetHousekeepingProblemListOutput> Get(int? projectId,
              int pageIndex, int pageSize,
             RectificationState? rectificationState,
              string keyword,
              string sortField,
              string sortState


           );
        MemoryStream Export(int? projectId,
              string title,
              Dictionary<string, string> comments,
              RectificationState? rectificationState,
              string keyword,
              List<int> housekeepingApplicationIds
           );
        /// <summary>
        /// 添加整改情况
        /// </summary>
        /// <param name="problemId"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        int AddRectification(int problemId, AddHousekeepingProblemRectificationInput input);
        /// <summary>
        /// 完成整改
        /// </summary>
        /// <param name="problemId"></param>
        /// <param name="completedTime"></param>
        void CompleteRetification(CompleteHousekeepingProblemInput input);

       
    }
}
