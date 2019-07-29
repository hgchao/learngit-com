using App.Core.Common.Entities;
using App.Core.FileManagement.Files;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace App.Quality.QualityStandards
{
    public class QualityStandardAttachment : Entity
    {
        public int QualityStandardId { get; set; }
        public int FileMetaId { get; set; }

        [ForeignKey("QualityStandardId")]
        public QualityStandard QualityStandard { get; set; }
        [ForeignKey("FileMetaId")]
        public FileMeta FileMeta { get; set; }
    }
}
