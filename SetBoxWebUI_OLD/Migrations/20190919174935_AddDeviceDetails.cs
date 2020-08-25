using Microsoft.EntityFrameworkCore.Migrations;

namespace SetBoxWebUI.Migrations
{
    public partial class AddDeviceDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApkVersion",
                table: "SetBoxDevices",
                maxLength: 255,
                nullable: true,
                defaultValue: "1");

            migrationBuilder.AddColumn<string>(
                name: "DeviceName",
                table: "SetBoxDevices",
                maxLength: 255,
                nullable: true,
                defaultValue: "SetBox");

            migrationBuilder.AddColumn<string>(
                name: "Manufacturer",
                table: "SetBoxDevices",
                maxLength: 255,
                nullable: true,
                defaultValue: "unknown");

            migrationBuilder.AddColumn<string>(
                name: "Model",
                table: "SetBoxDevices",
                maxLength: 255,
                nullable: true,
                defaultValue: "unknown");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApkVersion",
                table: "SetBoxDevices");

            migrationBuilder.DropColumn(
                name: "DeviceName",
                table: "SetBoxDevices");

            migrationBuilder.DropColumn(
                name: "Manufacturer",
                table: "SetBoxDevices");

            migrationBuilder.DropColumn(
                name: "Model",
                table: "SetBoxDevices");
        }
    }
}
