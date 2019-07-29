using Microsoft.EntityFrameworkCore.Migrations;

namespace App.Entry.Migrations
{
    public partial class _15 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Mid",
                table: "SafetyProblems",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProcessInstanceId",
                table: "SafetyProblems",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "SafetyProblems",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Mid",
                table: "SafetyAccidents",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProcessInstanceId",
                table: "SafetyAccidents",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "SafetyAccidents",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SafetyProblems_Mid",
                table: "SafetyProblems",
                column: "Mid");

            migrationBuilder.CreateIndex(
                name: "IX_SafetyProblems_ProcessInstanceId",
                table: "SafetyProblems",
                column: "ProcessInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_SafetyAccidents_Mid",
                table: "SafetyAccidents",
                column: "Mid");

            migrationBuilder.CreateIndex(
                name: "IX_SafetyAccidents_ProcessInstanceId",
                table: "SafetyAccidents",
                column: "ProcessInstanceId");

            migrationBuilder.AddForeignKey(
                name: "FK_SafetyAccidents_SafetyAccidents_Mid",
                table: "SafetyAccidents",
                column: "Mid",
                principalTable: "SafetyAccidents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SafetyAccidents_Wf_Hi_ProcessInstances_ProcessInstanceId",
                table: "SafetyAccidents",
                column: "ProcessInstanceId",
                principalTable: "Wf_Hi_ProcessInstances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SafetyProblems_SafetyProblems_Mid",
                table: "SafetyProblems",
                column: "Mid",
                principalTable: "SafetyProblems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SafetyProblems_Wf_Hi_ProcessInstances_ProcessInstanceId",
                table: "SafetyProblems",
                column: "ProcessInstanceId",
                principalTable: "Wf_Hi_ProcessInstances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SafetyAccidents_SafetyAccidents_Mid",
                table: "SafetyAccidents");

            migrationBuilder.DropForeignKey(
                name: "FK_SafetyAccidents_Wf_Hi_ProcessInstances_ProcessInstanceId",
                table: "SafetyAccidents");

            migrationBuilder.DropForeignKey(
                name: "FK_SafetyProblems_SafetyProblems_Mid",
                table: "SafetyProblems");

            migrationBuilder.DropForeignKey(
                name: "FK_SafetyProblems_Wf_Hi_ProcessInstances_ProcessInstanceId",
                table: "SafetyProblems");

            migrationBuilder.DropIndex(
                name: "IX_SafetyProblems_Mid",
                table: "SafetyProblems");

            migrationBuilder.DropIndex(
                name: "IX_SafetyProblems_ProcessInstanceId",
                table: "SafetyProblems");

            migrationBuilder.DropIndex(
                name: "IX_SafetyAccidents_Mid",
                table: "SafetyAccidents");

            migrationBuilder.DropIndex(
                name: "IX_SafetyAccidents_ProcessInstanceId",
                table: "SafetyAccidents");

            migrationBuilder.DropColumn(
                name: "Mid",
                table: "SafetyProblems");

            migrationBuilder.DropColumn(
                name: "ProcessInstanceId",
                table: "SafetyProblems");

            migrationBuilder.DropColumn(
                name: "State",
                table: "SafetyProblems");

            migrationBuilder.DropColumn(
                name: "Mid",
                table: "SafetyAccidents");

            migrationBuilder.DropColumn(
                name: "ProcessInstanceId",
                table: "SafetyAccidents");

            migrationBuilder.DropColumn(
                name: "State",
                table: "SafetyAccidents");
        }
    }
}
