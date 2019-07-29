using System;
using System.Collections.Generic;
using System.Text;

namespace App.Base
{
    public class StartProcessInput<TData>
    {
        public string ProcessDefinitionName { get; set; }
        /// <summary>
        /// 阻止提交
        /// </summary>
        public bool PreventCommit { get; set; }
        public TData Data { get; set; }
    }
}
