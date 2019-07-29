using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace App.Entry.Migrations
{
    public partial class _22 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Problems",
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
                    CategoryId = table.Column<int>(nullable: false),
                    Content = table.Column<string>(nullable: true),
                    ProposalSolution = table.Column<string>(nullable: true),
                    PlannedCompletionTime = table.Column<DateTime>(nullable: true),
                    CoordinationState = table.Column<int>(nullable: false),
                    ActualCompletionTime = table.Column<DateTime>(nullable: true),
                    Mid = table.Column<int>(nullable: true),
                    State = table.Column<int>(nullable: false),
                    ProcessInstanceId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Problems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Problems_Options_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Options",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Problems_Problems_Mid",
                        column: x => x.Mid,
                        principalTable: "Problems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Problems_Wf_Hi_ProcessInstances_ProcessInstanceId",
                        column: x => x.ProcessInstanceId,
                        principalTable: "Wf_Hi_ProcessInstances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Problems_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProblemAttachments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProblemId = table.Column<int>(nullable: false),
                    FileMetaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProblemAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProblemAttachments_FileMetas_FileMetaId",
                        column: x => x.FileMetaId,
                        principalTable: "FileMetas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProblemAttachments_Problems_ProblemId",
                        column: x => x.ProblemId,
                        principalTable: "Problems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProblemCoordinations",
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
                    ProblemId = table.Column<int>(nullable: false),
                    ExecDate = table.Column<DateTime>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProblemCoordinations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProblemCoordinations_Problems_ProblemId",
                        column: x => x.ProblemId,
                        principalTable: "Problems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProblemCoordinationAttachments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProblemCoordinationId = table.Column<int>(nullable: false),
                    FileMetaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProblemCoordinationAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProblemCoordinationAttachments_FileMetas_FileMetaId",
                        column: x => x.FileMetaId,
                        principalTable: "FileMetas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProblemCoordinationAttachments_ProblemCoordinations_ProblemC~",
                        column: x => x.ProblemCoordinationId,
                        principalTable: "ProblemCoordinations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProblemAttachments_FileMetaId",
                table: "ProblemAttachments",
                column: "FileMetaId");

            migrationBuilder.CreateIndex(
                name: "IX_ProblemAttachments_ProblemId",
                table: "ProblemAttachments",
                column: "ProblemId");

            migrationBuilder.CreateIndex(
                name: "IX_ProblemCoordinationAttachments_FileMetaId",
                table: "ProblemCoordinationAttachments",
                column: "FileMetaId");

            migrationBuilder.CreateIndex(
                name: "IX_ProblemCoordinationAttachments_ProblemCoordinationId",
                table: "ProblemCoordinationAttachments",
                column: "ProblemCoordinationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProblemCoordinations_ProblemId",
                table: "ProblemCoordinations",
                column: "ProblemId");

            migrationBuilder.CreateIndex(
                name: "IX_Problems_CategoryId",
                table: "Problems",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Problems_Mid",
                table: "Problems",
                column: "Mid");

            migrationBuilder.CreateIndex(
                name: "IX_Problems_ProcessInstanceId",
                table: "Problems",
                column: "ProcessInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_Problems_ProjectId",
                table: "Problems",
                column: "ProjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProblemAttachments");

            migrationBuilder.DropTable(
                name: "ProblemCoordinationAttachments");

            migrationBuilder.DropTable(
                name: "ProblemCoordinations");

            migrationBuilder.DropTable(
                name: "Problems");
        }
    }
}
