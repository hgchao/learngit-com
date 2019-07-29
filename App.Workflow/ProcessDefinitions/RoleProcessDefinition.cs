using App.Core.Authorization.Roles;
using App.Core.Common.Entities;
using App.Core.Workflow.ProcessDefinitions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace App.Workflow.ProcessDefinitions
{
    public class RoleProcessDefinition: EntityHaveTenant
    {
        public int ProcessDefinitionId { get; set; }
        public int RoleId { get; set; }

        [ForeignKey("ProcessDefinitionId")]
        public Wf_Re_ProcessDefinition ProcessDefinition { get; set; }
        [ForeignKey("RoleId")]
        public Role Role { get; set; }
    }
}
