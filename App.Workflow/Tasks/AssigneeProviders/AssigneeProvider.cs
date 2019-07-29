using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using App.Core.Authorization.Users;
using App.Core.Authorization.Users.Dto;
using App.Core.Common.Exceptions;
using App.Core.Workflow;
using App.Core.Workflow.Engine;

namespace App.Workflow.Tasks.AssigneeProviders
{
    public class AssigneeProvider : IAssigneeProvider
    {
        private IUserService _userService;
        private IWfEngine _wfEngine;
        public AssigneeProvider(
            IWfEngine wfEngine,
            IUserService userService
            )
        {
            _wfEngine = wfEngine;
            _userService = userService;
        }
        public string GetSuperiorUserList(ProcessInstance processInstance, string nodeUid)
        {
            var task = _wfEngine.GetHistoryProvider().GetTaskHistory(u => u.ProcessInstanceId == processInstance.Id && u.NodeUid == nodeUid).FirstOrDefault();
            if (task == null)
            {
                throw new AppCoreException($"找不到ProcessInstanceId为{processInstance.Id}, nodeUid为{nodeUid}的历史任务");
            }
            if (task.AssigneeList.Count == 0)
                throw new EntityPropertyException("NodeUid", nodeUid, "Wf_Hi_TaskInstance", "Assignee", "不存在");
            var assignee = Convert.ToInt32(task.AssigneeList[0]);
            var user = _userService.Get(assignee);
            if (user == null)
            {
                throw new EntityException("id", assignee, "User", "不存在");
            }
            if (user.SuperiorId == null)
            {
                throw new EntityPropertyException("name", user.Name, "User", "SuperiorId", "不存在");
            }
            return user.SuperiorId.ToString();
        }

        public List<string> GetRoleUser(ProcessInstance processInstance, int roleId)
        {
            List<GetUserListOutput> users = _userService.Get(u => u.UserRoles.Any(v => roleId == v.RoleId)).ToList();
            var res = users.Select(u => u.Id.ToString()).ToList();
            return res;
        }

        public List<string> GetDepartmentUser(ProcessInstance processInstance, int departmentId)
        {
            List<GetUserListOutput> users = _userService.Get(u => u.UserUnits.Any(v => departmentId == v.OrganizationUnitId)).ToList();
            var res = users.Select(u => u.Id.ToString()).ToList();
            return res;
        }

    }
}
