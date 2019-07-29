using System;
using System.Collections.Generic;
using System.Text;

namespace App.Projects.ProjectUnitMembers.Dto
{
    public class UpdateProjectUnitMemberInput
    {
        public int Id { get; set; }
        /// <summary>
        /// 单位角色
        /// </summary>
        public string ProjectUnitRole { get; set; }
        public string User { get; set; }
    }
}
