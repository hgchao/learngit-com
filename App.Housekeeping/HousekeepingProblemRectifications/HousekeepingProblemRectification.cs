using App.Core.Common.Entities;
using App.Housekeeping.Housekeepings;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace App.Housekeeping.HousekeepingProblemRectifications
{
    public class HousekeepingProblemRectification: EntityWithAuditHaveTenant
    {
        public int HousekeepingProblemId { get; set; }

        /// <summary>
        /// 文明施工问题
        /// </summary>
        [ForeignKey("HousekeepingProblemId")]
        public HousekeepingProblem HousekeepingProblem { get; set; }

        /// <summary>
        /// 整改日期
        /// </summary>
        public DateTime? ExecDate { get; set; }

        /// <summary>
        /// 整改描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 整改图片
        /// </summary>
        public List<HousekeepingProblemRectificationAttachment> RectificationPhotoSets { get; set; }

    }
}
