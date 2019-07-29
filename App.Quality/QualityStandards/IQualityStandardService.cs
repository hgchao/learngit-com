using App.Core.Common.Entities;
using App.Quality.QualityStandards.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Quality.QualityStandards
{
    public interface IQualityStandardService
    {
        int Add(AddQualityStandardInput input);
        void Update(UpdateQualityStandardInput input);
        GetQualityStandardOutput Get(int id);
        void Delete(int id);

        PaginationData<GetQualityStandardOutput> Get(int pageIndex, int pageSize, int? categoryId, string keyword,
               string sortField,
               string sortState);
    }
}
