using App.Core.Common.Entities;
using App.Core.FileManagement.Files;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace App.Problems.Problems
{
    public class ProblemAttachment : Entity
    {
        public int ProblemId { get; set; }
        public int FileMetaId { get; set; }

        [ForeignKey("ProblemId")]
        public Problem Problem { get; set; }
        [ForeignKey("FileMetaId")]
        public FileMeta FileMeta { get; set; }
    }
}
