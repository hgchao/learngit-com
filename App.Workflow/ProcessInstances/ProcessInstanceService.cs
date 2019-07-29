using App.Core.Authorization;
using App.Core.Authorization.Accounts;
using App.Core.Authorization.Repositories;
using App.Core.Authorization.Users;
using App.Core.Common.Entities;
using App.Core.Common.Exceptions;
using App.Core.Form.FieldQueryConditions;
using App.Core.Form.Fields.Dto;
using App.Core.Workflow;
using App.Core.Workflow.Engine;
using App.Core.Workflow.ProcessInstances;
using App.Core.Workflow.ProcessInstances.Dto;
using App.Core.Workflow.Providers;
using App.Core.Workflow.Repositories;
using App.Core.Workflow.Variables;
using App.Workflow.ProcessInstances.Dto;
using PoorFff.PfExpression.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace App.Workflow.ProcessInstances
{
    public class ProcessInstanceService: IProcessInstanceService
    {
        private IAuthorizationRepositoryBase<User> _userRepository;
        private IWorkflowRepositoryBase<Wf_Hi_Variable> _varRepository;

        private IAuthInfoProvider _authInfoProvider;
        private IRuntimeProvider _runtimeProvider;
        private ITaskProvider _taskProvider;
        private IFormProvider _formProvider;

        public ProcessInstanceService(
            IAuthInfoProvider authInfoProvider,
            IWorkflowRepositoryBase<Wf_Hi_Variable> varRepository,
            IAuthorizationRepositoryBase<User> userRepository,
            IWfEngine wfEngine)
        {
            _varRepository = varRepository;
            _userRepository = userRepository;
            _authInfoProvider = authInfoProvider;
            _runtimeProvider = wfEngine.GetRuntimeProvider();
            _taskProvider = wfEngine.GetTaskProvider();
            _formProvider = wfEngine.GetFormProvider();
        }

        public PaginationData<GetProcessInstanceListOutput> Get(int pageIndex, int pageSize, 
            string keyword,
            int? CreatorId,
            DateTime? createTimeL, DateTime? createTimeR)
        {

            var userId = _authInfoProvider.GetCurrent()?.User?.Id ?? 0;
            var userCode = _userRepository.Get().Where(u => u.Id == userId).Select(u => u.Code).FirstOrDefault();
            var userIdList = _userRepository.Get().Where(u => u.Code.StartsWith(userCode)).Select(u => u.Id).ToList();
            Expression<Func<Wf_Hi_ProcessInstance, bool>> expression = u => userIdList.Contains(u.CreatorId);
            if (CreatorId != null)
            {
                Expression<Func<Wf_Hi_ProcessInstance, bool>> expression2 = u => u.CreatorId == CreatorId;
                expression = expression.AndAlso(expression2);
            }
            if (createTimeL != null)
            {
                Expression<Func<Wf_Hi_ProcessInstance, bool>> expression2 = u => u.CreateTime >= createTimeL;
                expression = expression.AndAlso(expression2);
            }
            if (createTimeR != null)
            {
                Expression<Func<Wf_Hi_ProcessInstance, bool>> expression2 = u => u.CreateTime <= createTimeR;
                expression = expression.AndAlso(expression2);
            }
            return _runtimeProvider.Get(pageIndex, pageSize, keyword, expression);
        }

        public GetProcessInstanceOutput Get(int id)
        {
            return _runtimeProvider.GetProcessInstance(id);
        }

        public List<FieldDto> GetFields(int id)
        {
            var userId = _authInfoProvider.GetCurrent()?.User?.Id ?? 0;
            var userCode = _userRepository.Get().Where(u => u.Id == userId).Select(u => u.Code).FirstOrDefault();
            var userIdList = _userRepository.Get().Where(u => u.Code.StartsWith(userCode)).Select(u => u.Id).ToList();
            return _runtimeProvider.GetFields(id, u => userIdList.Contains(u.CreatorId));
        }

        public PaginationData<GetProcessInstancePoorOutput> GetForFields(int pageIndex, int pageSize, string formName, List<string> needFields, List<FieldQueryCondition> fieldQueryConditions, string sort, bool isAsc = true)
        {
            var userId = _authInfoProvider.GetCurrent()?.User?.Id ?? 0;
            var userCode = _userRepository.Get().Where(u => u.Id == userId).Select(u => u.Code).FirstOrDefault();
            var userIdList = _userRepository.Get().Where(u => u.Code.StartsWith(userCode)).Select(u => u.Id).ToList();
            var privilegedPersonIdList = AuthorizationContext.Instance.GetPrivilegedPersonIds(formName);
            bool isPrivilegedPerson = privilegedPersonIdList.Contains(userId);

            return _runtimeProvider.GetForFields(
                pageIndex, 
                pageSize, 
                formName,
                needFields: needFields,
                fieldCondition: fieldQueryConditions,
                processCondition: (u => (userIdList.Contains(u.CreatorId) || isPrivilegedPerson) && u.Pid == null),
                sort: sort,
                isAsc: isAsc);
        }

        public void Restart(int id)
        {
            _runtimeProvider.RestartProcessInstance(id);
        }

        public void Start(StartProcessInput input, Action<int, Dictionary<string, object>> setVariables)
        {
            var variables = new Dictionary<string, object>();
            var userId = _authInfoProvider.GetCurrent().User.Id;
            variables.Add("starter", userId.ToString());
            setVariables?.Invoke(userId, variables);
            int processInstanceId;
            if (string.IsNullOrEmpty(input.ProcessName))
            {
                processInstanceId = _runtimeProvider.StartProcessInstanceByName(input.ProcessDefinitionName, variables, parentProcessId: input.ParentProcessId);
            }
            else
            {
                processInstanceId = _runtimeProvider.StartProcessInstanceByName(input.ProcessDefinitionName, input.ProcessName, variables, parentProcessId: input.ParentProcessId);
            }
            if (input.PreventCommit)
            {
                return;
            }
            var tasks = _taskProvider.GetByProcessInstance(processInstanceId);
            if (tasks.Count == 0)
            {
                throw new AppCoreException("任务未创建成功！");
            }
            else
            {
                var task = tasks[0];
                if (task.Assignee != userId.ToString())
                {
                    throw new AppCoreException($"id:为{userId}的用户不是任务【{task.NodeName}】的委托人");
                }
                if (input.FormContents.Count > 0)
                {
                    _taskProvider.SetFields(task.Id, input.FormContents);
                }
                _taskProvider.Complete(task.Id);
            }
        }

        public void Start(StartProcessInput input, int previousProcessId, bool autoExecuteFirstTask)
        {
            var variables = new Dictionary<string, object>();
            var userId = _authInfoProvider.GetCurrent().User.Id;
            var previousStarter = (string)_varRepository.Get()
                .Where(u => u.ProcessInstanceId == previousProcessId && u.Name == "starter").FirstOrDefault().GetValue();
            variables.Add("starter", previousStarter);
            int processInstanceId;
            if (string.IsNullOrEmpty(input.ProcessName))
            {
                processInstanceId = _runtimeProvider.StartProcessInstanceByName(input.ProcessDefinitionName, input.ProcessDefinitionName, Convert.ToInt32(previousStarter), variables, parentProcessId: input.ParentProcessId);
            }
            else
            {
                processInstanceId = _runtimeProvider.StartProcessInstanceByName(input.ProcessDefinitionName, input.ProcessName, Convert.ToInt32(previousStarter), variables, parentProcessId: input.ParentProcessId);
            }
            if (input.FormContents.Count > 0)
            {
                _runtimeProvider.SetFields(processInstanceId, input.FormContents);
            }
            if (input.PreventCommit)
            {
                return;
            }
            if (!autoExecuteFirstTask)
                return;
            var tasks = _taskProvider.GetByProcessInstance(processInstanceId);
            if (tasks.Count == 0)
            {
                throw new AppCoreException("任务未创建成功！");
            }
            else
            {
                var task = tasks[0];
                _taskProvider.Complete(task.Id);
            }
        }


        public void Suspend(int id)
        {
            _runtimeProvider.SuspendProcessInstance(id);
        }

        public void Terminate(int id)
        {
            _runtimeProvider.TerminateProcessInstance(id);
        }

    }
}
