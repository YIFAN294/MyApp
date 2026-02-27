using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAgeColumnToExistingUsersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 2, 22, 16, 17, 3, 753, DateTimeKind.Utc).AddTicks(3215), "$2a$11$d1rMZUgAh8/L1NvboULcd.dexineq8pvDr.4uQlykS0wUGT/rxuOi" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 2, 22, 16, 14, 11, 643, DateTimeKind.Utc).AddTicks(6969), "$2a$11$n60iIyyhzPUueYKyBw01G.eOLBRWcPx4YSoxSsPmCqnJS1qaH1nwu" });
        }
    }
}
