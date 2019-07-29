using Microsoft.EntityFrameworkCore.Migrations;

namespace App.Entry.Migrations
{
    public partial class _9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Contracts_ConstructionUnits_Mid",
            //    table: "Contracts");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Contracts_Contracts_Mid",
            //    table: "Contracts",
            //    column: "Mid",
            //    principalTable: "Contracts",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_Contracts_Mid",
                table: "Contracts");

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_ConstructionUnits_Mid",
                table: "Contracts",
                column: "Mid",
                principalTable: "ConstructionUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
