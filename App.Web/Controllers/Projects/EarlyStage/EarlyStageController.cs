using App.Core.Common.Entities;
using App.ProjectEarlyStage.EarlyStages;
using App.ProjectEarlyStage.EarlyStages.Dto;
using App.ProjectEarlyStage.PmEarlyStages.Dto;
using App.Web.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Web.Controllers.EarlyStage
{
    /// <summary>
    /// 项目流程管理
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/earlystage")]
    public class EarlyStageController: ControllerBase
    {
        private IEarlyStageService _stageService;
        public EarlyStageController(IEarlyStageService stageService)
        {
            _stageService = stageService;
        }

        /// <summary>
        /// 获取项目流程
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [MyAuthorize]
        [HttpGet("{id:int}")]
        public GetEarlyStageOutput Get(int id)
        {
            var progress = _stageService.Get(id);
            return progress;
        }
        /// <summary>
        /// 验证项目流程是否重复
        /// </summary>
        /// <param name="name"></param>
        ///<param name="id">id（修改的时候传入）</param>
        /// <returns></returns>
        [HttpGet("count")]
        public CountData GetCount(string name, int? id)
        {
            return new CountData { Count = _stageService.Count(name, id) };
        }

        /// <summary>
        /// 导出项目流程列表
        /// </summary>
        /// <param name="projectId">项目id</param>
        /// <param name="safetyAccidentApplicationIds">使用id列表，格式"id1,id2,id3"</param>
        /// <param name="typeName">前期管理、竣工管理</param>
        /// <returns></returns>

        [HttpGet("export")]
        public IActionResult Export(
                int? projectId,
                string safetyAccidentApplicationIds,
                string typeName
)
        {
            var safetyApplicationIdList = new List<int>();
            if (!string.IsNullOrEmpty(safetyAccidentApplicationIds))
            {
                safetyApplicationIdList = safetyAccidentApplicationIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(u => Convert.ToInt32(u)).ToList();
            }
            var ms = _stageService.Export(
                projectId,
                "项目流程表",
                AppWebContext.Instance.Comments,
                typeName,
                safetyApplicationIdList

                );
            var bytes = new byte[ms.Length];
            ms.Read(bytes);
            ms.Close();
            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "项目流程表.xlsx");
        }
        /// <summary>
        /// 分页获取项目流程列表
        /// </summary>
        /// <param name="projectId">项目id</param>
        /// <param name="typeName">前期管理、竣工管理</param>
        /// <param name="sortField">需要排序的字段(批复文号:ReplyNumber)</param>
        /// <param name="sortState">1降序2升序0默认</param>
        /// <returns></returns>
        [MyAuthorize]
        [HttpGet()]
        public List<GetEarlyStageOutput> Get(
            int? projectId,
            string typeName,
            string sortField,
             string sortState)
        {
            var datas = _stageService.Get(projectId, typeName, sortField, sortState);
            return datas;
        }

    }
}
