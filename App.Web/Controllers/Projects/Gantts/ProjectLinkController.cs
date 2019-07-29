using App.ProjectGantts.Links;
using App.ProjectGantts.Links.Dto;
using App.Web.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace App.Web.Controllers.Projects.Gantts
{
    [ApiVersion("1.0")]
    [MyAuthorize]
    [Route("api/v{version:apiVersion}/project-links")]
    public class ProjectLinkController : ControllerBase
    {
        private IProjectLinkService _linkService;
        public ProjectLinkController(IProjectLinkService linkService)
        {
            _linkService = linkService;
        }

        /// <summary>
        /// 根据Id获取连接
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        public GetProjectLinkOutput Get(int id)
        {
            return _linkService.Get(id);
        }

        /// <summary>
        /// 新增连接
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Add([FromBody] AddProjectLinkInput input)
        {
            int id = _linkService.Add(input);
            return Created("", new { id });
        }

        /// <summary>
        /// 更新连接
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody]UpdateProjectLinkInput input)
        {
            input.Id = id;
            _linkService.Update(input);
            return Created("", new {id});
        }

        /// <summary>
        /// 删除连接
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            _linkService.Delete(id);
            return NoContent();
        }

    }
}
