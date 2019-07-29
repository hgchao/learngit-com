using App.Core.Authorization.Accounts;
using App.Core.Authorization.Repositories;
using App.Core.Authorization.Users;
using App.Core.Common.Entities;
using App.Core.Workflow;
using App.Core.Workflow.ProcessDefinitions;
using App.Core.Workflow.ProcessDefinitions.Dto;
using App.Core.Workflow.Providers;
using App.Core.Workflow.Repositories;
using App.Base.EntityFramework;
using App.Base.Repositories;
using PoorFff.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using App.Core.Common;

namespace App.Workflow.ProcessDefinitions
{
    public class ProcessDefinitionService: IProcessDefinitionService
    {
        private IDefinitionProvider _definitionProvider;
        private IAppDbContextProvider _dbContextProvider;
        private IAuthorizationRepositoryBase<User> _userRepository;
        private IAppRepositoryBase<RoleProcessDefinition> _roleProcessDefinitionRepository;
        private IWorkflowRepositoryBase<Wf_Re_ProcessDefinition> _processDefinitionRepository;
        private IAuthInfoProvider _authInfoProvider;
        public ProcessDefinitionService(IWfEngine wfEngine,
            IAppDbContextProvider dbContextProvider,
            IAppRepositoryBase<RoleProcessDefinition> roleProcessDefinitionRepository,
            IWorkflowRepositoryBase<Wf_Re_ProcessDefinition> processDefinitionRepository,
            IAuthorizationRepositoryBase<User> userRepository,
            IAuthInfoProvider authInfoProvider
            )
        {
            _userRepository = userRepository;
            _authInfoProvider = authInfoProvider;
            _definitionProvider = wfEngine.GetDefinitionProvider();
            _dbContextProvider = dbContextProvider;
            _roleProcessDefinitionRepository = roleProcessDefinitionRepository;
            _processDefinitionRepository = processDefinitionRepository;
        }
        public int Add(Dto.ProcessDefinitionWithRole input)
        {
            var deletedRoleProcessDefinition = _roleProcessDefinitionRepository.Get().Where(u => u.ProcessDefinition.Name == input.ProcessDefinition.Name).ToList();
            int id = 0;
            using (var transaction = _dbContextProvider.BeginTransaction())
            {
                id = _definitionProvider.DeployNewProcessDefinition(input.ProcessDefinition);
                var roleProcessDefinitions = input.RoleIds.Select(u => new RoleProcessDefinition
                {
                    ProcessDefinitionId = id,
                    RoleId = u
                }).ToList();

                _roleProcessDefinitionRepository.BatchDelete(deletedRoleProcessDefinition);
                _roleProcessDefinitionRepository.BatchAdd(roleProcessDefinitions);
                transaction.Commit();
            }
            return id;
        }

        public PaginationData<GetProcessDefinitionListOutput> Get(int pageIndex, int pageSize, string keyword, Expression<Func<Wf_Re_ProcessDefinition, bool>> extraCondition)
        {
            var userId = _authInfoProvider.GetCurrent().User.Id;
            var queryForUserRole = _userRepository.Get().Where(u => u.Id == userId).SelectMany(u => u.UserRoles);
            var queryForRolePf = _roleProcessDefinitionRepository.Get().Where(u => queryForUserRole.Any(v => v.RoleId == u.RoleId));
            var query = _processDefinitionRepository.Get().Where(u => u.CreatorId == userId || queryForRolePf.Any(v => v.ProcessDefinitionId == u.Id));
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(u => u.Name.Contains(keyword) || u.Category.Contains(keyword) || u.Description.Contains(keyword));
            }
            if (extraCondition != null)
            {
                query = query.Where(extraCondition);
            }
            query = query.GroupBy(u => u.Name).Select(u => u.OrderByDescending(v => v.Version).First());
            var queryForRolePfAll = _roleProcessDefinitionRepository.Get();
            var queryJoinRole = query.GroupJoin(queryForRolePfAll, u => u.Id, v => v.ProcessDefinitionId, (u, v) => new ProcessDefinitionWithRole { ProcessDefinition = u, RoleNames = v.Select(w => w.Role.Name) });
            return PaginationDataHelper.WrapData(queryJoinRole, pageIndex, pageSize, u => u.ProcessDefinition.CreateTime).TransferTo<GetProcessDefinitionListOutput>(
                u =>
                {
                    var output = u.ProcessDefinition.MapTo<GetProcessDefinitionListOutput>();
                    output.Remark = string.Join(",", u.RoleNames);
                    return output;
                }
                );
        }
        private class ProcessDefinitionWithRole
        {
            public Wf_Re_ProcessDefinition ProcessDefinition { get; set; }
            public IEnumerable<string> RoleNames { get; set; }
        }

        public List<string> GetNameByUser(int userId)
        {
            var queryForUserRole = _userRepository.Get().Where(u => u.Id == userId).SelectMany(u => u.UserRoles);
            var query = _roleProcessDefinitionRepository.Get().Where(u => queryForUserRole.Any(v => v.RoleId == u.RoleId)).Select(u => u.ProcessDefinition);
            var query2 = _processDefinitionRepository.Get().Where(u => u.CreatorId == userId);
            query = query.Union(query2).Distinct();
            return query.Select(u => u.Name).Distinct().ToList();
        }

        public List<int> GetIdByProcess(int processId)
        {
            return _roleProcessDefinitionRepository.Get().Where(u => u.ProcessDefinitionId == processId).Select(u => u.RoleId).Distinct().ToList();
        }

    }
}
