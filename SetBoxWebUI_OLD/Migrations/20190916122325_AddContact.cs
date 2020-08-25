using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SetBoxWebUI.Migrations
{
    public partial class AddContact : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FilesDevices_SetBoxDevices_DeviceId",
                table: "FilesDevices");

            migrationBuilder.DropForeignKey(
                name: "FK_FilesDevices_SetBoxFileCheckSum_FileId",
                table: "FilesDevices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FilesDevices",
                table: "FilesDevices");

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

            migrationBuilder.RenameTable(
                name: "FilesDevices",
                newName: "SetBoxFilesDevices");

            migrationBuilder.RenameIndex(
                name: "IX_FilesDevices_FileId",
                table: "SetBoxFilesDevices",
                newName: "IX_SetBoxFilesDevices_FileId");

            migrationBuilder.AlterColumn<Guid>(
                name: "FileId",
                table: "SetBoxFileCheckSum",
                nullable: false,
                defaultValue: new Guid("ee599237-6c25-479e-b22e-0d31bd86b975"),
                oldClrType: typeof(Guid),
                oldDefaultValue: new Guid("e2baa21f-eba8-4789-ae80-12e10f7c6833"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDateTime",
                table: "SetBoxDevices",
                nullable: false,
                defaultValue: new DateTime(2019, 9, 16, 9, 23, 24, 206, DateTimeKind.Local).AddTicks(7384),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 9, 13, 14, 44, 45, 397, DateTimeKind.Local).AddTicks(6715));

            migrationBuilder.AlterColumn<Guid>(
                name: "DeviceId",
                table: "SetBoxDevices",
                nullable: false,
                defaultValue: new Guid("4cf4285d-f566-4a29-8e6f-e6872c0e829b"),
                oldClrType: typeof(Guid),
                oldDefaultValue: new Guid("d1787ecc-13a0-4cd3-876f-5eb708c720cd"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDateTime",
                table: "SetBoxDeviceLogAccesses",
                nullable: false,
                defaultValue: new DateTime(2019, 9, 16, 9, 23, 24, 202, DateTimeKind.Local).AddTicks(8947),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 9, 13, 14, 44, 45, 393, DateTimeKind.Local).AddTicks(161));

            migrationBuilder.AlterColumn<Guid>(
                name: "DeviceLogAccessesId",
                table: "SetBoxDeviceLogAccesses",
                nullable: false,
                defaultValue: new Guid("0c784995-6fb8-4662-ae55-b0af52203582"),
                oldClrType: typeof(Guid),
                oldDefaultValue: new Guid("dcaaf105-fb1f-4635-8a50-bf7ff31b42f7"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDateTime",
                table: "SetBoxConfigs",
                nullable: false,
                defaultValue: new DateTime(2019, 9, 16, 9, 23, 24, 199, DateTimeKind.Local).AddTicks(9040),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 9, 13, 14, 44, 45, 389, DateTimeKind.Local).AddTicks(6143));

            migrationBuilder.AlterColumn<Guid>(
                name: "ConfigId",
                table: "SetBoxConfigs",
                nullable: false,
                defaultValue: new Guid("6d237ec9-753e-4cd7-8f33-198586937ee8"),
                oldClrType: typeof(Guid),
                oldDefaultValue: new Guid("858d3f0f-1644-40a8-8e91-d3830daf290b"));

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                table: "SetBoxCompany",
                nullable: false,
                defaultValue: new Guid("c2643177-b523-48ad-b614-9122ef7c0adc"),
                oldClrType: typeof(Guid),
                oldDefaultValue: new Guid("6e323eda-c059-40e8-86a5-513542e18872"));

            migrationBuilder.AlterColumn<Guid>(
                name: "AddressId",
                table: "SetBoxAddress",
                nullable: false,
                defaultValue: new Guid("ffa6b833-8cec-4234-9bb3-0e5c4bb85639"),
                oldClrType: typeof(Guid),
                oldDefaultValue: new Guid("349c4e7d-067e-46c3-b602-5f18ef02472e"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_SetBoxFilesDevices",
                table: "SetBoxFilesDevices",
                columns: new[] { "DeviceId", "FileId" });

            migrationBuilder.CreateTable(
                name: "Contact",
                columns: table => new
                {
                    ContactId = table.Column<Guid>(nullable: false),
                    CompanyId = table.Column<Guid>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Telephone1 = table.Column<string>(nullable: true),
                    Telephone2 = table.Column<string>(nullable: true),
                    Email1 = table.Column<string>(nullable: true),
                    Email2 = table.Column<string>(nullable: true),
                    Document = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contact", x => x.ContactId);
                    table.ForeignKey(
                        name: "FK_Contact_SetBoxCompany_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "SetBoxCompany",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contact_CompanyId",
                table: "Contact",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_SetBoxFilesDevices_SetBoxDevices_DeviceId",
                table: "SetBoxFilesDevices",
                column: "DeviceId",
                principalTable: "SetBoxDevices",
                principalColumn: "DeviceId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SetBoxFilesDevices_SetBoxFileCheckSum_FileId",
                table: "SetBoxFilesDevices",
                column: "FileId",
                principalTable: "SetBoxFileCheckSum",
                principalColumn: "FileId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SetBoxFilesDevices_SetBoxDevices_DeviceId",
                table: "SetBoxFilesDevices");

            migrationBuilder.DropForeignKey(
                name: "FK_SetBoxFilesDevices_SetBoxFileCheckSum_FileId",
                table: "SetBoxFilesDevices");

            migrationBuilder.DropTable(
                name: "Contact");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SetBoxFilesDevices",
                table: "SetBoxFilesDevices");

            migrationBuilder.RenameTable(
                name: "SetBoxFilesDevices",
                newName: "FilesDevices");

            migrationBuilder.RenameIndex(
                name: "IX_SetBoxFilesDevices_FileId",
                table: "FilesDevices",
                newName: "IX_FilesDevices_FileId");

            migrationBuilder.AlterColumn<Guid>(
                name: "FileId",
                table: "SetBoxFileCheckSum",
                nullable: false,
                defaultValue: new Guid("e2baa21f-eba8-4789-ae80-12e10f7c6833"),
                oldClrType: typeof(Guid),
                oldDefaultValue: new Guid("ee599237-6c25-479e-b22e-0d31bd86b975"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDateTime",
                table: "SetBoxDevices",
                nullable: false,
                defaultValue: new DateTime(2019, 9, 13, 14, 44, 45, 397, DateTimeKind.Local).AddTicks(6715),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 9, 16, 9, 23, 24, 206, DateTimeKind.Local).AddTicks(7384));

            migrationBuilder.AlterColumn<Guid>(
                name: "DeviceId",
                table: "SetBoxDevices",
                nullable: false,
                defaultValue: new Guid("d1787ecc-13a0-4cd3-876f-5eb708c720cd"),
                oldClrType: typeof(Guid),
                oldDefaultValue: new Guid("4cf4285d-f566-4a29-8e6f-e6872c0e829b"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDateTime",
                table: "SetBoxDeviceLogAccesses",
                nullable: false,
                defaultValue: new DateTime(2019, 9, 13, 14, 44, 45, 393, DateTimeKind.Local).AddTicks(161),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 9, 16, 9, 23, 24, 202, DateTimeKind.Local).AddTicks(8947));

            migrationBuilder.AlterColumn<Guid>(
                name: "DeviceLogAccessesId",
                table: "SetBoxDeviceLogAccesses",
                nullable: false,
                defaultValue: new Guid("dcaaf105-fb1f-4635-8a50-bf7ff31b42f7"),
                oldClrType: typeof(Guid),
                oldDefaultValue: new Guid("0c784995-6fb8-4662-ae55-b0af52203582"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDateTime",
                table: "SetBoxConfigs",
                nullable: false,
                defaultValue: new DateTime(2019, 9, 13, 14, 44, 45, 389, DateTimeKind.Local).AddTicks(6143),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 9, 16, 9, 23, 24, 199, DateTimeKind.Local).AddTicks(9040));

            migrationBuilder.AlterColumn<Guid>(
                name: "ConfigId",
                table: "SetBoxConfigs",
                nullable: false,
                defaultValue: new Guid("858d3f0f-1644-40a8-8e91-d3830daf290b"),
                oldClrType: typeof(Guid),
                oldDefaultValue: new Guid("6d237ec9-753e-4cd7-8f33-198586937ee8"));

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                table: "SetBoxCompany",
                nullable: false,
                defaultValue: new Guid("6e323eda-c059-40e8-86a5-513542e18872"),
                oldClrType: typeof(Guid),
                oldDefaultValue: new Guid("c2643177-b523-48ad-b614-9122ef7c0adc"));

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
                oldDefaultValue: new Guid("ffa6b833-8cec-4234-9bb3-0e5c4bb85639"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_FilesDevices",
                table: "FilesDevices",
                columns: new[] { "DeviceId", "FileId" });

            migrationBuilder.AddForeignKey(
                name: "FK_FilesDevices_SetBoxDevices_DeviceId",
                table: "FilesDevices",
                column: "DeviceId",
                principalTable: "SetBoxDevices",
                principalColumn: "DeviceId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FilesDevices_SetBoxFileCheckSum_FileId",
                table: "FilesDevices",
                column: "FileId",
                principalTable: "SetBoxFileCheckSum",
                principalColumn: "FileId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
