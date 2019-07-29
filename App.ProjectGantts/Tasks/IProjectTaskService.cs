using App.Base;
using App.ProjectGantts.Tasks.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.ProjectGantts.Tasks
{
    public interface IProjectTaskService
    {
        int Add(AddProjectTaskInput input);
        void Update(UpdateProjectTaskInput input);
        void Delete(int id);
        GetProjectTaskOutput Get(int id);

        void CompleteTask(CompleteProjectTaskInput input);
    }
}
