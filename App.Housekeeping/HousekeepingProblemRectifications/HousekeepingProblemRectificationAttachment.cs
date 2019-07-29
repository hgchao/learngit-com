using App.Core.Common.Entities;
using App.Core.FileManagement.Files;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace App.Housekeeping.HousekeepingProblemRectifications
{
   public class HousekeepingProblemRectificationAttachment : Entity
    {
        public int HousekeepingProblemRectificationId { get; set; }
        public int FileMetaId { get; set; }

        [ForeignKey("HousekeepingProblemRectificationId")]
        public HousekeepingProblemRectification HousekeepingProblemRectification { get; set; }
        [ForeignKey("FileMetaId")]
        public FileMeta FileMeta { get; set; }
    }
}
