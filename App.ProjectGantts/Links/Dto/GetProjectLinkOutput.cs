using System;
using System.Collections.Generic;
using System.Text;

namespace App.ProjectGantts.Links.Dto
{
    public class GetProjectLinkOutput
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public int Source { get; set; }
        public int Target { get; set; }
    }
}
