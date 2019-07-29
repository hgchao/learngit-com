using Microsoft.EntityFrameworkCore.Migrations;

namespace App.Entry.Migrations
{
    public partial class _35 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_ProjectLocations_LocationId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_LocationId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "Projects");

            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "ProjectLocations",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectLocations_ProjectId",
                table: "ProjectLocations",
                column: "ProjectId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectLocations_Projects_ProjectId",
                table: "ProjectLocations",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectLocations_Projects_ProjectId",
                table: "ProjectLocations");

            migrationBuilder.DropIndex(
                name: "IX_ProjectLocations_ProjectId",
                table: "ProjectLocations");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "ProjectLocations");

            migrationBuilder.AddColumn<int>(
                name: "LocationId",
                table: "Projects",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_LocationId",
                table: "Projects",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_ProjectLocations_LocationId",
                table: "Projects",
                column: "LocationId",
                principalTable: "ProjectLocations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
