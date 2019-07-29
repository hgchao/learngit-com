using App.Core.Common.Entities;
using App.Projects.ProjectBaseInfos.Dto;
using App.Projects.Projects;
using App.Statistics.ProjectStatistics;
using App.Statistics.ProjectStatistics.Dto;
using App.Web.FileUrl;
using App.Web.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Web.Controllers.Projects
{

    /// <summary>
    /// 项目-项目信息
    /// </summary>
    [ApiVersion("1.0")]
    [MyAuthorize]
    [Route("api/v{version:apiVersion}/projects")]
    public class ProjectController: ControllerBase
    {
        private IProjectService _projectService;
        private IProjectStatisticsService _projectStatisticsService;
        private IFileUrlBuilder _fileUrlBuilder;

        public ProjectController(
            IProjectService projectService,
            IProjectStatisticsService projectStatisticsService,
            IFileUrlBuilder fileUrlBuilder)
        {
            _projectService = projectService;
            _projectStatisticsService = projectStatisticsService;
            _fileUrlBuilder = fileUrlBuilder;
        }

        /// <summary>
        /// 项目对大事记，安全信息等是否有添加权限，循环项目找权限
        /// </summary>
        /// <param name="projectRole"></param>
        /// <param name="projectIdStr">项目id列表，例：1,2,3</param>
        /// <returns></returns>
        [HttpGet("permission")]
        public List<CurrentMemberPermission> GetPermission(string projectRole, string projectIdStr)
        {
            List<int> projectIdList = new List<int>();
            if (!string.IsNullOrWhiteSpace(projectIdStr))
            {
                projectIdList = projectIdStr.Split(",").ToList().Select(u => Convert.ToInt32(u)).ToList();
            }
            return _projectService.GetCurrentPermission(projectIdList, projectRole);
        }

        /// <summary>
        /// 统计项目性质个数，按月统计
        /// </summary>
        /// <returns></returns>
        [HttpGet("totalcount")]
        public GetProjectStatisticsOutput GetTotalCount(string year)
        {
            var project = _projectService.GetTotalCount(year);
            return project;
        }

        /// <summary>
        /// 首页统计项目金额
        /// </summary>
        /// <returns></returns>
        [HttpGet("totalmoney")]
        public GetProjectCountOutput GetTotalMoney(DateTime? commencementDateL, DateTime? commencementDateR)
        {
            var project = _projectService.GetTotalMoney(commencementDateL, commencementDateR);
            return project;
        }

        /// <summary>
        /// 通过Id获取项目信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        public GetProjectOutput Get(int id)
        {
            var data = _projectService.Get(id);
            _fileUrlBuilder.SetAttachFileUrl(data);
            return data;
        }

        /// <summary>
        /// 获取项目信息列表
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="address">位置</param>
        /// <param name="typeId">项目类型OptionId</param>
        /// <param name="Name"></param>
        /// <param name="No"></param>
        /// <param name="projectNatureId">项目性质OptionId</param>
        /// <param name="stateId">项目状态OptionId</param>
        /// <param name="projectLeaderId">项目负责人Id</param>
        /// <param name="generalEstimateL">最小概算</param>
        /// <param name="generalEstimateR">最大概算</param>
        /// <param name="commencementDateL">最小开工时间</param>
        /// <param name="commencementDateR">最大开工时间</param>
        /// <returns></returns>
        [HttpGet("list")]
        public List<GetProjectListOutput> Get(string keyword,
            string address,
            int? typeId,
            string Name,
            string No,
            int? projectNatureId,
            int? stateId,
            int? projectLeaderId,
            int? generalEstimateL, int? generalEstimateR,
            DateTime? commencementDateL, DateTime? commencementDateR
            )
        {
            return _projectService.Get(keyword,
             address,
             typeId,
             Name,
             No,
             projectNatureId,
             stateId,
             projectLeaderId,
             generalEstimateL,  generalEstimateR,
            commencementDateL, commencementDateR);
        }

        /// <summary>
        /// 分页获取项目信息
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyword"></param>
        /// <param name="address">位置</param>
        /// <param name="typeId">项目类型OptionId</param>
        /// <param name="Name"></param>
        /// <param name="No"></param>
        /// <param name="projectNatureId">项目性质OptionId</param>
        /// <param name="stateId">项目状态OptionId</param>
        /// <param name="projectLeaderId">项目负责人Id</param>
        /// <param name="generalEstimateL">最小概算</param>
        /// <param name="generalEstimateR">最大概算</param>
        /// <param name="commencementDateL">最小开工时间</param>
        /// <param name="commencementDateR">最大开工时间</param>
        /// <param name="sortField">需要排序的字段(项目编号:No,项目名称:Name,项目类型:TypeId,项目性质:ProjectNatureId,项目状态:StateId,项目概算:GeneralEstimate,开工时间:CommencementDate,项目负责人:UserId,发布人:CreatorId)</param>
        /// <param name="sortState">1降序2升序0默认</param>
        /// <returns></returns>
        [HttpGet]
        public PaginationData<GetProjectListOutput> Get(int pageIndex, int pageSize, string keyword,
            string address,
            int? typeId,
            string Name,
            string No,
            int? projectNatureId,
            int? stateId,
            int? projectLeaderId,
            int? generalEstimateL, int? generalEstimateR,
            DateTime? commencementDateL, DateTime? commencementDateR,
            string sortField,
            string sortState
            )
        {
            return _projectService.Get(pageIndex, pageSize, keyword,
             address,
             typeId,
             Name,
             No,
             projectNatureId,
             stateId,
             projectLeaderId,
             generalEstimateL,  generalEstimateR,
            commencementDateL, commencementDateR,
            sortField,
            sortState
            );
        }

        /// <summary>
        /// 分页获取带问题的项目信息
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyword"></param>
        /// <param name="address">位置</param>
        /// <param name="typeId">项目类型OptionId</param>
        /// <param name="Name"></param>
        /// <param name="No"></param>
        /// <param name="projectNatureId">项目性质OptionId</param>
        /// <param name="stateId">项目状态OptionId</param>
        /// <param name="projectLeaderId">项目负责人Id</param>
        /// <param name="generalEstimateL">最小概算</param>
        /// <param name="generalEstimateR">最大概算</param>
        /// <param name="commencementDateL">最小开工时间</param>
        /// <param name="commencementDateR">最大开工时间</param>
        /// <param name="sortField">需要排序的字段(项目编号:No,项目名称:Name,项目类型:TypeId,项目性质:ProjectNatureId,项目状态:StateId,项目概算:GeneralEstimate,开工时间:CommencementDate,项目负责人:UserId,发布人:CreatorId)</param>
        /// <param name="sortState">1降序2升序0默认</param>
        /// <returns></returns>
        [HttpGet("with-problems")]
        public PaginationData<GetProjectWithProblemListOutput> GetWithProblem(int pageIndex, int pageSize, string keyword,
            string address,
            int? typeId,
            string Name,
            string No,
            int? projectNatureId,
            int? stateId,
            int? projectLeaderId,
            int? generalEstimateL, int? generalEstimateR,
            DateTime? commencementDateL, DateTime? commencementDateR,
            string sortField,
            string sortState
            )
        {
            return _projectStatisticsService.Get(pageIndex, pageSize, keyword,
             address,
             typeId,
             Name,
             No,
             projectNatureId,
             stateId,
             projectLeaderId,
             generalEstimateL,  generalEstimateR,
            commencementDateL, commencementDateR,
            sortField,
            sortState
            );
        }

        /// <summary>
        /// 删除项目信息
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id:int}")]
        public void Delete(int id)
        {
            _projectService.Delete(id);
        }

        /// <summary>
        /// 首页分页获取质量和安全问题项目
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyword"></param>
        /// <param name="sortField">需要排序的字段(项目编号:No,项目名称:Name,项目类型:TypeId,项目性质:ProjectNatureId,项目状态:StateId,项目概算:GeneralEstimate,开工时间:CommencementDate,项目负责人:UserId,发布人:CreatorId)</param>
        /// <param name="sortState">1降序2升序0默认</param>
        /// <returns></returns>
        [HttpGet("projectstatistics")]
        public PaginationData<GetProjectStatisticsListOutput> Get(int pageIndex, int pageSize, string keyword,
           
            string sortField,
            string sortState
            )
        {
            return _projectStatisticsService.Get(pageIndex, pageSize, keyword,
            sortField,
            sortState
            );
        }
    }
}
