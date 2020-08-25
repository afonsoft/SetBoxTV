using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SetBoxWebUI.Migrations
{
    public partial class AddCompanyCreateDateTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDateTime",
                table: "SetBoxSupport",
                nullable: false,
                defaultValueSql: "getdate()",
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 9, 16, 12, 8, 38, 550, DateTimeKind.Local).AddTicks(1153));

            migrationBuilder.AlterColumn<Guid>(
                name: "SupportId",
                table: "SetBoxSupport",
                nullable: false,
                oldClrType: typeof(Guid),
                oldDefaultValue: new Guid("f46db707-3254-45ca-af6e-7d9623dc5f07"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDateTime",
                table: "SetBoxFilesDevices",
                nullable: false,
                defaultValueSql: "getdate()");

            migrationBuilder.AlterColumn<Guid>(
                name: "FileId",
                table: "SetBoxFileCheckSum",
                nullable: false,
                oldClrType: typeof(Guid),
                oldDefaultValue: new Guid("61826a4a-d824-4ba8-8981-a6dc2c459fe4"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDateTime",
                table: "SetBoxFileCheckSum",
                nullable: false,
                defaultValueSql: "getdate()");

            migrationBuilder.AlterColumn<string>(
                name: "Version",
                table: "SetBoxDevices",
                maxLength: 255,
                nullable: true,
                defaultValue: "unknown",
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Platform",
                table: "SetBoxDevices",
                maxLength: 255,
                nullable: true,
                defaultValue: "unknown",
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true,
                oldDefaultValue: "Android");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDateTime",
                table: "SetBoxDevices",
                nullable: false,
                defaultValueSql: "getdate()",
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 9, 16, 12, 8, 38, 533, DateTimeKind.Local).AddTicks(6269));

            migrationBuilder.AlterColumn<Guid>(
                name: "DeviceId",
                table: "SetBoxDevices",
                nullable: false,
                oldClrType: typeof(Guid),
                oldDefaultValue: new Guid("ac24f993-12b1-4217-920c-2405690ce3ac"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDateTime",
                table: "SetBoxDeviceLogAccesses",
                nullable: false,
                defaultValueSql: "getdate()",
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 9, 16, 12, 8, 38, 527, DateTimeKind.Local).AddTicks(442));

            migrationBuilder.AlterColumn<Guid>(
                name: "DeviceLogAccessesId",
                table: "SetBoxDeviceLogAccesses",
                nullable: false,
                oldClrType: typeof(Guid),
                oldDefaultValue: new Guid("ba7b8c97-d75d-4216-b313-f92de47d315e"));

            migrationBuilder.AlterColumn<Guid>(
                name: "ContactId",
                table: "SetBoxContact",
                nullable: false,
                oldClrType: typeof(Guid),
                oldDefaultValue: new Guid("18b12cba-bf35-4efc-a7ee-b03a1a9b59f6"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDateTime",
                table: "SetBoxContact",
                nullable: false,
                defaultValueSql: "getdate()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDateTime",
                table: "SetBoxConfigs",
                nullable: false,
                defaultValueSql: "getdate()",
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 9, 16, 12, 8, 38, 522, DateTimeKind.Local).AddTicks(4830));

            migrationBuilder.AlterColumn<Guid>(
                name: "ConfigId",
                table: "SetBoxConfigs",
                nullable: false,
                oldClrType: typeof(Guid),
                oldDefaultValue: new Guid("611b1365-c408-4f53-a99e-9a404ceeb3dd"));

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                table: "SetBoxCompany",
                nullable: false,
                oldClrType: typeof(Guid),
                oldDefaultValue: new Guid("1552a667-9fcb-41f3-87eb-84bbd4278354"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDateTime",
                table: "SetBoxCompany",
                nullable: false,
                defaultValueSql: "getdate()");

            migrationBuilder.AlterColumn<Guid>(
                name: "AddressId",
                table: "SetBoxAddress",
                nullable: false,
                oldClrType: typeof(Guid),
                oldDefaultValue: new Guid("fdb66ff9-2d7d-4dd7-838e-40767364102f"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDateTime",
                table: "SetBoxAddress",
                nullable: false,
                defaultValueSql: "getdate()");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationDateTime",
                table: "SetBoxFilesDevices");

            migrationBuilder.DropColumn(
                name: "CreationDateTime",
                table: "SetBoxFileCheckSum");

            migrationBuilder.DropColumn(
                name: "CreationDateTime",
                table: "SetBoxContact");

            migrationBuilder.DropColumn(
                name: "CreationDateTime",
                table: "SetBoxCompany");

            migrationBuilder.DropColumn(
                name: "CreationDateTime",
                table: "SetBoxAddress");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDateTime",
                table: "SetBoxSupport",
                nullable: false,
                defaultValue: new DateTime(2019, 9, 16, 12, 8, 38, 550, DateTimeKind.Local).AddTicks(1153),
                oldClrType: typeof(DateTime),
                oldDefaultValueSql: "getdate()");

            migrationBuilder.AlterColumn<Guid>(
                name: "SupportId",
                table: "SetBoxSupport",
                nullable: false,
                defaultValue: new Guid("f46db707-3254-45ca-af6e-7d9623dc5f07"),
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<Guid>(
                name: "FileId",
                table: "SetBoxFileCheckSum",
                nullable: false,
                defaultValue: new Guid("61826a4a-d824-4ba8-8981-a6dc2c459fe4"),
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<string>(
                name: "Version",
                table: "SetBoxDevices",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true,
                oldDefaultValue: "unknown");

            migrationBuilder.AlterColumn<string>(
                name: "Platform",
                table: "SetBoxDevices",
                maxLength: 255,
                nullable: true,
                defaultValue: "Android",
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true,
                oldDefaultValue: "unknown");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDateTime",
                table: "SetBoxDevices",
                nullable: false,
                defaultValue: new DateTime(2019, 9, 16, 12, 8, 38, 533, DateTimeKind.Local).AddTicks(6269),
                oldClrType: typeof(DateTime),
                oldDefaultValueSql: "getdate()");

            migrationBuilder.AlterColumn<Guid>(
                name: "DeviceId",
                table: "SetBoxDevices",
                nullable: false,
                defaultValue: new Guid("ac24f993-12b1-4217-920c-2405690ce3ac"),
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDateTime",
                table: "SetBoxDeviceLogAccesses",
                nullable: false,
                defaultValue: new DateTime(2019, 9, 16, 12, 8, 38, 527, DateTimeKind.Local).AddTicks(442),
                oldClrType: typeof(DateTime),
                oldDefaultValueSql: "getdate()");

            migrationBuilder.AlterColumn<Guid>(
                name: "DeviceLogAccessesId",
                table: "SetBoxDeviceLogAccesses",
                nullable: false,
                defaultValue: new Guid("ba7b8c97-d75d-4216-b313-f92de47d315e"),
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<Guid>(
                name: "ContactId",
                table: "SetBoxContact",
                nullable: false,
                defaultValue: new Guid("18b12cba-bf35-4efc-a7ee-b03a1a9b59f6"),
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDateTime",
                table: "SetBoxConfigs",
                nullable: false,
                defaultValue: new DateTime(2019, 9, 16, 12, 8, 38, 522, DateTimeKind.Local).AddTicks(4830),
                oldClrType: typeof(DateTime),
                oldDefaultValueSql: "getdate()");

            migrationBuilder.AlterColumn<Guid>(
                name: "ConfigId",
                table: "SetBoxConfigs",
                nullable: false,
                defaultValue: new Guid("611b1365-c408-4f53-a99e-9a404ceeb3dd"),
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                table: "SetBoxCompany",
                nullable: false,
                defaultValue: new Guid("1552a667-9fcb-41f3-87eb-84bbd4278354"),
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<Guid>(
                name: "AddressId",
                table: "SetBoxAddress",
                nullable: false,
                defaultValue: new Guid("fdb66ff9-2d7d-4dd7-838e-40767364102f"),
                oldClrType: typeof(Guid));
        }
    }
}
