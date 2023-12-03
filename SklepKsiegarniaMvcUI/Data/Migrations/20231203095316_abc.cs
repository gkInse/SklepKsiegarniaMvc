using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SklepKsiegarniaMvcUI.Data.Migrations
{
    /// <inheritdoc />
    public partial class abc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Qutantity",
                table: "OrderDetail",
                newName: "Quantity");

            migrationBuilder.AddColumn<double>(
                name: "UnitPrice",
                table: "CartDetail",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnitPrice",
                table: "CartDetail");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "OrderDetail",
                newName: "Qutantity");
        }
    }
}
