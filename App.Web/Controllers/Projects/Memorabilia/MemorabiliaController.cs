using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using App.Core.Common.Entities;
using App.Memorabilia.MemorabiliaRecords;
using App.Memorabilia.MemorabiliaRecords.Dto;
using App.Web.FileUrl;
using App.Web.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.Web.Controllers.Projects.Memorabilia
{
    /// <summary>
    /// 项目-大事记基本信息
    /// </summary>
    [ApiVersion("1.0")]
    [MyAuthorize]
    [Route("api/v{version:apiVersion}/memorabilia")]
    public class MemorabiliaController : ControllerBase
    {
        private IMemorabiliaRecordService _recordService;
        private IFileUrlBuilder _fileUrlBuilder;
        // private IImportService _importService;
        public MemorabiliaController(
            IMemorabiliaRecordService recordService,
             IFileUrlBuilder fileUrlBuilder
            //   IImportService importService
            )
        {
            //  _importService = importService;
            _fileUrlBuilder = fileUrlBuilder;
            _recordService = recordService;
        }


        /// <summary>
        /// 验证名称是否重复
        /// </summary>
        /// <param name="name"></param>
        ///<param name="id">大事记id（修改的时候传入）</param>
        /// <returns></returns>
        [HttpGet("count")]
        public CountData GetCount(string name, int? id)
        {
            return new CountData { Count = _recordService.Count(name, id) };
        }
       


        /// <summary>
        /// 根据id获取大事记信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        public GetMemorabiliaRecordOutput Get(int id)
        {
            var supplier = _recordService.Get(id);
            _fileUrlBuilder.SetAttachFileUrl(supplier);
            return supplier;
        }

        [HttpPost("import")]
        public IActionResult Import()
        {
            foreach (var file in Request.Form.Files)
            {
                var ms = new MemoryStream();
                file.CopyTo(ms);
                //_importService.Import(ms);
            }
            return Created("", null);
        }


        /// <summary>
        /// 导出大事记信息
        /// </summary>
        /// <param name="supplierApplicationIds">使用大事记id列表，格式"id1,id2,id3"</param>
        /// <param name="keyword">关键字</param>
        /// <returns></returns>

        [HttpGet("export")]
        public IActionResult Export(
                string supplierApplicationIds,
                string keyword
)
        {
            var supplierApplicationIdList = new List<int>();
            if (!string.IsNullOrEmpty(supplierApplicationIds))
            {
                supplierApplicationIdList = supplierApplicationIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(u => Convert.ToInt32(u)).ToList();
            }
            var ms = _recordService.Export(
                "大事记管理表",
                AppWebContext.Instance.Comments,
                keyword,
                supplierApplicationIdList

                );
            var bytes = new byte[ms.Length];
            ms.Read(bytes);
            ms.Close();
            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "大事记管理表.xlsx");
        }
        /// <summary>
        /// 分页获取大事记信息
        /// </summary>
        /// <param name="ProjectId">项目id</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyword">关键字</param>
        /// <param name="sortField">需要排序的字段(事项类型:CategoryId,事项名称:Name,参与人员:Participant,创建时间:CreateTime)</param>
        /// <param name="sortState">1降序2升序0默认</param>

        /// <returns></returns>
        [HttpGet]
        public PaginationData<GetMemorabiliaRecordListOutput> Get(int? projectId,
                int pageIndex, int pageSize,
                string keyword,
                string sortField,
                string sortState


)
        {
            return _recordService.Get(projectId,
                pageIndex, pageSize,
                keyword,
                sortField,
                sortState
                );
        }
        /// <summary>
        /// 删除大事记
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id:int}")]
        public void Delete(int id)
        {
            _recordService.Delete(id);
        }
    }
}