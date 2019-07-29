using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace App.Entry.Migrations
{
    public partial class _40 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Week",
                table: "WeeklyProgresses");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "WeeklyProgresses");

            migrationBuilder.AddColumn<DateTime>(
                name: "AddDate",
                table: "WeeklyProgresses",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddDate",
                table: "WeeklyProgresses");

            migrationBuilder.AddColumn<int>(
                name: "Week",
                table: "WeeklyProgresses",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "WeeklyProgresses",
                nullable: false,
                defaultValue: 0);
        }
    }
}
