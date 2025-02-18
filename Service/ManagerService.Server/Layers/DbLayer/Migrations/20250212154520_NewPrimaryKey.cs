using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ManagerService.Server.Layers.DbLayer.Migrations
{
    /// <inheritdoc />
    public partial class NewPrimaryKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Timers",
                table: "Timers");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Timers");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Timers",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Timers",
                table: "Timers",
                columns: new[] { "UserId", "Name" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Timers",
                table: "Timers");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Timers",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Timers",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Timers",
                table: "Timers",
                column: "Id");
        }
    }
}
