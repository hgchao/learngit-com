using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using App.Base.Repositories;
using App.Core.Authorization.Accounts;
using App.Core.Common.Exceptions;
using App.ProjectGantts.Gantts;
using App.ProjectGantts.Links.Dto;
using App.Projects.ProjectBaseInfos;
using PoorFff.Mapper;

namespace App.ProjectGantts.Links
{
    public class ProjectLinkService : IProjectLinkService
    {
        private IAppRepositoryBase<ProjectLink> _linkRepository;
        private IAppRepositoryBase<ProjectGantt> _ganttRepository;
        private IAppRepositoryBase<Project> _projectRepository;
        private IProjectHelper _projectHelper;
        public ProjectLinkService(IAppRepositoryBase<ProjectLink> linkRepository,
            IProjectHelper projectHelper,
            IAppRepositoryBase<ProjectGantt> ganttRepository
            )
        {
            _ganttRepository = ganttRepository;
            _linkRepository = linkRepository;
            _projectHelper = projectHelper;
        }

        public int Add(AddProjectLinkInput input)
        {
            var projectId = _ganttRepository.Get().Where(u => u.Id == input.GanttId).Select(u => u.ProjectId).FirstOrDefault();
            var list = _projectRepository.Get().Where(u => u.Id == projectId).SelectMany(u => u.Members).Where(u => u.ProjectRole == "项目进度负责人").Select(u => u.UserId).ToList();
            if (_projectHelper.HasPermission("项目进度负责人", projectId))
            {
                throw new AppCoreException("项目进度没有权限");
            }
            var link = input.MapTo<ProjectLink>();
            _linkRepository.Add(link);
            return link.Id;
        }

        public void Delete(int id)
        {
            var projectId = _ganttRepository.Get().Where(u => u.Links.Any(v=>v.Id == id)).Select(u => u.ProjectId).FirstOrDefault();
            var list = _projectRepository.Get().Where(u => u.Id == projectId).SelectMany(u => u.Members).Where(u => u.ProjectRole == "项目进度负责人").Select(u => u.UserId).ToList();
            if (_projectHelper.HasPermission("项目进度负责人", id))
            {
                throw new AppCoreException("项目进度没有权限");
            }
            _linkRepository.Delete(new ProjectLink{ Id = id });
        }

        public GetProjectLinkOutput Get(int id)
        {
            return _linkRepository.Get(id).MapTo<GetProjectLinkOutput>();
        }

        public void Update(UpdateProjectLinkInput input)
        {
            var projectId = _ganttRepository.Get().Where(u => u.Links.Any(v=>v.Id == input.Id)).Select(u => u.ProjectId).FirstOrDefault();
            if (_projectHelper.HasPermission("项目进度负责人", projectId))
            {
                throw new AppCoreException("项目进度没有权限");
            }
            var link = input.MapTo<ProjectLink>();
            _linkRepository.Update(link);
        }
    }
}
