using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace App.Entry.Migrations
{
    public partial class _33 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PmMonthlyProgresses",
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
                    ProjectId = table.Column<int>(nullable: false),
                    RecordDate = table.Column<DateTime>(nullable: false),
                    CompletedDemolitionFee = table.Column<decimal>(type: "decimal(15,2)", nullable: false),
                    CompletedProjectCosts = table.Column<decimal>(type: "decimal(15,2)", nullable: false),
                    AccumulatedImageProgress = table.Column<string>(nullable: true),
                    ImageProgress = table.Column<string>(nullable: true),
                    Supervision = table.Column<string>(nullable: true),
                    NextMonthPlannedImageProgress = table.Column<string>(nullable: true),
                    Mid = table.Column<int>(nullable: true),
                    DataState = table.Column<int>(nullable: false),
                    ProcessInstanceId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PmMonthlyProgresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PmMonthlyProgresses_Projects_Mid",
                        column: x => x.Mid,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PmMonthlyProgresses_Wf_Hi_ProcessInstances_ProcessInstanceId",
                        column: x => x.ProcessInstanceId,
                        principalTable: "Wf_Hi_ProcessInstances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PmMonthlyProgresses_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PmMonthlyProgresses_Mid",
                table: "PmMonthlyProgresses",
                column: "Mid");

            migrationBuilder.CreateIndex(
                name: "IX_PmMonthlyProgresses_ProcessInstanceId",
                table: "PmMonthlyProgresses",
                column: "ProcessInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_PmMonthlyProgresses_ProjectId",
                table: "PmMonthlyProgresses",
                column: "ProjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PmMonthlyProgresses");
        }
    }
}
