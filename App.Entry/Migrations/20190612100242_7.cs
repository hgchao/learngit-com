using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace App.Entry.Migrations
{
    public partial class _7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ActualDuration",
                table: "ProjectTasks",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ActualStartDate",
                table: "ProjectTasks",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Remark",
                table: "ProjectTasks",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProjectTaskAttachment",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: false),
                    ProjectTaskId = table.Column<int>(nullable: false),
                    FileMetaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectTaskAttachment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectTaskAttachment_FileMetas_FileMetaId",
                        column: x => x.FileMetaId,
                        principalTable: "FileMetas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectTaskAttachment_ProjectTasks_ProjectTaskId",
                        column: x => x.ProjectTaskId,
                        principalTable: "ProjectTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTaskAttachment_FileMetaId",
                table: "ProjectTaskAttachment",
                column: "FileMetaId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTaskAttachment_ProjectTaskId",
                table: "ProjectTaskAttachment",
                column: "ProjectTaskId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectTaskAttachment");

            migrationBuilder.DropColumn(
                name: "ActualDuration",
                table: "ProjectTasks");

            migrationBuilder.DropColumn(
                name: "ActualStartDate",
                table: "ProjectTasks");

            migrationBuilder.DropColumn(
                name: "Remark",
                table: "ProjectTasks");
        }
    }
}
