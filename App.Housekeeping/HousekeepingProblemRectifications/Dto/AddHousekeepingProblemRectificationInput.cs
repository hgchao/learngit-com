using App.Core.FileManagement.AttachmentFileMetas.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Housekeeping.HousekeepingProblemRectifications.Dto
{
    public class AddHousekeepingProblemRectificationInput
    {
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
        public List<AddAttachmentFileMetaInput> RectificationPhotoSets { get; set; }
        public AddHousekeepingProblemRectificationInput()
        {
            RectificationPhotoSets = new List<AddAttachmentFileMetaInput>();
        }
    }
}
