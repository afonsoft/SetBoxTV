using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SetBoxWebUI.Migrations
{
    public partial class AddFileCheckSum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDateTime",
                table: "SetBoxDevices",
                nullable: false,
                defaultValue: new DateTime(2019, 9, 6, 16, 47, 23, 223, DateTimeKind.Local).AddTicks(9860),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 9, 6, 16, 22, 41, 441, DateTimeKind.Local).AddTicks(6871));

            migrationBuilder.AlterColumn<Guid>(
                name: "DeviceId",
                table: "SetBoxDevices",
                nullable: false,
                defaultValue: new Guid("326da4b4-9242-4c22-88db-4ed51bb6b754"),
                oldClrType: typeof(Guid),
                oldDefaultValue: new Guid("bb4b8126-ccfd-44f4-8d8c-23bfca2f364a"));

            migrationBuilder.AddColumn<Guid>(
                name: "FileDevicesId",
                table: "SetBoxDevices",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDateTime",
                table: "SetBoxDeviceLogAccesses",
                nullable: true,
                defaultValue: new DateTime(2019, 9, 6, 16, 47, 23, 221, DateTimeKind.Local).AddTicks(1517),
                oldClrType: typeof(DateTime),
                oldNullable: true,
                oldDefaultValue: new DateTime(2019, 9, 6, 16, 22, 41, 438, DateTimeKind.Local).AddTicks(6869));

            migrationBuilder.AlterColumn<Guid>(
                name: "DeviceLogAccessesId",
                table: "SetBoxDeviceLogAccesses",
                nullable: false,
                defaultValue: new Guid("d9934de6-5db7-40be-96c0-1141644ae0fd"),
                oldClrType: typeof(Guid),
                oldDefaultValue: new Guid("8eba78b8-b4b8-4110-8af0-51220b196598"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDateTime",
                table: "SetBoxConfigs",
                nullable: false,
                defaultValue: new DateTime(2019, 9, 6, 16, 47, 23, 218, DateTimeKind.Local).AddTicks(1769),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 9, 6, 16, 22, 41, 435, DateTimeKind.Local).AddTicks(6898));

            migrationBuilder.AlterColumn<Guid>(
                name: "ConfigId",
                table: "SetBoxConfigs",
                nullable: false,
                defaultValue: new Guid("fd4547a7-4ed9-4bf1-8423-e428adb5f89f"),
                oldClrType: typeof(Guid),
                oldDefaultValue: new Guid("f8dc38bd-d86e-4878-aadb-b9d855687423"));

            migrationBuilder.AlterColumn<Guid>(
                name: "AddressId",
                table: "SetBoxCompany",
                nullable: false,
                defaultValue: new Guid("1e403c33-c36e-4294-babb-d770f718956c"),
                oldClrType: typeof(Guid),
                oldDefaultValue: new Guid("02ff6e2b-80fb-4f8f-8aa7-fbcc2aa27dc4"));

            migrationBuilder.AlterColumn<Guid>(
                name: "AddressId",
                table: "SetBoxAddress",
                nullable: false,
                defaultValue: new Guid("68017fa0-6b11-4f84-95a3-1cef43122d66"),
                oldClrType: typeof(Guid),
                oldDefaultValue: new Guid("54bb1266-f917-4cf9-badf-ae5d3eff3a6e"));

            migrationBuilder.CreateTable(
                name: "SetBoxDeviceFiles",
                columns: table => new
                {
                    DeviceFilesId = table.Column<Guid>(nullable: false, defaultValue: new Guid("b8f08b6a-4518-4d15-bf2e-4dd39a3052b5")),
                    DeviceId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SetBoxDeviceFiles", x => x.DeviceFilesId);
                    table.ForeignKey(
                        name: "FK_SetBoxDeviceFiles_SetBoxDevices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "SetBoxDevices",
                        principalColumn: "DeviceId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SetBoxFileCheckSum",
                columns: table => new
                {
                    FileId = table.Column<Guid>(nullable: false, defaultValue: new Guid("e31301a4-6259-4db0-9ec1-5ea298592e47")),
                    Name = table.Column<string>(maxLength: 255, nullable: true),
                    Extension = table.Column<string>(maxLength: 10, nullable: true),
                    Size = table.Column<long>(nullable: false),
                    Url = table.Column<string>(maxLength: 4000, nullable: true),
                    CheckSum = table.Column<string>(nullable: true),
                    DeviceFilesId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SetBoxFileCheckSum", x => x.FileId);
                    table.ForeignKey(
                        name: "FK_SetBoxFileCheckSum_SetBoxDeviceFiles_DeviceFilesId",
                        column: x => x.DeviceFilesId,
                        principalTable: "SetBoxDeviceFiles",
                        principalColumn: "DeviceFilesId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SetBoxFileDevices",
                columns: table => new
                {
                    FileDevicesId = table.Column<Guid>(nullable: false, defaultValue: new Guid("2389091b-7326-4700-beba-13bfc2f2d2a7")),
                    FileId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SetBoxFileDevices", x => x.FileDevicesId);
                    table.ForeignKey(
                        name: "FK_SetBoxFileDevices_SetBoxFileCheckSum_FileId",
                        column: x => x.FileId,
                        principalTable: "SetBoxFileCheckSum",
                        principalColumn: "FileId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SetBoxDevices_FileDevicesId",
                table: "SetBoxDevices",
                column: "FileDevicesId");

            migrationBuilder.CreateIndex(
                name: "IX_SetBoxDeviceFiles_DeviceId",
                table: "SetBoxDeviceFiles",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_SetBoxFileCheckSum_DeviceFilesId",
                table: "SetBoxFileCheckSum",
                column: "DeviceFilesId");

            migrationBuilder.CreateIndex(
                name: "IX_SetBoxFileDevices_FileId",
                table: "SetBoxFileDevices",
                column: "FileId");

            migrationBuilder.AddForeignKey(
                name: "FK_SetBoxDevices_SetBoxFileDevices_FileDevicesId",
                table: "SetBoxDevices",
                column: "FileDevicesId",
                principalTable: "SetBoxFileDevices",
                principalColumn: "FileDevicesId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SetBoxDevices_SetBoxFileDevices_FileDevicesId",
                table: "SetBoxDevices");

            migrationBuilder.DropTable(
                name: "SetBoxFileDevices");

            migrationBuilder.DropTable(
                name: "SetBoxFileCheckSum");

            migrationBuilder.DropTable(
                name: "SetBoxDeviceFiles");

            migrationBuilder.DropIndex(
                name: "IX_SetBoxDevices_FileDevicesId",
                table: "SetBoxDevices");

            migrationBuilder.DropColumn(
                name: "FileDevicesId",
                table: "SetBoxDevices");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDateTime",
                table: "SetBoxDevices",
                nullable: false,
                defaultValue: new DateTime(2019, 9, 6, 16, 22, 41, 441, DateTimeKind.Local).AddTicks(6871),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 9, 6, 16, 47, 23, 223, DateTimeKind.Local).AddTicks(9860));

            migrationBuilder.AlterColumn<Guid>(
                name: "DeviceId",
                table: "SetBoxDevices",
                nullable: false,
                defaultValue: new Guid("bb4b8126-ccfd-44f4-8d8c-23bfca2f364a"),
                oldClrType: typeof(Guid),
                oldDefaultValue: new Guid("326da4b4-9242-4c22-88db-4ed51bb6b754"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDateTime",
                table: "SetBoxDeviceLogAccesses",
                nullable: true,
                defaultValue: new DateTime(2019, 9, 6, 16, 22, 41, 438, DateTimeKind.Local).AddTicks(6869),
                oldClrType: typeof(DateTime),
                oldNullable: true,
                oldDefaultValue: new DateTime(2019, 9, 6, 16, 47, 23, 221, DateTimeKind.Local).AddTicks(1517));

            migrationBuilder.AlterColumn<Guid>(
                name: "DeviceLogAccessesId",
                table: "SetBoxDeviceLogAccesses",
                nullable: false,
                defaultValue: new Guid("8eba78b8-b4b8-4110-8af0-51220b196598"),
                oldClrType: typeof(Guid),
                oldDefaultValue: new Guid("d9934de6-5db7-40be-96c0-1141644ae0fd"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDateTime",
                table: "SetBoxConfigs",
                nullable: false,
                defaultValue: new DateTime(2019, 9, 6, 16, 22, 41, 435, DateTimeKind.Local).AddTicks(6898),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 9, 6, 16, 47, 23, 218, DateTimeKind.Local).AddTicks(1769));

            migrationBuilder.AlterColumn<Guid>(
                name: "ConfigId",
                table: "SetBoxConfigs",
                nullable: false,
                defaultValue: new Guid("f8dc38bd-d86e-4878-aadb-b9d855687423"),
                oldClrType: typeof(Guid),
                oldDefaultValue: new Guid("fd4547a7-4ed9-4bf1-8423-e428adb5f89f"));

            migrationBuilder.AlterColumn<Guid>(
                name: "AddressId",
                table: "SetBoxCompany",
                nullable: false,
                defaultValue: new Guid("02ff6e2b-80fb-4f8f-8aa7-fbcc2aa27dc4"),
                oldClrType: typeof(Guid),
                oldDefaultValue: new Guid("1e403c33-c36e-4294-babb-d770f718956c"));

            migrationBuilder.AlterColumn<Guid>(
                name: "AddressId",
                table: "SetBoxAddress",
                nullable: false,
                defaultValue: new Guid("54bb1266-f917-4cf9-badf-ae5d3eff3a6e"),
                oldClrType: typeof(Guid),
                oldDefaultValue: new Guid("68017fa0-6b11-4f84-95a3-1cef43122d66"));
        }
    }
}
