using Microsoft.EntityFrameworkCore.Migrations;

namespace App.Entry.Migrations
{
    public partial class _36 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PmMonthlyProgresses_Projects_Mid",
                table: "PmMonthlyProgresses");

            migrationBuilder.DropForeignKey(
                name: "FK_WeeklyProgresses_Projects_Mid",
                table: "WeeklyProgresses");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ProjectUnitMember");

            migrationBuilder.AddColumn<string>(
                name: "User",
                table: "ProjectUnitMember",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PmMonthlyProgresses_PmMonthlyProgresses_Mid",
                table: "PmMonthlyProgresses",
                column: "Mid",
                principalTable: "PmMonthlyProgresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WeeklyProgresses_WeeklyProgresses_Mid",
                table: "WeeklyProgresses",
                column: "Mid",
                principalTable: "WeeklyProgresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PmMonthlyProgresses_PmMonthlyProgresses_Mid",
                table: "PmMonthlyProgresses");

            migrationBuilder.DropForeignKey(
                name: "FK_WeeklyProgresses_WeeklyProgresses_Mid",
                table: "WeeklyProgresses");

            migrationBuilder.DropColumn(
                name: "User",
                table: "ProjectUnitMember");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "ProjectUnitMember",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_PmMonthlyProgresses_Projects_Mid",
                table: "PmMonthlyProgresses",
                column: "Mid",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WeeklyProgresses_Projects_Mid",
                table: "WeeklyProgresses",
                column: "Mid",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
