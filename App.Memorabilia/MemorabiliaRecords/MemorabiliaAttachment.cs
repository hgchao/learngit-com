using App.Core.Common.Entities;
using App.Core.FileManagement.Files;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace App.Memorabilia.MemorabiliaRecords
{
    public class MemorabiliaAttachment : Entity
    {

        public int MemorabiliaId { get; set; }
        public int FileMetaId { get; set; }

        [ForeignKey("MemorabiliaId")]
        public MemorabiliaRecord MemorabiliaRecord { get; set; }
        [ForeignKey("FileMetaId")]
        public FileMeta FileMeta { get; set; }
    }
}
