using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace App.Entry.Migrations
{
    public partial class _19 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EarlyStages",
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
                    NodeId = table.Column<int>(nullable: false),
                    TypeName = table.Column<string>(nullable: true),
                    ReplyNumber = table.Column<string>(nullable: true),
                    ReportDate = table.Column<DateTime>(nullable: true),
                    ReplyDate = table.Column<DateTime>(nullable: true),
                    Mid = table.Column<int>(nullable: true),
                    State = table.Column<int>(nullable: false),
                    ProcessInstanceId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EarlyStages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EarlyStages_EarlyStages_Mid",
                        column: x => x.Mid,
                        principalTable: "EarlyStages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EarlyStages_Options_NodeId",
                        column: x => x.NodeId,
                        principalTable: "Options",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EarlyStages_Wf_Hi_ProcessInstances_ProcessInstanceId",
                        column: x => x.ProcessInstanceId,
                        principalTable: "Wf_Hi_ProcessInstances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EarlyStages_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EarlyStageAttachments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EarlyStageId = table.Column<int>(nullable: false),
                    FileMetaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EarlyStageAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EarlyStageAttachments_EarlyStages_EarlyStageId",
                        column: x => x.EarlyStageId,
                        principalTable: "EarlyStages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EarlyStageAttachments_FileMetas_FileMetaId",
                        column: x => x.FileMetaId,
                        principalTable: "FileMetas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EarlyStageAttachments_EarlyStageId",
                table: "EarlyStageAttachments",
                column: "EarlyStageId");

            migrationBuilder.CreateIndex(
                name: "IX_EarlyStageAttachments_FileMetaId",
                table: "EarlyStageAttachments",
                column: "FileMetaId");

            migrationBuilder.CreateIndex(
                name: "IX_EarlyStages_Mid",
                table: "EarlyStages",
                column: "Mid");

            migrationBuilder.CreateIndex(
                name: "IX_EarlyStages_NodeId",
                table: "EarlyStages",
                column: "NodeId");

            migrationBuilder.CreateIndex(
                name: "IX_EarlyStages_ProcessInstanceId",
                table: "EarlyStages",
                column: "ProcessInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_EarlyStages_ProjectId",
                table: "EarlyStages",
                column: "ProjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EarlyStageAttachments");

            migrationBuilder.DropTable(
                name: "EarlyStages");
        }
    }
}
