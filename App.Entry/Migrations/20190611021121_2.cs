using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace App.Entry.Migrations
{
    public partial class _2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MemorabiliaRecords",
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
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Participant = table.Column<string>(nullable: true),
                    Mid = table.Column<int>(nullable: true),
                    State = table.Column<int>(nullable: false),
                    ProcessInstanceId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemorabiliaRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MemorabiliaRecords_Options_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Options",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MemorabiliaRecords_MemorabiliaRecords_Mid",
                        column: x => x.Mid,
                        principalTable: "MemorabiliaRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MemorabiliaRecords_Wf_Hi_ProcessInstances_ProcessInstanceId",
                        column: x => x.ProcessInstanceId,
                        principalTable: "Wf_Hi_ProcessInstances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MemorabiliaRecords_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MemorabiliaAttachments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    MemorabiliaId = table.Column<int>(nullable: false),
                    FileMetaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemorabiliaAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MemorabiliaAttachments_FileMetas_FileMetaId",
                        column: x => x.FileMetaId,
                        principalTable: "FileMetas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MemorabiliaAttachments_MemorabiliaRecords_MemorabiliaId",
                        column: x => x.MemorabiliaId,
                        principalTable: "MemorabiliaRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MemorabiliaAttachments_FileMetaId",
                table: "MemorabiliaAttachments",
                column: "FileMetaId");

            migrationBuilder.CreateIndex(
                name: "IX_MemorabiliaAttachments_MemorabiliaId",
                table: "MemorabiliaAttachments",
                column: "MemorabiliaId");

            migrationBuilder.CreateIndex(
                name: "IX_MemorabiliaRecords_CategoryId",
                table: "MemorabiliaRecords",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_MemorabiliaRecords_Mid",
                table: "MemorabiliaRecords",
                column: "Mid");

            migrationBuilder.CreateIndex(
                name: "IX_MemorabiliaRecords_ProcessInstanceId",
                table: "MemorabiliaRecords",
                column: "ProcessInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_MemorabiliaRecords_ProjectId",
                table: "MemorabiliaRecords",
                column: "ProjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MemorabiliaAttachments");

            migrationBuilder.DropTable(
                name: "MemorabiliaRecords");
        }
    }
}
