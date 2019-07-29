using Microsoft.EntityFrameworkCore.Migrations;

namespace App.Entry.Migrations
{
    public partial class _42 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Link",
                table: "SafetyStandards",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Link",
                table: "QualityStandards",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Link",
                table: "SafetyStandards");

            migrationBuilder.DropColumn(
                name: "Link",
                table: "QualityStandards");
        }
    }
}
