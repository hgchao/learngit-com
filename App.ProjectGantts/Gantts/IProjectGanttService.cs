using App.ProjectGantts.Gantts.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.ProjectGantts.Gantts
{
    public interface IProjectGanttService
    {
        GetProjectGanttOutput Get(int id);
        GetProjectGanttOutput GetByProject(int projectId);
        int AddByProject(int projectId);
    }
}
