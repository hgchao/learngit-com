using App.Core.Common.Entities;
using App.Core.FileManagement.Files;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace App.Problems.ProblemCoordinations
{
    public class ProblemCoordinationAttachment : Entity
    {
        public int ProblemCoordinationId { get; set; }
        public int FileMetaId { get; set; }

        [ForeignKey("ProblemCoordinationId")]
        public ProblemCoordination ProblemCoordination { get; set; }
        [ForeignKey("FileMetaId")]
        public FileMeta FileMeta { get; set; }
    }
}
