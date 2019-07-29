using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.ProjectBriefings.ProjectBriefings;
using App.ProjectBriefings.ProjectBriefings.Dto;
using App.Web.FileUrl;
using App.Web.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.Web.Controllers.Projects.ProjectBriefing
{
    /// <summary>
    /// 项目简报
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/projectbriefing")]
    public class ProjectBriefingController : ControllerBase
    {
        private IProjectBriefingService _projectBriefingService;
        private IFileUrlBuilder _fileUrlBuilder;
        public ProjectBriefingController(IProjectBriefingService projectBriefingService,
            IFileUrlBuilder fileUrlBuilder)
        {
            _projectBriefingService = projectBriefingService;
            _fileUrlBuilder = fileUrlBuilder;
        }
        /// <summary>
        /// 获取项目简报
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [MyAuthorize]
        [HttpGet("{projectId:int}/briefings")]
        public GetProjectBriefingOutput GetProjectBriefing(int projectId)
        {
            var briefing = _projectBriefingService.GetLatest(projectId);
            _fileUrlBuilder.SetAttachFileUrl(briefing);
            return briefing;
        }
    }
}