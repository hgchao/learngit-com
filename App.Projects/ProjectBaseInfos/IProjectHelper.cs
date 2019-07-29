using App.Base.Repositories;
using App.Core.Authorization.Accounts;
using App.Projects.ProjectBaseInfos.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Projects.ProjectBaseInfos
{
    public interface IProjectHelper
    {
        bool HasPermission(string role, int projectId);
        List<CurrentMemberPermission> HasPermission(string role, List<int> projectIds);
    }
}
