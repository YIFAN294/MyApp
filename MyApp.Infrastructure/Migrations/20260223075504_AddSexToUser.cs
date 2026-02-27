using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSexToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Sex",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "Sex" },
                values: new object[] { new DateTime(2026, 2, 23, 7, 55, 4, 572, DateTimeKind.Utc).AddTicks(9398), "$2a$11$dfbGIDLDm09XivZupUHX0.V5JrBNrgt2ww8YF8ngWyrqqCgbLlkn.", "" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sex",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 2, 22, 16, 17, 3, 753, DateTimeKind.Utc).AddTicks(3215), "$2a$11$d1rMZUgAh8/L1NvboULcd.dexineq8pvDr.4uQlykS0wUGT/rxuOi" });
        }
    }
}
