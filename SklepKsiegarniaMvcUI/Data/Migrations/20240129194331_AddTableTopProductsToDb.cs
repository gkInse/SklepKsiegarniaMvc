using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SklepKsiegarniaMvcUI.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTableTopProductsToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TopProducts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TopProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TopProducts_Book_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Book",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TopProducts_ProductId",
                table: "TopProducts",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TopProducts");
        }
    }
}
