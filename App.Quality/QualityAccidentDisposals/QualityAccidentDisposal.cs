using App.Core.Common.Entities;
using App.Quality.QualityAccidents;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace App.Quality.QualityAccidentDisposals
{
    public class QualityAccidentDisposal: EntityWithAuditHaveTenant
    {

        public int QualityAccidentId { get; set; }

        [ForeignKey("QualityAccidentId")]
        public QualityAccident QualityAccident { get; set; }

        /// <summary>
        /// 处置时间
        /// </summary>
        public DateTime? ExecDate { get; set; }

        /// <summary>
        /// 处置方案
        /// </summary>
        public string Plan { get; set; }

        /// <summary>
        /// 处置图片
        /// </summary>
        public List<QualityAccidentDisposalAttachment> DisposalPhotoSets { get; set; }
    }
}
