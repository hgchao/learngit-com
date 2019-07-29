using System;
using System.Collections.Generic;
using System.Text;

namespace App.Projects.ProjectUnitMembers.Dto
{
    public class AddProjectUnitInput
    {
        public int Id { get; set; }
        /// <summary>
        /// 单位类型
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 公司名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 单位参与人员
        /// </summary>
        public List<AddProjectUnitMemberInput> Members { get; set; }
        public AddProjectUnitInput()
        {
            Members = new List<AddProjectUnitMemberInput>();
        }
    }
}
