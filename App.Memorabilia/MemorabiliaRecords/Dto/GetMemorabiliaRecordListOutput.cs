
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Memorabilia.MemorabiliaRecords.Dto
{
    public class GetMemorabiliaRecordListOutput
    {
        public int Id { get; set; }

        public int ProjectId { get; set; }

        /// <summary>
        /// 项目编号
        /// </summary>
        public string ProjectNumber { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// 事项分类
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// 事项名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 参与人员
        /// </summary>
        public string Participant { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 是否有权限操作
        /// </summary>
        public bool HasPermission { get; set; }

        ///// <summary>
        ///// 附件
        ///// </summary>
        //public List<GetPmAttachOutput> Attaches { get; set; }

        //public GetMemorabiliaRecordListOutput()
        //{
        //    Attaches = new List<GetPmAttachOutput>();
        //}
    }
}
