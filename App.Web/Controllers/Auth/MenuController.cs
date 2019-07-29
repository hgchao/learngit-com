using App.Core.Authorization.Apis.Dto;
using App.Core.Authorization.Menus;
using App.Core.Authorization.Menus.Dto;
using App.Core.Common.Entities;
using Microsoft.AspNetCore.Mvc;
using App.Web.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace App.Web.Controllers.Auth
{
    /// <summary>
    /// 验证授权-菜单
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/menus")]
    public class MenuController: ControllerBase
    {
        private IMenuService _menuService;
        public MenuController(IMenuService menuService)
        {
            _menuService = menuService;
        }

        /// <summary>
        /// 上移菜单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [MyAuthorize]
        [HttpPut("{id:int}/up")]
        public IActionResult MoveUp(int id)
        {
            _menuService.Sort(id, true);
            return Created("", new { id});
        }
        /// <summary>
        /// 下移菜单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [MyAuthorize]
        [HttpPut("{id:int}/down")]
        public IActionResult MoveDown(int id)
        {
            _menuService.Sort(id, false);
            return Created("", new { id});
        }
        /// <summary>
        /// 获取菜单详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [MyAuthorize]
        [HttpGet("{id:int}")]
        public GetMenuOutput Get(int id)
        {
            return _menuService.Get(id);
        }

        /// <summary>
        /// 获取菜单下子菜单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [MyAuthorize]
        [HttpGet("{id:int}/children")]
        public List<GetMenuOutput> GetChildren(int id)
        {
            return _menuService.GetChildren(id);
        }

        /// <summary>
        /// 获取菜单根节点
        /// </summary>
        /// <returns></returns>
        [MyAuthorize]
        [HttpGet]
        public PaginationData<GetMenuOutput> GetRoot(int pageIndex, int pageSize)
        {
            return _menuService.GetRoots(pageIndex, pageSize);
        }

        /// <summary>
        /// 新增菜单
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        [MyAuthorize]
        [HttpPost("")]
        public IActionResult Add([FromBody]AddMenuInput menu)
        {
            int id = _menuService.Add(menu);
            return Created("", new { id });
        }

        /// <summary>
        /// 更新菜单
        /// </summary>
        /// <param name="id"></param>
        /// <param name="menu"></param>
        /// <returns></returns>
        [MyAuthorize]
        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody]UpdateMenuInput menu)
        {
            menu.Id = id;
            _menuService.Update(menu);
            return Created("", new { menu.Id });
        }

        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [MyAuthorize]
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            _menuService.Delete(id);
            return NoContent();
        }

    }
}