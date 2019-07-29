using Microsoft.EntityFrameworkCore.Migrations;

namespace App.Entry.Migrations
{
    public partial class _31 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Options_CategoryId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Options_ConstructionNatureId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Options_FundsSourceId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Options_TagId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_CategoryId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_ConstructionNatureId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_FundsSourceId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ConstructionNatureId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "FundsSourceId",
                table: "Projects");

            migrationBuilder.RenameColumn(
                name: "TagId",
                table: "Projects",
                newName: "TypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Projects_TagId",
                table: "Projects",
                newName: "IX_Projects_TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Options_TypeId",
                table: "Projects",
                column: "TypeId",
                principalTable: "Options",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Options_TypeId",
                table: "Projects");

            migrationBuilder.RenameColumn(
                name: "TypeId",
                table: "Projects",
                newName: "TagId");

            migrationBuilder.RenameIndex(
                name: "IX_Projects_TypeId",
                table: "Projects",
                newName: "IX_Projects_TagId");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Projects",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ConstructionNatureId",
                table: "Projects",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FundsSourceId",
                table: "Projects",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_CategoryId",
                table: "Projects",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ConstructionNatureId",
                table: "Projects",
                column: "ConstructionNatureId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_FundsSourceId",
                table: "Projects",
                column: "FundsSourceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Options_CategoryId",
                table: "Projects",
                column: "CategoryId",
                principalTable: "Options",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Options_ConstructionNatureId",
                table: "Projects",
                column: "ConstructionNatureId",
                principalTable: "Options",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Options_FundsSourceId",
                table: "Projects",
                column: "FundsSourceId",
                principalTable: "Options",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Options_TagId",
                table: "Projects",
                column: "TagId",
                principalTable: "Options",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
