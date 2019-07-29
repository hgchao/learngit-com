using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace App.Entry.Migrations
{
    public partial class _10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HousekeepingProblems",
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
                    Content = table.Column<string>(nullable: true),
                    RectificationState = table.Column<int>(nullable: false),
                    Deadline = table.Column<DateTime>(nullable: false),
                    CompletionTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HousekeepingProblems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HousekeepingProblems_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HousekeepingCompletionAttachments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    HousekeepingProblemId = table.Column<int>(nullable: false),
                    FileMetaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HousekeepingCompletionAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HousekeepingCompletionAttachments_FileMetas_FileMetaId",
                        column: x => x.FileMetaId,
                        principalTable: "FileMetas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HousekeepingCompletionAttachments_HousekeepingProblems_House~",
                        column: x => x.HousekeepingProblemId,
                        principalTable: "HousekeepingProblems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HousekeepingProblemAttachments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    HousekeepingProblemId = table.Column<int>(nullable: false),
                    FileMetaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HousekeepingProblemAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HousekeepingProblemAttachments_FileMetas_FileMetaId",
                        column: x => x.FileMetaId,
                        principalTable: "FileMetas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HousekeepingProblemAttachments_HousekeepingProblems_Housekee~",
                        column: x => x.HousekeepingProblemId,
                        principalTable: "HousekeepingProblems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HousekeepingProblemRectifications",
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
                    HousekeepingProblemId = table.Column<int>(nullable: false),
                    ExecDate = table.Column<DateTime>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HousekeepingProblemRectifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HousekeepingProblemRectifications_HousekeepingProblems_House~",
                        column: x => x.HousekeepingProblemId,
                        principalTable: "HousekeepingProblems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HousekeepingProblemRectificationAttachments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    HousekeepingProblemRectificationId = table.Column<int>(nullable: false),
                    FileMetaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HousekeepingProblemRectificationAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HousekeepingProblemRectificationAttachments_FileMetas_FileMe~",
                        column: x => x.FileMetaId,
                        principalTable: "FileMetas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HousekeepingProblemRectificationAttachments_HousekeepingProb~",
                        column: x => x.HousekeepingProblemRectificationId,
                        principalTable: "HousekeepingProblemRectifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HousekeepingCompletionAttachments_FileMetaId",
                table: "HousekeepingCompletionAttachments",
                column: "FileMetaId");

            migrationBuilder.CreateIndex(
                name: "IX_HousekeepingCompletionAttachments_HousekeepingProblemId",
                table: "HousekeepingCompletionAttachments",
                column: "HousekeepingProblemId");

            migrationBuilder.CreateIndex(
                name: "IX_HousekeepingProblemAttachments_FileMetaId",
                table: "HousekeepingProblemAttachments",
                column: "FileMetaId");

            migrationBuilder.CreateIndex(
                name: "IX_HousekeepingProblemAttachments_HousekeepingProblemId",
                table: "HousekeepingProblemAttachments",
                column: "HousekeepingProblemId");

            migrationBuilder.CreateIndex(
                name: "IX_HousekeepingProblemRectificationAttachments_FileMetaId",
                table: "HousekeepingProblemRectificationAttachments",
                column: "FileMetaId");

            migrationBuilder.CreateIndex(
                name: "IX_HousekeepingProblemRectificationAttachments_HousekeepingProb~",
                table: "HousekeepingProblemRectificationAttachments",
                column: "HousekeepingProblemRectificationId");

            migrationBuilder.CreateIndex(
                name: "IX_HousekeepingProblemRectifications_HousekeepingProblemId",
                table: "HousekeepingProblemRectifications",
                column: "HousekeepingProblemId");

            migrationBuilder.CreateIndex(
                name: "IX_HousekeepingProblems_ProjectId",
                table: "HousekeepingProblems",
                column: "ProjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HousekeepingCompletionAttachments");

            migrationBuilder.DropTable(
                name: "HousekeepingProblemAttachments");

            migrationBuilder.DropTable(
                name: "HousekeepingProblemRectificationAttachments");

            migrationBuilder.DropTable(
                name: "HousekeepingProblemRectifications");

            migrationBuilder.DropTable(
                name: "HousekeepingProblems");
        }
    }
}
