using Microsoft.EntityFrameworkCore.Migrations;

namespace VPBase.Custom.Core.Migrations
{
    public partial class RenameSchemaSampleTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Custom_VP_Template_SimpleMvcs",
                table: "Custom_VP_Template_SimpleMvcs");

            migrationBuilder.EnsureSchema(
                name: "Custom.Sample");

            migrationBuilder.RenameTable(
                name: "VP_Template_Mvcs",
                newName: "VP_Template_Mvcs",
                newSchema: "Custom.Sample");

            migrationBuilder.RenameTable(
                name: "VP_Template_GraphQLs",
                newName: "VP_Template_GraphQLs",
                newSchema: "Custom.Sample");

            migrationBuilder.RenameTable(
                name: "Custom_VP_Template_SimpleMvcs",
                newName: "VP_Template_SimpleMvcs",
                newSchema: "Custom.Sample");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VP_Template_SimpleMvcs",
                schema: "Custom.Sample",
                table: "VP_Template_SimpleMvcs",
                column: "VP_Template_SimpleMvcId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_VP_Template_SimpleMvcs",
                schema: "Custom.Sample",
                table: "VP_Template_SimpleMvcs");

            migrationBuilder.RenameTable(
                name: "VP_Template_Mvcs",
                schema: "Custom.Sample",
                newName: "VP_Template_Mvcs");

            migrationBuilder.RenameTable(
                name: "VP_Template_GraphQLs",
                schema: "Custom.Sample",
                newName: "VP_Template_GraphQLs");

            migrationBuilder.RenameTable(
                name: "VP_Template_SimpleMvcs",
                schema: "Custom.Sample",
                newName: "Custom_VP_Template_SimpleMvcs");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Custom_VP_Template_SimpleMvcs",
                table: "Custom_VP_Template_SimpleMvcs",
                column: "VP_Template_SimpleMvcId");
        }
    }
}
