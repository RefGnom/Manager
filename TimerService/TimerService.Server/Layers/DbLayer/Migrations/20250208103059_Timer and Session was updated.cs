#nullable disable

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ManagerService.Server.Layers.DbLayer.Migrations
{
    /// <inheritdoc />
    public partial class TimerandSessionwasupdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PingTime",
                table: "Timers");

            migrationBuilder.AddColumn<bool>(
                name: "IsOver",
                table: "TimerSessions",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Timers",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "PingTimeout",
                table: "Timers",
                type: "interval",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Timers",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsOver",
                table: "TimerSessions");

            migrationBuilder.DropColumn(
                name: "PingTimeout",
                table: "Timers");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Timers");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Timers",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<DateTime>(
                name: "PingTime",
                table: "Timers",
                type: "timestamp with time zone",
                nullable: true);
        }
    }
}
