using App.Core.Common.Entities;
using App.Problems.ProblemCoordinations;
using App.Problems.Problems;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace App.Problems.ProblemCoordinations
{
    public class ProblemCoordination: EntityWithAuditHaveTenant
    {
        public int ProblemId { get; set; }

        /// <summary>
        /// 问题
        /// </summary>
        [ForeignKey("ProblemId")]
        public Problem Problem { get; set; }

        /// <summary>
        /// 协调日期
        /// </summary>
        public DateTime? ExecDate { get; set; }

        /// <summary>
        /// 协调描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 协调图片
        /// </summary>
        public List<ProblemCoordinationAttachment> CoordinationPhotoSets { get; set; }
    }
}
