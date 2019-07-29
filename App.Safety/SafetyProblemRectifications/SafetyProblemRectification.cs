using App.Core.Common.Entities;
using App.Safety.SafetyProblemRectifications;
using App.Safety.SafetyProblems;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace App.Safety.SafetyProblemProgresses
{
    public class SafetyProblemRectification: EntityWithAuditHaveTenant
    {
        public int SafetyProblemId { get; set; }

        /// <summary>
        /// 安全问题
        /// </summary>
        [ForeignKey("SafetyProblemId")]
        public SafetyProblem SafetyProblem { get; set; }

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
        public List<SafetyProblemRectificationAttachment> RectificationPhotoSets { get; set; }

    }
}
