using App.Base;
using App.Base.EntityFramework;
using App.Base.Repositories;
using App.Core.Authorization.Accounts;
using App.Core.Authorization.Repositories;
using App.Core.Authorization.Users;
using App.Core.Common;
using App.Core.Common.Entities;
using App.Core.Common.Exceptions;
using App.Core.Messaging;
using App.Core.Workflow;
using App.Core.Workflow.Engine.Definition.Nodes.Tasks;
using App.Core.Workflow.Providers;
using App.Projects.ProjectBaseInfos.Dto;
using App.Projects.ProjectLocations;
using App.Projects.Projects;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Oa.Project;
using PoorFff.Mapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PoorFff.Collection;
using System.Text;
using App.Projects.ProjectUnits;
using System.Linq.Expressions;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using App.Core.Authorization;

namespace App.Projects.ProjectBaseInfos
{
    public class ProjectService : IProjectService
    {
        private IAuthInfoProvider _authInfoProvider;
        private IHistoryProvider _historyProvider;
        private IRuntimeProvider _runtimeProvider;
        private ITaskProvider _taskProvider;
        private IDefinitionProvider _definitionProvder;
        private IAuthorizationRepositoryBase<User> _userRepository;
        private IAuthorizationRepositoryBase<UserUnit> _userUnitRepository;
        private IAppDbContextProvider _dbContextProvider;
        private IMessagingProvider _messagingProvider;
        private IAppRepositoryBase<Project> _projectRepository;
        private IAppRepositoryBase<ProjectUnit> _projectUnitRepository;
        private IProjectHelper _projectHelper;
        public ProjectService(
            IAppRepositoryBase<Project> projectRepository,
            IAppRepositoryBase<ProjectUnit> projectUnitRepository,
            IAuthInfoProvider authInfoProvider,
            IWfEngine wfEngine,
            IAuthorizationRepositoryBase<User> userRepository,
            IAuthorizationRepositoryBase<UserUnit> userUnitRepository,
            IAppDbContextProvider dbContextProvider,
            IMessagingProvider messagingProvider,
            IProjectHelper projectHelper
            )
        {
            _projectHelper = projectHelper;
            _messagingProvider = messagingProvider;
            _authInfoProvider = authInfoProvider;
            _userRepository = userRepository;
            _userUnitRepository = userUnitRepository;
            _runtimeProvider = wfEngine.GetRuntimeProvider();
            _taskProvider = wfEngine.GetTaskProvider();
            _definitionProvder = wfEngine.GetDefinitionProvider();
            _historyProvider = wfEngine.GetHistoryProvider();
            _dbContextProvider = dbContextProvider;
            _projectRepository = projectRepository;
            _projectUnitRepository = projectUnitRepository;
        }

        public int Add(StartProcessInput<AddProjectInput> input)
        {
            NullableHelper.SetNull(input.Data);
            var variables = new Dictionary<string, object>();
            var userId = _authInfoProvider.GetCurrent().User.Id;
            variables.Add("starter", userId.ToString());
            int processInstanceId = _runtimeProvider.StartProcessInstanceByName(
                input.ProcessDefinitionName,
                $"{input.ProcessDefinitionName}-{input.Data.Name}",
                variables);
            var tasks = _taskProvider.GetByProcessInstance(processInstanceId);
            if (tasks.Count == 0)
            {
                throw new AppCoreException("任务未创建成功！");
            }
            var project = input.Data.MapTo<Project>();
            var count = ProjectContext.Instance.GetProjectBaseInfoNoCount(() => _projectRepository.Count(u => u.CreateTime.Value.Year == DateTime.Now.Year && (u.DataState == DataState.Stable || u.DataState == DataState.Creating)));
            project.ProcessInstanceId = processInstanceId;
            project.DataState = DataState.Creating;
            project.No = $"{DateTime.Now.Year}{count.ToString("0000")}";
            project.Mid = null;
            _projectRepository.Add(project);
            var task = tasks[0];
            if(!input.PreventCommit)
                _taskProvider.Complete(task.Id);
            return project.Id;
        }

        public void CompleteProcess(int processInstanceId)
        {
            var project = _projectRepository.Get()
                .Include(u=>u.Location)
                .Include(u=>u.Members)
                .Include(u=>u.Units).ThenInclude(u=>u.Members)
                .Include(u=>u.Attachments)
                .Where(u => u.ProcessInstanceId == processInstanceId).FirstOrDefault();
            if (project == null)
            {
                throw new EntityException("ProjectInstanceId", processInstanceId, "Project", "不存在");
            }
            if (project.DataState == DataState.Creating)
            {
                //project.State = DataState.Stable;
                //_projectBaseInfoRepository.Update(project, new System.Linq.Expressions.Expression<Func<ProjectBaseInfo, object>>[] {
                //    u => u.State
                //}, true);
                //using (var transaction = _dbContextProvider.BeginTransaction())
                //{

                    var temp = JsonConvert.DeserializeObject<Project>(JsonConvert.SerializeObject(project));
                    temp.DataState = DataState.Stable;
                    temp.ProcessInstanceId = null;
                    _projectRepository.Update(temp, new System.Linq.Expressions.Expression<Func<Project, object>>[] {
                        u => u.DataState,
                        u => u.ProcessInstanceId
                    }, true);

                    temp = JsonConvert.DeserializeObject<Project>(JsonConvert.SerializeObject(project));
                    temp.Attachments.ForEach(u => u.Id = 0);
                    temp.Members.ForEach(u => u.Id = 0);
                    temp.Units.ForEach(u =>
                    {
                        u.Id = 0;
                        u.Members.ForEach(v => v.Id = 0);
                    });
                    if (temp.Location != null)
                    {
                        temp.Location.Id = 0;
                        temp.Location.ProjectId = 0;
                    }
                    temp.DataState = DataState.Created;
                    temp.Id = 0;
                    temp.Mid = project.Id;
                    _projectRepository.Add(temp);
                //    transaction.Commit();
                //}
            }
            else if (project.Mid != null && project.DataState == DataState.Updating)
            {
                using (var transaction = _dbContextProvider.BeginTransaction())
                {
                    var temp = JsonConvert.DeserializeObject<Project>(JsonConvert.SerializeObject(project));
                    temp.DataState = DataState.Updated;
                    _projectRepository.Update(temp, new System.Linq.Expressions.Expression<Func<Project, object>>[] {
                        u=>u.DataState
                    }, true);
                    temp = JsonConvert.DeserializeObject<Project>(JsonConvert.SerializeObject(project));
                    temp.Id = project.Mid.Value;
                    temp.Mid = null;
                    temp.Attachments.ForEach(u=>u.Id = 0);
                    temp.Members.ForEach(u=>u.Id = 0);
                    if (temp.Location != null)
                    {
                        temp.Location.Id = 0;
                    }
                    temp.Units.ForEach(u=> {
                        u.Id = 0;
                        u.Members.ForEach(v=>v.Id = 0);
                    });
                    var existing = _projectRepository.Get()
                        .Include(u=>u.Location)
                        .Include(u=>u.Members)
                        .Include(u=>u.Attachments)
                        .Include(u=>u.Units).ThenInclude(u=>u.Members)
                        .Where(u => u.Id == temp.Id).FirstOrDefault();
                    _projectRepository.Update(temp, existing, new System.Linq.Expressions.Expression<Func<Project, object>>[] {
                        u=>u.ProcessInstanceId,
                        u=>u.DataState,
                        u=>u.No,
                        u=>u.Units
                    }, false);
                    var addedUnits = temp.Units.Except(existing.Units, u => u.Id).ToList();
                    var deletedUnits = existing.Units.Except(temp.Units, u => u.Id).ToList();
                    var updatedUnits = temp.Units.Intersect(existing.Units, u => u.Id).ToList();
                    foreach(var added in addedUnits)
                    {
                        _projectUnitRepository.Add(added);
                    }
                    foreach(var deleted in deletedUnits)
                    {
                        _projectUnitRepository.Delete(deleted);
                    }
                    foreach(var updated in updatedUnits)
                    {
                        var existed = existing.Units.Where(u => u.Id == updated.Id).FirstOrDefault();
                        _projectUnitRepository.Update(updated, existed, new Expression<Func<ProjectUnit, object>>[] { }, false);
                    }
                    transaction.Commit();
                }
            }
            else
            {

                throw new AppCoreException("当前数据不再更新状态");
            }
        }

        public void CompleteTask(CompleteTaskInput<UpdateProjectInput> input)
        {
            NullableHelper.SetNull(input.Data);
            var authInfo = _authInfoProvider.GetCurrent();
            var userId = authInfo?.User?.Id;
            var task = _taskProvider.GetTask(input.Id);
            if (task.Assignee != userId.Value.ToString())
            {
                throw new AppCoreException($"id:为{userId.Value}的用户不是任务【{task.NodeName}】的委托人");
            }
            var processDefinition = _definitionProvder.Get(task.ProcessDefinitionId);
            var editableFields = ((UserTaskNode)processDefinition.GetNode(task.NodeUid)).GetEditableFields();
            if (input.Data != null)
            {
                var project = input.Data.MapTo<Project>();
                var existing = _projectRepository.Get()
                    .Include(u=>u.Location)
                    .Include(u=>u.Members)
                    .Include(u=>u.Units).ThenInclude(u=>u.Members)
                    .Include(u=>u.Attachments)
                    .Where(u => u.ProcessInstanceId == task.ProcessInstanceId).FirstOrDefault();
                project.Id = existing.Id;
                if (existing == null)
                {
                    throw new EntityException("ProjectInstanceId", task.ProcessInstanceId, "Project", "不存在");
                }
                if (editableFields == null)
                {
                    using(var transaction = _dbContextProvider.BeginTransaction())
                    {
                        if(project.Location != null && project.Location.IsTransient())
                        {
                            project.Location.ProjectId = project.Id;
                        }
                        _projectRepository.Update(project, existing, new System.Linq.Expressions.Expression<Func<Project, object>>[] {
                            u => u.DataState,
                            u=>u.Mid,
                            u=>u.ProcessInstanceId,
                            u=>u.Units,
                            u => u.No
                        }, false);
                        var addedUnits = project.Units.Except(existing.Units, u => u.Id).ToList();
                        var deletedUnits = existing.Units.Except(project.Units, u => u.Id).ToList();
                        var updatedUnits = project.Units.Intersect(existing.Units, u => u.Id).ToList();
                        foreach(var added in addedUnits)
                        {
                            _projectUnitRepository.Add(added);
                        }
                        foreach(var deleted in deletedUnits)
                        {
                            _projectUnitRepository.Delete(deleted);
                        }
                        foreach(var updated in updatedUnits)
                        {
                            var existed = existing.Units.Where(u => u.Id == updated.Id).FirstOrDefault();
                            _projectUnitRepository.Update(updated, existed, new Expression<Func<ProjectUnit, object>>[] { }, false);
                        }
                        transaction.Commit();
                    }
                }
                else
                {
                    if (editableFields.Contains("Units"))
                    {
                        var addedUnits = project.Units.Except(existing.Units, u => u.Id).ToList();
                        var deletedUnits = existing.Units.Except(project.Units, u => u.Id).ToList();
                        var updatedUnits = project.Units.Intersect(existing.Units, u => u.Id).ToList();
                        foreach(var added in addedUnits)
                        {
                            _projectUnitRepository.Add(added);
                        }
                        foreach(var deleted in deletedUnits)
                        {
                            _projectUnitRepository.Delete(deleted);
                        }
                        foreach(var updated in updatedUnits)
                        {
                            var existed = existing.Units.Where(u => u.Id == updated.Id).FirstOrDefault();
                            _projectUnitRepository.Update(updated, existed, new Expression<Func<ProjectUnit, object>>[] { }, false);
                        }
                        editableFields.Remove("Units");
                    }
                    _projectRepository.Update(project, editableFields.ToArray());
                }
            }
            if (!input.PreventCommit)
            {
                _taskProvider.Complete(input.Id, input.Comment);
            }
        }

        public int Update(StartProcessInput<UpdateProjectInput> input)
        {

            //if (!_projectHelper.HasPermission("项目负责人", input.Data.Id))
            //{
            //    throw new AppCoreException("项目没有权限");
            //}
            if(_authInfoProvider.GetCurrent().User.Id != _projectRepository.Get().Where(u=>u.Id == input.Data.Id).Select(u => u.CreatorId).FirstOrDefault())
            {
                throw new AppCoreException("项目没有权限");
            }

            NullableHelper.SetNull(input.Data);
            var variables = new Dictionary<string, object>();
            var userId = _authInfoProvider.GetCurrent().User.Id;


            variables.Add("starter", userId.ToString());
            int processInstanceId = _runtimeProvider.StartProcessInstanceByName(
                input.ProcessDefinitionName,
                $"{input.ProcessDefinitionName}-{input.Data.Name}",
                variables);
            var tasks = _taskProvider.GetByProcessInstance(processInstanceId);
            if (tasks.Count == 0)
            {
                throw new AppCoreException("任务未创建成功！");
            }
            var project = input.Data.MapTo<Project>();
            var projectNo = _projectRepository.Get().Where(u => u.Id == project.Id).Select(u => u.No).FirstOrDefault();
            project.ProcessInstanceId = processInstanceId;
            project.No = projectNo;
            project.Attachments.ForEach(u=>u.Id = 0);
            project.Members.ForEach(u=>u.Id = 0);
            project.Units.ForEach(u=> {
                u.Id = 0;
                u.Members.ForEach(v=>v.Id = 0);
            });
            if (project.Location != null)
            {
                project.Location.Id = 0;
                project.Location.ProjectId = 0;
            }
            project.DataState = DataState.Updating;
            project.Mid = project.Id;
            project.Id = 0;
            _projectRepository.Add(project);
            var task = tasks[0];
            _taskProvider.Complete(task.Id);
            return project.Id;
        }

        public void Delete(int id)
        {
            //if (!_projectHelper.HasPermission("项目负责人", id))
            //{
            //    throw new AppCoreException("项目没有权限");
            //}

            if(_authInfoProvider.GetCurrent().User.Id != _projectRepository.Get().Where(u=>u.Id == id).Select(u => u.CreatorId).FirstOrDefault())
            {
                throw new AppCoreException("项目没有权限");
            }
            _projectRepository.Delete(new Project { Id = id });
        }

        public MemoryStream Export(int pageIndex, int pageSize, string keyword)
        {
            throw new NotImplementedException();
        }

        public GetProjectOutput Get(int id)
        {
            var project = _projectRepository.Get()
                .Include(u=>u.Location)
                .Include(u=>u.Members)
                .Include(u=>u.Units).ThenInclude(u=>u.Members)
                .Include(u=>u.Attachments).ThenInclude(u=>u.FileMeta)
                .Where(u => u.Id == id).FirstOrDefault();
            if (project == null)
            {
                throw new EntityException("Id", id, "Project", "不存在");
            }
            return project.MapTo<GetProjectOutput>();
        }

        public PaginationData<GetProjectListOutput> Get(int pageIndex, int pageSize, string keyword,
            string address,
            int? typeId,
            string Name,
            string No,
            int? projectNatureId,
            int? stateId,
            int? projectLeaderId,
            int? generalEstimateL, int? generalEstimateR,
            DateTime? commencementDateL, DateTime? commencementDateR,
            string sortField,
            string sortState
            )
        {
            var currentUserId = _authInfoProvider.GetCurrent().User.Id;
            var currentUnitUserIds = _userUnitRepository.Get()
                .Where(v =>_userUnitRepository.Get().Where(u => u.UserId == currentUserId).Any(u => u.OrganizationUnitId == v.OrganizationUnitId)).Select(u => u.UserId).ToList();
            var privilegedPersonIds = AuthorizationContext.Instance.GetPrivilegedPersonIds("项目信息");

            IQueryable<Project> query = _projectRepository.Get()
                .Include(u=>u.Location)
                .Include(u => u.State)
                .Include(u => u.Type)
                .Include(u => u.ProjectNature)
                .Include(u=>u.Members)
                .Where(u=>u.DataState == DataState.Stable 
                && (u.Members.Any(v=>v.UserId == currentUserId) || u.CreatorId == currentUserId || currentUnitUserIds.Contains(u.CreatorId) || privilegedPersonIds.Contains(currentUserId))
                );
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(u=>u.Name.Contains(keyword) || u.No.Contains(keyword));
            }
            if (!string.IsNullOrEmpty(address))
            {
                query = query.Where(u=>(string.Concat(u.Location.Province, u.Location.City, u.Location.District, u.Location.Town, u.Location.Street, u.Location.AddressDetail).Contains(address)));
            }
            if (!string.IsNullOrEmpty(No))
            {
                query = query.Where(u=>u.No.Contains(No));
            }
            if (!string.IsNullOrEmpty(Name))
            {
                query = query.Where(u=>u.Name.Contains(Name));
            }
            if(stateId != null)
            {
                query = query.Where(u => u.StateId == stateId);
            }
            if(typeId != null)
            {
                query = query.Where(u => u.TypeId == typeId);
            }
            if(projectNatureId != null)
            {
                query = query.Where(u => u.ProjectNatureId == projectNatureId);
            }
            if(projectLeaderId != null)
            {
                query = query.Where(u => u.Members.Any(v=>v.ProjectRole=="项目负责人" && v.UserId == projectLeaderId));
            }
            if(generalEstimateL != null)
            {
                query = query.Where(u => u.GeneralEstimate >= generalEstimateL);
            }
            if(generalEstimateR != null)
            {
                query = query.Where(u => u.GeneralEstimate <= generalEstimateR);
            }
            if(commencementDateL != null)
            {
                query = query.Where(u => u.CommencementDate >= commencementDateL);
            }
            if(commencementDateR != null)
            {
                query = query.Where(u => u.CommencementDate <= commencementDateR);
            }
            //sortState 1降序2升序0默认
            if (!string.IsNullOrEmpty(sortField) && !string.IsNullOrEmpty(sortState))
            {
                switch (sortField)
                {
                    case "No"://项目编号
                        query = sortState == "1" ? query.OrderByDescending(u => u.No) : sortState == "2" ? query.OrderBy(u => u.No) : query.OrderByDescending(u => u.CreateTime);
                        break;
                    case "Name"://项目名称
                        query = sortState == "1" ? query.OrderByDescending(u => u.Name) : sortState == "2" ? query.OrderBy(u => u.Name) : query.OrderByDescending(u => u.CreateTime);
                        break;
                    case "TypeId"://项目类型
                        query = sortState == "1" ? query.OrderByDescending(u => u.TypeId) : sortState == "2" ? query.OrderBy(u => u.TypeId) : query.OrderByDescending(u => u.CreateTime);
                        break;
                    case "ProjectNatureId"://项目性质
                        query = sortState == "1" ? query.OrderByDescending(u => u.ProjectNatureId) : sortState == "2" ? query.OrderBy(u => u.ProjectNatureId) : query.OrderByDescending(u => u.CreateTime);
                        break;
                    case "StateId"://项目状态
                        query = sortState == "1" ? query.OrderByDescending(u => u.StateId) : sortState == "2" ? query.OrderBy(u => u.StateId) : query.OrderByDescending(u => u.CreateTime);
                        break;
                    case "GeneralEstimate"://项目概算
                        query = sortState == "1" ? query.OrderByDescending(u => u.GeneralEstimate) : sortState == "2" ? query.OrderBy(u => u.GeneralEstimate) : query.OrderByDescending(u => u.CreateTime);
                        break;
                    case "CommencementDate"://开工时间
                        query = sortState == "1" ? query.OrderByDescending(u => u.CommencementDate) : sortState == "2" ? query.OrderBy(u => u.CommencementDate) : query.OrderByDescending(u => u.CreateTime);
                        break;
                    case "CreatorId"://发布人
                        query = sortState == "1" ? query.OrderByDescending(u => u.CreatorId) : sortState == "2" ? query.OrderBy(u => u.CreatorId) : query.OrderByDescending(u => u.CreateTime);
                        break;
                    case "UserId"://项目负责人
                        query = sortState == "1" ? query.OrderByDescending(u => u.Members.Any(v => v.ProjectRole == "项目负责人" && v.UserId == projectLeaderId)) : sortState == "2" ? query.OrderBy(u => u.Members.Any(v => v.ProjectRole == "项目负责人" && v.UserId == projectLeaderId)) : query.OrderByDescending(u => u.CreateTime);
                        break;
                    default:
                        query = query.OrderByDescending(u => u.CreateTime);
                        break;
                }

            }
            else
            {
                query = query.OrderByDescending(u => u.CreateTime);
            }
            var paging = PaginationDataHelper.WrapData<Project, T>(query, pageIndex, pageSize).TransferTo<GetProjectListOutput>();
            //paging.Data.ForEach(u=>u.HasPermission = _projectHelper.HasPermission("项目负责人", u.Id));
            paging.Data.ForEach(u=>u.HasPermission = u.CreatorId == _authInfoProvider.GetCurrent().User.Id);
            return paging;
        }
        public List<GetProjectListOutput> Get(string keyword,
            string address,
            int? typeId,
            string Name,
            string No,
            int? projectNatureId,
            int? stateId,
            int? projectLeaderId,
            int? generalEstimateL, int? generalEstimateR,
            DateTime? commencementDateL, DateTime? commencementDateR
            )
        {
            var currentUserId = _authInfoProvider.GetCurrent().User.Id;
            var currentUnitUserIds = _userUnitRepository.Get()
                .Where(v =>_userUnitRepository.Get().Where(u => u.UserId == currentUserId).Any(u => u.OrganizationUnitId == v.OrganizationUnitId)).Select(u => u.UserId).ToList();
            var privilegedPersonIds = AuthorizationContext.Instance.GetPrivilegedPersonIds("项目信息");
            IQueryable<Project> query = _projectRepository.Get()
                .Include(u=>u.Location)
                .Include(u => u.State)
                .Include(u => u.Type)
                .Include(u => u.ProjectNature)
                .Include(u=>u.Members)
                .Where(u=>u.DataState == DataState.Stable
                && (u.Members.Any(v=>v.UserId == currentUserId) || u.CreatorId == currentUserId || currentUnitUserIds.Contains(u.CreatorId) || privilegedPersonIds.Contains(currentUserId))
                );
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(u=>u.Name.Contains(keyword) || u.No.Contains(keyword));
            }
            if (!string.IsNullOrEmpty(address))
            {
                query = query.Where(u=>(string.Concat(u.Location.Province, u.Location.City, u.Location.District, u.Location.Town, u.Location.Street, u.Location.AddressDetail).Contains(address)));
            }
            if (!string.IsNullOrEmpty(No))
            {
                query = query.Where(u=>u.No.Contains(No));
            }
            if (!string.IsNullOrEmpty(Name))
            {
                query = query.Where(u=>u.Name.Contains(Name));
            }
            if(stateId != null)
            {
                query = query.Where(u => u.StateId == stateId);
            }
            if(typeId != null)
            {
                query = query.Where(u => u.TypeId == typeId);
            }
            if(projectNatureId != null)
            {
                query = query.Where(u => u.ProjectNatureId == projectNatureId);
            }
            if(projectLeaderId != null)
            {
                query = query.Where(u => u.Members.Any(v=>v.ProjectRole=="项目负责人" && v.UserId == projectLeaderId));
            }
            if(generalEstimateL != null)
            {
                query = query.Where(u => u.GeneralEstimate >= generalEstimateL);
            }
            if(generalEstimateR != null)
            {
                query = query.Where(u => u.GeneralEstimate <= generalEstimateR);
            }
            if(commencementDateL != null)
            {
                query = query.Where(u => u.CommencementDate >= commencementDateL);
            }
            if(commencementDateR != null)
            {
                query = query.Where(u => u.CommencementDate <= commencementDateR);
            }
            var data = query.ToList();
            return  data.MapToList<GetProjectListOutput>();
        }

        public GetProjectOutput GetByProcess(int processId)
        {
            var project = _projectRepository.Get()
                .Include(u=>u.Location)
                .Include(u=>u.Members)
                .Include(u=>u.Attachments).ThenInclude(u=>u.FileMeta)
                .Include(u=>u.Units).ThenInclude(u=>u.Members)
                .Where(u => u.ProcessInstanceId == processId).FirstOrDefault();
            if (project == null)
            {
                throw new EntityException("ProcessInstanceId", processId, "Project", "不存在");
            }
            return project.MapTo<GetProjectOutput>();
        }

        public GetProjectOutput GetByTask(int taskId)
        {
            var authInfo = _authInfoProvider.GetCurrent();
            var userId = authInfo?.User?.Id;
            var task = _taskProvider.GetTask(taskId);
            if (task.Assignee != userId.Value.ToString())
            {
                throw new AppCoreException($"id:为{userId.Value}的用户不是任务【{task.NodeName}】的委托人");
            }
            var project = _projectRepository.Get()
                .Include(u=>u.Location)
                .Include(u=>u.Members)
                .Include(u=>u.Units).ThenInclude(u=>u.Members)
                .Include(u=>u.Attachments).ThenInclude(u=>u.FileMeta)
                .Where(u => u.ProcessInstanceId == task.ProcessInstanceId).FirstOrDefault();
            if (project == null)
            {
                throw new EntityException("ProcessInstanceId", task.ProcessInstanceId, "Project", "不存在");
            }
            return project.MapTo<GetProjectOutput>();
        }

        public GetProjectOutput GetByTaskHistory(int taskHistoryId)
        {
            var authInfo = _authInfoProvider.GetCurrent();
            var userId = authInfo?.User?.Id;
            var task = _historyProvider.GetTaskHistory(taskHistoryId);
            if (!task.AssigneeList.Contains($"{userId}"))
            {
                throw new AppCoreException($"id:为{userId.Value}的用户不是任务【{task.NodeName}】的委托人");
            }
            var project = _projectRepository.Get()
                .Include(u=>u.Location)
                .Include(u=>u.Members)
                .Include(u=>u.Units).ThenInclude(u=>u.Members)
                .Include(u=>u.Attachments).ThenInclude(u=>u.FileMeta)
                .Where(u => u.ProcessInstanceId == task.ProcessInstanceId).FirstOrDefault();
            if (project == null)
            {
                throw new EntityException("ProcessInstanceId", task.ProcessInstanceId, "Project", "不存在");
            }
            return project.MapTo<GetProjectOutput>();
        }
        public GetProjectStatisticsOutput GetTotalCount(string year)
        {
            var projectList = _projectRepository.Get().Select(u => new { u.DataState,u.TypeId, u.CreateTime }).Where(u => u.DataState == DataState.Stable && u.CreateTime.Value.Year == Convert.ToInt32(year)).ToList();
            var January = projectList.Count(u => u.CreateTime.Value.Month == 1 && u.TypeId == 530);
            var February = projectList.Count(u => u.CreateTime.Value.Month == 2 && u.TypeId == 530);
            var March = projectList.Count(u => u.CreateTime.Value.Month == 3 && u.TypeId == 530);
            var April = projectList.Count(u => u.CreateTime.Value.Month == 4 && u.TypeId == 530);
            var May = projectList.Count(u => u.CreateTime.Value.Month == 5 && u.TypeId == 530);
            var June = projectList.Count(u => u.CreateTime.Value.Month == 6 && u.TypeId == 530);
            var July = projectList.Count(u => u.CreateTime.Value.Month == 7 && u.TypeId == 530);
            var August = projectList.Count(u => u.CreateTime.Value.Month == 8 && u.TypeId == 530);
            var September = projectList.Count(u => u.CreateTime.Value.Month == 9 && u.TypeId == 530);
            var October = projectList.Count(u => u.CreateTime.Value.Month == 10 && u.TypeId == 530);
            var November = projectList.Count(u => u.CreateTime.Value.Month == 11 && u.TypeId == 530);
            var December = projectList.Count(u => u.CreateTime.Value.Month == 12 && u.TypeId == 530);
            var projectHousing = January + "," + February + "," + March + "," + April + "," + May + "," + June + "," + July + "," + August + "," + September + "," + October + "," + November + "," + December;

            var JanuaryRemould = projectList.Count(u => u.CreateTime.Value.Month == 1 && u.TypeId == 531);
            var FebruaryRemould = projectList.Count(u => u.CreateTime.Value.Month == 2 && u.TypeId == 531);
            var MarchRemould = projectList.Count(u => u.CreateTime.Value.Month == 3 && u.TypeId == 531);
            var AprilRemould = projectList.Count(u => u.CreateTime.Value.Month == 4 && u.TypeId == 531);
            var MayRemould = projectList.Count(u => u.CreateTime.Value.Month == 5 && u.TypeId == 531);
            var JuneRemould = projectList.Count(u => u.CreateTime.Value.Month == 6 && u.TypeId == 531);
            var JulyRemould = projectList.Count(u => u.CreateTime.Value.Month == 7 && u.TypeId == 531);
            var AugustRemould = projectList.Count(u => u.CreateTime.Value.Month == 8 && u.TypeId == 531);
            var SeptemberRemould = projectList.Count(u => u.CreateTime.Value.Month == 9 && u.TypeId == 531);
            var OctoberRemould = projectList.Count(u => u.CreateTime.Value.Month == 10 && u.TypeId == 531);
            var NovemberRemould = projectList.Count(u => u.CreateTime.Value.Month == 11 && u.TypeId == 531);
            var DecemberRemould = projectList.Count(u => u.CreateTime.Value.Month == 12 && u.TypeId == 531);
            var projectMunicipal = JanuaryRemould + "," + FebruaryRemould + "," + MarchRemould + "," + AprilRemould + "," + MayRemould + "," + JuneRemould + "," + JulyRemould + "," + AugustRemould + "," + SeptemberRemould + "," + OctoberRemould + "," + NovemberRemould + "," + DecemberRemould;

            var JanuaryExtend = projectList.Count(u => u.CreateTime.Value.Month == 1 && u.TypeId == 532);
            var FebruaryExtend = projectList.Count(u => u.CreateTime.Value.Month == 2 && u.TypeId == 532);
            var MarchExtend = projectList.Count(u => u.CreateTime.Value.Month == 3 && u.TypeId == 532);
            var AprilExtend = projectList.Count(u => u.CreateTime.Value.Month == 4 && u.TypeId == 532);
            var MayExtend = projectList.Count(u => u.CreateTime.Value.Month == 5 && u.TypeId == 532);
            var JuneExtend = projectList.Count(u => u.CreateTime.Value.Month == 6 && u.TypeId == 532);
            var JulyExtend = projectList.Count(u => u.CreateTime.Value.Month == 7 && u.TypeId == 532);
            var AugustExtend = projectList.Count(u => u.CreateTime.Value.Month == 8 && u.TypeId == 532);
            var SeptemberExtend = projectList.Count(u => u.CreateTime.Value.Month == 9 && u.TypeId == 532);
            var OctoberExtend = projectList.Count(u => u.CreateTime.Value.Month == 10 && u.TypeId == 532);
            var NovemberExtend = projectList.Count(u => u.CreateTime.Value.Month == 11 && u.TypeId == 532);
            var DecemberExtend = projectList.Count(u => u.CreateTime.Value.Month == 12 && u.TypeId == 532);
            var projectGreening = JanuaryExtend + "," + FebruaryExtend + "," + MarchExtend + "," + AprilExtend + "," + MayExtend + "," + JuneExtend + "," + JulyExtend + "," + AugustExtend + "," + SeptemberExtend + "," + OctoberExtend + "," + NovemberExtend + "," + DecemberExtend;

            var JanuaryContinued = projectList.Count(u => u.CreateTime.Value.Month == 1 && u.TypeId == 533);
            var FebruaryContinued = projectList.Count(u => u.CreateTime.Value.Month == 2 && u.TypeId == 533);
            var MarchContinued = projectList.Count(u => u.CreateTime.Value.Month == 3 && u.TypeId == 533);
            var AprilContinued = projectList.Count(u => u.CreateTime.Value.Month == 4 && u.TypeId == 533);
            var MayContinued = projectList.Count(u => u.CreateTime.Value.Month == 5 && u.TypeId == 533);
            var JuneContinued = projectList.Count(u => u.CreateTime.Value.Month == 6 && u.TypeId == 533);
            var JulyContinued = projectList.Count(u => u.CreateTime.Value.Month == 7 && u.TypeId == 533);
            var AugustContinued = projectList.Count(u => u.CreateTime.Value.Month == 8 && u.TypeId == 533);
            var SeptemberContinued = projectList.Count(u => u.CreateTime.Value.Month == 9 && u.TypeId == 533);
            var OctoberContinued = projectList.Count(u => u.CreateTime.Value.Month == 10 && u.TypeId == 533);
            var NovemberContinued = projectList.Count(u => u.CreateTime.Value.Month == 11 && u.TypeId == 533);
            var DecemberContinued = projectList.Count(u => u.CreateTime.Value.Month == 12 && u.TypeId == 533);
            var projectEarth = JanuaryContinued + "," + FebruaryContinued + "," + MarchContinued + "," + AprilContinued + "," + MayContinued + "," + JuneContinued + "," + JulyContinued + "," + AugustContinued + "," + SeptemberContinued + "," + OctoberContinued + "," + NovemberContinued + "," + DecemberContinued;

            var JanuaryWorks = projectList.Count(u => u.CreateTime.Value.Month == 1 && u.TypeId == 535);
            var FebruaryWorks = projectList.Count(u => u.CreateTime.Value.Month == 2 && u.TypeId == 535);
            var MarchWorks = projectList.Count(u => u.CreateTime.Value.Month == 3 && u.TypeId == 535);
            var AprilWorks = projectList.Count(u => u.CreateTime.Value.Month == 4 && u.TypeId == 535);
            var MayWorks = projectList.Count(u => u.CreateTime.Value.Month == 5 && u.TypeId == 535);
            var JuneWorks = projectList.Count(u => u.CreateTime.Value.Month == 6 && u.TypeId == 535);
            var JulyWorks = projectList.Count(u => u.CreateTime.Value.Month == 7 && u.TypeId == 535);
            var AugustWorks = projectList.Count(u => u.CreateTime.Value.Month == 8 && u.TypeId == 535);
            var SeptemberWorks = projectList.Count(u => u.CreateTime.Value.Month == 9 && u.TypeId == 535);
            var OctoberWorks = projectList.Count(u => u.CreateTime.Value.Month == 10 && u.TypeId == 535);
            var NovemberWorks = projectList.Count(u => u.CreateTime.Value.Month == 11 && u.TypeId == 535);
            var DecemberWorks = projectList.Count(u => u.CreateTime.Value.Month == 12 && u.TypeId == 535);
            var projectWorks = JanuaryWorks + "," + FebruaryWorks + "," + MarchWorks + "," + AprilWorks + "," + MayWorks + "," + JuneWorks + "," + JulyWorks + "," + AugustWorks + "," + SeptemberWorks + "," + OctoberWorks + "," + NovemberWorks + "," + DecemberWorks;

            var countOutput = new GetProjectStatisticsOutput();
            countOutput.Housing = projectHousing;//房屋建筑
            countOutput.Municipal = projectMunicipal;//市政道路
            countOutput.Greening = projectGreening;//市政绿化个数
            countOutput.Earth = projectEarth;//土方平场个数
            countOutput.Works = projectWorks;//市政安装工程个数
            return countOutput;
        }

        public GetProjectCountOutput GetTotalMoney(DateTime? commencementDateL, DateTime? commencementDateR)
        {
            var query = _projectRepository.Get().Select(u => new { u.DataState, u.GeneralEstimate,u.FinancialInvestment,u.NonFinancialInvestment, u.CommencementDate, u.CreateTime }).Where(u => u.DataState == DataState.Stable);

            if (commencementDateL != null)
            {
                query = query.Where(u => u.CommencementDate >= commencementDateL);
            }
            if (commencementDateR != null)
            {
                query = query.Where(u => u.CommencementDate <= commencementDateR);
            }
          
            

            var countOutput = new GetProjectCountOutput();
            countOutput.ProjectCount = query.ToList().Count();//项目个数
            foreach (var res in query.ToList())
            {
                countOutput.GeneralMoney += res.GeneralEstimate;//总概算金额
                countOutput.MacrocontractMoney += res.FinancialInvestment;//总合同金额
                countOutput.ReserveMoney += res.NonFinancialInvestment;//总预留金
            }
            return countOutput;
        }

        public List<CurrentMemberPermission> GetCurrentPermission(List<int> projectIdList, string projectRole)
        {
            return _projectHelper.HasPermission(projectRole, projectIdList);
        }

       
    }
}
