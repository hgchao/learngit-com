using App.Core.Common.Entities;
using App.Core.FileManagement.Files;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace App.Contract.Contracts
{
    public class ContractAttachment : Entity
    {
        public int ContractId { get; set; }
        public int FileMetaId { get; set; }

        [ForeignKey("ContractId")]
        public Contractt Contract { get; set; }
        [ForeignKey("FileMetaId")]
        public FileMeta FileMeta { get; set; }
    }
}
