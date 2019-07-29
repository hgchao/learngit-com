using App.ProjectGantts.Tasks;
using App.ProjectGantts.Tasks.Dto;
using App.Web.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Web.Controllers.Projects.Gantts
{
    [ApiVersion("1.0")]
    [MyAuthorize]
    [Route("api/v{version:apiVersion}/project-tasks")]
    public class ProjectTaskController : ControllerBase
    {
        private IProjectTaskService _taskService;
        public ProjectTaskController(IProjectTaskService taskService)
        {
            _taskService = taskService;
        }

        /// <summary>
        /// 通过Id获取项目任务
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        public GetProjectTaskOutput Get(int id)
        {
            return _taskService.Get(id);
        }

        /// <summary>
        /// 新增项目任务
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Add([FromBody] AddProjectTaskInput input)
        {
            int id = _taskService.Add(input);
            return Created("", new { id });
        }

        /// <summary>
        /// 更新项目任务
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody]UpdateProjectTaskInput input)
        {
            input.Id = id;
            _taskService.Update(input);
            return Created("", new {id});
        }

        /// <summary>
        /// 完成项目任务
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("complete/{id:int}")]
        public IActionResult CompleteTask(int id, [FromBody]CompleteProjectTaskInput input)
        {
            input.Id = id;
            _taskService.CompleteTask(input);
            return Created("", new {id});
        }

        /// <summary>
        /// 删除项目任务
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            _taskService.Delete(id);
            return NoContent();
        }

    }
}
