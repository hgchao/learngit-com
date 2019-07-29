using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace App.Entry.Migrations
{
    public partial class _28 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Options_ConstructionAgentUnitId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Options_ProprietorUnitId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Options_ResponsibleUnitId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Options_SupervisorUnitId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_ConstructionAgentUnitId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_ProprietorUnitId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_ResponsibleUnitId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_SupervisorUnitId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ActualCommencementDate",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ActualCompletionDate",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ActualFinancialAccountsDate",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ActualFinishDate",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ActualTransferDate",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "AffiliationUnit",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ConstructionAgentUnitId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Headquarters",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "PlannedCommencementDate",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "PlannedCompletionDate",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "PlannedFinancialAccountsDate",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "PlannedFinishDate",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ProprietorUnitId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ResponsibleUnitId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "SupervisorUnitId",
                table: "Projects");

            migrationBuilder.RenameColumn(
                name: "PreparetoryWorkPlanDate",
                table: "Projects",
                newName: "FinishDate");

            migrationBuilder.RenameColumn(
                name: "PlannedTransferDate",
                table: "Projects",
                newName: "CommencementDate");

            migrationBuilder.CreateTable(
                name: "ProjectUnit",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: false),
                    ProjectId = table.Column<int>(nullable: false),
                    Type = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectUnit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectUnit_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectUnitMember",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: false),
                    ProjectUnitId = table.Column<int>(nullable: false),
                    ProjectUnitRole = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectUnitMember", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectUnitMember_ProjectUnit_ProjectUnitId",
                        column: x => x.ProjectUnitId,
                        principalTable: "ProjectUnit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectUnit_ProjectId",
                table: "ProjectUnit",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectUnitMember_ProjectUnitId",
                table: "ProjectUnitMember",
                column: "ProjectUnitId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectUnitMember");

            migrationBuilder.DropTable(
                name: "ProjectUnit");

            migrationBuilder.RenameColumn(
                name: "FinishDate",
                table: "Projects",
                newName: "PreparetoryWorkPlanDate");

            migrationBuilder.RenameColumn(
                name: "CommencementDate",
                table: "Projects",
                newName: "PlannedTransferDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "ActualCommencementDate",
                table: "Projects",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ActualCompletionDate",
                table: "Projects",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ActualFinancialAccountsDate",
                table: "Projects",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ActualFinishDate",
                table: "Projects",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ActualTransferDate",
                table: "Projects",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AffiliationUnit",
                table: "Projects",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ConstructionAgentUnitId",
                table: "Projects",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Headquarters",
                table: "Projects",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PlannedCommencementDate",
                table: "Projects",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PlannedCompletionDate",
                table: "Projects",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PlannedFinancialAccountsDate",
                table: "Projects",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PlannedFinishDate",
                table: "Projects",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProprietorUnitId",
                table: "Projects",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ResponsibleUnitId",
                table: "Projects",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SupervisorUnitId",
                table: "Projects",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ConstructionAgentUnitId",
                table: "Projects",
                column: "ConstructionAgentUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ProprietorUnitId",
                table: "Projects",
                column: "ProprietorUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ResponsibleUnitId",
                table: "Projects",
                column: "ResponsibleUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_SupervisorUnitId",
                table: "Projects",
                column: "SupervisorUnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Options_ConstructionAgentUnitId",
                table: "Projects",
                column: "ConstructionAgentUnitId",
                principalTable: "Options",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Options_ProprietorUnitId",
                table: "Projects",
                column: "ProprietorUnitId",
                principalTable: "Options",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Options_ResponsibleUnitId",
                table: "Projects",
                column: "ResponsibleUnitId",
                principalTable: "Options",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Options_SupervisorUnitId",
                table: "Projects",
                column: "SupervisorUnitId",
                principalTable: "Options",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
