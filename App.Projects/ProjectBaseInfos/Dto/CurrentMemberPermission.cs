using System;
using System.Collections.Generic;
using System.Text;

namespace App.Projects.ProjectBaseInfos.Dto
{
    public class CurrentMemberPermission
    {
        /// <summary>
        /// 项目角色
        /// </summary>
        public string ProjectRole { get; set; }
        /// <summary>
        /// 项目Id
        /// </summary>
        public int ProjectId { get; set; }
        /// <summary>
        /// 是否有权限
        /// </summary>
        public bool HasPermission { get; set; }
    }
}
