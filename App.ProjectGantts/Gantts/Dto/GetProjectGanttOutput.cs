using App.ProjectGantts.Links.Dto;
using App.ProjectGantts.Tasks.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.ProjectGantts.Gantts.Dto
{
    public class GetProjectGanttOutput
    {
        public int Id { get; set; }
        public List<GetProjectTaskOutput> Data { get; set; }
        public List<GetProjectLinkOutput> Links { get; set; }
        public GetProjectGanttOutput()
        {
            Data = new List<GetProjectTaskOutput>();
            Links = new List<GetProjectLinkOutput>();
        }
    }
}
