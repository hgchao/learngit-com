using System;
using System.Collections.Generic;
using System.Text;

namespace App.Projects.ProjectMembers.Dto
{
    public class GetProjectUnitOutput
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
        public List<GetProjectUnitMemberOutput> Members { get; set; }
        public GetProjectUnitOutput()
        {
            Members = new List<GetProjectUnitMemberOutput>();
        }
    }
}
