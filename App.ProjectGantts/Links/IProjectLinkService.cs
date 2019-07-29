using App.ProjectGantts.Links.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.ProjectGantts.Links
{
    public interface IProjectLinkService
    {
        int Add(AddProjectLinkInput input);
        void Update(UpdateProjectLinkInput input);
        void Delete(int id);
        GetProjectLinkOutput Get(int id);
    }
}
