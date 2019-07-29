using App.Base;
using App.Core.Common.Entities;
using App.Quality.QualityProblemRectifications;
using App.Quality.QualityProblemRectifications.Dto;
using App.Quality.QualityProblems.Dto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace App.Quality.QualityProblems
{
    public interface IQualityProblemService
    {
        //首页统计
        GetQProblemCountOutput GetCount(DateTime? createDateL, DateTime? createDateR);
        int Count(string name, int? id);

        void Delete(int id);
        int Add(StartProcessInput<AddQualityProblemInput> input);
        int Update(StartProcessInput<UpdateQualityProblemInput> input);
        /// <summary>
        /// 最后一个审批结束执行
        /// </summary>
        /// <param name="projectInstanceId"></param>
        void CompleteApproval(int projectInstanceId);
        /// <summary>
        /// 每次审核办理执行
        /// </summary>
        /// <param name="input"></param>
        void CompleteTask(CompleteTaskInput<UpdateQualityProblemInput> input);
        /// <summary>
        /// 发起记录
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        GetQualityProblemOutput GetByTask(int taskId);
        /// <summary>
        /// 等待记录
        /// </summary>
        /// <param name="processId"></param>
        /// <returns></returns>
        GetQualityProblemOutput GetByProcess(int processId);
        /// <summary>
        /// 处理记录
        /// </summary>
        /// <param name="taskHistoryId"></param>
        /// <returns></returns>
        GetQualityProblemOutput GetByTaskHistory(int taskHistoryId);
        GetQualityProblemOutput Get(int id);
        PaginationData<GetQualityProblemListOutput> Get(int? projectId,
              int pageIndex, int pageSize,
             RectificationState? rectificationState,
              string keyword,
              int? categoryId,
              string sortField,
              string sortState


           );
        MemoryStream Export(int? projectId,
              string title,
              Dictionary<string, string> comments,
              RectificationState? rectificationState,
              string keyword,
              int? categoryId,
              List<int> qualityAccidentApplicationIds
           );
        /// <summary>
        /// 添加整改情况
        /// </summary>
        /// <param name="problemId"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        int AddRectification(int problemId, AddQualityProblemRectificationInput input);
        /// <summary>
        /// 完成整改
        /// </summary>
        /// <param name="problemId"></param>
        /// <param name="completedTime"></param>
        void CompleteRetification(CompleteQualityProblemInput input);
    }
}
