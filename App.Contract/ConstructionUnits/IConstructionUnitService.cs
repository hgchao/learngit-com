using App.Base;
using App.Contract.ConstructionUnits.Dto;
using App.Core.Common.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace App.Contract.ConstructionUnits
{
    public interface IConstructionUnitService
    {
        int Count(string name, int? id);


        int Add(StartProcessInput<AddConstructionUnitInput> input);
        int Update(StartProcessInput<UpdateConstructionUnitInput> input);
        /// <summary>
        /// 最后一个审批结束执行
        /// </summary>
        /// <param name="projectInstanceId"></param>
        void CompleteApproval(int projectInstanceId);
        /// <summary>
        /// 每次审核办理执行
        /// </summary>
        /// <param name="input"></param>
        void CompleteTask(CompleteTaskInput<UpdateConstructionUnitInput> input);
        /// <summary>
        /// 发起记录
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        GetConstructionUnitOutput GetByTask(int taskId);
        /// <summary>
        /// 等待记录
        /// </summary>
        /// <param name="processId"></param>
        /// <returns></returns>
        GetConstructionUnitOutput GetByProcess(int processId);
        /// <summary>
        /// 处理记录
        /// </summary>
        /// <param name="taskHistoryId"></param>
        /// <returns></returns>
        GetConstructionUnitOutput GetByTaskHistory(int taskHistoryId);
        GetConstructionUnitOutput Get(int id);

        PaginationData<GetConstructionUnitListOutput> Get(
               int pageIndex, int pageSize,
               string keyword,
               string sortField,
               string sortState


            );
        MemoryStream Export(
               string title,
               Dictionary<string, string> comments,
               string keyword,
               List<int> constructionUnitApplicationIds
            );
    }
}
