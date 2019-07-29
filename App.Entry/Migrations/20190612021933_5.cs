using Microsoft.EntityFrameworkCore.Migrations;

namespace App.Entry.Migrations
{
    public partial class _5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectLinks_ProjectGantts_ProjectGanttId",
                table: "ProjectLinks");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTasks_ProjectGantts_ProjectGanttId",
                table: "ProjectTasks");

            migrationBuilder.DropIndex(
                name: "IX_ProjectTasks_ProjectGanttId",
                table: "ProjectTasks");

            migrationBuilder.DropIndex(
                name: "IX_ProjectLinks_ProjectGanttId",
                table: "ProjectLinks");

            migrationBuilder.DropColumn(
                name: "ProjectGanttId",
                table: "ProjectTasks");

            migrationBuilder.DropColumn(
                name: "ProjectGanttId",
                table: "ProjectLinks");

            migrationBuilder.AddColumn<int>(
                name: "GanttId",
                table: "ProjectTasks",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GanttId",
                table: "ProjectLinks",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTasks_GanttId",
                table: "ProjectTasks",
                column: "GanttId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectLinks_GanttId",
                table: "ProjectLinks",
                column: "GanttId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectLinks_ProjectGantts_GanttId",
                table: "ProjectLinks",
                column: "GanttId",
                principalTable: "ProjectGantts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTasks_ProjectGantts_GanttId",
                table: "ProjectTasks",
                column: "GanttId",
                principalTable: "ProjectGantts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectLinks_ProjectGantts_GanttId",
                table: "ProjectLinks");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTasks_ProjectGantts_GanttId",
                table: "ProjectTasks");

            migrationBuilder.DropIndex(
                name: "IX_ProjectTasks_GanttId",
                table: "ProjectTasks");

            migrationBuilder.DropIndex(
                name: "IX_ProjectLinks_GanttId",
                table: "ProjectLinks");

            migrationBuilder.DropColumn(
                name: "GanttId",
                table: "ProjectTasks");

            migrationBuilder.DropColumn(
                name: "GanttId",
                table: "ProjectLinks");

            migrationBuilder.AddColumn<int>(
                name: "ProjectGanttId",
                table: "ProjectTasks",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProjectGanttId",
                table: "ProjectLinks",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTasks_ProjectGanttId",
                table: "ProjectTasks",
                column: "ProjectGanttId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectLinks_ProjectGanttId",
                table: "ProjectLinks",
                column: "ProjectGanttId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectLinks_ProjectGantts_ProjectGanttId",
                table: "ProjectLinks",
                column: "ProjectGanttId",
                principalTable: "ProjectGantts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTasks_ProjectGantts_ProjectGanttId",
                table: "ProjectTasks",
                column: "ProjectGanttId",
                principalTable: "ProjectGantts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
