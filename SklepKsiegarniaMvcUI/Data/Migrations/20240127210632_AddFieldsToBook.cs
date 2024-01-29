using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SklepKsiegarniaMvcUI.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldsToBook : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CoverType",
                table: "Book",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Book",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "ReleaseDate",
                table: "Book",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoverType",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "ReleaseDate",
                table: "Book");
        }
    }
}
