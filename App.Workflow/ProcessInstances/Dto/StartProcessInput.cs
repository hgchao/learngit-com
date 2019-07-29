using App.Core.Form.Fields.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Workflow.ProcessInstances.Dto
{
    public class StartProcessInput
    {
        public string ProcessDefinitionName { get; set; }
        public int? ParentProcessId { get; set; }
        public string ProcessName { get; set; }
        public bool PreventCommit { get; set; }
        public List<FieldDto> FormContents { get; set; }
        public StartProcessInput()
        {
            FormContents = new List<FieldDto>();
        }
    }
}
