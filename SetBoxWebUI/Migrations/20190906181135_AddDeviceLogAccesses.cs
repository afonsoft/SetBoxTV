using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SetBoxWebUI.Migrations
{
    public partial class AddDeviceLogAccesses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastAccessedDate",
                table: "SetBoxDevices");

            migrationBuilder.DropColumn(
                name: "LastIpAcessed",
                table: "SetBoxDevices");

            migrationBuilder.AlterColumn<string>(
                name: "Version",
                table: "SetBoxDevices",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Platform",
                table: "SetBoxDevices",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DeviceIdentifier",
                table: "SetBoxDevices",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDateTime",
                table: "SetBoxDevices",
                nullable: false,
                defaultValue: new DateTime(2019, 9, 6, 15, 11, 34, 400, DateTimeKind.Local).AddTicks(7380),
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<Guid>(
                name: "DeviceId",
                table: "SetBoxDevices",
                nullable: false,
                defaultValue: new Guid("6afb1cdc-2092-4217-892c-213d218c2c75"),
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDateTime",
                table: "SetBoxConfigs",
                nullable: false,
                defaultValue: new DateTime(2019, 9, 6, 15, 11, 34, 403, DateTimeKind.Local).AddTicks(7604),
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<Guid>(
                name: "ConfigId",
                table: "SetBoxConfigs",
                nullable: false,
                defaultValue: new Guid("59609e07-3808-4674-901a-9614d82ca577"),
                oldClrType: typeof(Guid));

            migrationBuilder.CreateTable(
                name: "DeviceLogAccesses",
                columns: table => new
                {
                    DeviceLogAccessesId = table.Column<Guid>(nullable: false),
                    CreationDateTime = table.Column<DateTime>(nullable: true),
                    IpAcessed = table.Column<string>(nullable: true),
                    Message = table.Column<string>(nullable: true),
                    DeviceId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceLogAccesses", x => x.DeviceLogAccessesId);
                    table.ForeignKey(
                        name: "FK_DeviceLogAccesses_SetBoxDevices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "SetBoxDevices",
                        principalColumn: "DeviceId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeviceLogAccesses_DeviceId",
                table: "DeviceLogAccesses",
                column: "DeviceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeviceLogAccesses");

            migrationBuilder.AlterColumn<string>(
                name: "Version",
                table: "SetBoxDevices",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Platform",
                table: "SetBoxDevices",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DeviceIdentifier",
                table: "SetBoxDevices",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDateTime",
                table: "SetBoxDevices",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 9, 6, 15, 11, 34, 400, DateTimeKind.Local).AddTicks(7380));

            migrationBuilder.AlterColumn<Guid>(
                name: "DeviceId",
                table: "SetBoxDevices",
                nullable: false,
                oldClrType: typeof(Guid),
                oldDefaultValue: new Guid("6afb1cdc-2092-4217-892c-213d218c2c75"));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastAccessedDate",
                table: "SetBoxDevices",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastIpAcessed",
                table: "SetBoxDevices",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDateTime",
                table: "SetBoxConfigs",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 9, 6, 15, 11, 34, 403, DateTimeKind.Local).AddTicks(7604));

            migrationBuilder.AlterColumn<Guid>(
                name: "ConfigId",
                table: "SetBoxConfigs",
                nullable: false,
                oldClrType: typeof(Guid),
                oldDefaultValue: new Guid("59609e07-3808-4674-901a-9614d82ca577"));
        }
    }
}
