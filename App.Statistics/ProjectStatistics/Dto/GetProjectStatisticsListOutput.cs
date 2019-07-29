using App.Projects.ProjectLocations.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Statistics.ProjectStatistics.Dto
{
    public class GetProjectStatisticsListOutput
    {
        public int Id { get; set; }

        /// <summary>
        /// 项目编号
        /// </summary>
        public string No { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 项目类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 项目性质
        /// </summary>
        public string ProjectNature { get; set; }

        /// <summary>
        /// 项目状态
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// 项目概算
        /// </summary>
        public decimal GeneralEstimate { get; set; }

        /// <summary>
        /// 资金来源
        /// </summary>
        public string FundsSource { get; set; }

        /// <summary>
        /// 项目负责人
        /// </summary>
        public int ProjectLeader { get; set; }

        /// <summary>
        /// 开工时间
        /// </summary>
        public DateTime? CommencementDate { get; set; }

        public GetProjectLocationOutput Location { get; set; }

        /// <summary>
        /// 是否有权限操作
        /// </summary>
        public bool HasPermission { get; set; }

        /// <summary>
        /// 是否有项目进度权限操作
        /// </summary>
        public bool HasGanttPermission { get; set; }

        /// <summary>
        /// 是否有项目计划权限操作
        /// </summary>
        public bool HasPlanPermission { get; set; }

        /// <summary>
        /// 创建人Id
        /// </summary>
        public int CreatorId { get; set; }

        /// <summary>
        /// 质量问题个数
        /// </summary>
        public int QualityProblemCount { get; set; }

        /// <summary>
        /// 安全问题个数
        /// </summary>
        public int SafetyProblemCount { get; set; }
    }
    
}
