using App.Core.Common.Entities;
using App.Core.FileManagement.Files;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace App.ProjectProgress.WeeklyProgresses
{
  public  class WeeklyProgressAttachment : Entity
    {
        public int WeeklyProgressId { get; set; }
        public int FileMetaId { get; set; }

        [ForeignKey("WeeklyProgressId")]
        public WeeklyProgress WeeklyProgress { get; set; }
        [ForeignKey("FileMetaId")]
        public FileMeta FileMeta { get; set; }
    }
}
