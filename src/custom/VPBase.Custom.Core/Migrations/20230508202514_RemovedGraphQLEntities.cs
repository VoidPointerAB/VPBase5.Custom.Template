using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VPBase.Custom.Core.Migrations
{
    /// <inheritdoc />
    public partial class RemovedGraphQLEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VP_Template_GraphQLs",
                schema: "Custom.Sample");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VP_Template_GraphQLs",
                schema: "Custom.Sample",
                columns: table => new
                {
                    VP_Template_GraphQLId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AnonymizedUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VP_Template_GraphQLs", x => x.VP_Template_GraphQLId);
                });
        }
    }
}
