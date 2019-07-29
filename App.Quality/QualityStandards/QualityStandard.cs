using App.Core.Common.Entities;
using App.Core.Parameter.Selections;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace App.Quality.QualityStandards
{
    /// <summary>
    /// 质量管理标准
    /// </summary>
    public class QualityStandard: EntityWithAuditHaveTenant
    {

        public string Title { get; set; }

        public int CategoryId { get; set; }
        /// <summary>
        /// 标准分类
        /// </summary>
        [ForeignKey("CategoryId")]
        public Option Category { get; set; }

        /// <summary>
        /// 链接
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        /// 附件
        /// </summary>
        public List<QualityStandardAttachment> Attachments { get; set; }

        public QualityStandard()
        {
            Attachments = new List<QualityStandardAttachment>();
        }

    }
}
