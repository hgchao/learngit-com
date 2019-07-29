using App.Core.Common.Entities;
using App.Core.FileManagement.Files;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace App.Safety.SafetyAccidentDisposals
{
    public class SafetyAccidentDisposalAttachment : Entity
    {
        public int SafetyAccidentDisposalId { get; set; }
        public int FileMetaId { get; set; }

        [ForeignKey("SafetyAccidentDisposalId")]
        public SafetyAccidentDisposal SafetyAccidentDisposal { get; set; }
        [ForeignKey("FileMetaId")]
        public FileMeta FileMeta { get; set; }
    }
}
