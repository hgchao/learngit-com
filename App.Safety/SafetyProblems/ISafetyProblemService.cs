using App.Base;
using App.Core.Common.Entities;
using App.Safety.SafetyProblemRectifications;
using App.Safety.SafetyProblemRectifications.Dto;
using App.Safety.SafetyProblems.Dto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace App.Safety.SafetyProblems
{
    public interface ISafetyProblemService
    {
        //首页统计
        GetSProblemCountOutput GetCount(DateTime? createDateL, DateTime? createDateR);
        int Count(string name, int? id);


        int Add(StartProcessInput<AddSafetyProblemInput> input);
        void Delete(int id);
        int Update(StartProcessInput<UpdateSafetyProblemInput> input);
        /// <summary>
        /// 最后一个审批结束执行
        /// </summary>
        /// <param name="projectInstanceId"></param>
        void CompleteApproval(int projectInstanceId);
        /// <summary>
        /// 每次审核办理执行
        /// </summary>
        /// <param name="input"></param>
        void CompleteTask(CompleteTaskInput<UpdateSafetyProblemInput> input);
        /// <summary>
        /// 发起记录
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        GetSafetyProblemOutput GetByTask(int taskId);
        /// <summary>
        /// 等待记录
        /// </summary>
        /// <param name="processId"></param>
        /// <returns></returns>
        GetSafetyProblemOutput GetByProcess(int processId);
        /// <summary>
        /// 处理记录
        /// </summary>
        /// <param name="taskHistoryId"></param>
        /// <returns></returns>
        GetSafetyProblemOutput GetByTaskHistory(int taskHistoryId);
        GetSafetyProblemOutput Get(int id);
        PaginationData<GetSafetyProblemListOutput> Get(
            int? projectId,
              int pageIndex, int pageSize,
             RectificationState? rectificationState,
              string keyword,
              int? categoryId,
              int? sourceId,
              string sortField,
              string sortState


           );
        MemoryStream Export(
              int? projectId,
              string title,
              Dictionary<string, string> comments,
              RectificationState? rectificationState,
              string keyword,
              int? categoryId,
              int? sourceId,
              List<int> qualityAccidentApplicationIds
           );
        /// <summary>
        /// 添加整改情况
        /// </summary>
        /// <param name="problemId"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        int AddRectification(int problemId, AddSafetyProblemRectificationInput input);
        /// <summary>
        /// 完成整改
        /// </summary>
        /// <param name="problemId"></param>
        /// <param name="completedTime"></param>
        void CompleteRetification(CompleteSafetyProblemInput input);
    }
}
