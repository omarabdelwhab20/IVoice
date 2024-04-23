using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IVoice.Migrations
{
    /// <inheritdoc />
    public partial class addcolumnquan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "products",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "products");
        }
    }
}
