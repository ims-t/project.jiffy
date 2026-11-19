using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zeiterfassungssoftware.Migrations
{
    /// <inheritdoc />
    public partial class AddEntryEditTracking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EditedAt",
                table: "Entries",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Justification",
                table: "Entries",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EditedAt",
                table: "Entries");

            migrationBuilder.DropColumn(
                name: "Justification",
                table: "Entries");
        }
    }
}
