using App.ProjectGantts.Gantts;
using App.ProjectGantts.Gantts.Dto;
using App.Web.FileUrl;
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
    [Route("api/v{version:apiVersion}/project-gantts")]
    public class ProjectGanttController: ControllerBase
    {
        private IProjectGanttService _ganttService;
        private IFileUrlBuilder _fileUrlBuilder;
        public ProjectGanttController(IProjectGanttService ganttService, IFileUrlBuilder fileUrlBuilder)
        {
            _ganttService = ganttService;
            _fileUrlBuilder = fileUrlBuilder;
        }

        [HttpGet("{id:int}")]
        public GetProjectGanttOutput Get(int id)
        {
            var data = _ganttService.Get(id);
            _fileUrlBuilder.SetAttachFileUrl(data);
            return data;
        }

        [HttpGet("by-project/{projectId:int}")]
        public GetProjectGanttOutput GetByProject(int projectId)
        {
            var data = _ganttService.GetByProject(projectId);
            _fileUrlBuilder.SetAttachFileUrl(data);
            return data;
        }
    }
}
