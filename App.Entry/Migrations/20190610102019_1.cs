using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace App.Entry.Migrations
{
    public partial class _1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Configs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SortNo = table.Column<int>(nullable: false),
                    TenantId = table.Column<int>(nullable: false),
                    Key = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectLocations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: false),
                    Province = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    District = table.Column<string>(nullable: true),
                    Town = table.Column<string>(nullable: true),
                    AddressDetail = table.Column<string>(nullable: true),
                    Longitude = table.Column<float>(nullable: true),
                    Latitude = table.Column<float>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectLocations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Selections",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SortNo = table.Column<int>(nullable: false),
                    TenantId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Selections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Options",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SortNo = table.Column<int>(nullable: false),
                    TenantId = table.Column<int>(nullable: false),
                    SelectionId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    IsDisabled = table.Column<bool>(nullable: false),
                    Pid = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Options", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Options_Options_Pid",
                        column: x => x.Pid,
                        principalTable: "Options",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Options_Selections_SelectionId",
                        column: x => x.SelectionId,
                        principalTable: "Selections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
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
                    No = table.Column<string>(nullable: true),
                    CategoryId = table.Column<int>(nullable: true),
                    ProjectNatureId = table.Column<int>(nullable: true),
                    ConstructionNatureId = table.Column<int>(nullable: true),
                    StateId = table.Column<int>(nullable: true),
                    StageId = table.Column<int>(nullable: true),
                    FundsSourceId = table.Column<int>(nullable: true),
                    FinancialInvestment = table.Column<float>(nullable: false),
                    NonFinancialInvestment = table.Column<float>(nullable: false),
                    GeneralEstimate = table.Column<float>(nullable: false),
                    TentativeEstimatedTotalInvestment = table.Column<float>(nullable: false),
                    PlannedCommencementDate = table.Column<DateTime>(nullable: true),
                    ActualCommencementDate = table.Column<DateTime>(nullable: true),
                    PlannedCompletionDate = table.Column<DateTime>(nullable: true),
                    ActualCompletionDate = table.Column<DateTime>(nullable: true),
                    PlannedFinancialAccountsDate = table.Column<DateTime>(nullable: true),
                    ActualFinancialAccountsDate = table.Column<DateTime>(nullable: true),
                    PlannedFinishDate = table.Column<DateTime>(nullable: true),
                    ActualFinishDate = table.Column<DateTime>(nullable: true),
                    PlannedTransferDate = table.Column<DateTime>(nullable: true),
                    ActualTransferDate = table.Column<DateTime>(nullable: true),
                    PreparetoryWorkPlanDate = table.Column<DateTime>(nullable: true),
                    LocationId = table.Column<int>(nullable: true),
                    TagId = table.Column<int>(nullable: true),
                    ProprietorUnitId = table.Column<int>(nullable: true),
                    ConstructionAgentUnitId = table.Column<int>(nullable: true),
                    SupervisorUnitId = table.Column<int>(nullable: true),
                    ResponsibleUnitId = table.Column<int>(nullable: true),
                    Headquarters = table.Column<string>(nullable: true),
                    AffiliationUnit = table.Column<string>(nullable: true),
                    Summary = table.Column<string>(nullable: true),
                    Mid = table.Column<int>(nullable: true),
                    DataState = table.Column<int>(nullable: false),
                    ProcessInstanceId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Projects_Options_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Options",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Projects_Options_ConstructionAgentUnitId",
                        column: x => x.ConstructionAgentUnitId,
                        principalTable: "Options",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Projects_Options_ConstructionNatureId",
                        column: x => x.ConstructionNatureId,
                        principalTable: "Options",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Projects_Options_FundsSourceId",
                        column: x => x.FundsSourceId,
                        principalTable: "Options",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Projects_ProjectLocations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "ProjectLocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Projects_Projects_Mid",
                        column: x => x.Mid,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Projects_Wf_Hi_ProcessInstances_ProcessInstanceId",
                        column: x => x.ProcessInstanceId,
                        principalTable: "Wf_Hi_ProcessInstances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Projects_Options_ProjectNatureId",
                        column: x => x.ProjectNatureId,
                        principalTable: "Options",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Projects_Options_ProprietorUnitId",
                        column: x => x.ProprietorUnitId,
                        principalTable: "Options",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Projects_Options_ResponsibleUnitId",
                        column: x => x.ResponsibleUnitId,
                        principalTable: "Options",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Projects_Options_StageId",
                        column: x => x.StageId,
                        principalTable: "Options",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Projects_Options_StateId",
                        column: x => x.StateId,
                        principalTable: "Options",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Projects_Options_SupervisorUnitId",
                        column: x => x.SupervisorUnitId,
                        principalTable: "Options",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Projects_Options_TagId",
                        column: x => x.TagId,
                        principalTable: "Options",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProjectAttachments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: false),
                    ProjectId = table.Column<int>(nullable: false),
                    FileMetaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectAttachments_FileMetas_FileMetaId",
                        column: x => x.FileMetaId,
                        principalTable: "FileMetas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectAttachments_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectMembers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: false),
                    ProjectId = table.Column<int>(nullable: false),
                    ProjectRole = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectMembers_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Options_Pid",
                table: "Options",
                column: "Pid");

            migrationBuilder.CreateIndex(
                name: "IX_Options_SelectionId",
                table: "Options",
                column: "SelectionId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectAttachments_FileMetaId",
                table: "ProjectAttachments",
                column: "FileMetaId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectAttachments_ProjectId",
                table: "ProjectAttachments",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectMembers_ProjectId",
                table: "ProjectMembers",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_CategoryId",
                table: "Projects",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ConstructionAgentUnitId",
                table: "Projects",
                column: "ConstructionAgentUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ConstructionNatureId",
                table: "Projects",
                column: "ConstructionNatureId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_FundsSourceId",
                table: "Projects",
                column: "FundsSourceId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_LocationId",
                table: "Projects",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_Mid",
                table: "Projects",
                column: "Mid");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ProcessInstanceId",
                table: "Projects",
                column: "ProcessInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ProjectNatureId",
                table: "Projects",
                column: "ProjectNatureId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ProprietorUnitId",
                table: "Projects",
                column: "ProprietorUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ResponsibleUnitId",
                table: "Projects",
                column: "ResponsibleUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_StageId",
                table: "Projects",
                column: "StageId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_StateId",
                table: "Projects",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_SupervisorUnitId",
                table: "Projects",
                column: "SupervisorUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_TagId",
                table: "Projects",
                column: "TagId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Configs");

            migrationBuilder.DropTable(
                name: "ProjectAttachments");

            migrationBuilder.DropTable(
                name: "ProjectMembers");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "Options");

            migrationBuilder.DropTable(
                name: "ProjectLocations");

            migrationBuilder.DropTable(
                name: "Selections");
        }
    }
}
