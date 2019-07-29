
using App.Projects.ProjectBaseInfos;
using App.Projects.ProjectBaseInfos.Dto;
using App.Statistics.ProjectStatistics.Dto;
using PoorFff.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Statistics
{
    public class StatisticsProfile : PfProfile
    {
        public StatisticsProfile()
        {
            base.CreateMap<Project, GetProjectWithProblemListOutput>()
                .ForMember(u => u.State, expr => expr.MapFrom(s => s.State.Name))
                .ForMember(u => u.Type, expr => expr.MapFrom(s => s.Type.Name))
                .ForMember(u => u.ProjectNature, expr => expr.MapFrom(s => s.ProjectNature.Name))
                .ForMember(u=>u.ProjectLeader, expr=>expr.MapFrom(s=>s.Members.Where(u=>u.ProjectRole == "项目负责人").Select(u=>u.UserId).FirstOrDefault()));
            base.CreateMap<Project, GetProjectStatisticsListOutput>()
                 .ForMember(u => u.State, expr => expr.MapFrom(s => s.State.Name))
                 .ForMember(u => u.Type, expr => expr.MapFrom(s => s.Type.Name))
                 .ForMember(u => u.ProjectNature, expr => expr.MapFrom(s => s.ProjectNature.Name))
                 .ForMember(u => u.ProjectLeader, expr => expr.MapFrom(s => s.Members.Where(u => u.ProjectRole == "项目负责人").Select(u => u.UserId).FirstOrDefault()));

        }
    }
}
