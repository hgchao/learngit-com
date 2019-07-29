using App.Core.Common.Entities;
using App.Core.FileManagement.Files;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace App.Safety.SafetyAccidents
{
    public class SafetySettlementAttachment : Entity
    {
        public int SafetyAccidentId { get; set; }
        public int FileMetaId { get; set; }

        [ForeignKey("SafetyAccidentId")]
        public SafetyAccident SafetyAccident { get; set; }
        [ForeignKey("FileMetaId")]
        public FileMeta FileMeta { get; set; }
    }
}
