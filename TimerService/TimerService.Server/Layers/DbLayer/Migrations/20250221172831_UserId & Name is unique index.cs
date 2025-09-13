#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace ManagerService.Server.Layers.DbLayer.Migrations
{
    /// <inheritdoc />
    public partial class UserIdNameisuniqueindex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Timers",
                table: "Timers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Timers",
                table: "Timers",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Timers_UserId_Name",
                table: "Timers",
                columns: new[] { "UserId", "Name" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Timers",
                table: "Timers");

            migrationBuilder.DropIndex(
                name: "IX_Timers_UserId_Name",
                table: "Timers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Timers",
                table: "Timers",
                columns: new[] { "UserId", "Name" });
        }
    }
}
