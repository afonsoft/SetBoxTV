using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SetBoxWebUI.Migrations
{
    public partial class AddSupportCreateDateTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "SupportId",
                table: "SetBoxSupport",
                nullable: false,
                defaultValue: new Guid("f46db707-3254-45ca-af6e-7d9623dc5f07"),
                oldClrType: typeof(Guid),
                oldDefaultValue: new Guid("bed31a2b-fc91-4919-b5ff-566b826f3b97"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDateTime",
                table: "SetBoxSupport",
                nullable: false,
                defaultValue: new DateTime(2019, 9, 16, 12, 8, 38, 550, DateTimeKind.Local).AddTicks(1153));

            migrationBuilder.AlterColumn<Guid>(
                name: "FileId",
                table: "SetBoxFileCheckSum",
                nullable: false,
                defaultValue: new Guid("61826a4a-d824-4ba8-8981-a6dc2c459fe4"),
                oldClrType: typeof(Guid),
                oldDefaultValue: new Guid("fbaf1494-acd8-4682-adcf-5e7a306d0524"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDateTime",
                table: "SetBoxDevices",
                nullable: false,
                defaultValue: new DateTime(2019, 9, 16, 12, 8, 38, 533, DateTimeKind.Local).AddTicks(6269),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 9, 16, 10, 47, 38, 77, DateTimeKind.Local).AddTicks(620));

            migrationBuilder.AlterColumn<Guid>(
                name: "DeviceId",
                table: "SetBoxDevices",
                nullable: false,
                defaultValue: new Guid("ac24f993-12b1-4217-920c-2405690ce3ac"),
                oldClrType: typeof(Guid),
                oldDefaultValue: new Guid("ec0bceda-87c1-424a-bfb9-136d428f5eb9"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDateTime",
                table: "SetBoxDeviceLogAccesses",
                nullable: false,
                defaultValue: new DateTime(2019, 9, 16, 12, 8, 38, 527, DateTimeKind.Local).AddTicks(442),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 9, 16, 10, 47, 38, 72, DateTimeKind.Local).AddTicks(9246));

            migrationBuilder.AlterColumn<Guid>(
                name: "DeviceLogAccessesId",
                table: "SetBoxDeviceLogAccesses",
                nullable: false,
                defaultValue: new Guid("ba7b8c97-d75d-4216-b313-f92de47d315e"),
                oldClrType: typeof(Guid),
                oldDefaultValue: new Guid("75cfa058-a2d4-4d13-a3d8-9719cfefec42"));

            migrationBuilder.AlterColumn<Guid>(
                name: "ContactId",
                table: "SetBoxContact",
                nullable: false,
                defaultValue: new Guid("18b12cba-bf35-4efc-a7ee-b03a1a9b59f6"),
                oldClrType: typeof(Guid),
                oldDefaultValue: new Guid("3e2382c6-3851-496d-8e83-ffad9c3a9697"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDateTime",
                table: "SetBoxConfigs",
                nullable: false,
                defaultValue: new DateTime(2019, 9, 16, 12, 8, 38, 522, DateTimeKind.Local).AddTicks(4830),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 9, 16, 10, 47, 38, 70, DateTimeKind.Local).AddTicks(219));

            migrationBuilder.AlterColumn<Guid>(
                name: "ConfigId",
                table: "SetBoxConfigs",
                nullable: false,
                defaultValue: new Guid("611b1365-c408-4f53-a99e-9a404ceeb3dd"),
                oldClrType: typeof(Guid),
                oldDefaultValue: new Guid("9e48dadf-233c-4c05-b9d4-5affbb10ca9b"));

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                table: "SetBoxCompany",
                nullable: false,
                defaultValue: new Guid("1552a667-9fcb-41f3-87eb-84bbd4278354"),
                oldClrType: typeof(Guid),
                oldDefaultValue: new Guid("34cb1fcb-f57d-4414-895c-42ebe1a079f0"));

            migrationBuilder.AlterColumn<Guid>(
                name: "AddressId",
                table: "SetBoxAddress",
                nullable: false,
                defaultValue: new Guid("fdb66ff9-2d7d-4dd7-838e-40767364102f"),
                oldClrType: typeof(Guid),
                oldDefaultValue: new Guid("04455014-506e-4719-ba51-5d2cb07d11b5"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationDateTime",
                table: "SetBoxSupport");

            migrationBuilder.AlterColumn<Guid>(
                name: "SupportId",
                table: "SetBoxSupport",
                nullable: false,
                defaultValue: new Guid("bed31a2b-fc91-4919-b5ff-566b826f3b97"),
                oldClrType: typeof(Guid),
                oldDefaultValue: new Guid("f46db707-3254-45ca-af6e-7d9623dc5f07"));

            migrationBuilder.AlterColumn<Guid>(
                name: "FileId",
                table: "SetBoxFileCheckSum",
                nullable: false,
                defaultValue: new Guid("fbaf1494-acd8-4682-adcf-5e7a306d0524"),
                oldClrType: typeof(Guid),
                oldDefaultValue: new Guid("61826a4a-d824-4ba8-8981-a6dc2c459fe4"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDateTime",
                table: "SetBoxDevices",
                nullable: false,
                defaultValue: new DateTime(2019, 9, 16, 10, 47, 38, 77, DateTimeKind.Local).AddTicks(620),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 9, 16, 12, 8, 38, 533, DateTimeKind.Local).AddTicks(6269));

            migrationBuilder.AlterColumn<Guid>(
                name: "DeviceId",
                table: "SetBoxDevices",
                nullable: false,
                defaultValue: new Guid("ec0bceda-87c1-424a-bfb9-136d428f5eb9"),
                oldClrType: typeof(Guid),
                oldDefaultValue: new Guid("ac24f993-12b1-4217-920c-2405690ce3ac"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDateTime",
                table: "SetBoxDeviceLogAccesses",
                nullable: false,
                defaultValue: new DateTime(2019, 9, 16, 10, 47, 38, 72, DateTimeKind.Local).AddTicks(9246),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 9, 16, 12, 8, 38, 527, DateTimeKind.Local).AddTicks(442));

            migrationBuilder.AlterColumn<Guid>(
                name: "DeviceLogAccessesId",
                table: "SetBoxDeviceLogAccesses",
                nullable: false,
                defaultValue: new Guid("75cfa058-a2d4-4d13-a3d8-9719cfefec42"),
                oldClrType: typeof(Guid),
                oldDefaultValue: new Guid("ba7b8c97-d75d-4216-b313-f92de47d315e"));

            migrationBuilder.AlterColumn<Guid>(
                name: "ContactId",
                table: "SetBoxContact",
                nullable: false,
                defaultValue: new Guid("3e2382c6-3851-496d-8e83-ffad9c3a9697"),
                oldClrType: typeof(Guid),
                oldDefaultValue: new Guid("18b12cba-bf35-4efc-a7ee-b03a1a9b59f6"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDateTime",
                table: "SetBoxConfigs",
                nullable: false,
                defaultValue: new DateTime(2019, 9, 16, 10, 47, 38, 70, DateTimeKind.Local).AddTicks(219),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 9, 16, 12, 8, 38, 522, DateTimeKind.Local).AddTicks(4830));

            migrationBuilder.AlterColumn<Guid>(
                name: "ConfigId",
                table: "SetBoxConfigs",
                nullable: false,
                defaultValue: new Guid("9e48dadf-233c-4c05-b9d4-5affbb10ca9b"),
                oldClrType: typeof(Guid),
                oldDefaultValue: new Guid("611b1365-c408-4f53-a99e-9a404ceeb3dd"));

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                table: "SetBoxCompany",
                nullable: false,
                defaultValue: new Guid("34cb1fcb-f57d-4414-895c-42ebe1a079f0"),
                oldClrType: typeof(Guid),
                oldDefaultValue: new Guid("1552a667-9fcb-41f3-87eb-84bbd4278354"));

            migrationBuilder.AlterColumn<Guid>(
                name: "AddressId",
                table: "SetBoxAddress",
                nullable: false,
                defaultValue: new Guid("04455014-506e-4719-ba51-5d2cb07d11b5"),
                oldClrType: typeof(Guid),
                oldDefaultValue: new Guid("fdb66ff9-2d7d-4dd7-838e-40767364102f"));
        }
    }
}
