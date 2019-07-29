using App.Base;
using App.Core.Common.Entities;
using App.Projects.ProjectBaseInfos.Dto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace App.Projects.Projects
{
    public interface IProjectService
    { /// <summary>
      /// 首页矩形图，数据为不同月份项目分布个数
      /// </summary>
      /// <param name="year"></param>
      /// <returns></returns>
        GetProjectStatisticsOutput GetTotalCount(string year);
        //首页统计
        GetProjectCountOutput GetTotalMoney(DateTime? commencementDateL, DateTime? commencementDateR);

        int Add(StartProcessInput<AddProjectInput> input);
        int Update(StartProcessInput<UpdateProjectInput> input);
        void Delete(int id);
        void CompleteProcess(int processInstanceId);
        void CompleteTask(CompleteTaskInput<UpdateProjectInput> input);
        GetProjectOutput GetByTask(int taskId);
        GetProjectOutput GetByProcess(int processId);
        GetProjectOutput GetByTaskHistory(int taskHistoryId);
        GetProjectOutput Get(int id);
        PaginationData<GetProjectListOutput> Get(int pageIndex, int pageSize, string keyword,
            string address,
            int? typeId,
            string Name,
            string No,
            int? projectNatureId,
            int? stateId,
            int? projectLeaderId,
            int? generalEstimateL, int? generalEstimateR,
            DateTime? commencementDateL, DateTime? commencementDateR,
            string sortField,
            string sortState
            );
        List<GetProjectListOutput> Get(string keyword,
            string address,
            int? typeId,
            string Name,
            string No,
            int? projectNatureId,
            int? stateId,
            int? projectLeaderId,
            int? generalEstimateL, int? generalEstimateR,
            DateTime? commencementDateL, DateTime? commencementDateR
            );
        MemoryStream Export(int pageIndex, int pageSize, string keyword);

        List<CurrentMemberPermission> GetCurrentPermission(List<int> projectIdList, string projectRole);
    }

}
