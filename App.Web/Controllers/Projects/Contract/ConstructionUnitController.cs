using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using App.Contract.ConstructionUnits;
using App.Contract.ConstructionUnits.Dto;
using App.Core.Common.Entities;
using App.Web.FileUrl;
using App.Web.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.Web.Controllers.Projects.Contract
{
    /// <summary>
    /// 项目-参建单位基本信息
    /// </summary>
    [ApiVersion("1.0")]
    [MyAuthorize]
    [Route("api/v{version:apiVersion}/constructionunit")]
    public class ConstructionUnitController : ControllerBase
    {
        private IConstructionUnitService _constructionUnitService;
        private IFileUrlBuilder _fileUrlBuilder;
        // private IImportService _importService;
        public ConstructionUnitController(
            IConstructionUnitService constructionUnitService,
             IFileUrlBuilder fileUrlBuilder
            //   IImportService importService
            )
        {
            //  _importService = importService;
            _fileUrlBuilder = fileUrlBuilder;
            _constructionUnitService = constructionUnitService;
        }


        /// <summary>
        /// 验证名称是否重复
        /// </summary>
        /// <param name="name"></param>
        ///<param name="id">id（修改的时候传入）</param>
        /// <returns></returns>
        [HttpGet("count")]
        public CountData GetCount(string name, int? id)
        {
            return new CountData { Count = _constructionUnitService.Count(name, id) };
        }



        /// <summary>
        /// 根据id获取参建单位信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        public GetConstructionUnitOutput Get(int id)
        {
            var supplier = _constructionUnitService.Get(id);
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
        /// 导出参建单位信息
        /// </summary>
        /// <param name="constructionUnitApplicationIds">使用id列表，格式"id1,id2,id3"</param>
        /// <param name="keyword">关键字</param>
        /// <returns></returns>

        [HttpGet("export")]
        public IActionResult Export(
                string constructionUnitApplicationIds,
                string keyword
)
        {
            var contractUnitApplicationIdList = new List<int>();
            if (!string.IsNullOrEmpty(constructionUnitApplicationIds))
            {
                contractUnitApplicationIdList = constructionUnitApplicationIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(u => Convert.ToInt32(u)).ToList();
            }
            var ms = _constructionUnitService.Export(
                "参建单位管理表",
                AppWebContext.Instance.Comments,
                keyword,
                contractUnitApplicationIdList

                );
            var bytes = new byte[ms.Length];
            ms.Read(bytes);
            ms.Close();
            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "参建单位管理表.xlsx");
        }
        /// <summary>
        /// 分页获取参建单位信息
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyword">关键字</param>
        /// <param name="sortField">需要排序的字段(单位名称:Name,单位类型:Type,企业联系人:CompanyContact,联系方式:Link,评价等级:EvaluationLevel,得分:Score)</param>
        /// <param name="sortState">1降序2升序0默认</param>

        /// <returns></returns>
        [HttpGet]
        public PaginationData<GetConstructionUnitListOutput> Get(
                int pageIndex, int pageSize,
                string keyword,
                string sortField,
                string sortState


)
        {
            return _constructionUnitService.Get(
                pageIndex, pageSize,
                keyword,
                sortField,
                sortState
                );
        }
    }
}