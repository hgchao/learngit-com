﻿using App.Core.Common.Entities;
using App.Core.FileManagement.Files;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace App.ProjectEarlyStage.EarlyStages
{
    public class EarlyStageAttachment : Entity
    {
        public int EarlyStageId { get; set; }
        public int FileMetaId { get; set; }

        [ForeignKey("EarlyStageId")]
        public EarlyStage EarlyStage { get; set; }
        [ForeignKey("FileMetaId")]
        public FileMeta FileMeta { get; set; }
    }
}
