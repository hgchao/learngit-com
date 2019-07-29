
using App.Core.FileManagement.AttachmentFileMetas.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Safety.SafetyAccidents.Dto
{
    public class SettleSafetyAccidentInput
    {
        public int Id { get; set; }
        /// <summary>
        /// 解决的时间
        /// </summary>
        public DateTime? SettlementTime { get; set; }
        /// <summary>
        /// 解决后图片
        /// </summary>
        public List<AddAttachmentFileMetaInput> SettlementPhotoSets { get; set; }
        public SettleSafetyAccidentInput()
        {
            SettlementPhotoSets = new List<AddAttachmentFileMetaInput>();
        }
    }
}
