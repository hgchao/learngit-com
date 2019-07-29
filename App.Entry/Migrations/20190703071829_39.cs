using Microsoft.EntityFrameworkCore.Migrations;

namespace App.Entry.Migrations
{
    public partial class _39 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "QualityAccidentAndDescription",
                table: "ProjectBriefings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SafetyAccidentAndDescription",
                table: "ProjectBriefings",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QualityAccidentAndDescription",
                table: "ProjectBriefings");

            migrationBuilder.DropColumn(
                name: "SafetyAccidentAndDescription",
                table: "ProjectBriefings");
        }
    }
}
