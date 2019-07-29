using App.Core.Common.Entities;
using App.Core.Common.Repositories;
using App.Core.FileManagement.Files;
using App.ProjectGantts.Tasks;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace App.ProjectGantts.TaskAttachments
{
    public class ProjectTaskAttachment: EntityHaveTenant
    {
        [DisableUpdate]
        public int ProjectTaskId { get; set; }
        [ForeignKey("ProjectTaskId")]
        public ProjectTask ProjectTask { get; set; }

        public int FileMetaId { get; set; }
        [ForeignKey("FileMetaId")]
        public FileMeta FileMeta { get; set; }
    }
}
