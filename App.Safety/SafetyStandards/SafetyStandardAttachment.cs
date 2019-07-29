using App.Core.Common.Entities;
using App.Core.FileManagement.Files;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace App.Safety.SafetyStandards
{
    public class SafetyStandardAttachment : Entity
    {
        public int SafetyStandardId { get; set; }
        public int FileMetaId { get; set; }

        [ForeignKey("SafetyStandardId")]
        public SafetyStandard SafetyStandard { get; set; }
        [ForeignKey("FileMetaId")]
        public FileMeta FileMeta { get; set; }
    }
}
