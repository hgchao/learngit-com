using System;
using System.Collections.Generic;
using System.Text;

namespace App.Memorabilia.MemorabiliaRecords.Dto
{
  public  class ExportMemorabiliaRecordOutput
    {
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
        /// 事项内容描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 参与人员
        /// </summary>
        public string Participant { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }
    }
}
