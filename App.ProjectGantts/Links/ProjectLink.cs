using App.Core.Common.Entities;
using App.ProjectGantts.Gantts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace App.ProjectGantts.Links
{
    public class ProjectLink: EntityHaveTenant
    {
        public int GanttId { get; set; }
        [ForeignKey("GanttId")]
        public ProjectGantt Gantt { get; set; }

        public string Type { get; set; }
        public int SourceTaskId { get; set; }
        public int TargetTaskId { get; set; }
    }
}
