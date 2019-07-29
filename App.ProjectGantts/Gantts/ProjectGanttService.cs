using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using App.Base.Repositories;
using App.Core.Common.Exceptions;
using App.ProjectGantts.Gantts.Dto;
using Microsoft.EntityFrameworkCore;
using PoorFff.Mapper;

namespace App.ProjectGantts.Gantts
{
    public class ProjectGanttService : IProjectGanttService
    {
        private IAppRepositoryBase<ProjectGantt> _taskRepository;
        public ProjectGanttService(IAppRepositoryBase<ProjectGantt> taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public int AddByProject(int projectId)
        {
            var gantt = new ProjectGantt { ProjectId = projectId };
            _taskRepository.Add(gantt);
            return gantt.Id;
        }

        public GetProjectGanttOutput Get(int id)
        {
            var data = _taskRepository.Get()
                .Include(u => u.Tasks).ThenInclude(u=>u.Attachments).ThenInclude(u=>u.FileMeta)
                .Include(u => u.Links)
                .Where(u => u.Id == id)
                .FirstOrDefault();
            data.Tasks = data.Tasks.OrderBy(u => u.SortNo).ToList();
            return data.MapTo<GetProjectGanttOutput>();
        }

        public GetProjectGanttOutput GetByProject(int projectId)
        {
            var data = _taskRepository.Get()
                .Include(u => u.Tasks).ThenInclude(u=>u.Attachments).ThenInclude(u=>u.FileMeta)
                .Include(u => u.Links)
                .Where(u => u.ProjectId == projectId)
                .FirstOrDefault();
            if (data == null)
                throw new AppCoreException("当前项目gantt未生成");
            data.Tasks = data.Tasks.OrderBy(u => u.SortNo).ToList();
            return data.MapTo<GetProjectGanttOutput>();
        }
    }
}
