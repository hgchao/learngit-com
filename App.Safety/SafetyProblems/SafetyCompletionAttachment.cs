using App.Core.Common.Entities;
using App.Core.FileManagement.Files;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace App.Safety.SafetyProblems
{
    public class SafetyCompletionAttachment : Entity
    {
        public int SafetyProblemId { get; set; }
        public int FileMetaId { get; set; }

        [ForeignKey("SafetyProblemId")]
        public SafetyProblem SafetyProblem { get; set; }
        [ForeignKey("FileMetaId")]
        public FileMeta FileMeta { get; set; }
    }
}
