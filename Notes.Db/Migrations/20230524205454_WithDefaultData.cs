using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Notes.Db.Migrations
{
    /// <inheritdoc />
    public partial class WithDefaultData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "recordsTable",
                columns: new[] { "id", "recordCreationTime", "recordInfo", "recordName", "recordType" },
                values: new object[] { 1, new DateTime(2023, 5, 24, 23, 54, 54, 684, DateTimeKind.Local).AddTicks(9501), "someText", "first", "defaultNote" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "recordsTable",
                keyColumn: "id",
                keyValue: 1);
        }
    }
}
