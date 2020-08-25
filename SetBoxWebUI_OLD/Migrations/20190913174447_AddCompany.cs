using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SetBoxWebUI.Migrations
{
    public partial class AddCompany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "SetBoxCompany",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "CNPJ",
                table: "SetBoxCompany",
                newName: "Document");

            migrationBuilder.AlterColumn<Guid>(
                name: "FileId",
                table: "SetBoxFileCheckSum",
                nullable: false,
                defaultValue: new Guid("e2baa21f-eba8-4789-ae80-12e10f7c6833"),
                oldClrType: typeof(Guid),
                oldDefaultValue: new Guid("1a47514e-55fb-4033-9b95-0b1c38eb6a23"));

            migrationBuilder.AddColumn<string>(
                name: "Path",
                table: "SetBoxFileCheckSum",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDateTime",
                table: "SetBoxDevices",
                nullable: false,
                defaultValue: new DateTime(2019, 9, 13, 14, 44, 45, 397, DateTimeKind.Local).AddTicks(6715),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 9, 12, 15, 7, 2, 183, DateTimeKind.Local).AddTicks(333));

            migrationBuilder.AlterColumn<Guid>(
                name: "DeviceId",
                table: "SetBoxDevices",
                nullable: false,
                defaultValue: new Guid("d1787ecc-13a0-4cd3-876f-5eb708c720cd"),
                oldClrType: typeof(Guid),
                oldDefaultValue: new Guid("19986d8c-aba0-48b9-b7f9-30ed3646ff9c"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDateTime",
                table: "SetBoxDeviceLogAccesses",
                nullable: false,
                defaultValue: new DateTime(2019, 9, 13, 14, 44, 45, 393, DateTimeKind.Local).AddTicks(161),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 9, 12, 15, 7, 2, 178, DateTimeKind.Local).AddTicks(3569));

            migrationBuilder.AlterColumn<Guid>(
                name: "DeviceLogAccessesId",
                table: "SetBoxDeviceLogAccesses",
                nullable: false,
                defaultValue: new Guid("dcaaf105-fb1f-4635-8a50-bf7ff31b42f7"),
                oldClrType: typeof(Guid),
                oldDefaultValue: new Guid("de494b93-8c7f-472e-93fd-caeaee6f93e4"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDateTime",
                table: "SetBoxConfigs",
                nullable: false,
                defaultValue: new DateTime(2019, 9, 13, 14, 44, 45, 389, DateTimeKind.Local).AddTicks(6143),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 9, 12, 15, 7, 2, 174, DateTimeKind.Local).AddTicks(4892));

            migrationBuilder.AlterColumn<Guid>(
                name: "ConfigId",
                table: "SetBoxConfigs",
                nullable: false,
                defaultValue: new Guid("858d3f0f-1644-40a8-8e91-d3830daf290b"),
                oldClrType: typeof(Guid),
                oldDefaultValue: new Guid("e9c43e6c-abbb-4f5d-acbb-1bb0bd4702a2"));

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                table: "SetBoxCompany",
                nullable: false,
                defaultValue: new Guid("6e323eda-c059-40e8-86a5-513542e18872"),
                oldClrType: typeof(Guid),
                oldDefaultValue: new Guid("1cded759-c3ce-41de-a10b-eba9659bb93e"));

            migrationBuilder.AddColumn<string>(
                name: "Fatansy",
                table: "SetBoxCompany",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Responsible1",
                table: "SetBoxCompany",
                maxLength: 400,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Responsible2",
                table: "SetBoxCompany",
                maxLength: 400,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Telephone1",
                table: "SetBoxCompany",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Telephone2",
                table: "SetBoxCompany",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "AddressId",
                table: "SetBoxAddress",
                nullable: false,
                defaultValue: new Guid("349c4e7d-067e-46c3-b602-5f18ef02472e"),
                oldClrType: typeof(Guid),
                oldDefaultValue: new Guid("6f9c8c80-aa96-4447-ab24-182c34b2c370"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Path",
                table: "SetBoxFileCheckSum");

            migrationBuilder.DropColumn(
                name: "Fatansy",
                table: "SetBoxCompany");

            migrationBuilder.DropColumn(
                name: "Responsible1",
                table: "SetBoxCompany");

            migrationBuilder.DropColumn(
                name: "Responsible2",
                table: "SetBoxCompany");

            migrationBuilder.DropColumn(
                name: "Telephone1",
                table: "SetBoxCompany");

            migrationBuilder.DropColumn(
                name: "Telephone2",
                table: "SetBoxCompany");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "SetBoxCompany",
                newName: "FullName");

            migrationBuilder.RenameColumn(
                name: "Document",
                table: "SetBoxCompany",
                newName: "CNPJ");

            migrationBuilder.AlterColumn<Guid>(
                name: "FileId",
                table: "SetBoxFileCheckSum",
                nullable: false,
                defaultValue: new Guid("1a47514e-55fb-4033-9b95-0b1c38eb6a23"),
                oldClrType: typeof(Guid),
                oldDefaultValue: new Guid("e2baa21f-eba8-4789-ae80-12e10f7c6833"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDateTime",
                table: "SetBoxDevices",
                nullable: false,
                defaultValue: new DateTime(2019, 9, 12, 15, 7, 2, 183, DateTimeKind.Local).AddTicks(333),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 9, 13, 14, 44, 45, 397, DateTimeKind.Local).AddTicks(6715));

            migrationBuilder.AlterColumn<Guid>(
                name: "DeviceId",
                table: "SetBoxDevices",
                nullable: false,
                defaultValue: new Guid("19986d8c-aba0-48b9-b7f9-30ed3646ff9c"),
                oldClrType: typeof(Guid),
                oldDefaultValue: new Guid("d1787ecc-13a0-4cd3-876f-5eb708c720cd"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDateTime",
                table: "SetBoxDeviceLogAccesses",
                nullable: false,
                defaultValue: new DateTime(2019, 9, 12, 15, 7, 2, 178, DateTimeKind.Local).AddTicks(3569),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 9, 13, 14, 44, 45, 393, DateTimeKind.Local).AddTicks(161));

            migrationBuilder.AlterColumn<Guid>(
                name: "DeviceLogAccessesId",
                table: "SetBoxDeviceLogAccesses",
                nullable: false,
                defaultValue: new Guid("de494b93-8c7f-472e-93fd-caeaee6f93e4"),
                oldClrType: typeof(Guid),
                oldDefaultValue: new Guid("dcaaf105-fb1f-4635-8a50-bf7ff31b42f7"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDateTime",
                table: "SetBoxConfigs",
                nullable: false,
                defaultValue: new DateTime(2019, 9, 12, 15, 7, 2, 174, DateTimeKind.Local).AddTicks(4892),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 9, 13, 14, 44, 45, 389, DateTimeKind.Local).AddTicks(6143));

            migrationBuilder.AlterColumn<Guid>(
                name: "ConfigId",
                table: "SetBoxConfigs",
                nullable: false,
                defaultValue: new Guid("e9c43e6c-abbb-4f5d-acbb-1bb0bd4702a2"),
                oldClrType: typeof(Guid),
                oldDefaultValue: new Guid("858d3f0f-1644-40a8-8e91-d3830daf290b"));

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                table: "SetBoxCompany",
                nullable: false,
                defaultValue: new Guid("1cded759-c3ce-41de-a10b-eba9659bb93e"),
                oldClrType: typeof(Guid),
                oldDefaultValue: new Guid("6e323eda-c059-40e8-86a5-513542e18872"));

            migrationBuilder.AlterColumn<Guid>(
                name: "AddressId",
                table: "SetBoxAddress",
                nullable: false,
                defaultValue: new Guid("6f9c8c80-aa96-4447-ab24-182c34b2c370"),
                oldClrType: typeof(Guid),
                oldDefaultValue: new Guid("349c4e7d-067e-46c3-b602-5f18ef02472e"));
        }
    }
}
