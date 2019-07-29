using App.Core.Common.Entities;
using App.Projects.ProjectBaseInfos.Dto;
using App.Statistics.ProjectStatistics.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Statistics.ProjectStatistics
{
  public interface IProjectStatisticsService
    {
        PaginationData<GetProjectStatisticsListOutput> Get(int pageIndex, int pageSize, string keyword,
         string sortField,
         string sortState
         );
        PaginationData<GetProjectWithProblemListOutput> Get(int pageIndex, int pageSize, string keyword,
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
    }
}
