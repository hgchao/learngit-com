using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Core.Common.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using App.RecordMgmt.Records;
using App.RecordMgmt.Records.Dto;
using App.Web.Authentication;
using App.Web.FileUrl;
using App.Web.Filters;

namespace Oa.Web.Controllers.Record
{
    /// <summary>
    /// 档案管理-新建档案
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/record-records")]
    public class RecordsController : ControllerBase
    {
        private IRecordService _recordsService;
        private IFileUrlBuilder _fileUrlBuilder;

        public RecordsController(IRecordService RecordsService, IFileUrlBuilder fileUrlBuilder)
        {
            _recordsService = RecordsService;
            _fileUrlBuilder = fileUrlBuilder;
        }



        /// <summary>
        /// 分页获取档案列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="projectId"></param>
        /// <param name="typeId"></param>
        /// <param name="keyword"></param>
        /// <param name="sortField">需要排序的字段(档案类型:RecordTypeId,档案名称:RecordName,描述:Description,创建时间:CreateTime,附件:Attachments)</param>
        /// <param name="sortState">1降序2升序0默认</param>
        /// <returns></returns>
        [MyAuthorize]
        [HttpGet]
        public PaginationData<GetRecordListOutput> Get(int pageIndex, int pageSize, int projectId, int? typeId, string keyword,
              string sortField,
              string sortState)
        {
            var paging = _recordsService.Get(pageIndex, pageSize, projectId, typeId, keyword, sortField, sortState);
            paging.Data.ForEach(u =>
            {
                _fileUrlBuilder.SetAttachFileUrl(u);
            });
            return paging;
        }

        /// <summary>
        /// 根据id获取档案详情
        /// </summary>
        /// <returns></returns>
        [MyAuthorize]
        [HttpGet("{id:int}")]
        public GetRecordOutput Get(int id)
        {
            var paging = _recordsService.Get(id);
            _fileUrlBuilder.SetAttachFileUrl(paging);
            return paging;
        }
  
        /// <summary>
        /// 删除档案
        /// </summary>
        /// <returns></returns>
        [MyAuthorize]
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            _recordsService.Delete(id);
            return NoContent();
        }
    }
}