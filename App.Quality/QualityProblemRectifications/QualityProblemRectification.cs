using App.Core.Common.Entities;
using App.Quality.QualityProblemRectifications;
using App.Quality.QualityProblems;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace App.Quality.QualityProblemRectifications
{
    public class QualityProblemRectification: EntityWithAuditHaveTenant
    {
        public int QualityProblemId { get; set; }

        /// <summary>
        /// 质量问题
        /// </summary>
        [ForeignKey("QualityProblemId")]
        public QualityProblem QualityProblem { get; set; }

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
        public List<QualityProblemRectificationAttachment> RectificationPhotoSets { get; set; }
    }
}
