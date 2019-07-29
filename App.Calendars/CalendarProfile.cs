using App.Calendars.Calendars;
using App.Calendars.Calendars.Dto;
using PoorFff.Mapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Calendars
{
    public class CalendarProfile : PfProfile
    {
        public CalendarProfile()
        {
            CreateMap<AddCalendarInput, Calendar>();
            CreateMap<Calendar, GetCalendarOutput>();
            CreateMap<Calendar, GetCalendarListOutput>();
            CreateMap<UpdateCalendarInput, Calendar>();
            CreateMap<Calendar, ExportCalendarOutput>()
                .ForMember(u => u.AllDay, expr => expr.MapFrom(u => u.AllDay == true ? "是" : "否"));
        }
    }
}
