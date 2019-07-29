using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace App.Entry.Migrations
{
    public partial class _23 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProjectBriefings",
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
                    Version = table.Column<int>(nullable: false),
                    ProgressLimitDate = table.Column<string>(nullable: true),
                    ThisWeekProgress = table.Column<string>(nullable: true),
                    CumulativeImageProgress = table.Column<string>(nullable: true),
                    NextWeekProgressPlan = table.Column<string>(nullable: true),
                    ProblemAndSolution = table.Column<string>(nullable: true),
                    Supervision = table.Column<string>(nullable: true),
                    QualitySourceAndDescription = table.Column<string>(nullable: true),
                    SafetySourceAndDescription = table.Column<string>(nullable: true),
                    HousekeepingConetent = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectBriefings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectBriefings_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectBriefings_ProjectId",
                table: "ProjectBriefings",
                column: "ProjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectBriefings");
        }
    }
}
