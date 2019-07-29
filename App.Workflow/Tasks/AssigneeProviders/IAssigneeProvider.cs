using App.Core.Workflow.Engine;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Workflow.Tasks.AssigneeProviders
{
    public interface IAssigneeProvider
    {
        List<string> GetRoleUser(ProcessInstance processInstance, int roleId);
        List<string> GetDepartmentUser(ProcessInstance processInstance, int departmentId);
        string GetSuperiorUserList(ProcessInstance processInstance, string nodeuid);
    }
}
