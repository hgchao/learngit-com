using App.Core.Common.Entities;
using App.Safety.SafetyAccidentDisposals;
using App.Safety.SafetyAccidentDisposals.Dto;
using App.Safety.SafetyAccidents;
using App.Safety.SafetyAccidents.Dto;
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
    /// 安全管理-安全事故
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/safetyaccident")]
    public class SafetyAccidentController: ControllerBase
    {
        private ISafetyAccidentService _problemService;
        private IFileUrlBuilder _fileUrlBuilder;
        public SafetyAccidentController(
            ISafetyAccidentService problemService,
            IFileUrlBuilder fileUrlBuilder
            )
        {
            _problemService = problemService;
            _fileUrlBuilder = fileUrlBuilder;
        }
        /// <summary>
        /// 验证是否重复
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
        /// 获取安全事故详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [MyAuthorize]
        [HttpGet("{id:int}")]
        public GetSafetyAccidentOutput Get(int id)
        {
            var progress = _problemService.Get(id);
            _fileUrlBuilder.SetAttachFileUrl(progress);
            return progress;
        }


        /// <summary>
        /// 分页获取安全事故列表
        /// </summary>
        /// <param name="projectId">项目id</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="state">处置状态（1未处置2正在处置3已处置）</param>
        /// <param name="keyword"></param>
        /// <param name="sortField">需要排序的字段(安全事故标题:Title,事故内容:Content,事故发现时间:DiscoveryTime,处置状态:DisposalState,解决的时间:SettlementTime)</param>
        /// <param name="sortState">1降序2升序0默认</param>
        /// <returns></returns>
        [MyAuthorize]
        [HttpGet()]
        public PaginationData<GetSafetyAccidentListOutput> get(
            int? projectId, 
            int pageIndex, int pageSize,
            DisposalState? state,
            string keyword,
             string sortField,
             string sortState)
        {
            var datas = _problemService.Get(projectId,pageIndex, pageSize, state,keyword, sortField, sortState);
            return datas;
        }
        /// <summary>
        /// 导出安全事故列表
        /// </summary>
        /// <param name="projectId">项目id</param>
        /// <param name="safetyAccidentApplicationIds">使用id列表，格式"id1,id2,id3"</param>
        /// <param name="keyword">关键字</param>
        /// <param name="state">处置状态（1未处置2正在处置3已处置）</param>
        /// <returns></returns>

        [HttpGet("export")]
        public IActionResult Export(
              int? projectId,
                string safetyAccidentApplicationIds,
                string keyword,
                DisposalState? state
)
        {
            var contractApplicationIdList = new List<int>();
            if (!string.IsNullOrEmpty(safetyAccidentApplicationIds))
            {
                contractApplicationIdList = safetyAccidentApplicationIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(u => Convert.ToInt32(u)).ToList();
            }
            var ms = _problemService.Export(
                projectId,
                "安全事故表",
                AppWebContext.Instance.Comments,
                  state,
                keyword,
                contractApplicationIdList

                );
            var bytes = new byte[ms.Length];
            ms.Read(bytes);
            ms.Close();
            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "安全事故表.xlsx");
        }
      
      

        /// <summary>
        /// 新增处置情况
        /// </summary>
        /// <returns></returns>
        [MyAuthorize]
        [HttpPost("{id:int}/disposals")]
        public IActionResult AddRectification(int id, [FromBody]AddSafetyAccidentDisposalInput disposalInput)
        {
            var disposalId = _problemService.AddDisposal(id, disposalInput);
            return Created("", new { disposalId });
        }

        /// <summary>
        /// 解决事故
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [MyAuthorize]
        [HttpPut("settle/{id:int}")]
        public IActionResult SettleAccident(int id, [FromBody]SettleSafetyAccidentInput input)
        {
            input.Id = id;
            _problemService.SettleAccident(input);
            return Created("", new { id });
        }
        /// <summary>
        /// 删除安全事故
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id:int}")]
        public void Delete(int id)
        {
            _problemService.Delete(id);
        }
    }
}
