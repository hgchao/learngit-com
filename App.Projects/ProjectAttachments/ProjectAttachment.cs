using App.Core.Common.Entities;
using App.Core.Common.Repositories;
using App.Core.FileManagement.Files;
using App.Projects.ProjectBaseInfos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace App.Projects.ProjectAttachments
{
    public class ProjectAttachment: EntityHaveTenant
    {
        [DisableUpdate]
        public int ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        public Project Project { get; set; }

        public int FileMetaId { get; set; }
        [ForeignKey("FileMetaId")]
        public FileMeta FileMeta { get; set; }
    }
}
