using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace App.Entry.Migrations
{
    public partial class _20 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecordBorrows");

            migrationBuilder.DropColumn(
                name: "Content",
                table: "Records");

            migrationBuilder.DropColumn(
                name: "IsRecord",
                table: "Records");

            migrationBuilder.DropColumn(
                name: "PlaceStorage",
                table: "Records");

            migrationBuilder.DropColumn(
                name: "RecordNo",
                table: "Records");

            migrationBuilder.RenameColumn(
                name: "FileApproverId",
                table: "RecordTypes",
                newName: "Code");

            migrationBuilder.RenameColumn(
                name: "StorageLife",
                table: "Records",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "RecordPage",
                table: "Records",
                newName: "ProjectId");

            migrationBuilder.AddColumn<int>(
                name: "Pid",
                table: "RecordTypes",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "RecordTypes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DataState",
                table: "Records",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Mid",
                table: "Records",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProcessInstanceId",
                table: "Records",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RecordTypes_Pid",
                table: "RecordTypes",
                column: "Pid");

            migrationBuilder.CreateIndex(
                name: "IX_RecordTypes_ProjectId",
                table: "RecordTypes",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Records_Mid",
                table: "Records",
                column: "Mid");

            migrationBuilder.CreateIndex(
                name: "IX_Records_ProcessInstanceId",
                table: "Records",
                column: "ProcessInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_Records_ProjectId",
                table: "Records",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Records_Records_Mid",
                table: "Records",
                column: "Mid",
                principalTable: "Records",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Records_Wf_Hi_ProcessInstances_ProcessInstanceId",
                table: "Records",
                column: "ProcessInstanceId",
                principalTable: "Wf_Hi_ProcessInstances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Records_Projects_ProjectId",
                table: "Records",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecordTypes_RecordTypes_Pid",
                table: "RecordTypes",
                column: "Pid",
                principalTable: "RecordTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RecordTypes_Projects_ProjectId",
                table: "RecordTypes",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Records_Records_Mid",
                table: "Records");

            migrationBuilder.DropForeignKey(
                name: "FK_Records_Wf_Hi_ProcessInstances_ProcessInstanceId",
                table: "Records");

            migrationBuilder.DropForeignKey(
                name: "FK_Records_Projects_ProjectId",
                table: "Records");

            migrationBuilder.DropForeignKey(
                name: "FK_RecordTypes_RecordTypes_Pid",
                table: "RecordTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_RecordTypes_Projects_ProjectId",
                table: "RecordTypes");

            migrationBuilder.DropIndex(
                name: "IX_RecordTypes_Pid",
                table: "RecordTypes");

            migrationBuilder.DropIndex(
                name: "IX_RecordTypes_ProjectId",
                table: "RecordTypes");

            migrationBuilder.DropIndex(
                name: "IX_Records_Mid",
                table: "Records");

            migrationBuilder.DropIndex(
                name: "IX_Records_ProcessInstanceId",
                table: "Records");

            migrationBuilder.DropIndex(
                name: "IX_Records_ProjectId",
                table: "Records");

            migrationBuilder.DropColumn(
                name: "Pid",
                table: "RecordTypes");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "RecordTypes");

            migrationBuilder.DropColumn(
                name: "DataState",
                table: "Records");

            migrationBuilder.DropColumn(
                name: "Mid",
                table: "Records");

            migrationBuilder.DropColumn(
                name: "ProcessInstanceId",
                table: "Records");

            migrationBuilder.RenameColumn(
                name: "Code",
                table: "RecordTypes",
                newName: "FileApproverId");

            migrationBuilder.RenameColumn(
                name: "ProjectId",
                table: "Records",
                newName: "RecordPage");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Records",
                newName: "StorageLife");

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "Records",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsRecord",
                table: "Records",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PlaceStorage",
                table: "Records",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RecordNo",
                table: "Records",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RecordBorrows",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    CreatorId = table.Column<int>(nullable: false),
                    ExamineId = table.Column<int>(nullable: true),
                    ExamineTime = table.Column<DateTime>(nullable: true),
                    RecordId = table.Column<int>(nullable: false),
                    State = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecordBorrows", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecordBorrows_Records_RecordId",
                        column: x => x.RecordId,
                        principalTable: "Records",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RecordBorrows_RecordId",
                table: "RecordBorrows",
                column: "RecordId");
        }
    }
}
