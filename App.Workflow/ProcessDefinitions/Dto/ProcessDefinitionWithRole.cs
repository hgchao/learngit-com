using App.Core.Workflow.Engine.Definition;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Workflow.ProcessDefinitions.Dto
{
    public class ProcessDefinitionWithRole
    {
        public ProcessDefinition ProcessDefinition { get; set; }
        public List<int> RoleIds { get; set; }
        public ProcessDefinitionWithRole()
        {
            RoleIds = new List<int>();
        }
    }
}
