using App.Core.Common.Entities;
using App.Core.FileManagement.Files;
using App.Safety.SafetyProblemProgresses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace App.Safety.SafetyProblemRectifications
{
    public class SafetyProblemRectificationAttachment : Entity
    {
        public int SafetyProblemRectificationId { get; set; }
        public int FileMetaId { get; set; }

        [ForeignKey("SafetyProblemRectificationId")]
        public SafetyProblemRectification SafetyProblemRectification { get; set; }
        [ForeignKey("FileMetaId")]
        public FileMeta FileMeta { get; set; }
    }
}
