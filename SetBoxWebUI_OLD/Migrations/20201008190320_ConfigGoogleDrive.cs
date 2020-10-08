using Microsoft.EntityFrameworkCore.Migrations;

namespace SetBoxWebUI.Migrations
{
    public partial class ConfigGoogleDrive : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GoogleDrivePassword",
                table: "SetBoxConfigs",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GoogleDriveUrl",
                table: "SetBoxConfigs",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GoogleDriveUserName",
                table: "SetBoxConfigs",
                maxLength: 500,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GoogleDrivePassword",
                table: "SetBoxConfigs");

            migrationBuilder.DropColumn(
                name: "GoogleDriveUrl",
                table: "SetBoxConfigs");

            migrationBuilder.DropColumn(
                name: "GoogleDriveUserName",
                table: "SetBoxConfigs");
        }
    }
}
