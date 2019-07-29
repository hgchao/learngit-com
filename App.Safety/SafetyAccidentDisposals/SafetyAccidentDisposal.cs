using App.Core.Common.Entities;
using App.Safety.SafetyAccidents;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace App.Safety.SafetyAccidentDisposals
{
    public class SafetyAccidentDisposal: EntityWithAuditHaveTenant
    {

        public int SafetyAccidentId { get; set; }

        [ForeignKey("SafetyAccidentId")]
        public SafetyAccident SafetyAccident { get; set; }

        /// <summary>
        /// 处置时间
        /// </summary>
        public DateTime? ExecDate { get; set; }

        /// <summary>
        /// 处置方案
        /// </summary>
        public string Solution { get; set; }

        /// <summary>
        /// 处置图片
        /// </summary>
        public List<SafetyAccidentDisposalAttachment> DisposalPhotoSets { get; set; }
    }
}
