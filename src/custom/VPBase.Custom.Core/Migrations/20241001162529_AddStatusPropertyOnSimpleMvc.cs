using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VPBase.Custom.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddStatusPropertyOnSimpleMvc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                schema: "Custom.Sample",
                table: "VP_Template_SimpleMvcs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                schema: "Custom.Sample",
                table: "VP_Template_SimpleMvcs");
        }
    }
}
