using App.Core.Common.Entities;
using App.Core.Form.Fields.Dto;
using App.Core.Workflow.ProcessInstances.Dto;
using Microsoft.AspNetCore.Mvc;
using App.Web.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Web.Filters;
using App.Workflow.ProcessInstances;
using App.Workflow.ProcessInstances.Dto;
using App.Core.Form.FieldQueryConditions;
using App.Core.Common.Exceptions;
using PoorFff;
using App.Projects.Projects;
using App.Projects.ProjectBaseInfos.Dto;
using App.Base;
using App.Web.FileUrl;
using App.Memorabilia.MemorabiliaRecords;
using App.Memorabilia.MemorabiliaRecords.Dto;
using App.Contract.Contracts;
using App.Contract.ConstructionUnits;
using App.Contract.Contracts.Dto;
using App.Contract.ConstructionUnits.Dto;
using App.ProjectGantts.Gantts;
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
using App.ProjectEarlyStage.PmEarlyStages.Dto;
using App.ProjectEarlyStage.EarlyStages.Dto;
using App.RecordMgmt.Records.Dto;
using App.RecordMgmt.Records;
using App.Problems.Problems;
using App.Problems.Problems.Dto;
using App.ProjectProgress.WeeklyProgresses;
using App.ProjectProgress.WeeklyProgresses.Dto;
using App.Funds.ContractPayments;
using App.Funds.ContractPayments.Dto;
using App.ProjectProgress.PmMonthlyProgresses;
using App.ProjectProgress.PmMonthlyProgresses.Dto;

namespace App.Web.Controllers.Workflow
{
    /// <summary>
    /// 流程-审批
    /// </summary>
    [ApiVersion("1.0")]
    [MyAuthorize]
    [Route("api/v{version:apiVersion}/process-instances")]
    public class ProcessInstanceController: ControllerBase
    {
        private IProcessInstanceService _processInstanceService;
        private IFileUrlBuilder _fileUrlBuilder;
        private IProjectService _projectService;
        private IMemorabiliaRecordService _memoService;
        private IRecordService _recordService;
        private IContractService _contractService;
        private IConstructionUnitService _constructionUnitService;
        private IProjectGanttService _ganttService;
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
        public ProcessInstanceController(
            IProcessInstanceService processInstanceService,
            IFileUrlBuilder fileUrlBuilder,
            IProjectService projectService,
            IMemorabiliaRecordService memoService,
            IRecordService recordService,
            IProjectGanttService ganttService,
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
            _processInstanceService = processInstanceService;
            _projectService = projectService;
            _fileUrlBuilder = fileUrlBuilder;
            _memoService = memoService;
            _recordService = recordService;
            _contractService = contractService;
            _constructionUnitService = constructionUnitService;
            _ganttService = ganttService;
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

        /// <summary>
        /// 获取流程实例信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        public GetProcessInstanceOutput Get(int id)
        {
            return _processInstanceService.Get(id);
        }

        ///// <summary>
        ///// 获取流程表单字段信息
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //[HttpGet("{id:int}/fields")]
        //public List<FieldDto> GetFields(int id)
        //{
        //    return _processInstanceService.GetFields(id);
        //}

        /// <summary>
        /// 分页获取表单流程字段
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="formName">表单名称</param>
        /// <param name="needFields">需要的表单字段列表， 格式为: "field1,field2"</param>
        /// <param name="fieldQueryConditions">字段查询条件列表， 格式为："fieldName1|type1|keyword1|value1|left1|right1,fieldName2|type2|keyword2|value2|left2|right2"</param>
        /// <param name="sort">排序, 格式为："fieldName|type"</param>
        /// <param name="isAsc">是否为升序</param>
        /// <returns></returns>
        [HttpGet("for-fields")]
        public PaginationData<GetProcessInstancePoorOutput> GetForFields(int pageIndex, int pageSize, string formName, 
            string needFields, 
            string fieldQueryConditions, 
            string sort, 
            bool isAsc = true)
        {
            List<string> needFieldsArr = null;
            if (!string.IsNullOrEmpty(needFields))
            {
                needFieldsArr = needFields.Split(',').ToList();
            }
            List<string> fieldQueryConditionsArr = new List<string>();
            if (!string.IsNullOrEmpty(fieldQueryConditions))
            {
                fieldQueryConditionsArr = fieldQueryConditions.Split(',').ToList();
            }
            List<FieldQueryCondition> fieldQueryConditonObjArr = new List<FieldQueryCondition>();
            foreach (var condition in fieldQueryConditionsArr)
            {
                var queryPara = condition.Split('|');
                if (queryPara.Length != 6)
                {
                    throw new AppCoreException("fieldQueryConditons格式错误");
                }
                var conditionObj = new FieldQueryCondition()
                {
                    FieldName = queryPara[0],
                    Type = string.IsNullOrEmpty(queryPara[1]) ? 0 : (ClassTypeEnum)Convert.ToInt32(queryPara[1]),
                    Keyword = string.IsNullOrEmpty(queryPara[2]) ? null : queryPara[2],
                    Value = string.IsNullOrEmpty(queryPara[3]) ? null : queryPara[3],
                    Left = string.IsNullOrEmpty(queryPara[4]) ? null : queryPara[4],
                    Right = string.IsNullOrEmpty(queryPara[5]) ? null : queryPara[5],
                };
                fieldQueryConditonObjArr.Add(conditionObj);
            }
            return _processInstanceService.GetForFields(pageIndex, pageSize, formName, needFieldsArr, fieldQueryConditonObjArr, sort, isAsc);

        }

        /// <summary>
        /// 分页获取发起的审批列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyword"></param>
        /// <param name="creatorId">创建人Id</param>
        /// <param name="createTimeL">最小创建时间</param>
        /// <param name="createTimeR">最大创建时间</param>
        /// <returns></returns>
        [HttpGet]
        public PaginationData<GetProcessInstanceListOutput> Get(int pageIndex, int pageSize,
            string keyword,
            int? creatorId, 
            DateTime? createTimeL, DateTime? createTimeR)
        {
            return _processInstanceService.Get(pageIndex, pageSize, keyword, creatorId, createTimeL, createTimeR);
        }

        /// <summary>
        /// 终止流程
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("terminate/{id:int}")]
        public IActionResult Terminate(int id)
        {
            _processInstanceService.Terminate(id);
            return Created("", null);
        }

        /// <summary>
        /// 暂停流程
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("suspend/{id:int}")]
        public IActionResult Suspend(int id)
        {
            _processInstanceService.Suspend(id);
            return Created("", null);
        }

        /// <summary>
        /// 恢复流程
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("restart/{id:int}")]
        public IActionResult Restart(int id)
        {
            _processInstanceService.Restart(id);
            return Created("", null);
        }

        /// <summary>
        /// 发起流程
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Start([FromBody]StartProcessInput approval)
        {
            _processInstanceService.Start(approval, (starter, variables) => {
            });
            return Created("", null);
        }

        /// <summary>
        /// 获取流程字段
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}/fields")]
        public List<FieldDto> GetFields(int id)
        {
            return _processInstanceService.GetFields(id);
        }

        /// <summary>
        /// 查询当前流程的项目信息的字段
        /// </summary>
        /// <returns></returns>
        [HttpGet("project/{id:int}/fields")]
        public GetProjectOutput GetProjectByProcess(int id)
        {
            var data = _projectService.GetByProcess(id);
            _fileUrlBuilder.SetAttachFileUrl(data);
            return data;
        }
        /// <summary>
        /// 发起新增项目信息的审批
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("project")]
        public IActionResult StartProjectProcess([FromBody]StartProcessInput<AddProjectInput> input)
        {
            var id = _projectService.Add(input);
            _ganttService.AddByProject(id);
            return Created("", new { id });
        }
        /// <summary>
        /// 发起更新项目审批
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("project/{id:int}")]
        public IActionResult StartProjectProcess(int id, [FromBody]StartProcessInput<UpdateProjectInput> input)
        {
            input.Data.Id = id;
            _projectService.Update(input);
            return Created("", new { id });
        }

        /// <summary>
        /// 查询当前流程的大事记信息的字段
        /// </summary>
        /// <returns></returns>
        [HttpGet("memorabilia/{id:int}/fields")]
        public GetMemorabiliaRecordOutput GetRecordByProcess(int id)
        {
            var data = _memoService.GetByProcess(id);
            _fileUrlBuilder.SetAttachFileUrl(data);
            return data;
        }
        /// <summary>
        /// 发起新增大事记的审批
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("memorabilia")]
        public IActionResult StartRecordProcess([FromBody]StartProcessInput<AddMemorabiliaRecordInput> input)
        {
            var id = _memoService.Add(input);
            return Created("", new { id });
        }
        /// <summary>
        /// 发起更新大事记审批
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("memorabilia/{id:int}")]
        public IActionResult StartRecordProcess(int id, [FromBody]StartProcessInput<UpdateMemorabiliaRecordInput> input)
        {
            input.Data.Id = id;
            _memoService.Update(input);
            return Created("", new { id });
        }

        /// <summary>
        /// 查询当前流程的合同信息的字段
        /// </summary>
        /// <returns></returns>
        [HttpGet("contract/{id:int}/fields")]
        public GetContractOutput GetContractByProcess(int id)
        {
            var data = _contractService.GetByProcess(id);
            _fileUrlBuilder.SetAttachFileUrl(data);
            return data;
        }
        /// <summary>
        /// 发起新增合同的审批
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("contract")]
        public IActionResult StartContractProcess([FromBody]StartProcessInput<AddContractInput> input)
        {
            var id = _contractService.Add(input);
            return Created("", new { id });
        }
        /// <summary>
        /// 发起更新合同审批
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("contract/{id:int}")]
        public IActionResult StartContractProcess(int id, [FromBody]StartProcessInput<UpdateContractInput> input)
        {
            input.Data.Id = id;
            _contractService.Update(input);
            return Created("", new { id });
        }

        /// <summary>
        /// 查询当前流程的参建单位信息的字段
        /// </summary>
        /// <returns></returns>
        [HttpGet("constructionunit/{id:int}/fields")]
        public GetConstructionUnitOutput GetConstructionUnitByProcess(int id)
        {
            var data = _constructionUnitService.GetByProcess(id);
            _fileUrlBuilder.SetAttachFileUrl(data);
            return data;
        }
        /// <summary>
        /// 发起新增参建单位的审批
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("constructionunit")]
        public IActionResult StartConstructionUnitProcess([FromBody]StartProcessInput<AddConstructionUnitInput> input)
        {
            var id = _constructionUnitService.Add(input);
            return Created("", new { id });
        }
        /// <summary>
        /// 发起更新参建单位审批
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("constructionunit/{id:int}")]
        public IActionResult StartConstructionUnitProcess(int id, [FromBody]StartProcessInput<UpdateConstructionUnitInput> input)
        {
            input.Data.Id = id;
            _constructionUnitService.Update(input);
            return Created("", new { id });
        }

        /// <summary>
        /// 查询当前流程的质量事故的字段
        /// </summary>
        /// <returns></returns>
        [HttpGet("qualityaccident-task/{id:int}/fields")]
        public GetQualityAccidentOutput GetAccidentByProcess(int id)
        {
            var data = _accidentTaskService.GetByProcess(id);
            _fileUrlBuilder.SetAttachFileUrl(data);
            return data;
        }
        /// <summary>
        /// 发起新增质量事故的审批
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("qualityaccident-task")]
        public IActionResult StartAccidentProcess([FromBody]StartProcessInput<AddQualityAccidentInput> input)
        {
            var id = _accidentTaskService.Add(input);
            return Created("", new { id });
        }
        /// <summary>
        /// 发起更新质量事故审批
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("qualityaccident-task/{id:int}")]
        public IActionResult StartAccidentProcess(int id, [FromBody]StartProcessInput<UpdateQualityAccidentInput> input)
        {
            input.Data.Id = id;
            _accidentTaskService.Update(input);
            return Created("", new { id });
        }

        /// <summary>
        /// 查询当前流程的质量问题的字段
        /// </summary>
        /// <returns></returns>
        [HttpGet("qualityproblem-task/{id:int}/fields")]
        public GetQualityProblemOutput GetProblemByProcess(int id)
        {
            var data = _problemTaskService.GetByProcess(id);
            _fileUrlBuilder.SetAttachFileUrl(data);
            return data;
        }
        /// <summary>
        /// 发起新增质量问题的审批
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("qualityproblem-task")]
        public IActionResult StartProblemProcess([FromBody]StartProcessInput<AddQualityProblemInput> input)
        {
            var id = _problemTaskService.Add(input);
            return Created("", new { id });
        }
        /// <summary>
        /// 发起更新质量问题审批
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("qualityproblem-task/{id:int}")]
        public IActionResult StartProblemProcess(int id, [FromBody]StartProcessInput<UpdateQualityProblemInput> input)
        {
            input.Data.Id = id;
            _problemTaskService.Update(input);
            return Created("", new { id });
        }

        /// <summary>
        /// 查询当前流程的安全事故的字段
        /// </summary>
        /// <returns></returns>
        [HttpGet("safetyaccident-task/{id:int}/fields")]
        public GetSafetyAccidentOutput GetSafetyAccidentByProcess(int id)
        {
            var data = _safetyAccidentTaskService.GetByProcess(id);
            _fileUrlBuilder.SetAttachFileUrl(data);
            return data;
        }
        /// <summary>
        /// 发起新增安全事故的审批
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("safetyaccident-task")]
        public IActionResult StartSafetyAccidentProcess([FromBody]StartProcessInput<AddSafetyAccidentInput> input)
        {
            var id = _safetyAccidentTaskService.Add(input);
            return Created("", new { id });
        }
        /// <summary>
        /// 发起更新安全事故审批
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("safetyaccident-task/{id:int}")]
        public IActionResult StartSafetyAccidentProcess(int id, [FromBody]StartProcessInput<UpdateSafetyAccidentInput> input)
        {
            input.Data.Id = id;
            _safetyAccidentTaskService.Update(input);
            return Created("", new { id });
        }

        /// <summary>
        /// 查询当前流程的安全问题的字段
        /// </summary>
        /// <returns></returns>
        [HttpGet("safetyproblem-task/{id:int}/fields")]
        public GetSafetyProblemOutput GetSafetyProblemByProcess(int id)
        {
            var data = _safetyProblemTaskService.GetByProcess(id);
            _fileUrlBuilder.SetAttachFileUrl(data);
            return data;
        }
        /// <summary>
        /// 发起新增安全问题的审批
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("safetyproblem-task")]
        public IActionResult StartSafetyProblemProcess([FromBody]StartProcessInput<AddSafetyProblemInput> input)
        {
            var id = _safetyProblemTaskService.Add(input);
            return Created("", new { id });
        }
        /// <summary>
        /// 发起更新安全问题审批
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("safetyproblem-task/{id:int}")]
        public IActionResult StartSafetyProblemProcess(int id, [FromBody]StartProcessInput<UpdateSafetyProblemInput> input)
        {
            input.Data.Id = id;
            _safetyProblemTaskService.Update(input);
            return Created("", new { id });
        }

        /// <summary>
        /// 查询当前流程的文明施工的字段
        /// </summary>
        /// <returns></returns>
        [HttpGet("housekeeping-task/{id:int}/fields")]
        public GetHousekeepingProblemOutput GetHousekeepingByProcess(int id)
        {
            var data = _housekeepingProblemTaskService.GetByProcess(id);
            _fileUrlBuilder.SetAttachFileUrl(data);
            return data;
        }
        /// <summary>
        /// 发起新增文明施工的审批
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("housekeeping-task")]
        public IActionResult StartHousekeepingProcess([FromBody]StartProcessInput<AddHousekeepingProblemInput> input)
        {
            var id = _housekeepingProblemTaskService.Add(input);
            return Created("", new { id });
        }
        /// <summary>
        /// 发起更新文明施工审批
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("housekeeping-task/{id:int}")]
        public IActionResult StartHousekeepingProcess(int id, [FromBody]StartProcessInput<UpdateHousekeepingProblemInput> input)
        {
            input.Data.Id = id;
            _housekeepingProblemTaskService.Update(input);
            return Created("", new { id });
        }

        /// <summary>
        /// 查询当前流程的项目流程的字段
        /// </summary>
        /// <returns></returns>
        [HttpGet("earlystage-task/{id:int}/fields")]
        public GetEarlyStageOutput GetEarlyStageByProcess(int id)
        {
            var data = _earlyStageTaskService.GetByProcess(id);
            _fileUrlBuilder.SetAttachFileUrl(data);
            return data;
        }
        /// <summary>
        /// 发起新增项目流程的审批
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("earlystage-task")]
        public IActionResult StartEarlyStageProcess([FromBody]StartProcessInput<AddEarlyStageInput> input)
        {
            var id = _earlyStageTaskService.Add(input);
            return Created("", new { id });
        }
        /// <summary>
        /// 发起更新项目流程审批
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("earlystage-task/{id:int}")]
        public IActionResult StartEarlyStageProcess(int id, [FromBody]StartProcessInput<UpdateEarlyStageInput> input)
        {
            input.Data.Id = id;
            _earlyStageTaskService.Update(input);
            return Created("", new { id });
        }

        /// <summary>
        /// 查询当前流程的档案的字段
        /// </summary>
        /// <returns></returns>
        [HttpGet("record/{id:int}/fields")]
        public GetRecordOutput GetMemoByProcess(int id)
        {
            var data = _recordService.GetByProcess(id);
            _fileUrlBuilder.SetAttachFileUrl(data);
            return data;
        }
        /// <summary>
        /// 发起新增档案的审批
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("record")]
        public IActionResult StartMemoProcess([FromBody]StartProcessInput<AddRecordInput> input)
        {
            var id = _recordService.Add(input);
            return Created("", new { id });
        }
        /// <summary>
        /// 发起更新档案审批
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("record/{id:int}")]
        public IActionResult StartMemoProcess(int id, [FromBody]StartProcessInput<UpdateRecordInput> input)
        {
            input.Data.Id = id;
            _recordService.Update(input);
            return Created("", new { id });
        }


        /// <summary>
        /// 查询当前流程的存在问题的字段
        /// </summary>
        /// <returns></returns>
        [HttpGet("problems-task/{id:int}/fields")]
        public GetProblemOutput GetProblemsByProcess(int id)
        {
            var data = _problemsTaskService.GetByProcess(id);
            _fileUrlBuilder.SetAttachFileUrl(data);
            return data;
        }
        /// <summary>
        /// 发起新增存在问题的审批
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("problems-task")]
        public IActionResult StartProblemsProcess([FromBody]StartProcessInput<AddProblemInput> input)
        {
            var id = _problemsTaskService.Add(input);
            return Created("", new { id });
        }
        /// <summary>
        /// 发起更新存在问题审批
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("problems-task/{id:int}")]
        public IActionResult StartProblemsProcess(int id, [FromBody]StartProcessInput<UpdateProblemInput> input)
        {
            input.Data.Id = id;
            _problemsTaskService.Update(input);
            return Created("", new { id });
        }

        /// <summary>
        /// 查询当前流程的周报的字段
        /// </summary>
        /// <returns></returns>
        [HttpGet("weekly-progress/{id:int}/fields")]
        public GetWeeklyProgressOutput GetWeeklyProgressByProcess(int id)
        {
            var data = _weeklyProgressService.GetByProcess(id);
            _fileUrlBuilder.SetAttachFileUrl(data);
            return data;
        }
        /// <summary>
        /// 发起新增周报的审批
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("weekly-progress")]
        public IActionResult StartWeeklyProgressProcess([FromBody]StartProcessInput<AddWeeklyProgressInput> input)
        {
            var id = _weeklyProgressService.Add(input);
            return Created("", new { id });
        }
        /// <summary>
        /// 发起更新周报审批
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("weekly-progress/{id:int}")]
        public IActionResult StartWeeklyProgressProcess(int id, [FromBody]StartProcessInput<UpdateWeeklyProgressInput> input)
        {
            input.Data.Id = id;
            _weeklyProgressService.Update(input);
            return Created("", new { id });
        }

        /// <summary>
        /// 查询当前流程的月报的字段
        /// </summary>
        /// <returns></returns>
        [HttpGet("monthly-progress/{id:int}/fields")]
        public GetMonthlyProgressOutput GetMonthlyProgressByProcess(int id)
        {
            var data = _monthlyProgressService.GetByProcess(id);
            _fileUrlBuilder.SetAttachFileUrl(data);
            return data;
        }
        /// <summary>
        /// 发起新增月报的审批
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("monthly-progress")]
        public IActionResult StartMonthlyProgressProcess([FromBody]StartProcessInput<AddMonthlyProgressInput> input)
        {
            var id = _monthlyProgressService.Add(input);
            return Created("", new { id });
        }
        /// <summary>
        /// 发起更新月报审批
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("monthly-progress/{id:int}")]
        public IActionResult StartMonthlyProgressProcess(int id, [FromBody]StartProcessInput<UpdateMonthlyProgressInput> input)
        {
            input.Data.Id = id;
            _monthlyProgressService.Update(input);
            return Created("", new { id });
        }


        /// <summary>
        /// 查询当前流程的合同支付的字段
        /// </summary>
        /// <returns></returns>
        [HttpGet("contractpayment-task/{id:int}/fields")]
        public GetContractPaymentOutput GetPaymentByProcess(int id)
        {
            var data = _paymentService.GetByProcess(id);
            _fileUrlBuilder.SetAttachFileUrl(data);
            return data;
        }
        /// <summary>
        /// 发起新增合同支付的审批
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("contractpayment-task")]
        public IActionResult StartPaymentProcess([FromBody]StartProcessInput<AddContractPaymentInput> input)
        {
            var id = _paymentService.Add(input);
            return Created("", new { id });
        }
        /// <summary>
        /// 发起更新合同支付审批
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("contractpayment-task/{id:int}")]
        public IActionResult StartPaymentProcess(int id, [FromBody]StartProcessInput<UpdateContractPaymentInput> input)
        {
            input.Data.Id = id;
            _paymentService.Update(input);
            return Created("", new { id });
        }
    }
}
