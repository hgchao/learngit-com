using Microsoft.EntityFrameworkCore;
using App.Memorabilia.MemorabiliaRecords;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Memorabilia.EntityFramework
{
    public interface IMemorabiliaDbContext
    {
        /// <summary>
        /// 大事记 
        /// </summary>
        DbSet<MemorabiliaRecord> MemorabiliaRecords { get; set; }
        DbSet<MemorabiliaAttachment> MemorabiliaAttachments { get; set; }
    }
}
