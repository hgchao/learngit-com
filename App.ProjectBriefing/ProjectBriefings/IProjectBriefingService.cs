using App.ProjectBriefings.ProjectBriefings.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.ProjectBriefings.ProjectBriefings
{
    public interface IProjectBriefingService
    {
        GetProjectBriefingOutput GetLatest(int projectId);
        int AddNew(int tenantId, int projectId);
    }
}
