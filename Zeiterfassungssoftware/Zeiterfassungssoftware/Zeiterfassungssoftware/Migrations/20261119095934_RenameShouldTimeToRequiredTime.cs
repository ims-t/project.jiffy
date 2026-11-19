using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zeiterfassungssoftware.Migrations
{
    /// <inheritdoc />
    public partial class RenameShouldTimeToRequiredTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Entries_ShouldTimes_ShouldTimeId",
                table: "Entries");

            migrationBuilder.RenameTable(
                name: "ShouldTimes",
                newName: "RequiredTimes");

            migrationBuilder.RenameIndex(
                name: "IX_ShouldTimes_ClassId",
                table: "RequiredTimes",
                newName: "IX_RequiredTimes_ClassId");

            migrationBuilder.RenameColumn(
                name: "ShouldTimeId",
                table: "Entries",
                newName: "RequiredTimeId");

            migrationBuilder.RenameIndex(
                name: "IX_Entries_ShouldTimeId",
                table: "Entries",
                newName: "IX_Entries_RequiredTimeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Entries_RequiredTimes_RequiredTimeId",
                table: "Entries",
                column: "RequiredTimeId",
                principalTable: "RequiredTimes",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Entries_RequiredTimes_RequiredTimeId",
                table: "Entries");

            migrationBuilder.RenameIndex(
                name: "IX_Entries_RequiredTimeId",
                table: "Entries",
                newName: "IX_Entries_ShouldTimeId");

            migrationBuilder.RenameColumn(
                name: "RequiredTimeId",
                table: "Entries",
                newName: "ShouldTimeId");

            migrationBuilder.RenameIndex(
                name: "IX_RequiredTimes_ClassId",
                table: "RequiredTimes",
                newName: "IX_ShouldTimes_ClassId");

            migrationBuilder.RenameTable(
                name: "RequiredTimes",
                newName: "ShouldTimes");

            migrationBuilder.AddForeignKey(
                name: "FK_Entries_ShouldTimes_ShouldTimeId",
                table: "Entries",
                column: "ShouldTimeId",
                principalTable: "ShouldTimes",
                principalColumn: "Id");
        }
    }
}
