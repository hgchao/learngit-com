using System;
using System.Collections.Generic;
using System.Text;

namespace App.Projects.ProjectMembers.Dto
{
    public class UpdateProjectMemberInput
    {
        public int Id { get; set; }
        /// <summary>
        /// 项目角色
        /// </summary>
        public string ProjectRole { get; set; }
        public int UserId { get; set; }
    }
}
