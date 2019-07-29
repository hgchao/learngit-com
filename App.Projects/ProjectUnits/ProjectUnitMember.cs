using App.Core.Common.Entities;
using App.Core.Common.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace App.Projects.ProjectUnits
{
    public class ProjectUnitMember: EntityHaveTenant
    {
        [DisableUpdate]
        public int ProjectUnitId { get; set; }
        [ForeignKey("ProjectUnitId")]
        public ProjectUnit ProjectUnit { get; set; }
        /// <summary>
        /// 单位角色
        /// </summary>
        public string ProjectUnitRole { get; set; }
        /// <summary>
        /// 人员
        /// </summary>
        public string User { get; set; }
    }
}
