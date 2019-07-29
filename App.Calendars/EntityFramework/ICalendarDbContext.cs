using App.Calendars.Calendars;
using App.Core.Common.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Calendars.EntityFramework
{
   public interface ICalendarDbContext : IAppCoreDbContext
    {/// <summary>
     /// 日历备忘录
     /// </summary>
        DbSet<Calendar> Calendars { get; set; }
    }
}
