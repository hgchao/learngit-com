using Microsoft.EntityFrameworkCore.Migrations;

namespace App.Entry.Migrations
{
    public partial class _41 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTasks_ProjectTasks_Mid",
                table: "ProjectTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTasks_Wf_Hi_ProcessInstances_ProcessInstanceId",
                table: "ProjectTasks");

            migrationBuilder.DropIndex(
                name: "IX_ProjectTasks_Mid",
                table: "ProjectTasks");

            migrationBuilder.DropIndex(
                name: "IX_ProjectTasks_ProcessInstanceId",
                table: "ProjectTasks");

            migrationBuilder.DropColumn(
                name: "DataState",
                table: "ProjectTasks");

            migrationBuilder.DropColumn(
                name: "Mid",
                table: "ProjectTasks");

            migrationBuilder.DropColumn(
                name: "ProcessInstanceId",
                table: "ProjectTasks");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DataState",
                table: "ProjectTasks",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Mid",
                table: "ProjectTasks",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProcessInstanceId",
                table: "ProjectTasks",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTasks_Mid",
                table: "ProjectTasks",
                column: "Mid");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTasks_ProcessInstanceId",
                table: "ProjectTasks",
                column: "ProcessInstanceId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTasks_ProjectTasks_Mid",
                table: "ProjectTasks",
                column: "Mid",
                principalTable: "ProjectTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTasks_Wf_Hi_ProcessInstances_ProcessInstanceId",
                table: "ProjectTasks",
                column: "ProcessInstanceId",
                principalTable: "Wf_Hi_ProcessInstances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
