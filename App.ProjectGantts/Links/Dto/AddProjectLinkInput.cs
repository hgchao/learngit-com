using System;
using System.Collections.Generic;
using System.Text;

namespace App.ProjectGantts.Links.Dto
{
    public class AddProjectLinkInput
    {
        public int GanttId { get; set; }
        public string Type { get; set; }
        public int Source { get; set; }
        public int Target { get; set; }
    }
}
