using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace App.Entry.Migrations
{
    public partial class _6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QualityAccidents",
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
                    Title = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    DiscoveryTime = table.Column<DateTime>(nullable: false),
                    DisposalState = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    SettlementTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QualityAccidents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QualityAccidents_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QualityProblems",
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
                    Source = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    RectificationState = table.Column<int>(nullable: false),
                    CompletionTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QualityProblems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QualityProblems_Options_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Options",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QualityProblems_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QualityStandards",
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
                    table.PrimaryKey("PK_QualityStandards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QualityStandards_Options_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Options",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QualityAccidentAttachments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    QualityAccidentId = table.Column<int>(nullable: false),
                    FileMetaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QualityAccidentAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QualityAccidentAttachments_FileMetas_FileMetaId",
                        column: x => x.FileMetaId,
                        principalTable: "FileMetas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QualityAccidentAttachments_QualityAccidents_QualityAccidentId",
                        column: x => x.QualityAccidentId,
                        principalTable: "QualityAccidents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QualityAccidentDisposals",
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
                    QualityAccidentId = table.Column<int>(nullable: false),
                    ExecDate = table.Column<DateTime>(nullable: true),
                    Plan = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QualityAccidentDisposals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QualityAccidentDisposals_QualityAccidents_QualityAccidentId",
                        column: x => x.QualityAccidentId,
                        principalTable: "QualityAccidents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QualitySettlementAttachments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    QualitySettlementId = table.Column<int>(nullable: false),
                    FileMetaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QualitySettlementAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QualitySettlementAttachments_FileMetas_FileMetaId",
                        column: x => x.FileMetaId,
                        principalTable: "FileMetas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QualitySettlementAttachments_QualityAccidents_QualitySettlem~",
                        column: x => x.QualitySettlementId,
                        principalTable: "QualityAccidents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QualityCompletionAttachments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    QualityProblemId = table.Column<int>(nullable: false),
                    FileMetaId = table.Column<int>(nullable: false),
                    QualityCompletionId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QualityCompletionAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QualityCompletionAttachments_FileMetas_FileMetaId",
                        column: x => x.FileMetaId,
                        principalTable: "FileMetas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QualityCompletionAttachments_QualityProblems_QualityCompleti~",
                        column: x => x.QualityCompletionId,
                        principalTable: "QualityProblems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QualityProblemAttachments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    QualityProblemId = table.Column<int>(nullable: false),
                    FileMetaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QualityProblemAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QualityProblemAttachments_FileMetas_FileMetaId",
                        column: x => x.FileMetaId,
                        principalTable: "FileMetas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QualityProblemAttachments_QualityProblems_QualityProblemId",
                        column: x => x.QualityProblemId,
                        principalTable: "QualityProblems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QualityProblemRectifications",
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
                    QualityProblemId = table.Column<int>(nullable: false),
                    ExecDate = table.Column<DateTime>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QualityProblemRectifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QualityProblemRectifications_QualityProblems_QualityProblemId",
                        column: x => x.QualityProblemId,
                        principalTable: "QualityProblems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QualityStandardAttachments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    QualityStandardId = table.Column<int>(nullable: false),
                    FileMetaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QualityStandardAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QualityStandardAttachments_FileMetas_FileMetaId",
                        column: x => x.FileMetaId,
                        principalTable: "FileMetas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QualityStandardAttachments_QualityStandards_QualityStandardId",
                        column: x => x.QualityStandardId,
                        principalTable: "QualityStandards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QualityAccidentDisposalAttachments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    QualityAccidentDisposalId = table.Column<int>(nullable: false),
                    FileMetaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QualityAccidentDisposalAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QualityAccidentDisposalAttachments_FileMetas_FileMetaId",
                        column: x => x.FileMetaId,
                        principalTable: "FileMetas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QualityAccidentDisposalAttachments_QualityAccidentDisposals_~",
                        column: x => x.QualityAccidentDisposalId,
                        principalTable: "QualityAccidentDisposals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QualityProblemRectificationAttachments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    QualityProblemRectificationId = table.Column<int>(nullable: false),
                    FileMetaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QualityProblemRectificationAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QualityProblemRectificationAttachments_FileMetas_FileMetaId",
                        column: x => x.FileMetaId,
                        principalTable: "FileMetas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QualityProblemRectificationAttachments_QualityProblemRectifi~",
                        column: x => x.QualityProblemRectificationId,
                        principalTable: "QualityProblemRectifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QualityAccidentAttachments_FileMetaId",
                table: "QualityAccidentAttachments",
                column: "FileMetaId");

            migrationBuilder.CreateIndex(
                name: "IX_QualityAccidentAttachments_QualityAccidentId",
                table: "QualityAccidentAttachments",
                column: "QualityAccidentId");

            migrationBuilder.CreateIndex(
                name: "IX_QualityAccidentDisposalAttachments_FileMetaId",
                table: "QualityAccidentDisposalAttachments",
                column: "FileMetaId");

            migrationBuilder.CreateIndex(
                name: "IX_QualityAccidentDisposalAttachments_QualityAccidentDisposalId",
                table: "QualityAccidentDisposalAttachments",
                column: "QualityAccidentDisposalId");

            migrationBuilder.CreateIndex(
                name: "IX_QualityAccidentDisposals_QualityAccidentId",
                table: "QualityAccidentDisposals",
                column: "QualityAccidentId");

            migrationBuilder.CreateIndex(
                name: "IX_QualityAccidents_ProjectId",
                table: "QualityAccidents",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_QualityCompletionAttachments_FileMetaId",
                table: "QualityCompletionAttachments",
                column: "FileMetaId");

            migrationBuilder.CreateIndex(
                name: "IX_QualityCompletionAttachments_QualityCompletionId",
                table: "QualityCompletionAttachments",
                column: "QualityCompletionId");

            migrationBuilder.CreateIndex(
                name: "IX_QualityProblemAttachments_FileMetaId",
                table: "QualityProblemAttachments",
                column: "FileMetaId");

            migrationBuilder.CreateIndex(
                name: "IX_QualityProblemAttachments_QualityProblemId",
                table: "QualityProblemAttachments",
                column: "QualityProblemId");

            migrationBuilder.CreateIndex(
                name: "IX_QualityProblemRectificationAttachments_FileMetaId",
                table: "QualityProblemRectificationAttachments",
                column: "FileMetaId");

            migrationBuilder.CreateIndex(
                name: "IX_QualityProblemRectificationAttachments_QualityProblemRectifi~",
                table: "QualityProblemRectificationAttachments",
                column: "QualityProblemRectificationId");

            migrationBuilder.CreateIndex(
                name: "IX_QualityProblemRectifications_QualityProblemId",
                table: "QualityProblemRectifications",
                column: "QualityProblemId");

            migrationBuilder.CreateIndex(
                name: "IX_QualityProblems_CategoryId",
                table: "QualityProblems",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_QualityProblems_ProjectId",
                table: "QualityProblems",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_QualitySettlementAttachments_FileMetaId",
                table: "QualitySettlementAttachments",
                column: "FileMetaId");

            migrationBuilder.CreateIndex(
                name: "IX_QualitySettlementAttachments_QualitySettlementId",
                table: "QualitySettlementAttachments",
                column: "QualitySettlementId");

            migrationBuilder.CreateIndex(
                name: "IX_QualityStandardAttachments_FileMetaId",
                table: "QualityStandardAttachments",
                column: "FileMetaId");

            migrationBuilder.CreateIndex(
                name: "IX_QualityStandardAttachments_QualityStandardId",
                table: "QualityStandardAttachments",
                column: "QualityStandardId");

            migrationBuilder.CreateIndex(
                name: "IX_QualityStandards_CategoryId",
                table: "QualityStandards",
                column: "CategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QualityAccidentAttachments");

            migrationBuilder.DropTable(
                name: "QualityAccidentDisposalAttachments");

            migrationBuilder.DropTable(
                name: "QualityCompletionAttachments");

            migrationBuilder.DropTable(
                name: "QualityProblemAttachments");

            migrationBuilder.DropTable(
                name: "QualityProblemRectificationAttachments");

            migrationBuilder.DropTable(
                name: "QualitySettlementAttachments");

            migrationBuilder.DropTable(
                name: "QualityStandardAttachments");

            migrationBuilder.DropTable(
                name: "QualityAccidentDisposals");

            migrationBuilder.DropTable(
                name: "QualityProblemRectifications");

            migrationBuilder.DropTable(
                name: "QualityStandards");

            migrationBuilder.DropTable(
                name: "QualityAccidents");

            migrationBuilder.DropTable(
                name: "QualityProblems");
        }
    }
}
