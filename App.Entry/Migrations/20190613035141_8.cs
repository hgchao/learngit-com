using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace App.Entry.Migrations
{
    public partial class _8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SafetyAccidents",
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
                    SourceId = table.Column<int>(nullable: false),
                    CategoryId = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    DiscoveryTime = table.Column<DateTime>(nullable: false),
                    DisposalState = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    SeverityId = table.Column<int>(nullable: false),
                    InjuredNumber = table.Column<int>(nullable: false),
                    DeathNumber = table.Column<int>(nullable: false),
                    SettlementTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SafetyAccidents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SafetyAccidents_Options_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Options",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SafetyAccidents_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SafetyAccidents_Options_SeverityId",
                        column: x => x.SeverityId,
                        principalTable: "Options",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SafetyAccidents_Options_SourceId",
                        column: x => x.SourceId,
                        principalTable: "Options",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SafetyProblems",
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
                    SourceId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    RectificationState = table.Column<int>(nullable: false),
                    Deadline = table.Column<DateTime>(nullable: false),
                    CompletionTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SafetyProblems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SafetyProblems_Options_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Options",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SafetyProblems_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SafetyProblems_Options_SourceId",
                        column: x => x.SourceId,
                        principalTable: "Options",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SafetyStandards",
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
                    Title = table.Column<string>(nullable: true),
                    CategoryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SafetyStandards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SafetyStandards_Options_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Options",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SafetyAccidentAttachments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SafetyAccidentId = table.Column<int>(nullable: false),
                    FileMetaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SafetyAccidentAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SafetyAccidentAttachments_FileMetas_FileMetaId",
                        column: x => x.FileMetaId,
                        principalTable: "FileMetas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SafetyAccidentAttachments_SafetyAccidents_SafetyAccidentId",
                        column: x => x.SafetyAccidentId,
                        principalTable: "SafetyAccidents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SafetyAccidentDisposals",
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
                    SafetyAccidentId = table.Column<int>(nullable: false),
                    ExecDate = table.Column<DateTime>(nullable: true),
                    Solution = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SafetyAccidentDisposals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SafetyAccidentDisposals_SafetyAccidents_SafetyAccidentId",
                        column: x => x.SafetyAccidentId,
                        principalTable: "SafetyAccidents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SafetySettlementAttachments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SafetyAccidentId = table.Column<int>(nullable: false),
                    FileMetaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SafetySettlementAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SafetySettlementAttachments_FileMetas_FileMetaId",
                        column: x => x.FileMetaId,
                        principalTable: "FileMetas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SafetySettlementAttachments_SafetyAccidents_SafetyAccidentId",
                        column: x => x.SafetyAccidentId,
                        principalTable: "SafetyAccidents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SafetyCompletionAttachments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SafetyProblemId = table.Column<int>(nullable: false),
                    FileMetaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SafetyCompletionAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SafetyCompletionAttachments_FileMetas_FileMetaId",
                        column: x => x.FileMetaId,
                        principalTable: "FileMetas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SafetyCompletionAttachments_SafetyProblems_SafetyProblemId",
                        column: x => x.SafetyProblemId,
                        principalTable: "SafetyProblems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SafetyProblemAttachments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SafetyProblemId = table.Column<int>(nullable: false),
                    FileMetaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SafetyProblemAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SafetyProblemAttachments_FileMetas_FileMetaId",
                        column: x => x.FileMetaId,
                        principalTable: "FileMetas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SafetyProblemAttachments_SafetyProblems_SafetyProblemId",
                        column: x => x.SafetyProblemId,
                        principalTable: "SafetyProblems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SafetyProblemRectifications",
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
                    SafetyProblemId = table.Column<int>(nullable: false),
                    ExecDate = table.Column<DateTime>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SafetyProblemRectifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SafetyProblemRectifications_SafetyProblems_SafetyProblemId",
                        column: x => x.SafetyProblemId,
                        principalTable: "SafetyProblems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SafetyStandardAttachments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SafetyStandardId = table.Column<int>(nullable: false),
                    FileMetaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SafetyStandardAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SafetyStandardAttachments_FileMetas_FileMetaId",
                        column: x => x.FileMetaId,
                        principalTable: "FileMetas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SafetyStandardAttachments_SafetyStandards_SafetyStandardId",
                        column: x => x.SafetyStandardId,
                        principalTable: "SafetyStandards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SafetyAccidentDisposalAttachments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SafetyAccidentDisposalId = table.Column<int>(nullable: false),
                    FileMetaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SafetyAccidentDisposalAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SafetyAccidentDisposalAttachments_FileMetas_FileMetaId",
                        column: x => x.FileMetaId,
                        principalTable: "FileMetas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SafetyAccidentDisposalAttachments_SafetyAccidentDisposals_Sa~",
                        column: x => x.SafetyAccidentDisposalId,
                        principalTable: "SafetyAccidentDisposals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SafetyProblemRectificationAttachments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SafetyProblemRectificationId = table.Column<int>(nullable: false),
                    FileMetaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SafetyProblemRectificationAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SafetyProblemRectificationAttachments_FileMetas_FileMetaId",
                        column: x => x.FileMetaId,
                        principalTable: "FileMetas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SafetyProblemRectificationAttachments_SafetyProblemRectifica~",
                        column: x => x.SafetyProblemRectificationId,
                        principalTable: "SafetyProblemRectifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SafetyAccidentAttachments_FileMetaId",
                table: "SafetyAccidentAttachments",
                column: "FileMetaId");

            migrationBuilder.CreateIndex(
                name: "IX_SafetyAccidentAttachments_SafetyAccidentId",
                table: "SafetyAccidentAttachments",
                column: "SafetyAccidentId");

            migrationBuilder.CreateIndex(
                name: "IX_SafetyAccidentDisposalAttachments_FileMetaId",
                table: "SafetyAccidentDisposalAttachments",
                column: "FileMetaId");

            migrationBuilder.CreateIndex(
                name: "IX_SafetyAccidentDisposalAttachments_SafetyAccidentDisposalId",
                table: "SafetyAccidentDisposalAttachments",
                column: "SafetyAccidentDisposalId");

            migrationBuilder.CreateIndex(
                name: "IX_SafetyAccidentDisposals_SafetyAccidentId",
                table: "SafetyAccidentDisposals",
                column: "SafetyAccidentId");

            migrationBuilder.CreateIndex(
                name: "IX_SafetyAccidents_CategoryId",
                table: "SafetyAccidents",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_SafetyAccidents_ProjectId",
                table: "SafetyAccidents",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_SafetyAccidents_SeverityId",
                table: "SafetyAccidents",
                column: "SeverityId");

            migrationBuilder.CreateIndex(
                name: "IX_SafetyAccidents_SourceId",
                table: "SafetyAccidents",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_SafetyCompletionAttachments_FileMetaId",
                table: "SafetyCompletionAttachments",
                column: "FileMetaId");

            migrationBuilder.CreateIndex(
                name: "IX_SafetyCompletionAttachments_SafetyProblemId",
                table: "SafetyCompletionAttachments",
                column: "SafetyProblemId");

            migrationBuilder.CreateIndex(
                name: "IX_SafetyProblemAttachments_FileMetaId",
                table: "SafetyProblemAttachments",
                column: "FileMetaId");

            migrationBuilder.CreateIndex(
                name: "IX_SafetyProblemAttachments_SafetyProblemId",
                table: "SafetyProblemAttachments",
                column: "SafetyProblemId");

            migrationBuilder.CreateIndex(
                name: "IX_SafetyProblemRectificationAttachments_FileMetaId",
                table: "SafetyProblemRectificationAttachments",
                column: "FileMetaId");

            migrationBuilder.CreateIndex(
                name: "IX_SafetyProblemRectificationAttachments_SafetyProblemRectifica~",
                table: "SafetyProblemRectificationAttachments",
                column: "SafetyProblemRectificationId");

            migrationBuilder.CreateIndex(
                name: "IX_SafetyProblemRectifications_SafetyProblemId",
                table: "SafetyProblemRectifications",
                column: "SafetyProblemId");

            migrationBuilder.CreateIndex(
                name: "IX_SafetyProblems_CategoryId",
                table: "SafetyProblems",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_SafetyProblems_ProjectId",
                table: "SafetyProblems",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_SafetyProblems_SourceId",
                table: "SafetyProblems",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_SafetySettlementAttachments_FileMetaId",
                table: "SafetySettlementAttachments",
                column: "FileMetaId");

            migrationBuilder.CreateIndex(
                name: "IX_SafetySettlementAttachments_SafetyAccidentId",
                table: "SafetySettlementAttachments",
                column: "SafetyAccidentId");

            migrationBuilder.CreateIndex(
                name: "IX_SafetyStandardAttachments_FileMetaId",
                table: "SafetyStandardAttachments",
                column: "FileMetaId");

            migrationBuilder.CreateIndex(
                name: "IX_SafetyStandardAttachments_SafetyStandardId",
                table: "SafetyStandardAttachments",
                column: "SafetyStandardId");

            migrationBuilder.CreateIndex(
                name: "IX_SafetyStandards_CategoryId",
                table: "SafetyStandards",
                column: "CategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SafetyAccidentAttachments");

            migrationBuilder.DropTable(
                name: "SafetyAccidentDisposalAttachments");

            migrationBuilder.DropTable(
                name: "SafetyCompletionAttachments");

            migrationBuilder.DropTable(
                name: "SafetyProblemAttachments");

            migrationBuilder.DropTable(
                name: "SafetyProblemRectificationAttachments");

            migrationBuilder.DropTable(
                name: "SafetySettlementAttachments");

            migrationBuilder.DropTable(
                name: "SafetyStandardAttachments");

            migrationBuilder.DropTable(
                name: "SafetyAccidentDisposals");

            migrationBuilder.DropTable(
                name: "SafetyProblemRectifications");

            migrationBuilder.DropTable(
                name: "SafetyStandards");

            migrationBuilder.DropTable(
                name: "SafetyAccidents");

            migrationBuilder.DropTable(
                name: "SafetyProblems");
        }
    }
}
