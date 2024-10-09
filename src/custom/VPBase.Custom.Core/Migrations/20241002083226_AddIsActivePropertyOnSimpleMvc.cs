using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VPBase.Custom.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddIsActivePropertyOnSimpleMvc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "Custom.Sample",
                table: "VP_Template_SimpleMvcs",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "Custom.Sample",
                table: "VP_Template_SimpleMvcs");
        }
    }
}
