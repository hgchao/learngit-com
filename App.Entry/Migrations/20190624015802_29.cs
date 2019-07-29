using Microsoft.EntityFrameworkCore.Migrations;

namespace App.Entry.Migrations
{
    public partial class _29 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "PlannedProjectCosts",
                table: "MonthlyPlans",
                type: "decimal(15,2)",
                nullable: false,
                oldClrType: typeof(float));

            migrationBuilder.AlterColumn<decimal>(
                name: "PlannedDemolitionFee",
                table: "MonthlyPlans",
                type: "decimal(15,2)",
                nullable: false,
                oldClrType: typeof(float));

            migrationBuilder.AlterColumn<decimal>(
                name: "ContractSettlementPrice",
                table: "Contracts",
                type: "decimal(15,2)",
                nullable: false,
                oldClrType: typeof(float));

            migrationBuilder.AlterColumn<decimal>(
                name: "ContractPrice",
                table: "Contracts",
                type: "decimal(15,2)",
                nullable: false,
                oldClrType: typeof(float));

            migrationBuilder.AlterColumn<decimal>(
                name: "PaidAmount",
                table: "ContractPayments",
                type: "decimal(15,2)",
                nullable: false,
                oldClrType: typeof(float));

            migrationBuilder.AlterColumn<decimal>(
                name: "CompleteAmount",
                table: "ContractPayments",
                type: "decimal(15,2)",
                nullable: false,
                oldClrType: typeof(float));

            migrationBuilder.AlterColumn<decimal>(
                name: "DepositAmount",
                table: "ContractDeposits",
                type: "decimal(15,2)",
                nullable: false,
                oldClrType: typeof(float));

            migrationBuilder.AlterColumn<decimal>(
                name: "ConventionalRefundAmount",
                table: "ContractDeposits",
                type: "decimal(15,2)",
                nullable: false,
                oldClrType: typeof(float));

            migrationBuilder.AlterColumn<decimal>(
                name: "RegisteredCapital",
                table: "ConstructionUnits",
                type: "decimal(15,2)",
                nullable: false,
                oldClrType: typeof(float));

            migrationBuilder.AlterColumn<decimal>(
                name: "PlannedProjectCosts",
                table: "AnnualPlans",
                type: "decimal(15,2)",
                nullable: false,
                oldClrType: typeof(float));

            migrationBuilder.AlterColumn<decimal>(
                name: "PlannedDemolitionFee",
                table: "AnnualPlans",
                type: "decimal(15,2)",
                nullable: false,
                oldClrType: typeof(float));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "PlannedProjectCosts",
                table: "MonthlyPlans",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(15,2)");

            migrationBuilder.AlterColumn<float>(
                name: "PlannedDemolitionFee",
                table: "MonthlyPlans",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(15,2)");

            migrationBuilder.AlterColumn<float>(
                name: "ContractSettlementPrice",
                table: "Contracts",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(15,2)");

            migrationBuilder.AlterColumn<float>(
                name: "ContractPrice",
                table: "Contracts",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(15,2)");

            migrationBuilder.AlterColumn<float>(
                name: "PaidAmount",
                table: "ContractPayments",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(15,2)");

            migrationBuilder.AlterColumn<float>(
                name: "CompleteAmount",
                table: "ContractPayments",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(15,2)");

            migrationBuilder.AlterColumn<float>(
                name: "DepositAmount",
                table: "ContractDeposits",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(15,2)");

            migrationBuilder.AlterColumn<float>(
                name: "ConventionalRefundAmount",
                table: "ContractDeposits",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(15,2)");

            migrationBuilder.AlterColumn<float>(
                name: "RegisteredCapital",
                table: "ConstructionUnits",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(15,2)");

            migrationBuilder.AlterColumn<float>(
                name: "PlannedProjectCosts",
                table: "AnnualPlans",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(15,2)");

            migrationBuilder.AlterColumn<float>(
                name: "PlannedDemolitionFee",
                table: "AnnualPlans",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(15,2)");
        }
    }
}
