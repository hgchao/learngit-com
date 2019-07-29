using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace App.Entry.Migrations
{
    public partial class _3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConstructionUnits",
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
                    Name = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    CompanySize = table.Column<string>(nullable: true),
                    RegisteredCapital = table.Column<float>(nullable: false),
                    LegalPerson = table.Column<string>(nullable: true),
                    UniformCreditCode = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    CompanyContact = table.Column<string>(nullable: true),
                    Link = table.Column<string>(nullable: true),
                    EvaluationLevel = table.Column<string>(nullable: true),
                    Score = table.Column<float>(nullable: false),
                    Mid = table.Column<int>(nullable: true),
                    State = table.Column<int>(nullable: false),
                    ProcessInstanceId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConstructionUnits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConstructionUnits_ConstructionUnits_Mid",
                        column: x => x.Mid,
                        principalTable: "ConstructionUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ConstructionUnits_Wf_Hi_ProcessInstances_ProcessInstanceId",
                        column: x => x.ProcessInstanceId,
                        principalTable: "Wf_Hi_ProcessInstances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ContractDeposits",
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
                    DepositAmount = table.Column<float>(nullable: false),
                    ConventionalRefundAmount = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractDeposits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Contracts",
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
                    Name = table.Column<string>(nullable: true),
                    ContractNumber = table.Column<string>(nullable: true),
                    CategoryId = table.Column<int>(nullable: true),
                    ContractionMethodId = table.Column<int>(nullable: true),
                    Section = table.Column<string>(nullable: true),
                    Employer = table.Column<string>(nullable: true),
                    ContractorId = table.Column<int>(nullable: true),
                    ThirdParty = table.Column<string>(nullable: true),
                    ContractPrice = table.Column<float>(nullable: false),
                    ContractSettlementPrice = table.Column<float>(nullable: false),
                    Content = table.Column<string>(nullable: true),
                    PaymentTerms = table.Column<string>(nullable: true),
                    SigningDate = table.Column<DateTime>(nullable: true),
                    Period = table.Column<int>(nullable: false),
                    ContractId = table.Column<int>(nullable: true),
                    PerformanceStartDate = table.Column<DateTime>(nullable: true),
                    PerformanceEndDate = table.Column<DateTime>(nullable: true),
                    Signature = table.Column<string>(nullable: true),
                    Mid = table.Column<int>(nullable: true),
                    State = table.Column<int>(nullable: false),
                    ProcessInstanceId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contracts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contracts_Options_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Options",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Contracts_ContractDeposits_ContractId",
                        column: x => x.ContractId,
                        principalTable: "ContractDeposits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Contracts_Options_ContractionMethodId",
                        column: x => x.ContractionMethodId,
                        principalTable: "Options",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Contracts_ConstructionUnits_ContractorId",
                        column: x => x.ContractorId,
                        principalTable: "ConstructionUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Contracts_ConstructionUnits_Mid",
                        column: x => x.Mid,
                        principalTable: "ConstructionUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Contracts_Wf_Hi_ProcessInstances_ProcessInstanceId",
                        column: x => x.ProcessInstanceId,
                        principalTable: "Wf_Hi_ProcessInstances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Contracts_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContractAttachments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ContractId = table.Column<int>(nullable: false),
                    FileMetaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContractAttachments_Contracts_ContractId",
                        column: x => x.ContractId,
                        principalTable: "Contracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContractAttachments_FileMetas_FileMetaId",
                        column: x => x.FileMetaId,
                        principalTable: "FileMetas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConstructionUnits_Mid",
                table: "ConstructionUnits",
                column: "Mid");

            migrationBuilder.CreateIndex(
                name: "IX_ConstructionUnits_ProcessInstanceId",
                table: "ConstructionUnits",
                column: "ProcessInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractAttachments_ContractId",
                table: "ContractAttachments",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractAttachments_FileMetaId",
                table: "ContractAttachments",
                column: "FileMetaId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_CategoryId",
                table: "Contracts",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_ContractId",
                table: "Contracts",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_ContractionMethodId",
                table: "Contracts",
                column: "ContractionMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_ContractorId",
                table: "Contracts",
                column: "ContractorId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_Mid",
                table: "Contracts",
                column: "Mid");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_ProcessInstanceId",
                table: "Contracts",
                column: "ProcessInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_ProjectId",
                table: "Contracts",
                column: "ProjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContractAttachments");

            migrationBuilder.DropTable(
                name: "Contracts");

            migrationBuilder.DropTable(
                name: "ContractDeposits");

            migrationBuilder.DropTable(
                name: "ConstructionUnits");
        }
    }
}
