using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VPBase.Custom.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddedCategoryOnMvcTemplate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Category",
                schema: "Custom.Sample",
                table: "VP_Template_Mvcs",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                schema: "Custom.Sample",
                table: "VP_Template_Mvcs");
        }
    }
}
