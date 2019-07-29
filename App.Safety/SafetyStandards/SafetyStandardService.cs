using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using App.Base.EntityFramework;
using App.Base.Repositories;
using App.Core.Common;
using App.Core.Common.Entities;
using App.Core.Common.Exceptions;
using App.Safety.SafetyStandards;
using App.Safety.SafetyStandards.Dto;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using PoorFff.EventBus;
using PoorFff.Mapper;

namespace Pm.Safety.PmSafetyStandards
{
    public class SafetyStandardService : ISafetyStandardService
    {
        private IAppRepositoryBase<SafetyStandard> _standardRepository;
        private IAppDbContextProvider _dbContextProvider;
        private IEventBus _eventBus;
        public SafetyStandardService(
            IAppRepositoryBase<SafetyStandard> standardRepository,
            IAppDbContextProvider dbContextProvider,
            IEventBus eventBus
            )
        {
            _standardRepository = standardRepository;
            _dbContextProvider = dbContextProvider;
            _eventBus = eventBus;
        }

        public int Add(AddSafetyStandardInput input)
        {
            var standard = input.MapTo<SafetyStandard>();
            _standardRepository.Add(standard);
            return standard.Id;
        }

        public void Delete(int id)
        {
            _standardRepository.Delete(new SafetyStandard { Id = id});
        }

        public GetSafetyStandardOutput Get(int id)
        {
            var standard = _standardRepository.Get()
                .Include(u=>u.Category)
                .Include(u => u.Attachments).ThenInclude(w => w.FileMeta)
                .Where(u=>u.Id==id)
                .FirstOrDefault();
            if (standard == null)
                throw new EntityException("Id", id, "SafetyStandard", "不存在");
            return standard.MapTo<GetSafetyStandardOutput>();
        }

        public PaginationData<GetSafetyStandardOutput> Get(int pageIndex, int pageSize, int? categoryId, string keyword,
               string sortField,
               string sortState)
        {
            IQueryable<SafetyStandard> query = _standardRepository.Get()
                .Include(u=>u.Category)
                .Include(u => u.Attachments).ThenInclude(w => w.FileMeta);
            if(categoryId != null)
            {
                query = query.Where(u => u.CategoryId == categoryId);
            }
            if(!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(u => u.Title.Contains(keyword));
            }
            //sortState 1降序2升序0默认
            if (!string.IsNullOrEmpty(sortField) && !string.IsNullOrEmpty(sortState))
            {
                switch (sortField)
                {
                    case "CategoryId"://标准分类
                        query = sortState == "1" ? query.OrderByDescending(u => u.CategoryId) : sortState == "2" ? query.OrderBy(u => u.CategoryId) : query.OrderByDescending(u => u.CreateTime);
                        break;
                    case "Title"://标题
                        query = sortState == "1" ? query.OrderByDescending(u => u.Title) : sortState == "2" ? query.OrderBy(u => u.Title) : query.OrderByDescending(u => u.CreateTime);
                        break;
                    case "Attachments"://附件
                        query = sortState == "1" ? query.OrderByDescending(u => u.Attachments.Any(v => v.FileMetaId > 0)) : sortState == "2" ? query.OrderBy(u => u.Attachments.Any(v => v.FileMetaId > 0)) : query.OrderByDescending(u => u.CreateTime);
                        break;
                    default:
                        query = query.OrderByDescending(u => u.CreateTime);
                        break;
                }

            }
            else
            {
                query = query.OrderByDescending(u => u.CreateTime);
            }
            return PaginationDataHelper.WrapData<SafetyStandard, T>(query, pageIndex, pageSize).TransferTo<GetSafetyStandardOutput>();
        }

        public void Update(UpdateSafetyStandardInput input)
        {
            var existing = _standardRepository.Get().Include(u => u.Attachments).Where(u => u.Id == input.Id).FirstOrDefault();
            var standard = input.MapTo<SafetyStandard>();
            _standardRepository.Update(standard, existing, new System.Linq.Expressions.Expression<Func<SafetyStandard, object>>[] { }, false);
        }
    }
}
