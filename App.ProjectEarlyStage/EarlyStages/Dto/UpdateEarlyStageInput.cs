
using App.Core.FileManagement.AttachmentFileMetas.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.ProjectEarlyStage.EarlyStages.Dto
{
    public class UpdateEarlyStageInput
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }

        /// <summary>
        /// 节点OptionId
        /// </summary>
        public int NodeId { get; set; }

        /// <summary>
        /// 类型(前期管理或竣工管理)
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// 批复文号
        /// </summary>
        public string ReplyNumber { get; set; }

        /// <summary>
        /// 上报日期
        /// </summary>
        public DateTime? ReportDate { get; set; }

        /// <summary>
        /// 批复日期
        /// </summary>
        public DateTime? ReplyDate { get; set; }

        /// <summary>
        /// 附件
        /// </summary>
        public List<AddAttachmentFileMetaInput> Attachments { get; set; }

        public UpdateEarlyStageInput()
        {
            Attachments = new List<AddAttachmentFileMetaInput>();
        }
    }
}
