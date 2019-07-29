using App.Core.Authorization;
using App.Core.Messaging;
using App.Core.Workflow;
using App.Base;
using App.Workflow.Tasks;
using App.Workflow.Tasks.AssigneeProviders;
using PoorFff.Modules;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using App.Workflow.ProcessInstances;

namespace App.Workflow
{
    [DependsOn(
        typeof(AppBaseModule)
        )]
    public class AppWorkflowModule: PfModuleBase
    {
        public override void Initialize()
        {
            IocManager.RegisterByConvention(GetAssembly());
            IocManager.Register<IAssigneeProvider, AssigneeProvider>();
        }

        public override void PostInitialize()
        {
            var wfEngine = IocManager.Resolve<IWfEngine>();
            var messagingProvider = IocManager.Resolve<IMessagingProvider>();
            var assigneeProvider = IocManager.Resolve<IAssigneeProvider>();

            wfEngine.GetRuntimeProvider().AddGetUsersDelegate((process, assigneeConfig) =>
            {
                List<string> users = new List<string>();
                if (!string.IsNullOrEmpty(assigneeConfig.Content))
                {
                    switch (assigneeConfig.Type)
                    {
                        case AssigneeConfigType.Role:
                            var res = assigneeProvider.GetRoleUser(process, Convert.ToInt32(assigneeConfig.Content));
                            users.AddRange(res);
                            break;
                        case AssigneeConfigType.Department:
                            var res3 = assigneeProvider.GetDepartmentUser(process, Convert.ToInt32(assigneeConfig.Content));
                            users.AddRange(res3);
                            break;
                        case AssigneeConfigType.SuperiorOfNodeAssignee:
                            var res2 = assigneeProvider.GetSuperiorUserList(process, assigneeConfig.Content);
                            users.Add(res2);
                            break;
                    }
                }
                return users;
            });
            wfEngine.GetRuntimeProvider().AddProcessCompletedEventHandler(process =>
            {
                // 编辑
                if (process.Pid != null)
                {
                    var fields = wfEngine.GetRuntimeProvider().GetFields(process.Id);
                    wfEngine.GetRuntimeProvider().SetFields(process.Pid.Value, fields);
                }
                else if (process.ProcessDefinition.NextProcesses.Count > 0)
                {
                    var nextProcesses = process.ProcessDefinition.NextProcesses;
                    foreach(var nextProcess in nextProcesses)
                    {
                        var fields = process.Fields.Where(u => nextProcess.CopyFields.Any(v => v == u.Name)).ToList();
                        IocManager.Resolve<IProcessInstanceService>().Start(new ProcessInstances.Dto.StartProcessInput
                        {
                            ProcessDefinitionName = nextProcess.Name,
                            FormContents = fields
                        },
                        process.Id,
                        nextProcess.AutoExecuteFirstTask);
                    }
                }

            });


        }
    }
}
