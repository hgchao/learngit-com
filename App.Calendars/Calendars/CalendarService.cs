using App.Base.Repositories;
using App.Core.Authorization.Accounts;
using App.Core.Authorization.PriviledgedPersons;
using App.Core.Authorization.Repositories;
using App.Core.Authorization.Users;
using App.Calendars.Calendars.Dto;
using PoorFff.Excel;
using PoorFff.Mapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace App.Calendars.Calendars
{
  public  class CalendarService: ICalendarService
    {
        private IAuthInfoProvider _authInfoProvider;
        private IAppRepositoryBase<Calendar> _calendarRepository;
        private IAuthorizationRepositoryBase<User> _userRepository;
        private IAuthorizationRepositoryBase<PrivilegedPerson> _privilegedPersonRepository;
        public CalendarService(
            IAuthInfoProvider authInfoProvider,
            IAppRepositoryBase<Calendar> calendarRepository,
            IAuthorizationRepositoryBase<User> userRepository,
            IAuthorizationRepositoryBase<PrivilegedPerson> privilegedPersonRepository
            )
        {
            _authInfoProvider = authInfoProvider;
            _privilegedPersonRepository = privilegedPersonRepository;
            _calendarRepository = calendarRepository;
            _userRepository = userRepository;
        }
        public int Add(AddCalendarInput input)
        {
            var calendar = input.MapTo<Calendar>();
            _calendarRepository.Add(calendar);
            return calendar.Id;
        }

        public void Delete(int id)
        {
            _calendarRepository.Delete(new Calendar() { Id = id });
        }

        public List<GetCalendarListOutput> Get()
        {
            var userId = _authInfoProvider.GetCurrent()?.User?.Id ?? 0;
            IQueryable<Calendar> query = null;
            var privilegedPersonIds = _privilegedPersonRepository.Get().Where(u => u.ModuleName == "日程备忘录").Select(u => u.UserId).ToList();//特权人列表
            if (privilegedPersonIds.Contains(userId))//当前用户是否特权人
            {
                query = _calendarRepository.Get();
            }
            else
            {
                query = _calendarRepository.Get().Where(u => u.CreatorId==userId);
            }
            return query.MapToList<GetCalendarListOutput>();
        }
        public MemoryStream Export(
          string title,
          Dictionary<string, string> comments,
          string keyword

          )
        {
            var query = _calendarRepository.Get();


            //var userId = _authInfoProvider.GetCurrent()?.User?.Id ?? 0;
            //var tenantId = _authInfoProvider.GetCurrent()?.TenantId;
            //var userCode = _userRepository.Get().Where(u => u.Id == userId).Select(u => u.Code).FirstOrDefault();
            //var userIdList = _userRepository.Get().Where(u => u.Code.StartsWith(userCode)).Select(u => u.Id).Distinct().ToList();
            //var privilegedPersonIdList = OaProjectContext.Instance.GetProjectPrivilegedPersonIds(ProjectPrivilegedPersons.ProjectModule.Project, tenantId.Value);
            //bool isPrivilegedPerson = privilegedPersonIdList.Contains(userId);
            //query = query.Where(u => isPrivilegedPerson || userIdList.Contains(u.CreatorId) || u.ResposiblePersonId.Contains($"[{userId}]"));
            #region query conditions
            if (!string.IsNullOrEmpty(keyword))
            {
                if (keyword == "今天")
                {
                    query = query.Where(u => u.StartTime >= Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")));
                }
                else if (keyword == "最近三天")
                {
                    query = query.Where(u => u.StartTime >= Convert.ToDateTime(DateTime.Now.AddDays(-3).ToString("yyyy-MM-dd")));
                }
                else if (keyword == "一周")
                {
                    query = query.Where(u => u.StartTime >= Convert.ToDateTime(DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd")));
                }
                else if (keyword == "一月")
                {
                    query = query.Where(u => u.StartTime >= Convert.ToDateTime(DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd")));
                }
                else if (keyword == "三月")
                {
                    query = query.Where(u => u.StartTime >= Convert.ToDateTime(DateTime.Now.AddDays(-90).ToString("yyyy-MM-dd")));
                }
               
                
            }

            #endregion

            // query = query.OrderByDescending(u => u.ContractNo);
            var userList = _userRepository.Get().Select(u => new { u.Id, u.Name }).ToList();
            //var organizationUnitList = _organizationUnitRepository.Get().Select(u => new { u.Id, u.Name }).ToList();
            var resList = query.ToList();
            var outputs = new List<ExportCalendarOutput>();
            foreach (var res in resList)
            {
                var output = res.MapTo<ExportCalendarOutput>();
                output.ProposerName = userList.Where(u => u.Id == res.CreatorId).Select(u => u.Name).FirstOrDefault();
                //output.DepartmentName = organizationUnitList.Where(u => u.Id == contract.DepartmentId).Select(u => u.Name).FirstOrDefault();
                outputs.Add(output);
            }
            return outputs.Entity2Excel(comments, title);
        }
        public GetCalendarOutput Get(int id)
        {
            var data = _calendarRepository.Get().Where(w => w.Id == id).FirstOrDefault();
            return data.MapTo<GetCalendarOutput>();
        }


        public void Update(UpdateCalendarInput input)
        {
            //修改
            var existing = _calendarRepository.Get().Where(u => u.Id == input.Id).FirstOrDefault();
            var calendar = input.MapTo<Calendar>();
            _calendarRepository.Update(calendar, existing, new Expression<Func<Calendar, object>>[] { }, false);

        }
    }
}
