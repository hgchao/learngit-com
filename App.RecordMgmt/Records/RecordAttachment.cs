using App.Core.Common.Entities;
using App.Core.FileManagement.Files;
using App.Core.FileManagement.PublicFiles;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace App.RecordMgmt.Records
{
   public class RecordAttachment : Entity
    {

        public int RecordId { get; set; }
        public int FileMetaId { get; set; }

        [ForeignKey("RecordId")]
        public Record Record { get; set; }
        [ForeignKey("FileMetaId")]
        public FileMeta FileMeta { get; set; }
    }
}
