using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace App.Entry.Migrations
{
    public partial class _21 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Records_RecordTypes_RecordTypeID",
                table: "Records");

            migrationBuilder.DropTable(
                name: "RecordTypes");

            migrationBuilder.RenameColumn(
                name: "RecordTypeID",
                table: "Records",
                newName: "RecordTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Records_RecordTypeID",
                table: "Records",
                newName: "IX_Records_RecordTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Records_Options_RecordTypeId",
                table: "Records",
                column: "RecordTypeId",
                principalTable: "Options",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Records_Options_RecordTypeId",
                table: "Records");

            migrationBuilder.RenameColumn(
                name: "RecordTypeId",
                table: "Records",
                newName: "RecordTypeID");

            migrationBuilder.RenameIndex(
                name: "IX_Records_RecordTypeId",
                table: "Records",
                newName: "IX_Records_RecordTypeID");

            migrationBuilder.CreateTable(
                name: "RecordTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Pid = table.Column<int>(nullable: true),
                    ProjectId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecordTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecordTypes_RecordTypes_Pid",
                        column: x => x.Pid,
                        principalTable: "RecordTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RecordTypes_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RecordTypes_Pid",
                table: "RecordTypes",
                column: "Pid");

            migrationBuilder.CreateIndex(
                name: "IX_RecordTypes_ProjectId",
                table: "RecordTypes",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Records_RecordTypes_RecordTypeID",
                table: "Records",
                column: "RecordTypeID",
                principalTable: "RecordTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
