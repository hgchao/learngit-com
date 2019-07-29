using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace App.Entry.Migrations
{
    public partial class b : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FormDefinition",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatorId = table.Column<int>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    DeleterId = table.Column<int>(nullable: true),
                    LastModifierId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: false),
                    Version = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Category = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormDefinition", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Wf_Hi_Variables",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatorId = table.Column<int>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    DeleterId = table.Column<int>(nullable: true),
                    LastModifierId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    ShortVar = table.Column<short>(nullable: true),
                    IntVar = table.Column<int>(nullable: true),
                    LongVar = table.Column<long>(nullable: true),
                    FloatVar = table.Column<float>(nullable: true),
                    StringVar = table.Column<string>(nullable: true),
                    DateTimeVar = table.Column<DateTime>(nullable: true),
                    BoolVar = table.Column<bool>(nullable: true),
                    ObjectVar = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    ProcessDefinitionId = table.Column<int>(nullable: false),
                    ProcessInstanceId = table.Column<int>(nullable: false),
                    TaskInstanceId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wf_Hi_Variables", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Wf_Re_ProcessModels",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatorId = table.Column<int>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    DeleterId = table.Column<int>(nullable: true),
                    LastModifierId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: false),
                    Path = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wf_Re_ProcessModels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FieldDefinition",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatorId = table.Column<int>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    DeleterId = table.Column<int>(nullable: true),
                    LastModifierId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: false),
                    FormDefinitionId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    Remark = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldDefinition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FieldDefinition_FormDefinition_FormDefinitionId",
                        column: x => x.FormDefinitionId,
                        principalTable: "FormDefinition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FormModel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatorId = table.Column<int>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    DeleterId = table.Column<int>(nullable: true),
                    LastModifierId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: false),
                    FormDefinitionId = table.Column<int>(nullable: false),
                    Data = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormModel_FormDefinition_FormDefinitionId",
                        column: x => x.FormDefinitionId,
                        principalTable: "FormDefinition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Wf_Re_ProcessDefinitions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatorId = table.Column<int>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    DeleterId = table.Column<int>(nullable: true),
                    LastModifierId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Version = table.Column<int>(nullable: false),
                    Category = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    FormType = table.Column<string>(nullable: true),
                    FormName = table.Column<string>(nullable: true),
                    ModelId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wf_Re_ProcessDefinitions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Wf_Re_ProcessDefinitions_Wf_Re_ProcessModels_ModelId",
                        column: x => x.ModelId,
                        principalTable: "Wf_Re_ProcessModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoleProcessDefinitions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: false),
                    ProcessDefinitionId = table.Column<int>(nullable: false),
                    RoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleProcessDefinitions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleProcessDefinitions_Wf_Re_ProcessDefinitions_ProcessDefin~",
                        column: x => x.ProcessDefinitionId,
                        principalTable: "Wf_Re_ProcessDefinitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleProcessDefinitions_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Wf_Hi_ProcessInstances",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatorId = table.Column<int>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    DeleterId = table.Column<int>(nullable: true),
                    LastModifierId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    ProcessDefinitionId = table.Column<int>(nullable: false),
                    FormDefinitionId = table.Column<int>(nullable: true),
                    StartTime = table.Column<DateTime>(nullable: true),
                    EndTime = table.Column<DateTime>(nullable: true),
                    Duration = table.Column<long>(nullable: true),
                    StartNodeUid = table.Column<string>(nullable: true),
                    StartNodeName = table.Column<string>(nullable: true),
                    EndNodeUid = table.Column<string>(nullable: true),
                    EndNodeName = table.Column<string>(nullable: true),
                    Pid = table.Column<int>(nullable: true),
                    State = table.Column<int>(nullable: false),
                    IsAutoCreated = table.Column<bool>(nullable: false),
                    StopReason = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wf_Hi_ProcessInstances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Wf_Hi_ProcessInstances_FormDefinition_FormDefinitionId",
                        column: x => x.FormDefinitionId,
                        principalTable: "FormDefinition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Wf_Hi_ProcessInstances_Wf_Hi_ProcessInstances_Pid",
                        column: x => x.Pid,
                        principalTable: "Wf_Hi_ProcessInstances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Wf_Hi_ProcessInstances_Wf_Re_ProcessDefinitions_ProcessDefin~",
                        column: x => x.ProcessDefinitionId,
                        principalTable: "Wf_Re_ProcessDefinitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Field",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatorId = table.Column<int>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    DeleterId = table.Column<int>(nullable: true),
                    LastModifierId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: false),
                    FormDefinitionId = table.Column<int>(nullable: false),
                    Remark = table.Column<string>(nullable: true),
                    FieldDefinitionId = table.Column<int>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    ShortVar = table.Column<short>(nullable: true),
                    IntVar = table.Column<int>(nullable: true),
                    LongVar = table.Column<long>(nullable: true),
                    FloatVar = table.Column<float>(nullable: true),
                    StringVar = table.Column<string>(nullable: true),
                    DateTimeVar = table.Column<DateTime>(nullable: true),
                    BoolVar = table.Column<bool>(nullable: true),
                    ObjectVar = table.Column<string>(nullable: true),
                    Wf_Hi_ProcessInstanceId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Field", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Field_FieldDefinition_FieldDefinitionId",
                        column: x => x.FieldDefinitionId,
                        principalTable: "FieldDefinition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Field_FormDefinition_FormDefinitionId",
                        column: x => x.FormDefinitionId,
                        principalTable: "FormDefinition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Field_Wf_Hi_ProcessInstances_Wf_Hi_ProcessInstanceId",
                        column: x => x.Wf_Hi_ProcessInstanceId,
                        principalTable: "Wf_Hi_ProcessInstances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Wf_Hi_TaskInstances",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatorId = table.Column<int>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    DeleterId = table.Column<int>(nullable: true),
                    LastModifierId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: false),
                    NodeUid = table.Column<string>(nullable: true),
                    NodeName = table.Column<string>(nullable: true),
                    ProcessDefinitionId = table.Column<int>(nullable: false),
                    ProcessInstanceId = table.Column<int>(nullable: false),
                    Owner = table.Column<string>(nullable: true),
                    Assignee = table.Column<string>(nullable: true),
                    StartTime = table.Column<DateTime>(nullable: true),
                    EndTime = table.Column<DateTime>(nullable: true),
                    Duration = table.Column<long>(nullable: true),
                    TimeLimit = table.Column<int>(nullable: false),
                    NonExecution = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wf_Hi_TaskInstances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Wf_Hi_TaskInstances_Wf_Hi_ProcessInstances_ProcessInstanceId",
                        column: x => x.ProcessInstanceId,
                        principalTable: "Wf_Hi_ProcessInstances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Wf_Ru_Executions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: false),
                    ProcessDefinitionId = table.Column<int>(nullable: false),
                    ProcessInstanceId = table.Column<int>(nullable: false),
                    NodeUid = table.Column<string>(nullable: true),
                    NodeName = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsConcurrent = table.Column<bool>(nullable: false),
                    IsSuspension = table.Column<bool>(nullable: false),
                    Pid = table.Column<int>(nullable: true),
                    CreatorId = table.Column<int>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wf_Ru_Executions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Wf_Ru_Executions_Wf_Ru_Executions_Pid",
                        column: x => x.Pid,
                        principalTable: "Wf_Ru_Executions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Wf_Ru_Executions_Wf_Hi_ProcessInstances_ProcessInstanceId",
                        column: x => x.ProcessInstanceId,
                        principalTable: "Wf_Hi_ProcessInstances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Wf_Hi_Activities",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatorId = table.Column<int>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    DeleterId = table.Column<int>(nullable: true),
                    LastModifierId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: false),
                    ProcessDefinitionId = table.Column<int>(nullable: false),
                    ProcessInstanceId = table.Column<int>(nullable: false),
                    NodeUid = table.Column<string>(nullable: true),
                    NodeName = table.Column<string>(nullable: true),
                    TaskInstanceId = table.Column<int>(nullable: true),
                    NodeType = table.Column<string>(nullable: true),
                    StartTime = table.Column<DateTime>(nullable: true),
                    EndTime = table.Column<DateTime>(nullable: true),
                    Duration = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wf_Hi_Activities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Wf_Hi_Activities_Wf_Hi_ProcessInstances_ProcessInstanceId",
                        column: x => x.ProcessInstanceId,
                        principalTable: "Wf_Hi_ProcessInstances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Wf_Hi_Activities_Wf_Hi_TaskInstances_TaskInstanceId",
                        column: x => x.TaskInstanceId,
                        principalTable: "Wf_Hi_TaskInstances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Wf_Hi_Comments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatorId = table.Column<int>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    DeleterId = table.Column<int>(nullable: true),
                    LastModifierId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: false),
                    ProcessDefinitionId = table.Column<int>(nullable: false),
                    ProcessInstanceId = table.Column<int>(nullable: false),
                    TaskInstanceId = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    Message = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wf_Hi_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Wf_Hi_Comments_Wf_Hi_ProcessInstances_ProcessInstanceId",
                        column: x => x.ProcessInstanceId,
                        principalTable: "Wf_Hi_ProcessInstances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Wf_Hi_Comments_Wf_Hi_TaskInstances_TaskInstanceId",
                        column: x => x.TaskInstanceId,
                        principalTable: "Wf_Hi_TaskInstances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Wf_Hi_IdentityLink",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: false),
                    TaskInstanceId = table.Column<int>(nullable: false),
                    TaskId = table.Column<int>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    User = table.Column<string>(nullable: true),
                    Group = table.Column<string>(nullable: true),
                    CreatorId = table.Column<int>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wf_Hi_IdentityLink", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Wf_Hi_IdentityLink_Wf_Hi_TaskInstances_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Wf_Hi_TaskInstances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Wf_Ru_Tasks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: false),
                    ProcessDefinitionId = table.Column<int>(nullable: false),
                    ProcessInstanceId = table.Column<int>(nullable: false),
                    TaskInstanceId = table.Column<int>(nullable: false),
                    ProcessExecutionId = table.Column<int>(nullable: false),
                    NodeUid = table.Column<string>(nullable: true),
                    NodeName = table.Column<string>(nullable: true),
                    ParentInstanceId = table.Column<int>(nullable: true),
                    Owner = table.Column<string>(nullable: true),
                    Assignee = table.Column<string>(nullable: true),
                    DelegationState = table.Column<int>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    TimeLimit = table.Column<int>(nullable: false),
                    CreatorId = table.Column<int>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wf_Ru_Tasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Wf_Ru_Tasks_Wf_Ru_Executions_ProcessExecutionId",
                        column: x => x.ProcessExecutionId,
                        principalTable: "Wf_Ru_Executions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Wf_Ru_Tasks_Wf_Hi_ProcessInstances_ProcessInstanceId",
                        column: x => x.ProcessInstanceId,
                        principalTable: "Wf_Hi_ProcessInstances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Wf_Ru_Variables",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    ShortVar = table.Column<short>(nullable: true),
                    IntVar = table.Column<int>(nullable: true),
                    LongVar = table.Column<long>(nullable: true),
                    FloatVar = table.Column<float>(nullable: true),
                    StringVar = table.Column<string>(nullable: true),
                    DateTimeVar = table.Column<DateTime>(nullable: true),
                    BoolVar = table.Column<bool>(nullable: true),
                    ObjectVar = table.Column<string>(nullable: true),
                    CreatorId = table.Column<int>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    ProcessDefinitionId = table.Column<int>(nullable: false),
                    ProcessInstanceId = table.Column<int>(nullable: false),
                    ProcessExecutionId = table.Column<int>(nullable: true),
                    TaskInstanceId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wf_Ru_Variables", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Wf_Ru_Variables_Wf_Ru_Executions_ProcessExecutionId",
                        column: x => x.ProcessExecutionId,
                        principalTable: "Wf_Ru_Executions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Wf_Ru_Variables_Wf_Hi_ProcessInstances_ProcessInstanceId",
                        column: x => x.ProcessInstanceId,
                        principalTable: "Wf_Hi_ProcessInstances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Field_FieldDefinitionId",
                table: "Field",
                column: "FieldDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_Field_FormDefinitionId",
                table: "Field",
                column: "FormDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_Field_Wf_Hi_ProcessInstanceId",
                table: "Field",
                column: "Wf_Hi_ProcessInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_FieldDefinition_FormDefinitionId",
                table: "FieldDefinition",
                column: "FormDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_FormModel_FormDefinitionId",
                table: "FormModel",
                column: "FormDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleProcessDefinitions_ProcessDefinitionId",
                table: "RoleProcessDefinitions",
                column: "ProcessDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleProcessDefinitions_RoleId",
                table: "RoleProcessDefinitions",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Wf_Hi_Activities_ProcessInstanceId",
                table: "Wf_Hi_Activities",
                column: "ProcessInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_Wf_Hi_Activities_TaskInstanceId",
                table: "Wf_Hi_Activities",
                column: "TaskInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_Wf_Hi_Comments_ProcessInstanceId",
                table: "Wf_Hi_Comments",
                column: "ProcessInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_Wf_Hi_Comments_TaskInstanceId",
                table: "Wf_Hi_Comments",
                column: "TaskInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_Wf_Hi_IdentityLink_TaskId",
                table: "Wf_Hi_IdentityLink",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Wf_Hi_ProcessInstances_FormDefinitionId",
                table: "Wf_Hi_ProcessInstances",
                column: "FormDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_Wf_Hi_ProcessInstances_Pid",
                table: "Wf_Hi_ProcessInstances",
                column: "Pid");

            migrationBuilder.CreateIndex(
                name: "IX_Wf_Hi_ProcessInstances_ProcessDefinitionId",
                table: "Wf_Hi_ProcessInstances",
                column: "ProcessDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_Wf_Hi_TaskInstances_ProcessInstanceId",
                table: "Wf_Hi_TaskInstances",
                column: "ProcessInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_Wf_Re_ProcessDefinitions_ModelId",
                table: "Wf_Re_ProcessDefinitions",
                column: "ModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Wf_Ru_Executions_Pid",
                table: "Wf_Ru_Executions",
                column: "Pid");

            migrationBuilder.CreateIndex(
                name: "IX_Wf_Ru_Executions_ProcessInstanceId",
                table: "Wf_Ru_Executions",
                column: "ProcessInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_Wf_Ru_Tasks_ProcessExecutionId",
                table: "Wf_Ru_Tasks",
                column: "ProcessExecutionId");

            migrationBuilder.CreateIndex(
                name: "IX_Wf_Ru_Tasks_ProcessInstanceId",
                table: "Wf_Ru_Tasks",
                column: "ProcessInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_Wf_Ru_Variables_ProcessExecutionId",
                table: "Wf_Ru_Variables",
                column: "ProcessExecutionId");

            migrationBuilder.CreateIndex(
                name: "IX_Wf_Ru_Variables_ProcessInstanceId",
                table: "Wf_Ru_Variables",
                column: "ProcessInstanceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Field");

            migrationBuilder.DropTable(
                name: "FormModel");

            migrationBuilder.DropTable(
                name: "RoleProcessDefinitions");

            migrationBuilder.DropTable(
                name: "Wf_Hi_Activities");

            migrationBuilder.DropTable(
                name: "Wf_Hi_Comments");

            migrationBuilder.DropTable(
                name: "Wf_Hi_IdentityLink");

            migrationBuilder.DropTable(
                name: "Wf_Hi_Variables");

            migrationBuilder.DropTable(
                name: "Wf_Ru_Tasks");

            migrationBuilder.DropTable(
                name: "Wf_Ru_Variables");

            migrationBuilder.DropTable(
                name: "FieldDefinition");

            migrationBuilder.DropTable(
                name: "Wf_Hi_TaskInstances");

            migrationBuilder.DropTable(
                name: "Wf_Ru_Executions");

            migrationBuilder.DropTable(
                name: "Wf_Hi_ProcessInstances");

            migrationBuilder.DropTable(
                name: "FormDefinition");

            migrationBuilder.DropTable(
                name: "Wf_Re_ProcessDefinitions");

            migrationBuilder.DropTable(
                name: "Wf_Re_ProcessModels");
        }
    }
}
