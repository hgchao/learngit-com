using App.Core.Common.Entities;
using App.Housekeeping.HousekeepingProblemRectifications;
using App.Housekeeping.HousekeepingProblemRectifications.Dto;
using App.Housekeeping.Housekeepings;
using App.Housekeeping.Housekeepings.Dto;
using App.Web.FileUrl;
using App.Web.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Web.Controllers.CivilizedConstruction
{
    /// <summary>
    /// 文明施工管理-文明施工问题
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/housekeepingproblem")]
    public class HousekeepingProblemController: ControllerBase
    {
        private IHousekeepingProblemService _problemService;
        private IFileUrlBuilder _fileUrlBuilder;
        public HousekeepingProblemController(IHousekeepingProblemService problemService,
            IFileUrlBuilder fileUrlBuilder
            )
        {
            _problemService = problemService;
            _fileUrlBuilder = fileUrlBuilder;
        }
        /// <summary>
        /// 验证文明施工问是否重复
        /// </summary>
        /// <param name="name"></param>
        ///<param name="id">id（修改的时候传入）</param>
        /// <returns></returns>
        [HttpGet("count")]
        public CountData GetCount(string name, int? id)
        {
            return new CountData { Count = _problemService.Count(name, id) };
        }
        /// <summary>
        /// 获取文明施工问题详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [MyAuthorize]
        [HttpGet("{id:int}")]
        public GetHousekeepingProblemOutput Get(int id)
        {
            var progress = _problemService.Get(id);
            _fileUrlBuilder.SetAttachFileUrl(progress);
            return progress;
        }
        /// <summary>
        /// 导出文明施工列表
        /// </summary>
        /// <param name="projectId">项目id</param>
        /// <param name="housekeepingApplicationIds">使用id列表，格式"id1,id2,id3"</param>
        /// <param name="keyword">关键字</param>
        ///  <param name="state">整改情况(1未整改2正在整改3已整改)</param>
        /// <returns></returns>

        [HttpGet("export")]
        public IActionResult Export(
                int? projectId,
                string housekeepingApplicationIds,
                string keyword,
                 RectificationState? state
)
        {
            var housekeepingApplicationIdList = new List<int>();
            if (!string.IsNullOrEmpty(housekeepingApplicationIds))
            {
                housekeepingApplicationIdList = housekeepingApplicationIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(u => Convert.ToInt32(u)).ToList();
            }
            var ms = _problemService.Export(
                projectId,
                "文明施工表",
                AppWebContext.Instance.Comments,
                state,
                keyword,
                housekeepingApplicationIdList

                );
            var bytes = new byte[ms.Length];
            ms.Read(bytes);
            ms.Close();
            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "文明施工表.xlsx");
        }
        /// <summary>
        /// 分页获取文明施工列表
        /// </summary>
        /// <param name="projectId">项目id</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="state">整改情况（1未整改2正在整改3已整改）</param>
        /// <param name="keyword"></param>
        /// <param name="sortField">需要排序的字段(文明施工问题内容:Content,限期整改时间:Deadline,整改情况:RectificationState,整改完成时间:CompletionTime)</param>
        /// <param name="sortState">1降序2升序0默认</param>
        /// <returns></returns>
        [MyAuthorize]
        [HttpGet()]
        public PaginationData<GetHousekeepingProblemListOutput> Get(
            int? projectId,
            int pageIndex, int pageSize,
            RectificationState? state,
            string keyword,
            string sortField,
             string sortState)
        {
            var datas = _problemService.Get(projectId, pageIndex, pageSize, state, keyword,  sortField, sortState);
            return datas;
        }
       
        /// <summary>
        /// 新增整改情况
        /// </summary>
        /// <returns></returns>
        [MyAuthorize]
        [HttpPost("{id:int}/rectifications")]
        public IActionResult AddRectification(int id, [FromBody]AddHousekeepingProblemRectificationInput rectificationInput)
        {
            var rectificationId = _problemService.AddRectification(id, rectificationInput);
            return Created("", new { rectificationId});
        }

        /// <summary>
        /// 完成整改
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [MyAuthorize]
        [HttpPut("complete/{id:int}")]
        public IActionResult CompleteRetification(int id, [FromBody]CompleteHousekeepingProblemInput input)
        {
            input.Id = id;
            _problemService.CompleteRetification(input);
            return Created("", new {id});
        }
        /// <summary>
        /// 删除文明施工
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id:int}")]
        public void Delete(int id)
        {
            _problemService.Delete(id);
        }
    }
}
