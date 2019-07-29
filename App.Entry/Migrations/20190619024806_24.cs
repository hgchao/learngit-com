﻿using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace App.Entry.Migrations
{
    public partial class _24 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WeeklyProgresses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatorId = table.Column<int>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    DeleterId = table.Column<int>(nullable: true),
                    LastModifierId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: false),
                    ProjectId = table.Column<int>(nullable: false),
                    Year = table.Column<int>(nullable: false),
                    Week = table.Column<int>(nullable: false),
                    AccumulatedImageProgress = table.Column<string>(nullable: true),
                    ImageProgress = table.Column<string>(nullable: true),
                    Supervision = table.Column<string>(nullable: true),
                    NextMonthPlannedImageProgress = table.Column<string>(nullable: true),
                    Mid = table.Column<int>(nullable: true),
                    DataState = table.Column<int>(nullable: false),
                    ProcessInstanceId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeeklyProgresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeeklyProgresses_Projects_Mid",
                        column: x => x.Mid,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WeeklyProgresses_Wf_Hi_ProcessInstances_ProcessInstanceId",
                        column: x => x.ProcessInstanceId,
                        principalTable: "Wf_Hi_ProcessInstances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WeeklyProgresses_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WeeklyProgresses_Mid",
                table: "WeeklyProgresses",
                column: "Mid");

            migrationBuilder.CreateIndex(
                name: "IX_WeeklyProgresses_ProcessInstanceId",
                table: "WeeklyProgresses",
                column: "ProcessInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_WeeklyProgresses_ProjectId",
                table: "WeeklyProgresses",
                column: "ProjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WeeklyProgresses");
        }
    }
}
