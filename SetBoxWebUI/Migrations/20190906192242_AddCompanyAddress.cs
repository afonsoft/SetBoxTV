using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SetBoxWebUI.Migrations
{
    public partial class AddCompanyAddress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeviceLogAccesses_SetBoxDevices_DeviceId",
                table: "DeviceLogAccesses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DeviceLogAccesses",
                table: "DeviceLogAccesses");

            migrationBuilder.RenameTable(
                name: "DeviceLogAccesses",
                newName: "SetBoxDeviceLogAccesses");

            migrationBuilder.RenameIndex(
                name: "IX_DeviceLogAccesses_DeviceId",
                table: "SetBoxDeviceLogAccesses",
                newName: "IX_SetBoxDeviceLogAccesses_DeviceId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDateTime",
                table: "SetBoxDevices",
                nullable: false,
                defaultValue: new DateTime(2019, 9, 6, 16, 22, 41, 441, DateTimeKind.Local).AddTicks(6871),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 9, 6, 15, 11, 34, 400, DateTimeKind.Local).AddTicks(7380));

            migrationBuilder.AlterColumn<Guid>(
                name: "DeviceId",
                table: "SetBoxDevices",
                nullable: false,
                defaultValue: new Guid("bb4b8126-ccfd-44f4-8d8c-23bfca2f364a"),
                oldClrType: typeof(Guid),
                oldDefaultValue: new Guid("6afb1cdc-2092-4217-892c-213d218c2c75"));

            migrationBuilder.AddColumn<Guid>(
                name: "CompanyId",
                table: "SetBoxDevices",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "License",
                table: "SetBoxDevices",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDateTime",
                table: "SetBoxConfigs",
                nullable: false,
                defaultValue: new DateTime(2019, 9, 6, 16, 22, 41, 435, DateTimeKind.Local).AddTicks(6898),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 9, 6, 15, 11, 34, 403, DateTimeKind.Local).AddTicks(7604));

            migrationBuilder.AlterColumn<Guid>(
                name: "ConfigId",
                table: "SetBoxConfigs",
                nullable: false,
                defaultValue: new Guid("f8dc38bd-d86e-4878-aadb-b9d855687423"),
                oldClrType: typeof(Guid),
                oldDefaultValue: new Guid("59609e07-3808-4674-901a-9614d82ca577"));

            migrationBuilder.AlterColumn<string>(
                name: "Message",
                table: "SetBoxDeviceLogAccesses",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDateTime",
                table: "SetBoxDeviceLogAccesses",
                nullable: true,
                defaultValue: new DateTime(2019, 9, 6, 16, 22, 41, 438, DateTimeKind.Local).AddTicks(6869),
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "DeviceLogAccessesId",
                table: "SetBoxDeviceLogAccesses",
                nullable: false,
                defaultValue: new Guid("8eba78b8-b4b8-4110-8af0-51220b196598"),
                oldClrType: typeof(Guid));

            migrationBuilder.AddPrimaryKey(
                name: "PK_SetBoxDeviceLogAccesses",
                table: "SetBoxDeviceLogAccesses",
                column: "DeviceLogAccessesId");

            migrationBuilder.CreateTable(
                name: "SetBoxCompany",
                columns: table => new
                {
                    AddressId = table.Column<Guid>(nullable: false, defaultValue: new Guid("02ff6e2b-80fb-4f8f-8aa7-fbcc2aa27dc4")),
                    FullName = table.Column<string>(maxLength: 500, nullable: true),
                    CNPJ = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SetBoxCompany", x => x.AddressId);
                });

            migrationBuilder.CreateTable(
                name: "SetBoxAddress",
                columns: table => new
                {
                    AddressId = table.Column<Guid>(nullable: false, defaultValue: new Guid("54bb1266-f917-4cf9-badf-ae5d3eff3a6e")),
                    City = table.Column<string>(maxLength: 255, nullable: true),
                    State = table.Column<string>(maxLength: 2, nullable: true),
                    Street = table.Column<string>(maxLength: 1000, nullable: true),
                    CompanyId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SetBoxAddress", x => x.AddressId);
                    table.ForeignKey(
                        name: "FK_SetBoxAddress_SetBoxCompany_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "SetBoxCompany",
                        principalColumn: "AddressId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SetBoxDevices_CompanyId",
                table: "SetBoxDevices",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_SetBoxAddress_CompanyId",
                table: "SetBoxAddress",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_SetBoxDeviceLogAccesses_SetBoxDevices_DeviceId",
                table: "SetBoxDeviceLogAccesses",
                column: "DeviceId",
                principalTable: "SetBoxDevices",
                principalColumn: "DeviceId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SetBoxDevices_SetBoxCompany_CompanyId",
                table: "SetBoxDevices",
                column: "CompanyId",
                principalTable: "SetBoxCompany",
                principalColumn: "AddressId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SetBoxDeviceLogAccesses_SetBoxDevices_DeviceId",
                table: "SetBoxDeviceLogAccesses");

            migrationBuilder.DropForeignKey(
                name: "FK_SetBoxDevices_SetBoxCompany_CompanyId",
                table: "SetBoxDevices");

            migrationBuilder.DropTable(
                name: "SetBoxAddress");

            migrationBuilder.DropTable(
                name: "SetBoxCompany");

            migrationBuilder.DropIndex(
                name: "IX_SetBoxDevices_CompanyId",
                table: "SetBoxDevices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SetBoxDeviceLogAccesses",
                table: "SetBoxDeviceLogAccesses");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "SetBoxDevices");

            migrationBuilder.DropColumn(
                name: "License",
                table: "SetBoxDevices");

            migrationBuilder.RenameTable(
                name: "SetBoxDeviceLogAccesses",
                newName: "DeviceLogAccesses");

            migrationBuilder.RenameIndex(
                name: "IX_SetBoxDeviceLogAccesses_DeviceId",
                table: "DeviceLogAccesses",
                newName: "IX_DeviceLogAccesses_DeviceId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDateTime",
                table: "SetBoxDevices",
                nullable: false,
                defaultValue: new DateTime(2019, 9, 6, 15, 11, 34, 400, DateTimeKind.Local).AddTicks(7380),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 9, 6, 16, 22, 41, 441, DateTimeKind.Local).AddTicks(6871));

            migrationBuilder.AlterColumn<Guid>(
                name: "DeviceId",
                table: "SetBoxDevices",
                nullable: false,
                defaultValue: new Guid("6afb1cdc-2092-4217-892c-213d218c2c75"),
                oldClrType: typeof(Guid),
                oldDefaultValue: new Guid("bb4b8126-ccfd-44f4-8d8c-23bfca2f364a"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDateTime",
                table: "SetBoxConfigs",
                nullable: false,
                defaultValue: new DateTime(2019, 9, 6, 15, 11, 34, 403, DateTimeKind.Local).AddTicks(7604),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 9, 6, 16, 22, 41, 435, DateTimeKind.Local).AddTicks(6898));

            migrationBuilder.AlterColumn<Guid>(
                name: "ConfigId",
                table: "SetBoxConfigs",
                nullable: false,
                defaultValue: new Guid("59609e07-3808-4674-901a-9614d82ca577"),
                oldClrType: typeof(Guid),
                oldDefaultValue: new Guid("f8dc38bd-d86e-4878-aadb-b9d855687423"));

            migrationBuilder.AlterColumn<string>(
                name: "Message",
                table: "DeviceLogAccesses",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 2000,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDateTime",
                table: "DeviceLogAccesses",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldNullable: true,
                oldDefaultValue: new DateTime(2019, 9, 6, 16, 22, 41, 438, DateTimeKind.Local).AddTicks(6869));

            migrationBuilder.AlterColumn<Guid>(
                name: "DeviceLogAccessesId",
                table: "DeviceLogAccesses",
                nullable: false,
                oldClrType: typeof(Guid),
                oldDefaultValue: new Guid("8eba78b8-b4b8-4110-8af0-51220b196598"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_DeviceLogAccesses",
                table: "DeviceLogAccesses",
                column: "DeviceLogAccessesId");

            migrationBuilder.AddForeignKey(
                name: "FK_DeviceLogAccesses_SetBoxDevices_DeviceId",
                table: "DeviceLogAccesses",
                column: "DeviceId",
                principalTable: "SetBoxDevices",
                principalColumn: "DeviceId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
