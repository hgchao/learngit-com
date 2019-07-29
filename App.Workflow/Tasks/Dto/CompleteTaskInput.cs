using App.Core.Form.Fields.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Workflow.Tasks.Dto
{
    public class CompleteTaskInput
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public bool PreventCommit { get; set; }
        public List<FieldDto> FormContents { get; set; }

        public CompleteTaskInput()
        {
            FormContents = new List<FieldDto>();
        }

    }
}
