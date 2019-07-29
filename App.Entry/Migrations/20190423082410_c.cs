using Microsoft.EntityFrameworkCore.Migrations;

namespace App.Entry.Migrations
{
    public partial class c : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Field_FieldDefinition_FieldDefinitionId",
                table: "Field");

            migrationBuilder.DropForeignKey(
                name: "FK_Field_FormDefinition_FormDefinitionId",
                table: "Field");

            migrationBuilder.DropForeignKey(
                name: "FK_Field_Wf_Hi_ProcessInstances_Wf_Hi_ProcessInstanceId",
                table: "Field");

            migrationBuilder.DropForeignKey(
                name: "FK_FieldDefinition_FormDefinition_FormDefinitionId",
                table: "FieldDefinition");

            migrationBuilder.DropForeignKey(
                name: "FK_FormModel_FormDefinition_FormDefinitionId",
                table: "FormModel");

            migrationBuilder.DropForeignKey(
                name: "FK_Wf_Hi_ProcessInstances_FormDefinition_FormDefinitionId",
                table: "Wf_Hi_ProcessInstances");

            migrationBuilder.DropForeignKey(
                name: "FK_Wf_Ru_Variables_Wf_Ru_Executions_ProcessExecutionId",
                table: "Wf_Ru_Variables");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FormModel",
                table: "FormModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FormDefinition",
                table: "FormDefinition");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FieldDefinition",
                table: "FieldDefinition");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Field",
                table: "Field");

            migrationBuilder.RenameTable(
                name: "FormModel",
                newName: "FormModels");

            migrationBuilder.RenameTable(
                name: "FormDefinition",
                newName: "FormDefinitions");

            migrationBuilder.RenameTable(
                name: "FieldDefinition",
                newName: "FieldDefinitions");

            migrationBuilder.RenameTable(
                name: "Field",
                newName: "Fields");

            migrationBuilder.RenameIndex(
                name: "IX_FormModel_FormDefinitionId",
                table: "FormModels",
                newName: "IX_FormModels_FormDefinitionId");

            migrationBuilder.RenameIndex(
                name: "IX_FieldDefinition_FormDefinitionId",
                table: "FieldDefinitions",
                newName: "IX_FieldDefinitions_FormDefinitionId");

            migrationBuilder.RenameIndex(
                name: "IX_Field_Wf_Hi_ProcessInstanceId",
                table: "Fields",
                newName: "IX_Fields_Wf_Hi_ProcessInstanceId");

            migrationBuilder.RenameIndex(
                name: "IX_Field_FormDefinitionId",
                table: "Fields",
                newName: "IX_Fields_FormDefinitionId");

            migrationBuilder.RenameIndex(
                name: "IX_Field_FieldDefinitionId",
                table: "Fields",
                newName: "IX_Fields_FieldDefinitionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FormModels",
                table: "FormModels",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FormDefinitions",
                table: "FormDefinitions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FieldDefinitions",
                table: "FieldDefinitions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Fields",
                table: "Fields",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FieldDefinitions_FormDefinitions_FormDefinitionId",
                table: "FieldDefinitions",
                column: "FormDefinitionId",
                principalTable: "FormDefinitions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Fields_FieldDefinitions_FieldDefinitionId",
                table: "Fields",
                column: "FieldDefinitionId",
                principalTable: "FieldDefinitions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Fields_FormDefinitions_FormDefinitionId",
                table: "Fields",
                column: "FormDefinitionId",
                principalTable: "FormDefinitions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Fields_Wf_Hi_ProcessInstances_Wf_Hi_ProcessInstanceId",
                table: "Fields",
                column: "Wf_Hi_ProcessInstanceId",
                principalTable: "Wf_Hi_ProcessInstances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FormModels_FormDefinitions_FormDefinitionId",
                table: "FormModels",
                column: "FormDefinitionId",
                principalTable: "FormDefinitions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Wf_Hi_ProcessInstances_FormDefinitions_FormDefinitionId",
                table: "Wf_Hi_ProcessInstances",
                column: "FormDefinitionId",
                principalTable: "FormDefinitions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Wf_Ru_Variables_Wf_Ru_Executions_ProcessExecutionId",
                table: "Wf_Ru_Variables",
                column: "ProcessExecutionId",
                principalTable: "Wf_Ru_Executions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FieldDefinitions_FormDefinitions_FormDefinitionId",
                table: "FieldDefinitions");

            migrationBuilder.DropForeignKey(
                name: "FK_Fields_FieldDefinitions_FieldDefinitionId",
                table: "Fields");

            migrationBuilder.DropForeignKey(
                name: "FK_Fields_FormDefinitions_FormDefinitionId",
                table: "Fields");

            migrationBuilder.DropForeignKey(
                name: "FK_Fields_Wf_Hi_ProcessInstances_Wf_Hi_ProcessInstanceId",
                table: "Fields");

            migrationBuilder.DropForeignKey(
                name: "FK_FormModels_FormDefinitions_FormDefinitionId",
                table: "FormModels");

            migrationBuilder.DropForeignKey(
                name: "FK_Wf_Hi_ProcessInstances_FormDefinitions_FormDefinitionId",
                table: "Wf_Hi_ProcessInstances");

            migrationBuilder.DropForeignKey(
                name: "FK_Wf_Ru_Variables_Wf_Ru_Executions_ProcessExecutionId",
                table: "Wf_Ru_Variables");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FormModels",
                table: "FormModels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FormDefinitions",
                table: "FormDefinitions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Fields",
                table: "Fields");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FieldDefinitions",
                table: "FieldDefinitions");

            migrationBuilder.RenameTable(
                name: "FormModels",
                newName: "FormModel");

            migrationBuilder.RenameTable(
                name: "FormDefinitions",
                newName: "FormDefinition");

            migrationBuilder.RenameTable(
                name: "Fields",
                newName: "Field");

            migrationBuilder.RenameTable(
                name: "FieldDefinitions",
                newName: "FieldDefinition");

            migrationBuilder.RenameIndex(
                name: "IX_FormModels_FormDefinitionId",
                table: "FormModel",
                newName: "IX_FormModel_FormDefinitionId");

            migrationBuilder.RenameIndex(
                name: "IX_Fields_Wf_Hi_ProcessInstanceId",
                table: "Field",
                newName: "IX_Field_Wf_Hi_ProcessInstanceId");

            migrationBuilder.RenameIndex(
                name: "IX_Fields_FormDefinitionId",
                table: "Field",
                newName: "IX_Field_FormDefinitionId");

            migrationBuilder.RenameIndex(
                name: "IX_Fields_FieldDefinitionId",
                table: "Field",
                newName: "IX_Field_FieldDefinitionId");

            migrationBuilder.RenameIndex(
                name: "IX_FieldDefinitions_FormDefinitionId",
                table: "FieldDefinition",
                newName: "IX_FieldDefinition_FormDefinitionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FormModel",
                table: "FormModel",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FormDefinition",
                table: "FormDefinition",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Field",
                table: "Field",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FieldDefinition",
                table: "FieldDefinition",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Field_FieldDefinition_FieldDefinitionId",
                table: "Field",
                column: "FieldDefinitionId",
                principalTable: "FieldDefinition",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Field_FormDefinition_FormDefinitionId",
                table: "Field",
                column: "FormDefinitionId",
                principalTable: "FormDefinition",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Field_Wf_Hi_ProcessInstances_Wf_Hi_ProcessInstanceId",
                table: "Field",
                column: "Wf_Hi_ProcessInstanceId",
                principalTable: "Wf_Hi_ProcessInstances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FieldDefinition_FormDefinition_FormDefinitionId",
                table: "FieldDefinition",
                column: "FormDefinitionId",
                principalTable: "FormDefinition",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FormModel_FormDefinition_FormDefinitionId",
                table: "FormModel",
                column: "FormDefinitionId",
                principalTable: "FormDefinition",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Wf_Hi_ProcessInstances_FormDefinition_FormDefinitionId",
                table: "Wf_Hi_ProcessInstances",
                column: "FormDefinitionId",
                principalTable: "FormDefinition",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Wf_Ru_Variables_Wf_Ru_Executions_ProcessExecutionId",
                table: "Wf_Ru_Variables",
                column: "ProcessExecutionId",
                principalTable: "Wf_Ru_Executions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
