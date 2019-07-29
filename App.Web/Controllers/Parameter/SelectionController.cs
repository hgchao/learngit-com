using App.Core.Parameter.Selections;
using App.Core.Parameter.Selections.Dto;
using App.Web.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Web.Controllers.Selection
{
    /// <summary>
    /// 参数设置-选择器
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/selections")]
    public class SelectionController: ControllerBase
    {
        private ISelectionService _selectionService;
        public SelectionController(ISelectionService selectionService)
        {
            _selectionService = selectionService;
        }

        /// <summary>
        /// 获取Selection
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns></returns>
        [MyAuthorize]
        [HttpGet("display")]
        public GetDisplaySelectionOutput Get(string name)
        {
            return _selectionService.Get(name);
        }


        /// <summary>
        /// 获取Selection列表
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <returns></returns>
        [MyAuthorize]
        [HttpGet]
        public List<GetSelectionOutput> GetList(string keyword = null)
        {
            return _selectionService.GetList(keyword);
        }

        /// <summary>
        /// 新增Selection
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [MyAuthorize]
        [HttpPost]
        public IActionResult AddSelection([FromBody]AddSelectionInput input)
        {
            int id = _selectionService.Add(input);
            return Created("", new {id});
        }

        /// <summary>
        /// 更新Selection
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [MyAuthorize]
        [HttpPut("{id:int}")]
        public IActionResult UpdateSelection(int id, [FromBody]UpdateSelectionInput input)
        {
            input.Id = id;
            _selectionService.UpdateSelection(input);
            return Created("", new { id });
        }

        /// <summary>
        /// 更新Option
        /// </summary>
        /// <param name="id"></param>
        /// <param name="optionId"></param>
        /// <param name="optionInput"></param>
        /// <returns></returns>
        [MyAuthorize]
        [HttpPut("{id:int}/options/{optionId:int}")]
        public IActionResult UpdateOptionName(int id, int optionId, [FromBody]UpdateOptionInput optionInput)
        {
            optionInput.Id = optionId;
            _selectionService.UpdateOption(id, optionInput);
            return Created("", new { optionId });
        }

        /// <summary>
        /// 添加Option
        /// </summary>
        /// <param name="id"></param>
        /// <param name="optionInput"></param>
        /// <returns></returns>
        [MyAuthorize]
        [HttpPost("{id:int}/options")]
        public IActionResult AddOptionName(int id, [FromBody]AddOptionInput optionInput)
        {
            int optionId = _selectionService.AddOption(id, optionInput);
            return Created("", new { optionId });
        }

        /// <summary>
        /// 切换option状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="optionId"></param>
        /// <param name="isDisabled"></param>
        /// <returns></returns>
        [MyAuthorize]
        [HttpPut("{id:int}/options/{optionId:int}/{isDisabled:bool}")]
        public IActionResult ToggleOptionStatus(int id, int optionId, bool isDisabled)
        {
            _selectionService.ToggleOptionState(id, isDisabled);
            return Created("", new { optionId });
        }

        /// <summary>
        /// Selection排序
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isUpper">是否向上</param>
        /// <returns></returns>
        [MyAuthorize]
        [HttpPut("{id:int}/sort/{isUpper:bool}")]
        public IActionResult SortSelection(int id, bool isUpper=true)
        {
            _selectionService.SortSelection(id, isUpper);
            return Created("", new { id });
        }

        /// <summary>
        /// Option排序
        /// </summary>
        /// <param name="id"></param>
        /// <param name="optionId"></param>
        /// <param name="isUpper">是否向上</param>
        /// <returns></returns>
        [MyAuthorize]
        [HttpPut("{id:int}/options/{optionId:int}/sort/{isUpper:bool}")]
        public IActionResult SortOption(int id, int optionId, bool isUpper=true)
        {
            _selectionService.SortOption(id, optionId, isUpper);
            return Created("", new { optionId });
        }
    }
}
