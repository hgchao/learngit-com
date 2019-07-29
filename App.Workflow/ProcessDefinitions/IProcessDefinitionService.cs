using App.Core.Common.Entities;
using App.Core.Workflow.ProcessDefinitions;
using App.Core.Workflow.ProcessDefinitions.Dto;
using App.Workflow.ProcessDefinitions.Dto;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace App.Workflow.ProcessDefinitions
{
    public interface IProcessDefinitionService
    {
        int Add(ProcessDefinitionWithRole input);
        List<string> GetNameByUser(int userId);
        List<int> GetIdByProcess(int processId);
        PaginationData<GetProcessDefinitionListOutput> Get(int pageIndex, int pageSize, string keyword, Expression<Func<Wf_Re_ProcessDefinition, bool>> extraCondition);
    }
}
