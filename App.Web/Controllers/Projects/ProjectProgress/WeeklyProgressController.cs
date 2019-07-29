using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using App.Core.Common.Entities;
using App.Web.FileUrl;
using App.Web.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using App.ProjectProgress.WeeklyProgresses;
using App.ProjectProgress.WeeklyProgresses.Dto;

namespace App.Web.Controllers.Projects.ProjectProgress
{
    /// <summary>
    /// 项目-周报
    /// </summary>
    [ApiVersion("1.0")]
    [MyAuthorize]
    [Route("api/v{version:apiVersion}/weekly-progresses")]
    public class WeeklyProgressController : ControllerBase
    {
        private IWeeklyProgressService _progressService;
        private IFileUrlBuilder _fileUrlBuilder;
        // private IImportService _importService;
        public WeeklyProgressController(
            IWeeklyProgressService progressService,
            IFileUrlBuilder fileUrlBuilder
            )
        {
            _progressService = progressService;
            _fileUrlBuilder = fileUrlBuilder;
        }

        /// <summary>
        /// 根据id获取周报
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        public GetWeeklyProgressOutput Get(int id)
        {
            var supplier = _progressService.Get(id);
            _fileUrlBuilder.SetAttachFileUrl(supplier);
            return supplier;
        }

        //        /// <summary>
        //        /// 导出周报
        //        /// </summary>
        //        /// <param name="contractApplicationIds">使用合同id列表，格式"id1,id2,id3"</param>
        //        /// <param name="keyword">关键字</param>
        //        /// <returns></returns>

        //        [HttpGet("export")]
        //        public IActionResult Export(
        //                string contractApplicationIds,
        //                string keyword
        //)
        //        {
        //            var contractApplicationIdList = new List<int>();
        //            if (!string.IsNullOrEmpty(contractApplicationIds))
        //            {
        //                contractApplicationIdList = contractApplicationIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(u => Convert.ToInt32(u)).ToList();
        //            }
        //            var ms = _progressService.Export(
        //                "合同管理表",
        //                AppWebContext.Instance.Comments,
        //                keyword,
        //                contractApplicationIdList

        //                );
        //            var bytes = new byte[ms.Length];
        //            ms.Read(bytes);
        //            ms.Close();
        //            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "合同管理表.xlsx");
        //        }
        /// <summary>
        /// 分页获取周报
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="projectId">项目id</param>
        /// <param name="keyword">关键字</param>
        /// <param name="sortField">需要排序的字段(新增时间:AddDate,详细信息:Information,附件:Attachments)</param>
        /// <param name="sortState">1降序2升序0默认</param>
        /// <returns></returns>
        [HttpGet]
        public PaginationData<GetWeeklyProgressListOutput> Get(
                int pageIndex, int pageSize,
                int? projectId,
                string keyword,
               string sortField,
               string sortState
)
        {
            var paging = _progressService.Get(
                pageIndex, pageSize,
                projectId,
                keyword,
               sortField,
                sortState);
            paging.Data.ForEach(u=> {
                _fileUrlBuilder.SetAttachFileUrl(u);
            });
            return paging;
        }
    }
}