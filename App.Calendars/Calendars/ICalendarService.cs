using App.Core.Common.Entities;
using App.Calendars.Calendars.Dto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace App.Calendars.Calendars
{
   public interface ICalendarService
    {

        int Add(AddCalendarInput input);
        void Update(UpdateCalendarInput input);

        void Delete(int id);
        List<GetCalendarListOutput> Get();
        //PaginationData<GetCalendarListOutput> Get(int pageIndex, int pageSize, string keyword, string Title, int? RecordTypeID, string StorageLife,
        //    DateTime? createTimeL, DateTime? createTimeR);
        MemoryStream Export(
       string title,
       Dictionary<string, string> comments,
       string keyword
    );
        GetCalendarOutput Get(int id);
    }
}
