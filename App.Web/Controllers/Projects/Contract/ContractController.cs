using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using App.Contract.Contracts;
using App.Contract.Contracts.Dto;
using App.Core.Common.Entities;
using App.Web.FileUrl;
using App.Web.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.Web.Controllers.Projects.Contract
{
    /// <summary>
    /// 项目-合同基本信息
    /// </summary>
    [ApiVersion("1.0")]
    [MyAuthorize]
    [Route("api/v{version:apiVersion}/contract")]
    public class ContractController : ControllerBase
    {
        private IContractService _contractService;
        private IFileUrlBuilder _fileUrlBuilder;
        // private IImportService _importService;
        public ContractController(
            IContractService contractService,
             IFileUrlBuilder fileUrlBuilder
            //   IImportService importService
            )
        {
            //  _importService = importService;
            _fileUrlBuilder = fileUrlBuilder;
            _contractService = contractService;
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
            return new CountData { Count = _contractService.Count(name, id) };
        }

        /// <summary>
        /// 统计合同月个数和合同月金额
        /// </summary>
        /// <returns></returns>
        [HttpGet("totalcount")]
        public GetContractCountOutput GetTotalCount(string year)
        {
            var contract = _contractService.GetTotalCount(year);
            return contract;
        }

        /// <summary>
        /// 根据id获取合同信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        public GetContractOutput Get(int id)
        {
            var supplier = _contractService.Get(id);
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
        /// 导出合同信息
        /// </summary>
        /// <param name="contractApplicationIds">使用合同id列表，格式"id1,id2,id3"</param>
        /// <param name="keyword">关键字</param>
        /// <returns></returns>

        [HttpGet("export")]
        public IActionResult Export(
                string contractApplicationIds,
                string keyword
)
        {
            var contractApplicationIdList = new List<int>();
            if (!string.IsNullOrEmpty(contractApplicationIds))
            {
                contractApplicationIdList = contractApplicationIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(u => Convert.ToInt32(u)).ToList();
            }
            var ms = _contractService.Export(
                "合同管理表",
                AppWebContext.Instance.Comments,
                keyword,
                contractApplicationIdList

                );
            var bytes = new byte[ms.Length];
            ms.Read(bytes);
            ms.Close();
            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "合同管理表.xlsx");
        }
        /// <summary>
        /// 分页获取合同信息
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyword">关键字</param>
        /// <param name="sortField">需要排序的字段(合同名称:Name,项目名称:ProjectId,合同分类:CategoryId,标段:Section,承包方:ContractorId,合同价:ContractPrice,签署对象:Signature,竣工时间:CompletionDate,附件:Attachments)</param>
        /// <param name="sortState">1降序2升序0默认</param>

        /// <returns></returns>
        [HttpGet]
        public PaginationData<GetContractListOutput> Get(int? projectId,
                int pageIndex, int pageSize,
                string keyword,
                string sortField,
                string sortState


)
        {
            return _contractService.Get(projectId,
                pageIndex, pageSize,
                keyword,
                sortField,
                sortState
                );
        }

        /// <summary>
        /// 删除合同信息
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id:int}")]
        public void Delete(int id)
        {
            _contractService.Delete(id);
        }
    }
}