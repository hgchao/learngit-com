using App.Core.Common.Entities;
using App.ProjectGantts.Links;
using App.ProjectGantts.Tasks;
using App.Projects.ProjectBaseInfos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace App.ProjectGantts.Gantts
{
    public class ProjectGantt: EntityWithAuditHaveTenant
    {
        public int ProjectId { get; set; }
        /// <summary>
        /// 项目信息
        /// </summary>
        [ForeignKey("ProjectId")]
        public Project Project { get; set; }
        public List<ProjectTask> Tasks { get; set; }
        public List<ProjectLink> Links { get; set; }
        public ProjectGantt()
        {
            Tasks = new List<ProjectTask>();
            Links = new List<ProjectLink>();
        }
    }
}
