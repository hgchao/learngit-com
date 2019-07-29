using Microsoft.EntityFrameworkCore.Migrations;

namespace App.Entry.Migrations
{
    public partial class _34 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Contracts_ContractDeposits_ContractId",
            //    table: "Contracts");

            //migrationBuilder.DropIndex(
            //    name: "IX_Contracts_ContractId",
            //    table: "Contracts");

            //migrationBuilder.DropColumn(
            //    name: "ContractId",
            //    table: "Contracts");

            //migrationBuilder.AddColumn<int>(
            //    name: "ContractId",
            //    table: "ContractDeposits",
            //    nullable: false,
            //    defaultValue: 0);

            //migrationBuilder.CreateIndex(
            //    name: "IX_ContractDeposits_ContractId",
            //    table: "ContractDeposits",
            //    column: "ContractId",
            //    unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ContractDeposits_Contracts_ContractId",
                table: "ContractDeposits",
                column: "ContractId",
                principalTable: "Contracts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContractDeposits_Contracts_ContractId",
                table: "ContractDeposits");

            migrationBuilder.DropIndex(
                name: "IX_ContractDeposits_ContractId",
                table: "ContractDeposits");

            migrationBuilder.DropColumn(
                name: "ContractId",
                table: "ContractDeposits");

            migrationBuilder.AddColumn<int>(
                name: "ContractId",
                table: "Contracts",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_ContractId",
                table: "Contracts",
                column: "ContractId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_ContractDeposits_ContractId",
                table: "Contracts",
                column: "ContractId",
                principalTable: "ContractDeposits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
