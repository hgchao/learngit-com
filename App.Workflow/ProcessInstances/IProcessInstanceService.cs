using App.Core.Common.Entities;
using App.Core.Form.FieldQueryConditions;
using App.Core.Form.Fields.Dto;
using App.Core.Workflow.ProcessInstances.Dto;
using App.Workflow.ProcessInstances.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Workflow.ProcessInstances
{
    public interface IProcessInstanceService
    {
        void Start(StartProcessInput input, Action<int, Dictionary<string, object>> setVariables);

        void Start(StartProcessInput input, int previous, bool autoExecuteFirstTask);

        void Terminate(int id);
        void Suspend(int id);
        void Restart(int id);
        GetProcessInstanceOutput Get(int id);
        List<FieldDto> GetFields(int id);
        PaginationData<GetProcessInstanceListOutput> Get(int pageIndex, int pageSize,
            string keyword,
            int? creatorId,
            DateTime? createTimeL, DateTime? createTimeR);
        PaginationData<GetProcessInstancePoorOutput> GetForFields(int pageIndex, int pageSize, string formName, List<string> needFields, List<FieldQueryCondition> fieldQueryConditions, string sort, bool isAsc = true);

    }
}
