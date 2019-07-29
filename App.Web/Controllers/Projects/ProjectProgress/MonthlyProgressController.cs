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
using App.ProjectProgress.PmMonthlyProgresses;
using App.ProjectProgress.PmMonthlyProgresses.Dto;

namespace App.Web.Controllers.Projects.ProjectProgress
{
    /// <summary>
    /// 项目-月报
    /// </summary>
    [ApiVersion("1.0")]
    [MyAuthorize]
    [Route("api/v{version:apiVersion}/monthly-progresses")]
    public class MonthlyProgressController : ControllerBase
    {
        private IMonthlyProgressService _progressService;
        // private IImportService _importService;
        public MonthlyProgressController(
            IMonthlyProgressService progressService
            )
        {
            _progressService = progressService;
        }

        /// <summary>
        /// 根据id获取月报
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        public GetMonthlyProgressOutput Get(int id)
        {
            var supplier = _progressService.Get(id);
            return supplier;
        }

        /// <summary>
        /// 获取总体进度
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns>例子：{progress: 0.3}</returns>
        [HttpGet("current/by-project/{projectId:int}")]
        public object GetCurrent(int projectId)
        {
            return new { progress = _progressService.GetCurrentProgress(projectId) };
        }
        //        /// <summary>
        //        /// 导出月报
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
        /// 分页获取月报
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyword">关键字</param>
        /// <param name="sortField">需要排序的字段(年份:Year,月份:Month,本月完成征拆费:CompletedDemolitionFee,本月完成工程费:CompletedProjectCosts,本月完成投资:Zhong)</param>
        /// <param name="sortState">1降序2升序0默认</param>
        /// <returns></returns>
        [HttpGet]
        public PaginationData<GetMonthlyProgressListOutput> Get(
                int pageIndex, int pageSize,
                int? projectId,
                string keyword,
               string sortField,
               string sortState
)
        {
            return _progressService.Get(
                pageIndex, pageSize,
                projectId,
                keyword, sortField, sortState);
        }
    }
}