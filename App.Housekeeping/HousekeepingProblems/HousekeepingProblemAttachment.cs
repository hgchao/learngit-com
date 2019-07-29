using App.Core.Common.Entities;
using App.Core.FileManagement.Files;
using App.Housekeeping.Housekeepings;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace App.Housekeeping.HousekeepingProblems
{
  public  class HousekeepingProblemAttachment : Entity
    {
        public int HousekeepingProblemId { get; set; }
        public int FileMetaId { get; set; }

        [ForeignKey("HousekeepingProblemId")]
        public HousekeepingProblem HousekeepingProblem { get; set; }
        [ForeignKey("FileMetaId")]
        public FileMeta FileMeta { get; set; }
    }
}
