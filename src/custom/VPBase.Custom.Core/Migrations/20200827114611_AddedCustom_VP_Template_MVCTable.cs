using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VPBase.Custom.Core.Migrations
{
    public partial class AddedCustom_VP_Template_MVCTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Custom_VP_Template_Mvcs",
                columns: table => new
                {
                    TenantId = table.Column<string>(nullable: false),
                    DeletedUtc = table.Column<DateTime>(nullable: true),
                    CreatedUtc = table.Column<DateTime>(nullable: false),
                    ModifiedUtc = table.Column<DateTime>(nullable: false),
                    AnonymizedUtc = table.Column<DateTime>(nullable: true),
                    VP_Template_MvcId = table.Column<string>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Custom_VP_Template_Mvcs", x => x.VP_Template_MvcId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Custom_VP_Template_Mvcs");
        }
    }
}
