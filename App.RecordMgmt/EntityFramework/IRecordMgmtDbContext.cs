using Microsoft.EntityFrameworkCore;
using App.RecordMgmt.Records;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.RecordMgmt.EntityFramework
{
    public interface IRecordMgmtDbContext
    {
        DbSet<Record> Records { get; set; }
        DbSet<RecordAttachment> RecordAttachments { get; set; }
    }
}
