using App.Base;
using App.Core.Common.Entities;
using App.ProjectEarlyStage.EarlyStages.Dto;
using App.ProjectEarlyStage.PmEarlyStages.Dto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace App.ProjectEarlyStage.EarlyStages
{
    public interface IEarlyStageService
    {
        //int Add(AddEarlyStageInput input);
        //void Update(UpdateEarlyStageInput input);
        //List<GetEarlyStageOutput> GetByProject(int projectId);
        //GetEarlyStageOutput Get(int id);
        int Count(string name, int? id);


        int Add(StartProcessInput<AddEarlyStageInput> input);
        int Update(StartProcessInput<UpdateEarlyStageInput> input);
        /// <summary>
        /// 最后一个审批结束执行
        /// </summary>
        /// <param name="projectInstanceId"></param>
        void CompleteApproval(int projectInstanceId);
        /// <summary>
        /// 每次审核办理执行
        /// </summary>
        /// <param name="input"></param>
        void CompleteTask(CompleteTaskInput<UpdateEarlyStageInput> input);
        /// <summary>
        /// 发起记录
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        GetEarlyStageOutput GetByTask(int taskId);
        /// <summary>
        /// 等待记录
        /// </summary>
        /// <param name="processId"></param>
        /// <returns></returns>
        GetEarlyStageOutput GetByProcess(int processId);
        /// <summary>
        /// 处理记录
        /// </summary>
        /// <param name="taskHistoryId"></param>
        /// <returns></returns>
        GetEarlyStageOutput GetByTaskHistory(int taskHistoryId);
        GetEarlyStageOutput Get(int id);
        List<GetEarlyStageOutput> Get(int? projectId ,
              string typeName,
              string sortField,
              string sortState
           );
        MemoryStream Export(int? projectId,
              string title,
              Dictionary<string, string> comments,
              string typeName,
              List<int> earlyStageApplicationIds
           );
    }
}
