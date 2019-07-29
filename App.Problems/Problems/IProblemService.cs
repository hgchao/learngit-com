using App.Base;
using App.Core.Common.Entities;
using App.Problems.ProblemRectifications;
using App.Problems.ProblemRectifications.Dto;
using App.Problems.Problems.Dto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace App.Problems.Problems
{
    public interface IProblemService
    {
       
        int Count(string name, int? id);

        void Delete(int id);
        int Add(StartProcessInput<AddProblemInput> input);
        int Update(StartProcessInput<UpdateProblemInput> input);
        /// <summary>
        /// 最后一个审批结束执行
        /// </summary>
        /// <param name="projectInstanceId"></param>
        void CompleteApproval(int projectInstanceId);
        /// <summary>
        /// 每次审核办理执行
        /// </summary>
        /// <param name="input"></param>
        void CompleteTask(CompleteTaskInput<UpdateProblemInput> input);
        /// <summary>
        /// 发起记录
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        GetProblemOutput GetByTask(int taskId);
        /// <summary>
        /// 等待记录
        /// </summary>
        /// <param name="processId"></param>
        /// <returns></returns>
        GetProblemOutput GetByProcess(int processId);
        /// <summary>
        /// 处理记录
        /// </summary>
        /// <param name="taskHistoryId"></param>
        /// <returns></returns>
        GetProblemOutput GetByTaskHistory(int taskHistoryId);
        GetProblemOutput Get(int id);
        PaginationData<GetProblemListOutput> Get(
              int? projectId,
              int pageIndex, int pageSize,
              int? categoryId,
              CoordinationState? coordinationState,
              string keyword,
              string sortField,
              string sortState


           );
        MemoryStream Export(
             int? projectId,
              string title,
              Dictionary<string, string> comments,
              int? categoryId,
              CoordinationState? coordinationState,
              string keyword,
              List<int> qualityAccidentApplicationIds
           );
        /// <summary>
        /// 添加协调情况
        /// </summary>
        /// <param name="problemId"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        int AddCoordination(int problemId, AddProblemCoordinationInput input);

        /// <summary>
        /// 完成协调
        /// </summary>
        void CompleteCoordination(CompleteProblemInput input);
    }
}
