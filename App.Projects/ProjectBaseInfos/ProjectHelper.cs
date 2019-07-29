using App.Base.Repositories;
using App.Core.Authorization.Accounts;
using App.Projects.ProjectBaseInfos.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Projects.ProjectBaseInfos
{
    public class ProjectHelper: IProjectHelper
    {
        private IAppRepositoryBase<Project> _projectRepository;
        private IAuthInfoProvider _authInfoProvider;
        public ProjectHelper(
            IAppRepositoryBase<Project> projectRepository,
            IAuthInfoProvider authInfoProvider
            )
        {
            _projectRepository = projectRepository;
            _authInfoProvider = authInfoProvider;
        }
        public bool HasPermission(string role, int projectId)
        {
            var userId = _authInfoProvider.GetCurrent().User.Id;
            var list = _projectRepository.Get().Where(u => u.Id == projectId).SelectMany(u => u.Members).Where(u => u.ProjectRole == role).Select(u => u.UserId).ToList();
            if (!list.Contains(userId))
                return false;
            else
                return true;
        }

        public List<CurrentMemberPermission> HasPermission(string role, List<int> projectIds)
        {

            var userId = _authInfoProvider.GetCurrent().User.Id;
            return _projectRepository.Get().Where(u => projectIds.Contains(u.Id))
                .Select(u => new {
                    ProjectId = u.Id,
                    UserIdList = u.Members.Where(v => v.ProjectRole == role).Select(v => v.UserId).ToList()
                }).ToList()
                .Select(u=>new CurrentMemberPermission {
                    ProjectRole = role,
                    ProjectId = u.ProjectId,
                    HasPermission = u.UserIdList.Contains(userId)
                }).ToList();
        }
    }
}
