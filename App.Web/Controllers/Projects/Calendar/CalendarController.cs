using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Calendars.Calendars;
using App.Calendars.Calendars.Dto;
using App.Web.FileUrl;
using App.Web.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.Web.Controllers.Projects.Calendar
{
    /// <summary>
    /// 项目-日历备忘录
    /// </summary>
    [ApiVersion("1.0")]
    [MyAuthorize]
    [Route("api/v{version:apiVersion}/calendar")]
    public class CalendarController : ControllerBase
    {
        private ICalendarService _calendarService;
        private IFileUrlBuilder _fileUrlBuilder;

        public CalendarController(ICalendarService calendarService, IFileUrlBuilder fileUrlBuilder)
        {
            _calendarService = calendarService;
            _fileUrlBuilder = fileUrlBuilder;
        }

        /// <summary>
        /// 新增日历备忘录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [MyAuthorize]
        [HttpPost()]
        public IActionResult Add([FromBody]  AddCalendarInput input)
        {
            var id = _calendarService.Add(input);
            return Created("", new { id });
        }


        /// <summary>
        /// 获取日历备忘录列表
        /// </summary>
        /// <returns></returns>
        [MyAuthorize]
        [HttpGet]
        public List<GetCalendarListOutput> Get()
        {
            var paging = _calendarService.Get();
            //paging.Data.ForEach(u => {
            //    _fileUrlBuilder.SetAttachFileUrl(u);
            //});
            return paging;
        }
        /// <summary>
        /// 导出日历备忘录
        /// </summary>
        /// <param name="keyword">关键字(今天,最近三天,一周,一月,三月)</param>
        /// <returns></returns>



        [HttpGet("export")]
        public IActionResult Export(

               string keyword
)
        {

            var ms = _calendarService.Export(
                "日历备忘录表",
                AppWebContext.Instance.Comments,
                keyword

                );
            var bytes = new byte[ms.Length];
            ms.Read(bytes);
            ms.Close();
            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "日历备忘录表.xlsx");
        }
        /// <summary>
        /// 根据id获取日历备忘录详情
        /// </summary>
        /// <returns></returns>
        [MyAuthorize]
        [HttpGet("{id:int}")]
        public GetCalendarOutput Get(int id)
        {
            var paging = _calendarService.Get(id);
            // _fileUrlBuilder.SetAttachFileUrl(paging);
            return paging;
        }
        /// <summary>
        /// 修改日历备忘录
        /// </summary>
        /// <returns></returns>
        [MyAuthorize]
        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody] UpdateCalendarInput input)
        {
            input.Id = id;
            _calendarService.Update(input);
            return Created("", new { id });
        }

        /// <summary>
        /// 删除日期备忘录
        /// </summary>
        /// <returns></returns>
        [MyAuthorize]
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            _calendarService.Delete(id);
            return NoContent();
        }
    }
}