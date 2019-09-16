using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SetBoxWebUI.Migrations
{
    public partial class AddSupport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contact_SetBoxCompany_CompanyId",
                table: "Contact");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Contact",
                table: "Contact");

            migrationBuilder.RenameTable(
                name: "Contact",
                newName: "SetBoxContact");

            migrationBuilder.RenameIndex(
                name: "IX_Contact_CompanyId",
                table: "SetBoxContact",
                newName: "IX_SetBoxContact_CompanyId");

            migrationBuilder.AlterColumn<Guid>(
                name: "FileId",
                table: "SetBoxFileCheckSum",
                nullable: false,
                defaultValue: new Guid("fbaf1494-acd8-4682-adcf-5e7a306d0524"),
                oldClrType: typeof(Guid),
                oldDefaultValue: new Guid("ee599237-6c25-479e-b22e-0d31bd86b975"));

            migrationBuilder.AlterColumn<string>(
                name: "Platform",
                table: "SetBoxDevices",
                maxLength: 255,
                nullable: true,
                defaultValue: "Android",
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDateTime",
                table: "SetBoxDevices",
                nullable: false,
                defaultValue: new DateTime(2019, 9, 16, 10, 47, 38, 77, DateTimeKind.Local).AddTicks(620),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 9, 16, 9, 23, 24, 206, DateTimeKind.Local).AddTicks(7384));

            migrationBuilder.AlterColumn<Guid>(
                name: "DeviceId",
                table: "SetBoxDevices",
                nullable: false,
                defaultValue: new Guid("ec0bceda-87c1-424a-bfb9-136d428f5eb9"),
                oldClrType: typeof(Guid),
                oldDefaultValue: new Guid("4cf4285d-f566-4a29-8e6f-e6872c0e829b"));

            migrationBuilder.AddColumn<Guid>(
                name: "SupportId",
                table: "SetBoxDevices",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDateTime",
                table: "SetBoxDeviceLogAccesses",
                nullable: false,
                defaultValue: new DateTime(2019, 9, 16, 10, 47, 38, 72, DateTimeKind.Local).AddTicks(9246),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 9, 16, 9, 23, 24, 202, DateTimeKind.Local).AddTicks(8947));

            migrationBuilder.AlterColumn<Guid>(
                name: "DeviceLogAccessesId",
                table: "SetBoxDeviceLogAccesses",
                nullable: false,
                defaultValue: new Guid("75cfa058-a2d4-4d13-a3d8-9719cfefec42"),
                oldClrType: typeof(Guid),
                oldDefaultValue: new Guid("0c784995-6fb8-4662-ae55-b0af52203582"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDateTime",
                table: "SetBoxConfigs",
                nullable: false,
                defaultValue: new DateTime(2019, 9, 16, 10, 47, 38, 70, DateTimeKind.Local).AddTicks(219),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 9, 16, 9, 23, 24, 199, DateTimeKind.Local).AddTicks(9040));

            migrationBuilder.AlterColumn<Guid>(
                name: "ConfigId",
                table: "SetBoxConfigs",
                nullable: false,
                defaultValue: new Guid("9e48dadf-233c-4c05-b9d4-5affbb10ca9b"),
                oldClrType: typeof(Guid),
                oldDefaultValue: new Guid("6d237ec9-753e-4cd7-8f33-198586937ee8"));

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                table: "SetBoxCompany",
                nullable: false,
                defaultValue: new Guid("34cb1fcb-f57d-4414-895c-42ebe1a079f0"),
                oldClrType: typeof(Guid),
                oldDefaultValue: new Guid("c2643177-b523-48ad-b614-9122ef7c0adc"));

            migrationBuilder.AlterColumn<Guid>(
                name: "AddressId",
                table: "SetBoxAddress",
                nullable: false,
                defaultValue: new Guid("04455014-506e-4719-ba51-5d2cb07d11b5"),
                oldClrType: typeof(Guid),
                oldDefaultValue: new Guid("ffa6b833-8cec-4234-9bb3-0e5c4bb85639"));

            migrationBuilder.AlterColumn<string>(
                name: "Telephone2",
                table: "SetBoxContact",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Telephone1",
                table: "SetBoxContact",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "SetBoxContact",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email2",
                table: "SetBoxContact",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email1",
                table: "SetBoxContact",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Document",
                table: "SetBoxContact",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ContactId",
                table: "SetBoxContact",
                nullable: false,
                defaultValue: new Guid("3e2382c6-3851-496d-8e83-ffad9c3a9697"),
                oldClrType: typeof(Guid));

            migrationBuilder.AddPrimaryKey(
                name: "PK_SetBoxContact",
                table: "SetBoxContact",
                column: "ContactId");

            migrationBuilder.CreateTable(
                name: "SetBoxSupport",
                columns: table => new
                {
                    SupportId = table.Column<Guid>(nullable: false, defaultValue: new Guid("bed31a2b-fc91-4919-b5ff-566b826f3b97")),
                    Company = table.Column<string>(maxLength: 200, nullable: true),
                    Telephone = table.Column<string>(maxLength: 50, nullable: true),
                    Email = table.Column<string>(maxLength: 255, nullable: true),
                    Name = table.Column<string>(maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SetBoxSupport", x => x.SupportId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SetBoxDevices_SupportId",
                table: "SetBoxDevices",
                column: "SupportId");

            migrationBuilder.AddForeignKey(
                name: "FK_SetBoxContact_SetBoxCompany_CompanyId",
                table: "SetBoxContact",
                column: "CompanyId",
                principalTable: "SetBoxCompany",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SetBoxDevices_SetBoxSupport_SupportId",
                table: "SetBoxDevices",
                column: "SupportId",
                principalTable: "SetBoxSupport",
                principalColumn: "SupportId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SetBoxContact_SetBoxCompany_CompanyId",
                table: "SetBoxContact");

            migrationBuilder.DropForeignKey(
                name: "FK_SetBoxDevices_SetBoxSupport_SupportId",
                table: "SetBoxDevices");

            migrationBuilder.DropTable(
                name: "SetBoxSupport");

            migrationBuilder.DropIndex(
                name: "IX_SetBoxDevices_SupportId",
                table: "SetBoxDevices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SetBoxContact",
                table: "SetBoxContact");

            migrationBuilder.DropColumn(
                name: "SupportId",
                table: "SetBoxDevices");

            migrationBuilder.RenameTable(
                name: "SetBoxContact",
                newName: "Contact");

            migrationBuilder.RenameIndex(
                name: "IX_SetBoxContact_CompanyId",
                table: "Contact",
                newName: "IX_Contact_CompanyId");

            migrationBuilder.AlterColumn<Guid>(
                name: "FileId",
                table: "SetBoxFileCheckSum",
                nullable: false,
                defaultValue: new Guid("ee599237-6c25-479e-b22e-0d31bd86b975"),
                oldClrType: typeof(Guid),
                oldDefaultValue: new Guid("fbaf1494-acd8-4682-adcf-5e7a306d0524"));

            migrationBuilder.AlterColumn<string>(
                name: "Platform",
                table: "SetBoxDevices",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true,
                oldDefaultValue: "Android");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDateTime",
                table: "SetBoxDevices",
                nullable: false,
                defaultValue: new DateTime(2019, 9, 16, 9, 23, 24, 206, DateTimeKind.Local).AddTicks(7384),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 9, 16, 10, 47, 38, 77, DateTimeKind.Local).AddTicks(620));

            migrationBuilder.AlterColumn<Guid>(
                name: "DeviceId",
                table: "SetBoxDevices",
                nullable: false,
                defaultValue: new Guid("4cf4285d-f566-4a29-8e6f-e6872c0e829b"),
                oldClrType: typeof(Guid),
                oldDefaultValue: new Guid("ec0bceda-87c1-424a-bfb9-136d428f5eb9"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDateTime",
                table: "SetBoxDeviceLogAccesses",
                nullable: false,
                defaultValue: new DateTime(2019, 9, 16, 9, 23, 24, 202, DateTimeKind.Local).AddTicks(8947),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 9, 16, 10, 47, 38, 72, DateTimeKind.Local).AddTicks(9246));

            migrationBuilder.AlterColumn<Guid>(
                name: "DeviceLogAccessesId",
                table: "SetBoxDeviceLogAccesses",
                nullable: false,
                defaultValue: new Guid("0c784995-6fb8-4662-ae55-b0af52203582"),
                oldClrType: typeof(Guid),
                oldDefaultValue: new Guid("75cfa058-a2d4-4d13-a3d8-9719cfefec42"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDateTime",
                table: "SetBoxConfigs",
                nullable: false,
                defaultValue: new DateTime(2019, 9, 16, 9, 23, 24, 199, DateTimeKind.Local).AddTicks(9040),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 9, 16, 10, 47, 38, 70, DateTimeKind.Local).AddTicks(219));

            migrationBuilder.AlterColumn<Guid>(
                name: "ConfigId",
                table: "SetBoxConfigs",
                nullable: false,
                defaultValue: new Guid("6d237ec9-753e-4cd7-8f33-198586937ee8"),
                oldClrType: typeof(Guid),
                oldDefaultValue: new Guid("9e48dadf-233c-4c05-b9d4-5affbb10ca9b"));

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                table: "SetBoxCompany",
                nullable: false,
                defaultValue: new Guid("c2643177-b523-48ad-b614-9122ef7c0adc"),
                oldClrType: typeof(Guid),
                oldDefaultValue: new Guid("34cb1fcb-f57d-4414-895c-42ebe1a079f0"));

            migrationBuilder.AlterColumn<Guid>(
                name: "AddressId",
                table: "SetBoxAddress",
                nullable: false,
                defaultValue: new Guid("ffa6b833-8cec-4234-9bb3-0e5c4bb85639"),
                oldClrType: typeof(Guid),
                oldDefaultValue: new Guid("04455014-506e-4719-ba51-5d2cb07d11b5"));

            migrationBuilder.AlterColumn<string>(
                name: "Telephone2",
                table: "Contact",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Telephone1",
                table: "Contact",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Contact",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email2",
                table: "Contact",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email1",
                table: "Contact",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Document",
                table: "Contact",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ContactId",
                table: "Contact",
                nullable: false,
                oldClrType: typeof(Guid),
                oldDefaultValue: new Guid("3e2382c6-3851-496d-8e83-ffad9c3a9697"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Contact",
                table: "Contact",
                column: "ContactId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contact_SetBoxCompany_CompanyId",
                table: "Contact",
                column: "CompanyId",
                principalTable: "SetBoxCompany",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
