
using App.Core.FileManagement.AttachmentFileMetas.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Housekeeping.HousekeepingProblemRectifications.Dto
{
    public class GetHousekeepingProblemRectificationOutput
    {
        public int Id { get; set; }
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
        public List<GetAttachmentFileMetaOutput> RectificationPhotoSets { get; set; }
        public GetHousekeepingProblemRectificationOutput()
        {
            RectificationPhotoSets = new List<GetAttachmentFileMetaOutput>();
        }
    }
}
