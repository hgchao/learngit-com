using App.Core.Common.Entities;
using App.Safety.SafetyStandards.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Safety.SafetyStandards
{
    public interface ISafetyStandardService
    {
        int Add(AddSafetyStandardInput input);
        void Update(UpdateSafetyStandardInput input);
        GetSafetyStandardOutput Get(int id);
        void Delete(int id);

        PaginationData<GetSafetyStandardOutput> Get(int pageIndex, int pageSize, int? categoryId, string keyword,
               string sortField,
               string sortState);
    }
}
