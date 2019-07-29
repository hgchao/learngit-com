using System;
using System.Collections.Generic;
using System.Text;

namespace App.Projects.ProjectMembers.Dto
{
    public class GetProjectUnitMemberOutput
    {
        public int Id { get; set; }
        /// <summary>
        /// 单位角色
        /// </summary>
        public string ProjectUnitRole { get; set; }
        public string User { get; set; }
    }
}
