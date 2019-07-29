using App.Core.Common.Entities;
using App.Core.FileManagement.Files;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace App.Quality.QualityAccidents
{
    public class QualityAccidentAttachment : Entity
    {
        public int QualityAccidentId { get; set; }
        public int FileMetaId { get; set; }

        [ForeignKey("QualityAccidentId")]
        public QualityAccident QualityAccident { get; set; }
        [ForeignKey("FileMetaId")]
        public FileMeta FileMeta { get; set; }
    }
}
