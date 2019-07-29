using App.Core.Common.Entities;
using App.Quality.QualityProblemRectifications;
using App.Quality.QualityProblemRectifications.Dto;
using App.Quality.QualityProblems;
using App.Quality.QualityProblems.Dto;
using App.Web;
using App.Web.FileUrl;
using App.Web.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pm.Web.Controllers.PmQuality
{
    /// <summary>
    /// 质量管理-质量问题
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/qualityproblem")]
    public class QualityProblemController: ControllerBase
    {
        private IQualityProblemService _problemService;
        private IFileUrlBuilder _fileUrlBuilder;
        public QualityProblemController(IQualityProblemService problemService,
            IFileUrlBuilder fileUrlBuilder)
        {
            _problemService = problemService;
            _fileUrlBuilder = fileUrlBuilder;
        }

        /// <summary>
        /// 验证来源是否重复
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
        /// 首页统计质量问题个数
        /// </summary>
        /// <returns></returns>
        [HttpGet("qualityproblemCount")]
        public GetQProblemCountOutput GetProblemCount(DateTime? createDateL, DateTime? createDateR)
        {
            var project = _problemService.GetCount(createDateL, createDateR);
            return project;
        }
        /// <summary>
        /// 获取质量问题详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [MyAuthorize]
        [HttpGet("{id:int}")]
        public GetQualityProblemOutput Get(int id)
        {
            var progress = _problemService.Get(id);
            _fileUrlBuilder.SetAttachFileUrl(progress);
            return progress;
        }

        /// <summary>
        /// 导出质量问题信息
        /// </summary>
        /// <param name="projectId">项目id</param>
        /// <param name="qualityProblemApplicationIds">使用id列表，格式"id1,id2,id3"</param>
        /// <param name="keyword">关键字</param>
        /// <param name="categoryId">质量问题分类</param>
        ///  <param name="state">整改情况(1未整改2正在整改3已整改)</param>
        /// <returns></returns>

        [HttpGet("export")]
        public IActionResult Export(
                int? projectId,
                string qualityProblemApplicationIds,
                string keyword,
                 int? categoryId,
                 RectificationState? state
)
        {
            var supplierApplicationIdList = new List<int>();
            if (!string.IsNullOrEmpty(qualityProblemApplicationIds))
            {
                supplierApplicationIdList = qualityProblemApplicationIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(u => Convert.ToInt32(u)).ToList();
            }
            var ms = _problemService.Export(
                projectId,
                "质量问题表",
                AppWebContext.Instance.Comments,
                state,
                keyword,
                categoryId,
                supplierApplicationIdList

                );
            var bytes = new byte[ms.Length];
            ms.Read(bytes);
            ms.Close();
            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "质量问题表.xlsx");
        }
        /// <summary>
        /// 分页获取质量问题列表
        /// </summary>
        /// <param name="projectId">项目id</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="state">整改情况（1未整改2正在整改3已整改）</param>
        /// <param name="keyword"></param>
        /// <param name="categoryId">质量问题分类</param>
        /// <param name="sortField">需要排序的字段(质量问题分类:CategoryId,质量问题来源:Source,创建时间:CreateTime,整改情况:RectificationState,整改完成时间:CompletionTime)</param>
        /// <param name="sortState">1降序2升序0默认</param>
        /// <returns></returns>
        [MyAuthorize]
        [HttpGet()]
        public PaginationData<GetQualityProblemListOutput> Get(int? projectId,
            int pageIndex, int pageSize,
            RectificationState? state, 
            string keyword,
            int? categoryId,
            string sortField,
             string sortState)
        {
            var datas = _problemService.Get(projectId, pageIndex, pageSize,  state, keyword, categoryId, sortField, sortState);
            return datas;
        }
       

        /// <summary>
        /// 新增整改情况
        /// </summary>
        /// <returns></returns>
        [MyAuthorize]
        [HttpPost("{id:int}/rectifications")]
        public IActionResult AddRectification(int id, [FromBody]AddQualityProblemRectificationInput rectificationInput)
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
        public IActionResult CompleteRetification(int id, [FromBody]CompleteQualityProblemInput input)
        {
            input.Id = id;
            _problemService.CompleteRetification(input);
            return Created("", new {id});
        }
        /// <summary>
        /// 删除质量问题
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id:int}")]
        public void Delete(int id)
        {
            _problemService.Delete(id);
        }
    }
}
