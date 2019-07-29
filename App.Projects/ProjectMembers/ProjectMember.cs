using App.Core.Common.Entities;
using App.Core.Common.Repositories;
using App.Projects.ProjectBaseInfos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace App.Projects.ProjectMembers
{
    public class ProjectMember: EntityHaveTenant
    { 
        [DisableUpdate]
        public int ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        public Project Project { get; set; }
        /// <summary>
        /// 项目角色
        /// </summary>
        public string ProjectRole { get; set; }
        /// <summary>
        /// 人员Id
        /// </summary>
        public int UserId { get; set; }
    }
}
