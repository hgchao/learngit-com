using App.Core.Common.Entities;
using App.Core.Common.Repositories;
using App.Core.Workflow.ProcessInstances;
using App.ProjectGantts.Gantts;
using App.ProjectGantts.TaskAttachments;
using Oa.Project;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace App.ProjectGantts.Tasks
{
    public class ProjectTask: EntityWithSortHaveTenant
    {
        public int GanttId { get; set; }
        [DisableUpdate]
        [ForeignKey("GanttId")]
        public ProjectGantt Gantt { get; set; }
        /// <summary>
        /// 节点执行人
        /// </summary>
        public int Assignee { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 任务内容
        /// </summary>
        public string Content { get; set; }
        public DateTime StartDate { get; set; }
        public int Duration { get; set; }
        [Column(TypeName = "decimal(3,2)")]
        public decimal Progress { get; set; }
        public int? Pid { get; set; }
        public string Type { get; set; }
        /// <summary>
        /// 实际开始时间
        /// </summary>
        public DateTime? ActualStartDate { get; set; }
        /// <summary>
        /// 实际持续天数
        /// </summary>
        public int? ActualDuration { get; set; }
        /// <summary>
        /// 备注 
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 附件
        /// </summary>
        public List<ProjectTaskAttachment> Attachments { get; set; }

        public ProjectTask()
        {
            Attachments = new List<ProjectTaskAttachment>();
        }
    }
}
