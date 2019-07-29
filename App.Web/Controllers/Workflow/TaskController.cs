using App.Core.Common.Entities;
using App.Core.FileManagement.AttachmentFileMetas.Dto;
using App.Core.FileManagement.Attachments.Dto;
using App.Core.Form.Fields.Dto;
using App.Core.Workflow.Tasks.Dto;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using App.Web.Authentication;
using App.Web.FileUrl;
using App.Workflow.Tasks;
using App.Workflow.Tasks.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Web.Filters;
using App.Base;
using App.Projects.Projects;
using App.Projects.ProjectBaseInfos.Dto;
using App.Memorabilia.MemorabiliaRecords;
using App.Memorabilia.MemorabiliaRecords.Dto;
using App.Contract.Contracts;
using App.Contract.ConstructionUnits;
using App.Contract.Contracts.Dto;
using App.Contract.ConstructionUnits.Dto;
using App.ProjectGantts.Tasks;
using App.ProjectGantts.Tasks.Dto;
using App.Quality.QualityAccidents;
using App.Quality.QualityProblems;
using App.Quality.QualityAccidents.Dto;
using App.Quality.QualityProblems.Dto;
using App.Safety.SafetyAccidents;
using App.Safety.SafetyProblems;
using App.Safety.SafetyAccidents.Dto;
using App.Safety.SafetyProblems.Dto;
using App.Housekeeping.Housekeepings;
using App.Housekeeping.Housekeepings.Dto;
using App.ProjectEarlyStage.EarlyStages;
using App.ProjectEarlyStage.EarlyStages.Dto;
using App.RecordMgmt.Records;
using App.RecordMgmt.Records.Dto;
using App.Problems.Problems;
using App.Problems.Problems.Dto;
using App.ProjectProgress.WeeklyProgresses.Dto;
using App.ProjectProgress.WeeklyProgresses;
using App.Funds.ContractPayments;
using App.Funds.ContractPayments.Dto;
using App.ProjectProgress.PmMonthlyProgresses;
using App.ProjectProgress.PmMonthlyProgresses.Dto;

namespace App.Web.Controllers.Workflow
{
    /// <summary>
    /// 流程-任务
    /// </summary>
    [ApiVersion("1.0")]
    [MyAuthorize]
    [Route("api/v{version:apiVersion}/tasks")]
    public class TaskController: ControllerBase
    {
        private ITaskService _taskService;
        private IFileUrlBuilder _fileUrlBuilder;
        private IProjectService _projectService;
        private IMemorabiliaRecordService _memoService;
        private IRecordService _recordService;
        private IContractService _contractService;
        private IConstructionUnitService _constructionUnitService;
        private IProjectTaskService _projectTaskService;
        private IQualityAccidentService _accidentTaskService;
        private IQualityProblemService _problemTaskService;
        private ISafetyAccidentService _safetyAccidentTaskService;
        private ISafetyProblemService _safetyProblemTaskService;
        private IHousekeepingProblemService _housekeepingProblemTaskService;
        private IEarlyStageService _earlyStageTaskService;
        private IProblemService _problemsTaskService;
        private IWeeklyProgressService _weeklyProgressService;
        private IMonthlyProgressService _monthlyProgressService;
        private IContractPaymentService _paymentService;
        public TaskController(
            ITaskService taskService,
            IFileUrlBuilder fileUrlBuilder,
            IProjectService projectService,
            IMemorabiliaRecordService memoService,
            IRecordService recordService,
            IContractService contractService,
            IConstructionUnitService constructionUnitService,
            IProjectTaskService projectTaskService,
            IQualityAccidentService accidentTaskService,
            IQualityProblemService problemTaskService,
            ISafetyAccidentService safetyAccidentTaskService,
            ISafetyProblemService safetyProblemTaskService,
            IHousekeepingProblemService housekeepingProblemTaskService,
            IEarlyStageService earlyStageTaskService,
            IProblemService problemsTaskService,
            IWeeklyProgressService weeklyProgressService,
            IMonthlyProgressService monthlyProgressService,
             IContractPaymentService paymentService
            )
        {
            _taskService = taskService;
            _fileUrlBuilder = fileUrlBuilder;
            _projectService = projectService;
            _memoService  = memoService;
            _recordService = recordService;
            _contractService = contractService;
            _constructionUnitService = constructionUnitService;
            _projectTaskService = projectTaskService;
            _accidentTaskService = accidentTaskService;
            _problemTaskService = problemTaskService;
            _safetyAccidentTaskService = safetyAccidentTaskService;
            _safetyProblemTaskService = safetyProblemTaskService;
            _housekeepingProblemTaskService = housekeepingProblemTaskService;
            _earlyStageTaskService = earlyStageTaskService;
            _problemsTaskService = problemsTaskService;
            _weeklyProgressService = weeklyProgressService;
            _monthlyProgressService = monthlyProgressService;
            _paymentService = paymentService;
        }
        public class TaskExistingState
        {
            public bool Exist { get; set; }
        }

        /// <summary>
        /// 判断任务存不存在
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}/existing")]
        public TaskExistingState Existing(int id)
        {
            return new TaskExistingState { Exist = _taskService.Exist(id) };
        }

        /// <summary>
        /// 获取任务详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        public GetTaskOutput Get(int id)
        {
            return _taskService.Get(id);
        }

        /// <summary>
        /// 根据流程实例获取任务详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("by-process/{id:int}")]
        public GetTaskOutput GetByProcessInstance(int id)
        {
            return _taskService.GetByProcessInstance(id);
        }

        /// <summary>
        /// 分页获取任务列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyword"></param>
        /// <param name="createTimeL">最小创建时间</param>
        /// <param name="createTimeR">最大创建时间</param>
        /// <param name="processDefinitionName">流程定义名称</param>
        /// <param name="include">是否包含</param>
        /// <returns></returns>
        [HttpGet]
        public PaginationData<GetTaskOutput> Get(int pageIndex, int pageSize, string keyword,
            DateTime? createTimeL, DateTime? createTimeR, string processDefinitionName = null, bool include = true)
        {
            return _taskService.Get(pageIndex, pageSize, keyword, createTimeL, createTimeR, processDefinitionName, include);
        }

        /// <summary>
        /// 分页获取任务历史列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyword"></param>
        /// <param name="createTimeL">最小创建时间</param>
        /// <param name="createTimeR">最大创建时间</param>
        /// <returns></returns>
        [HttpGet("history")]
        public PaginationData<GetTaskHistoryOutput> GetHisitory(int pageIndex, int pageSize, string keyword,
            DateTime? createTimeL, DateTime? createTimeR)
        {
            return _taskService.GetHistory(pageIndex, pageSize, keyword, createTimeL, createTimeR);
        }



        /// <summary>
        /// 重定向
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("redirect/{id:int}")]
        public IActionResult RedirectTask(int id, [FromBody]RedirectTaskInput input)
        {
            input.Id = id;
            _taskService.Redirect(input);
            return Created("", null);
        }

        /// <summary>
        /// 撤回 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("history/withdraw/{id:int}")]
        public IActionResult Withdraw(int id)
        {
            _taskService.Withdraw(id);
            return Created("", null);
        }

        /// <summary>
        /// 完成任务
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("complete/{id:int}")]
        public IActionResult CompleteTask(int id, [FromBody]CompleteTaskInput input)
        {
            input.Id = id;
            _taskService.CompleteTask(input);
            return Created("", null);
        }
        /// <summary>
        /// 查询当前任务的字段
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}/fields")]
        public List<FieldDto> GetFields(int id)
        {
            var fields =  _taskService.GetFields(id);
            fields.Where(u=>u.Remark == "附件").ToList().ForEach(u => {
                var json = (string)u.Value;
                if (!string.IsNullOrEmpty(json))
                {
                    List<GetAttachmentFileMetaOutput> attachments = JsonConvert.DeserializeObject<List<GetAttachmentFileMetaOutput>>(json);
                    _fileUrlBuilder.SetAttachFileUrl(attachments);
                    u.Value = JsonConvert.SerializeObject(attachments);
                }
            });
            return fields;
        }
        /// <summary>
        /// 查询历史任务的字段
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("history/{id:int}/fields")]
        public List<FieldDto> GetFieldsByHistory(int id)
        {
            var fields =  _taskService.GetFieldsByHistory(id);
            fields.Where(u=>u.Remark == "附件").ToList().ForEach(u => {
                var json = (string)u.Value;
                if (!string.IsNullOrEmpty(json))
                {
                    List<GetAttachmentFileMetaOutput> attachments = JsonConvert.DeserializeObject<List<GetAttachmentFileMetaOutput>>(json);
                    _fileUrlBuilder.SetAttachFileUrl(attachments);
                    u.Value = JsonConvert.SerializeObject(attachments);
                }
            });
            return fields;
        }

        /// <summary>
        /// 完成项目信息审批的任务
        /// </summary>
        /// <returns></returns>
        [HttpPut("project/complete/{id:int}")]
        public IActionResult CompleteProjectTask(int id, [FromBody]CompleteTaskInput<UpdateProjectInput> input)
        {
            input.Id = id;
            _projectService.CompleteTask(input);
            return Created("", null);
        }
        /// <summary>
        /// 查询当前流程的项目信息审批任务的字段
        /// </summary>
        /// <returns></returns>
        [HttpGet("project/{id:int}/fields")]
        public GetProjectOutput GetProjectByTask(int id)
        {
            var data = _projectService.GetByTask(id);
            _fileUrlBuilder.SetAttachFileUrl(data);
            return data;
        }
        /// <summary>
        /// 查询历史项目基本信息审批任务的字段
        /// </summary>
        /// <returns></returns>
        [HttpGet("project/history/{id:int}/fields")]
        public GetProjectOutput GetProjectByTaskHistory(int id)
        {
            var data = _projectService.GetByTaskHistory(id);
            _fileUrlBuilder.SetAttachFileUrl(data);
            return data;
        }

        /// <summary>
        /// 完成大事记信息审批的任务
        /// </summary>
        /// <returns></returns>
        [HttpPut("memorabilia/complete/{id:int}")]
        public IActionResult CompleteMemoTask(int id, [FromBody]CompleteTaskInput<UpdateMemorabiliaRecordInput> input)
        {
            input.Id = id;
            _memoService.CompleteTask(input);
            return Created("", null);
        }
        /// <summary>
        /// 查询当前流程的大事记信息审批任务的字段
        /// </summary>
        /// <returns></returns>
        [HttpGet("memorabilia/{id:int}/fields")]
        public GetMemorabiliaRecordOutput GetMemoByTask(int id)
        {
            var data = _memoService.GetByTask(id);
            _fileUrlBuilder.SetAttachFileUrl(data);
            return data;
        }
        /// <summary>
        /// 查询历史大事记基本信息审批任务的字段
        /// </summary>
        /// <returns></returns>
        [HttpGet("memorabilia/history/{id:int}/fields")]
        public GetMemorabiliaRecordOutput GetMemoByTaskHistory(int id)
        {
            var data = _memoService.GetByTaskHistory(id);
            _fileUrlBuilder.SetAttachFileUrl(data);
            return data;
        }


        /// <summary>
        /// 完成合同信息审批的任务
        /// </summary>
        /// <returns></returns>
        [HttpPut("contract/complete/{id:int}")]
        public IActionResult CompleteContractTask(int id, [FromBody]CompleteTaskInput<UpdateContractInput> input)
        {
            input.Id = id;
            _contractService.CompleteTask(input);
            return Created("", null);
        }
        /// <summary>
        /// 查询当前流程的合同信息审批任务的字段
        /// </summary>
        /// <returns></returns>
        [HttpGet("contract/{id:int}/fields")]
        public GetContractOutput GetContractByTask(int id)
        {
            var data = _contractService.GetByTask(id);
            _fileUrlBuilder.SetAttachFileUrl(data);
            return data;
        }
        /// <summary>
        /// 查询历史合同基本信息审批任务的字段
        /// </summary>
        /// <returns></returns>
        [HttpGet("contract/history/{id:int}/fields")]
        public GetContractOutput GetContractByTaskHistory(int id)
        {
            var data = _contractService.GetByTaskHistory(id);
            _fileUrlBuilder.SetAttachFileUrl(data);
            return data;
        }


        /// <summary>
        /// 完成参建单位信息审批的任务
        /// </summary>
        /// <returns></returns>
        [HttpPut("constructionunit/complete/{id:int}")]
        public IActionResult CompleteConstructionUnitTask(int id, [FromBody]CompleteTaskInput<UpdateConstructionUnitInput> input)
        {
            input.Id = id;
            _constructionUnitService.CompleteTask(input);
            return Created("", null);
        }
        /// <summary>
        /// 查询当前流程的参建单位信息审批任务的字段
        /// </summary>
        /// <returns></returns>
        [HttpGet("constructionunit/{id:int}/fields")]
        public GetConstructionUnitOutput GetConstructionUnitByTask(int id)
        {
            var data = _constructionUnitService.GetByTask(id);
            _fileUrlBuilder.SetAttachFileUrl(data);
            return data;
        }
        /// <summary>
        /// 查询历史参建单位基本信息审批任务的字段
        /// </summary>
        /// <returns></returns>
        [HttpGet("constructionunit/history/{id:int}/fields")]
        public GetConstructionUnitOutput GetConstructionUnitByTaskHistory(int id)
        {
            var data = _constructionUnitService.GetByTaskHistory(id);
            _fileUrlBuilder.SetAttachFileUrl(data);
            return data;
        }

        /// <summary>
        /// 完成质量事故任务审批的任务
        /// </summary>
        /// <returns></returns>
        [HttpPut("qualityaccident-task/complete/{id:int}")]
        public IActionResult CompleteAccidentTaskTask(int id, [FromBody]CompleteTaskInput<UpdateQualityAccidentInput> input)
        {
            input.Id = id;
            _accidentTaskService.CompleteTask(input);
            return Created("", null);
        }
        /// <summary>
        /// 查询当前流程的质量事故任务审批任务的字段
        /// </summary>
        /// <returns></returns>
        [HttpGet("qualityaccident-task/{id:int}/fields")]
        public GetQualityAccidentOutput GetAccidentTaskByTask(int id)
        {
            var data = _accidentTaskService.GetByTask(id);
            _fileUrlBuilder.SetAttachFileUrl(data);
            return data;
        }
        /// <summary>
        /// 查询历史质量事故任务审批任务的字段
        /// </summary>
        /// <returns></returns>
        [HttpGet("qualityaccident-task/history/{id:int}/fields")]
        public GetQualityAccidentOutput GetAccidentTaskByTaskHistory(int id)
        {
            var data = _accidentTaskService.GetByTaskHistory(id);
            _fileUrlBuilder.SetAttachFileUrl(data);
            return data;
        }

        /// <summary>
        /// 完成质量问题任务审批的任务
        /// </summary>
        /// <returns></returns>
        [HttpPut("qualityproblem-task/complete/{id:int}")]
        public IActionResult CompleteProblemTaskTask(int id, [FromBody]CompleteTaskInput<UpdateQualityProblemInput> input)
        {
            input.Id = id;
            _problemTaskService.CompleteTask(input);
            return Created("", null);
        }
        /// <summary>
        /// 查询当前流程的质量问题任务审批任务的字段
        /// </summary>
        /// <returns></returns>
        [HttpGet("qualityproblem-task/{id:int}/fields")]
        public GetQualityProblemOutput GetProblemTaskByTask(int id)
        {
            var data = _problemTaskService.GetByTask(id);
            _fileUrlBuilder.SetAttachFileUrl(data);
            return data;
        }
        /// <summary>
        /// 查询历史质量问题任务审批任务的字段
        /// </summary>
        /// <returns></returns>
        [HttpGet("qualityproblem-task/history/{id:int}/fields")]
        public GetQualityProblemOutput GetProblemTaskByTaskHistory(int id)
        {
            var data = _problemTaskService.GetByTaskHistory(id);
            _fileUrlBuilder.SetAttachFileUrl(data);
            return data;
        }

        /// <summary>
        /// 完成安全事故任务审批的任务
        /// </summary>
        /// <returns></returns>
        [HttpPut("safetyaccident-task/complete/{id:int}")]
        public IActionResult CompleteSafetyAccidentTaskTask(int id, [FromBody]CompleteTaskInput<UpdateSafetyAccidentInput> input)
        {
            input.Id = id;
            _safetyAccidentTaskService.CompleteTask(input);
            return Created("", null);
        }
        /// <summary>
        /// 查询当前流程的安全事故任务审批任务的字段
        /// </summary>
        /// <returns></returns>
        [HttpGet("safetyaccident-task/{id:int}/fields")]
        public GetSafetyAccidentOutput GetSafetyAccidentTaskByTask(int id)
        {
            var data = _safetyAccidentTaskService.GetByTask(id);
            _fileUrlBuilder.SetAttachFileUrl(data);
            return data;
        }
        /// <summary>
        /// 查询历史安全事故任务审批任务的字段
        /// </summary>
        /// <returns></returns>
        [HttpGet("safetyaccident-task/history/{id:int}/fields")]
        public GetSafetyAccidentOutput GetSafetyAccidentTaskByTaskHistory(int id)
        {
            var data = _safetyAccidentTaskService.GetByTaskHistory(id);
            _fileUrlBuilder.SetAttachFileUrl(data);
            return data;
        }


        /// <summary>
        /// 完成安全问题任务审批的任务
        /// </summary>
        /// <returns></returns>
        [HttpPut("safetyproblem-task/complete/{id:int}")]
        public IActionResult CompleteSafetyProblemTaskTask(int id, [FromBody]CompleteTaskInput<UpdateSafetyProblemInput> input)
        {
            input.Id = id;
            _safetyProblemTaskService.CompleteTask(input);
            return Created("", null);
        }
        /// <summary>
        /// 查询当前流程的安全问题任务审批任务的字段
        /// </summary>
        /// <returns></returns>
        [HttpGet("safetyproblem-task/{id:int}/fields")]
        public GetSafetyProblemOutput GetSafetyProblemTaskByTask(int id)
        {
            var data = _safetyProblemTaskService.GetByTask(id);
            _fileUrlBuilder.SetAttachFileUrl(data);
            return data;
        }
        /// <summary>
        /// 查询历史安全问题任务审批任务的字段
        /// </summary>
        /// <returns></returns>
        [HttpGet("safetyproblem-task/history/{id:int}/fields")]
        public GetSafetyProblemOutput GetSafetyProblemTaskByTaskHistory(int id)
        {
            var data = _safetyProblemTaskService.GetByTaskHistory(id);
            _fileUrlBuilder.SetAttachFileUrl(data);
            return data;
        }

        /// <summary>
        /// 完成文明施工任务审批的任务
        /// </summary>
        /// <returns></returns>
        [HttpPut("housekeepingproblem-task/complete/{id:int}")]
        public IActionResult CompleteHousekeepingProblemTaskTask(int id, [FromBody]CompleteTaskInput<UpdateHousekeepingProblemInput> input)
        {
            input.Id = id;
            _housekeepingProblemTaskService.CompleteTask(input);
            return Created("", null);
        }
        /// <summary>
        /// 查询当前流程的文明施工任务审批任务的字段
        /// </summary>
        /// <returns></returns>
        [HttpGet("housekeepingproblem-task/{id:int}/fields")]
        public GetHousekeepingProblemOutput GetHousekeepingProblemTaskByTask(int id)
        {
            var data = _housekeepingProblemTaskService.GetByTask(id);
            _fileUrlBuilder.SetAttachFileUrl(data);
            return data;
        }
        /// <summary>
        /// 查询历史文明施工任务审批任务的字段
        /// </summary>
        /// <returns></returns>
        [HttpGet("housekeepingproblem-task/history/{id:int}/fields")]
        public GetHousekeepingProblemOutput GetHousekeepingProblemTaskByTaskHistory(int id)
        {
            var data = _housekeepingProblemTaskService.GetByTaskHistory(id);
            _fileUrlBuilder.SetAttachFileUrl(data);
            return data;
        }


        /// <summary>
        /// 完成项目流程任务审批的任务
        /// </summary>
        /// <returns></returns>
        [HttpPut("earlystage-task/complete/{id:int}")]
        public IActionResult CompleteEarlyStageTaskTask(int id, [FromBody]CompleteTaskInput<UpdateEarlyStageInput> input)
        {
            input.Id = id;
            _earlyStageTaskService.CompleteTask(input);
            return Created("", null);
        }
        /// <summary>
        /// 查询当前流程的项目流程任务审批任务的字段
        /// </summary>
        /// <returns></returns>
        [HttpGet("earlystage-task/{id:int}/fields")]
        public GetEarlyStageOutput GetEarlyStageTaskByTask(int id)
        {
            var data = _earlyStageTaskService.GetByTask(id);
            _fileUrlBuilder.SetAttachFileUrl(data);
            return data;
        }
        /// <summary>
        /// 查询历史项目流程任务审批任务的字段
        /// </summary>
        /// <returns></returns>
        [HttpGet("earlystage-task/history/{id:int}/fields")]
        public GetEarlyStageOutput GetEarlyStageTaskByTaskHistory(int id)
        {
            var data = _earlyStageTaskService.GetByTaskHistory(id);
            _fileUrlBuilder.SetAttachFileUrl(data);
            return data;
        }

        /// <summary>
        /// 完成档案流程任务审批的任务
        /// </summary>
        /// <returns></returns>
        [HttpPut("record/complete/{id:int}")]
        public IActionResult CompleteRecordTask(int id, [FromBody]CompleteTaskInput<UpdateRecordInput> input)
        {
            input.Id = id;
            _recordService.CompleteTask(input);
            return Created("", null);
        }
        /// <summary>
        /// 查询当前流程的档案流程任务审批任务的字段
        /// </summary>
        /// <returns></returns>
        [HttpGet("record/{id:int}/fields")]
        public GetRecordOutput GetRecordByTask(int id)
        {
            var data = _recordService.GetByTask(id);
            _fileUrlBuilder.SetAttachFileUrl(data);
            return data;
        }
        /// <summary>
        /// 查询历史档案流程任务审批任务的字段
        /// </summary>
        /// <returns></returns>
        [HttpGet("record/history/{id:int}/fields")]
        public GetRecordOutput GetRecordByTaskHistory(int id)
        {
            var data = _recordService.GetByTaskHistory(id);
            _fileUrlBuilder.SetAttachFileUrl(data);
            return data;
        }

        /// <summary>
        /// 完成存在问题任务审批的任务
        /// </summary>
        /// <returns></returns>
        [HttpPut("problems-task/complete/{id:int}")]
        public IActionResult CompleteProblemsTaskTask(int id, [FromBody]CompleteTaskInput<UpdateProblemInput> input)
        {
            input.Id = id;
            _problemsTaskService.CompleteTask(input);
            return Created("", null);
        }
        /// <summary>
        /// 查询当前流程的存在问题任务审批任务的字段
        /// </summary>
        /// <returns></returns>
        [HttpGet("problems-task/{id:int}/fields")]
        public GetProblemOutput GetProblemsTaskByTask(int id)
        {
            var data = _problemsTaskService.GetByTask(id);
            _fileUrlBuilder.SetAttachFileUrl(data);
            return data;
        }
        /// <summary>
        /// 查询历史存在问题任务审批任务的字段
        /// </summary>
        /// <returns></returns>
        [HttpGet("problems-task/history/{id:int}/fields")]
        public GetProblemOutput GetProblemsTaskByTaskHistory(int id)
        {
            var data = _problemsTaskService.GetByTaskHistory(id);
            _fileUrlBuilder.SetAttachFileUrl(data);
            return data;
        }

        /// <summary>
        /// 完成周报任务审批的任务
        /// </summary>
        /// <returns></returns>
        [HttpPut("weekly-progress/complete/{id:int}")]
        public IActionResult CompleteWeeklyProgressTaskTask(int id, [FromBody]CompleteTaskInput<UpdateWeeklyProgressInput> input)
        {
            input.Id = id;
            _weeklyProgressService.CompleteTask(input);
            return Created("", null);
        }
        /// <summary>
        /// 查询当前流程的周报任务审批任务的字段
        /// </summary>
        /// <returns></returns>
        [HttpGet("weekly-progress/{id:int}/fields")]
        public GetWeeklyProgressOutput GetWeeklyProgressTaskByTask(int id)
        {
            var data = _weeklyProgressService.GetByTask(id);
            _fileUrlBuilder.SetAttachFileUrl(data);
            return data;
        }
        /// <summary>
        /// 查询历史周报任务审批任务的字段
        /// </summary>
        /// <returns></returns>
        [HttpGet("weekly-progress/history/{id:int}/fields")]
        public GetWeeklyProgressOutput GetWeeklyProgressTaskByTaskHistory(int id)
        {
            var data = _weeklyProgressService.GetByTaskHistory(id);
            return data;
        }

        /// <summary>
        /// 完成月报任务审批的任务
        /// </summary>
        /// <returns></returns>
        [HttpPut("monthly-progress/complete/{id:int}")]
        public IActionResult CompleteMonthlyProgressTaskTask(int id, [FromBody]CompleteTaskInput<UpdateMonthlyProgressInput> input)
        {
            input.Id = id;
            _monthlyProgressService.CompleteTask(input);
            return Created("", null);
        }
        /// <summary>
        /// 查询当前流程的月报任务审批任务的字段
        /// </summary>
        /// <returns></returns>
        [HttpGet("monthly-progress/{id:int}/fields")]
        public GetMonthlyProgressOutput GetMonthlyProgressTaskByTask(int id)
        {
            var data = _monthlyProgressService.GetByTask(id);
            _fileUrlBuilder.SetAttachFileUrl(data);
            return data;
        }
        /// <summary>
        /// 查询历史月报任务审批任务的字段
        /// </summary>
        /// <returns></returns>
        [HttpGet("monthly-progress/history/{id:int}/fields")]
        public GetMonthlyProgressOutput GetMonthlyProgressTaskByTaskHistory(int id)
        {
            var data = _monthlyProgressService.GetByTaskHistory(id);
            return data;
        }


        /// <summary>
        /// 完成合同支付任务审批的任务
        /// </summary>
        /// <returns></returns>
        [HttpPut("contractpayment-task/complete/{id:int}")]
        public IActionResult CompletePaymentTaskTask(int id, [FromBody]CompleteTaskInput<UpdateContractPaymentInput> input)
        {
            input.Id = id;
            _paymentService.CompleteTask(input);
            return Created("", null);
        }
        /// <summary>
        /// 查询当前流程的合同支付任务审批任务的字段
        /// </summary>
        /// <returns></returns>
        [HttpGet("contractpayment-task/{id:int}/fields")]
        public GetContractPaymentOutput GetPaymentTaskByTask(int id)
        {
            var data = _paymentService.GetByTask(id);
            _fileUrlBuilder.SetAttachFileUrl(data);
            return data;
        }
        /// <summary>
        /// 查询历史合同支付任务审批任务的字段
        /// </summary>
        /// <returns></returns>
        [HttpGet("contractpayment-task/history/{id:int}/fields")]
        public GetContractPaymentOutput GetPaymentTaskByTaskHistory(int id)
        {
            var data = _paymentService.GetByTaskHistory(id);
            return data;
        }
    }
}
