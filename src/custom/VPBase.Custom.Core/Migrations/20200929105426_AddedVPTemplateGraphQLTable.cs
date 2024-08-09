using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VPBase.Custom.Core.Migrations
{
    public partial class AddedVPTemplateGraphQLTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VP_Template_GraphQLs",
                columns: table => new
                {
                    TenantId = table.Column<string>(nullable: false),
                    DeletedUtc = table.Column<DateTime>(nullable: true),
                    CreatedUtc = table.Column<DateTime>(nullable: false),
                    ModifiedUtc = table.Column<DateTime>(nullable: false),
                    AnonymizedUtc = table.Column<DateTime>(nullable: true),
                    VP_Template_GraphQLId = table.Column<string>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VP_Template_GraphQLs", x => x.VP_Template_GraphQLId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VP_Template_GraphQLs");
        }
    }
}
