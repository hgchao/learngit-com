using App.Core.Common.Entities;
using App.Safety.SafetyStandards;
using App.Safety.SafetyStandards.Dto;
using App.Web.FileUrl;
using App.Web.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Web.Controllers.Safety
{
    /// <summary>
    /// 安全管理-安全管理标准
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/safetystandard")]
    public class SafetyStandardController: ControllerBase
    {
        private ISafetyStandardService _standardService;
        private IFileUrlBuilder _fileUrlBuilder;
        public SafetyStandardController(ISafetyStandardService standardService,
            IFileUrlBuilder fileUrlBuilder)
        {
            _standardService = standardService;
            _fileUrlBuilder = fileUrlBuilder;
        }

        /// <summary>
        /// 获取安全管理标准详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [MyAuthorize]
        [HttpGet("{id:int}")]
        public GetSafetyStandardOutput Get(int id)
        {
            var progress = _standardService.Get(id);
            _fileUrlBuilder.SetAttachFileUrl(progress);
            return progress;
        }

        /// <summary>
        /// 新增安全管理标准
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [MyAuthorize]
        [HttpPost]
        public IActionResult Add([FromBody]AddSafetyStandardInput input)
        {
            int id = _standardService.Add(input);
            return Created("", new { id });
        }

        /// <summary>
        /// 更新安全管理标准
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [MyAuthorize]
        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody]UpdateSafetyStandardInput input)
        {
            input.Id = id;
            _standardService.Update(input);
            return Created("", new { id });
        }

        /// <summary>
        /// 分页获取安全管理标准列表
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
        public PaginationData<GetSafetyStandardOutput> Get(int pageIndex, int pageSize, int? categoryId, string keyword,
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
        /// 删除安全管理标准
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id:int}")]
        public void Delete(int id)
        {
            _standardService.Delete(id);
        }
    }
}
