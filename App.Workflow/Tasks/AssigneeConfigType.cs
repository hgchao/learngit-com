using System;
using System.Collections.Generic;
using System.Text;

namespace App.Workflow.Tasks
{
    public class AssigneeConfigType
    {
        /// <summary>
        /// 角色
        /// </summary>
        public const string Role = "Role";
        /// <summary>
        /// 部门 
        /// </summary>
        public const string Department = "Department";
        /// <summary>
        /// 上级领导（节点执行人）
        /// </summary>
        public const string SuperiorOfNodeAssignee = "SuperiorOfNodeAssignee";
    }
}
