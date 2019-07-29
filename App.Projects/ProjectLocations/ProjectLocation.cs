using App.Core.Common.Entities;
using App.Core.Common.Repositories;
using App.Projects.ProjectBaseInfos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace App.Projects.ProjectLocations
{
    public class ProjectLocation: EntityHaveTenant
    {

        [DisableUpdate]
        public int ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        public Project Project { get; set; }

        /// <summary>
        /// 省
        /// </summary>
        public string Province { get; set; }

        /// <summary>
        /// 市
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 区/县
        /// </summary>
        public string District { get; set; }

        /// <summary>
        /// 镇/乡
        /// </summary>
        public string Town { get; set; }

        /// <summary>
        /// 街道
        /// </summary>
        public string Street { get; set; }

        /// <summary>
        /// 详细地址
        /// </summary>
        public string AddressDetail { get; set; }

        /// <summary>
        /// 经度
        /// </summary>
        public float? Longitude { get; set; }

        /// <summary>
        /// 纬度
        /// </summary>
        public float? Latitude { get; set; }
    }
}
