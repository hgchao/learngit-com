using Microsoft.EntityFrameworkCore.Migrations;

namespace App.Entry.Migrations
{
    public partial class _30 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_Options_ContractionMethodId",
                table: "Contracts");

            migrationBuilder.DropIndex(
                name: "IX_Contracts_ContractionMethodId",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "ContractionMethodId",
                table: "Contracts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ContractionMethodId",
                table: "Contracts",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_ContractionMethodId",
                table: "Contracts",
                column: "ContractionMethodId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_Options_ContractionMethodId",
                table: "Contracts",
                column: "ContractionMethodId",
                principalTable: "Options",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
