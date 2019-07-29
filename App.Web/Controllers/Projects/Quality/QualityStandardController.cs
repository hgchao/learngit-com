using App.Core.Common.Entities;
using App.Quality.QualityStandards;
using App.Quality.QualityStandards.Dto;
using App.Web.FileUrl;
using App.Web.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Web.Controllers.PmQuality
{
    /// <summary>
    /// 质量管理-质量管理标准
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/qualitystandard")]
    public class QualityStandardController: ControllerBase
    {
        private IQualityStandardService _standardService;
        private IFileUrlBuilder _fileUrlBuilder;
        public QualityStandardController(IQualityStandardService standardService,
            IFileUrlBuilder fileUrlBuilder
            )
        {
            _standardService = standardService;
            _fileUrlBuilder = fileUrlBuilder;
        }

        /// <summary>
        /// 获取质量管理标准详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [MyAuthorize]
        [HttpGet("{id:int}")]
        public GetQualityStandardOutput Get(int id)
        {
            var progress = _standardService.Get(id);
            _fileUrlBuilder.SetAttachFileUrl(progress);
            return progress;
        }

        /// <summary>
        /// 新增质量管理标准
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [MyAuthorize]
        [HttpPost]
        public IActionResult Add([FromBody]AddQualityStandardInput input)
        {
            int id = _standardService.Add(input);

            return Created("", new { id });
        }

        /// <summary>
        /// 更新质量管理标准
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [MyAuthorize]
        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody]UpdateQualityStandardInput input)
        {
            input.Id = id;
            _standardService.Update(input);
            return Created("", new { id });
        }

        /// <summary>
        /// 分页获取质量管理标准列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="categoryId"></param>
        /// <param name="keyword"></param>
        /// <param name="sortField">需要排序的字段(标准分类:CategoryId,标题:Title,附件:Attachments)</param>
        /// <param name="sortState">1降序2升序0默认</param>
        /// <returns></returns>
        [MyAuthorize]
        [HttpGet]
        public PaginationData<GetQualityStandardOutput> Get(int pageIndex, int pageSize, int? categoryId, string keyword,
               string sortField,
               string sortState)
        {
            var datas = _standardService.Get(pageIndex, pageSize, categoryId, keyword, sortField, sortState);
            datas.Data.ForEach(u => {
                _fileUrlBuilder.SetAttachFileUrl(u);
            });
            return datas;
        }

        /// <summary>
        /// 删除质量管理标准
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id:int}")]
        public void Delete(int id)
        {
            _standardService.Delete(id);
        }
    }
}
