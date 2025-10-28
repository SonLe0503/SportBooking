using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SportBooking.Migrations
{
    /// <inheritdoc />
    public partial class AddAvatarAndCourtFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Avatar",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Avatar",
                table: "Fields",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CourtDetails",
                table: "Fields",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<TimeOnly>(
                name: "closeTime",
                table: "Fields",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isFixedPrice",
                table: "Fields",
                type: "bit",
                nullable: true,
                defaultValue: true);

            migrationBuilder.AddColumn<string>(
                name: "link",
                table: "Fields",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "openDays",
                table: "Fields",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<TimeOnly>(
                name: "openTime",
                table: "Fields",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "type",
                table: "Fields",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Avatar",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Avatar",
                table: "Fields");

            migrationBuilder.DropColumn(
                name: "CourtDetails",
                table: "Fields");

            migrationBuilder.DropColumn(
                name: "closeTime",
                table: "Fields");

            migrationBuilder.DropColumn(
                name: "isFixedPrice",
                table: "Fields");

            migrationBuilder.DropColumn(
                name: "link",
                table: "Fields");

            migrationBuilder.DropColumn(
                name: "openDays",
                table: "Fields");

            migrationBuilder.DropColumn(
                name: "openTime",
                table: "Fields");

            migrationBuilder.DropColumn(
                name: "type",
                table: "Fields");
        }
    }
}
