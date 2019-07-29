using Microsoft.EntityFrameworkCore.Migrations;

namespace App.Entry.Migrations
{
    public partial class _18 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Mid",
                table: "HousekeepingProblems",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProcessInstanceId",
                table: "HousekeepingProblems",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "HousekeepingProblems",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_HousekeepingProblems_Mid",
                table: "HousekeepingProblems",
                column: "Mid");

            migrationBuilder.CreateIndex(
                name: "IX_HousekeepingProblems_ProcessInstanceId",
                table: "HousekeepingProblems",
                column: "ProcessInstanceId");

            migrationBuilder.AddForeignKey(
                name: "FK_HousekeepingProblems_HousekeepingProblems_Mid",
                table: "HousekeepingProblems",
                column: "Mid",
                principalTable: "HousekeepingProblems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HousekeepingProblems_Wf_Hi_ProcessInstances_ProcessInstanceId",
                table: "HousekeepingProblems",
                column: "ProcessInstanceId",
                principalTable: "Wf_Hi_ProcessInstances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HousekeepingProblems_HousekeepingProblems_Mid",
                table: "HousekeepingProblems");

            migrationBuilder.DropForeignKey(
                name: "FK_HousekeepingProblems_Wf_Hi_ProcessInstances_ProcessInstanceId",
                table: "HousekeepingProblems");

            migrationBuilder.DropIndex(
                name: "IX_HousekeepingProblems_Mid",
                table: "HousekeepingProblems");

            migrationBuilder.DropIndex(
                name: "IX_HousekeepingProblems_ProcessInstanceId",
                table: "HousekeepingProblems");

            migrationBuilder.DropColumn(
                name: "Mid",
                table: "HousekeepingProblems");

            migrationBuilder.DropColumn(
                name: "ProcessInstanceId",
                table: "HousekeepingProblems");

            migrationBuilder.DropColumn(
                name: "State",
                table: "HousekeepingProblems");
        }
    }
}
