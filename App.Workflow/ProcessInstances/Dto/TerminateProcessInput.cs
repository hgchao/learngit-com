using System;
using System.Collections.Generic;
using System.Text;

namespace App.Workflow.ProcessInstances.Dto
{
    public class TerminateProcessInput
    {
        /// <summary>
        /// 流程实例id
        /// </summary>
        public int ProcessInstanceId { get; set; }
        /// <summary>
        /// 终止原因
        /// </summary>
        public string Reason
        {
            get; set;
        }
    }
}
