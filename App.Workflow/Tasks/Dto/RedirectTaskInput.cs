using System;
using System.Collections.Generic;
using System.Text;

namespace App.Workflow.Tasks.Dto
{
    public class RedirectTaskInput
    {
        public int Id { get; set; }
        /// <summary>
        /// 转向的节点UID
        /// </summary>
        public string NodeUid { get; set; }
        /// <summary>
        /// 评论
        /// </summary>
        public string Comment { get; set; }
    }
}
