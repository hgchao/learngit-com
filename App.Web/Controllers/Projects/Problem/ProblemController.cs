using App.Core.Common.Entities;
using App.Problems.ProblemRectifications;
using App.Problems.ProblemRectifications.Dto;
using App.Problems.Problems;
using App.Problems.Problems.Dto;
using App.Web.FileUrl;
using App.Web.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Web.Controllers.Problem
{
    /// <summary>
    /// 存在管理-存在问题
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/problems")]
    public class ProblemController: ControllerBase
    {
        private IProblemService _problemService;
        private IFileUrlBuilder _fileUrlBuilder;
        public ProblemController(IProblemService problemService,
            IFileUrlBuilder fileUrlBuilder)
        {
            _problemService = problemService;
            _fileUrlBuilder = fileUrlBuilder;
        }
        /// <summary>
        /// 验证内容是否重复
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
        /// 获取存在问题详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [MyAuthorize]
        [HttpGet("{id:int}")]
        public GetProblemOutput Get(int id)
        {
            var progress = _problemService.Get(id);
            _fileUrlBuilder.SetAttachFileUrl(progress);
            return progress;
        }
        /// <summary>
        /// 导出存在问题信息
        /// </summary>
        /// <param name="projectId">项目id</param>
        /// <param name="problemApplicationIds">使用id列表，格式"id1,id2,id3"</param>
        /// <param name="keyword">关键字</param>
        ///  <param name="categoryId">问题分类</param>
        ///  <param name="coordinationState">协调情况(1未解决2正在解决3已解决)</param>

        /// <returns></returns>

        [HttpGet("export")]
        public IActionResult Export(
                int? projectId,
                string problemApplicationIds,
                string keyword,
                int? categoryId,
              CoordinationState? coordinationState
)
        {
            var problemApplicationIdList = new List<int>();
            if (!string.IsNullOrEmpty(problemApplicationIds))
            {
                problemApplicationIdList = problemApplicationIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(u => Convert.ToInt32(u)).ToList();
            }
            var ms = _problemService.Export(
                projectId,
                "存在问题表",
                AppWebContext.Instance.Comments,
                categoryId,
                coordinationState,
                keyword,
                problemApplicationIdList

                );
            var bytes = new byte[ms.Length];
            ms.Read(bytes);
            ms.Close();
            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "存在问题表.xlsx");
        }
        /// <summary>
        /// 分页获取存在问题列表
        /// </summary>
        /// <param name="projectId">项目id</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        ///  <param name="categoryId">问题分类</param>
        /// <param name="coordinationState">协调情况(1未解决2正在解决3已解决)</param>
        /// <param name="keyword"></param>
        /// <param name="sortField">需要排序的字段(存在问题分类:CategoryId,问题内容:Content,计划完成时间:PlannedCompletionTime,实际完成时间:ActualCompletionTime,协调情况:CoordinationState)</param>
        /// <param name="sortState">1降序2升序0默认</param>
        /// <returns></returns>
        [MyAuthorize]
        [HttpGet()]
        public PaginationData<GetProblemListOutput> Get(int? projectId,
            int pageIndex, int pageSize,
            int? categoryId,
            CoordinationState? coordinationState,
            string keyword,
            string sortField,
             string sortState)
        {
            var datas = _problemService.Get(projectId, pageIndex, pageSize, categoryId, coordinationState, keyword, sortField, sortState);
            return datas;
        }

      

        /// <summary>
        /// 新增协调情况
        /// </summary>
        /// <returns></returns>
        [MyAuthorize]
        [HttpPost("{id:int}/coordinations")]
        public IActionResult AddCoordination(int id, [FromBody]AddProblemCoordinationInput coordinationInput)
        {
            var rectificationId = _problemService.AddCoordination(id, coordinationInput);
            return Created("", new { rectificationId});
        }

        /// <summary>
        /// 完成协调
        /// </summary>
        /// <param name="id">问题id</param>
        /// <param name="input"></param>
        /// <returns></returns>
        [MyAuthorize]
        [HttpPut("complete/{id:int}")]
        public IActionResult CompleteCoordination(int id, [FromBody]CompleteProblemInput input)
        {
            input.Id = id;
            _problemService.CompleteCoordination(input);
            return Created("", new {id});
        }

        /// <summary>
        /// 删除存在问题
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id:int}")]
        public void Delete(int id)
        {
            _problemService.Delete(id);
        }
    }
}
