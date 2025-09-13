#nullable disable

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ManagerService.Server.Layers.DbLayer.Migrations
{
    /// <inheritdoc />
    public partial class Idreturned : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Timers",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                table: "Timers");
        }
    }
}
