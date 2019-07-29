using App.Core.Common.Entities;
using App.Core.FileManagement.Files;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace App.Quality.QualityProblems
{
    public class QualityProblemAttachment : Entity
    {
        public int QualityProblemId { get; set; }
        public int FileMetaId { get; set; }

        [ForeignKey("QualityProblemId")]
        public QualityProblem QualityProblem { get; set; }
        [ForeignKey("FileMetaId")]
        public FileMeta FileMeta { get; set; }
    }
}
