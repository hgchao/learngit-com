using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace App.Entry.Migrations
{
    public partial class _37 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Information",
                table: "WeeklyProgresses",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CompletionDate",
                table: "Contracts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Information",
                table: "Contracts",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "WeeklyProgressAttachments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    WeeklyProgressId = table.Column<int>(nullable: false),
                    FileMetaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeeklyProgressAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeeklyProgressAttachments_FileMetas_FileMetaId",
                        column: x => x.FileMetaId,
                        principalTable: "FileMetas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WeeklyProgressAttachments_WeeklyProgresses_WeeklyProgressId",
                        column: x => x.WeeklyProgressId,
                        principalTable: "WeeklyProgresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WeeklyProgressAttachments_FileMetaId",
                table: "WeeklyProgressAttachments",
                column: "FileMetaId");

            migrationBuilder.CreateIndex(
                name: "IX_WeeklyProgressAttachments_WeeklyProgressId",
                table: "WeeklyProgressAttachments",
                column: "WeeklyProgressId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WeeklyProgressAttachments");

            migrationBuilder.DropColumn(
                name: "Information",
                table: "WeeklyProgresses");

            migrationBuilder.DropColumn(
                name: "CompletionDate",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "Information",
                table: "Contracts");
        }
    }
}
