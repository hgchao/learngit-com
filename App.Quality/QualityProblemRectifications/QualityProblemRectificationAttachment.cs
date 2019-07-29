using App.Core.Common.Entities;
using App.Core.FileManagement.Files;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace App.Quality.QualityProblemRectifications
{
   public class QualityProblemRectificationAttachment : Entity
    {
        public int QualityProblemRectificationId { get; set; }
        public int FileMetaId { get; set; }

        [ForeignKey("QualityProblemRectificationId")]
        public QualityProblemRectification QualityProblemRectification { get; set; }
        [ForeignKey("FileMetaId")]
        public FileMeta FileMeta { get; set; }
    }
}
