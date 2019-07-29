using Microsoft.EntityFrameworkCore.Migrations;

namespace App.Entry.Migrations
{
    public partial class _14 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Mid",
                table: "QualityProblems",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProcessInstanceId",
                table: "QualityProblems",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "QualityProblems",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Mid",
                table: "QualityAccidents",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProcessInstanceId",
                table: "QualityAccidents",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "QualityAccidents",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_QualityProblems_Mid",
                table: "QualityProblems",
                column: "Mid");

            migrationBuilder.CreateIndex(
                name: "IX_QualityProblems_ProcessInstanceId",
                table: "QualityProblems",
                column: "ProcessInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_QualityAccidents_Mid",
                table: "QualityAccidents",
                column: "Mid");

            migrationBuilder.CreateIndex(
                name: "IX_QualityAccidents_ProcessInstanceId",
                table: "QualityAccidents",
                column: "ProcessInstanceId");

            migrationBuilder.AddForeignKey(
                name: "FK_QualityAccidents_QualityAccidents_Mid",
                table: "QualityAccidents",
                column: "Mid",
                principalTable: "QualityAccidents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_QualityAccidents_Wf_Hi_ProcessInstances_ProcessInstanceId",
                table: "QualityAccidents",
                column: "ProcessInstanceId",
                principalTable: "Wf_Hi_ProcessInstances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_QualityProblems_QualityProblems_Mid",
                table: "QualityProblems",
                column: "Mid",
                principalTable: "QualityProblems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_QualityProblems_Wf_Hi_ProcessInstances_ProcessInstanceId",
                table: "QualityProblems",
                column: "ProcessInstanceId",
                principalTable: "Wf_Hi_ProcessInstances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QualityAccidents_QualityAccidents_Mid",
                table: "QualityAccidents");

            migrationBuilder.DropForeignKey(
                name: "FK_QualityAccidents_Wf_Hi_ProcessInstances_ProcessInstanceId",
                table: "QualityAccidents");

            migrationBuilder.DropForeignKey(
                name: "FK_QualityProblems_QualityProblems_Mid",
                table: "QualityProblems");

            migrationBuilder.DropForeignKey(
                name: "FK_QualityProblems_Wf_Hi_ProcessInstances_ProcessInstanceId",
                table: "QualityProblems");

            migrationBuilder.DropIndex(
                name: "IX_QualityProblems_Mid",
                table: "QualityProblems");

            migrationBuilder.DropIndex(
                name: "IX_QualityProblems_ProcessInstanceId",
                table: "QualityProblems");

            migrationBuilder.DropIndex(
                name: "IX_QualityAccidents_Mid",
                table: "QualityAccidents");

            migrationBuilder.DropIndex(
                name: "IX_QualityAccidents_ProcessInstanceId",
                table: "QualityAccidents");

            migrationBuilder.DropColumn(
                name: "Mid",
                table: "QualityProblems");

            migrationBuilder.DropColumn(
                name: "ProcessInstanceId",
                table: "QualityProblems");

            migrationBuilder.DropColumn(
                name: "State",
                table: "QualityProblems");

            migrationBuilder.DropColumn(
                name: "Mid",
                table: "QualityAccidents");

            migrationBuilder.DropColumn(
                name: "ProcessInstanceId",
                table: "QualityAccidents");

            migrationBuilder.DropColumn(
                name: "State",
                table: "QualityAccidents");
        }
    }
}
