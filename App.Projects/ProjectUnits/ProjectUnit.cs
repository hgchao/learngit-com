using App.Core.Common.Entities;
using App.Core.Common.Repositories;
using App.Projects.ProjectBaseInfos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace App.Projects.ProjectUnits
{
    public class ProjectUnit: EntityHaveTenant
    {
        [DisableUpdate]
        public int ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        public Project Project { get; set; }

        /// <summary>
        /// 公司类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 公司名称
        /// </summary>
        public string Name { get; set; }

        public List<ProjectUnitMember> Members { get; set; }
        public ProjectUnit()
        {
            Members = new List<ProjectUnitMember>();
        }
    }
}
