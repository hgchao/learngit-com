using System;
using System.Collections.Generic;
using System.Text;

namespace App.Projects.ProjectLocations.Dto
{
    public class AddProjectLocationInput
    {
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
        /// 镇/乡/街道
        /// </summary>
        public string Town { get; set; }

        /// <summary>
        /// 详细地址
        /// </summary>
        public string AddressDetail { get; set; }

        /// <summary>
        /// 经度
        /// </summary>
        public float Longitude { get; set; }

        /// <summary>
        /// 纬度
        /// </summary>
        public float Latitude { get; set; }

    }
}
