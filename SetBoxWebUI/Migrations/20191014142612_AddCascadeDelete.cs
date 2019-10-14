using Microsoft.EntityFrameworkCore.Migrations;

namespace SetBoxWebUI.Migrations
{
    public partial class AddCascadeDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SetBoxAddress_SetBoxCompany_CompanyId",
                table: "SetBoxAddress");

            migrationBuilder.DropForeignKey(
                name: "FK_SetBoxContact_SetBoxCompany_CompanyId",
                table: "SetBoxContact");

            migrationBuilder.DropForeignKey(
                name: "FK_SetBoxDeviceLog_SetBoxDevices_DeviceId",
                table: "SetBoxDeviceLog");

            migrationBuilder.DropForeignKey(
                name: "FK_SetBoxDeviceLogAccesses_SetBoxDevices_DeviceId",
                table: "SetBoxDeviceLogAccesses");

            migrationBuilder.DropForeignKey(
                name: "FK_SetBoxFilesDevices_SetBoxFileCheckSum_FileId",
                table: "SetBoxFilesDevices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SetBoxFileCheckSum",
                table: "SetBoxFileCheckSum");

            migrationBuilder.RenameTable(
                name: "SetBoxFileCheckSum",
                newName: "SetBoxFiles");

            migrationBuilder.AddColumn<string>(
                name: "UrlApk",
                table: "SetBoxSupport",
                maxLength: 4000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VersionApk",
                table: "SetBoxSupport",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SetBoxFiles",
                table: "SetBoxFiles",
                column: "FileId");

            migrationBuilder.AddForeignKey(
                name: "FK_SetBoxAddress_SetBoxCompany_CompanyId",
                table: "SetBoxAddress",
                column: "CompanyId",
                principalTable: "SetBoxCompany",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SetBoxContact_SetBoxCompany_CompanyId",
                table: "SetBoxContact",
                column: "CompanyId",
                principalTable: "SetBoxCompany",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SetBoxDeviceLog_SetBoxDevices_DeviceId",
                table: "SetBoxDeviceLog",
                column: "DeviceId",
                principalTable: "SetBoxDevices",
                principalColumn: "DeviceId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SetBoxDeviceLogAccesses_SetBoxDevices_DeviceId",
                table: "SetBoxDeviceLogAccesses",
                column: "DeviceId",
                principalTable: "SetBoxDevices",
                principalColumn: "DeviceId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SetBoxFilesDevices_SetBoxFiles_FileId",
                table: "SetBoxFilesDevices",
                column: "FileId",
                principalTable: "SetBoxFiles",
                principalColumn: "FileId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SetBoxAddress_SetBoxCompany_CompanyId",
                table: "SetBoxAddress");

            migrationBuilder.DropForeignKey(
                name: "FK_SetBoxContact_SetBoxCompany_CompanyId",
                table: "SetBoxContact");

            migrationBuilder.DropForeignKey(
                name: "FK_SetBoxDeviceLog_SetBoxDevices_DeviceId",
                table: "SetBoxDeviceLog");

            migrationBuilder.DropForeignKey(
                name: "FK_SetBoxDeviceLogAccesses_SetBoxDevices_DeviceId",
                table: "SetBoxDeviceLogAccesses");

            migrationBuilder.DropForeignKey(
                name: "FK_SetBoxFilesDevices_SetBoxFiles_FileId",
                table: "SetBoxFilesDevices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SetBoxFiles",
                table: "SetBoxFiles");

            migrationBuilder.DropColumn(
                name: "UrlApk",
                table: "SetBoxSupport");

            migrationBuilder.DropColumn(
                name: "VersionApk",
                table: "SetBoxSupport");

            migrationBuilder.RenameTable(
                name: "SetBoxFiles",
                newName: "SetBoxFileCheckSum");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SetBoxFileCheckSum",
                table: "SetBoxFileCheckSum",
                column: "FileId");

            migrationBuilder.AddForeignKey(
                name: "FK_SetBoxAddress_SetBoxCompany_CompanyId",
                table: "SetBoxAddress",
                column: "CompanyId",
                principalTable: "SetBoxCompany",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SetBoxContact_SetBoxCompany_CompanyId",
                table: "SetBoxContact",
                column: "CompanyId",
                principalTable: "SetBoxCompany",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SetBoxDeviceLog_SetBoxDevices_DeviceId",
                table: "SetBoxDeviceLog",
                column: "DeviceId",
                principalTable: "SetBoxDevices",
                principalColumn: "DeviceId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SetBoxDeviceLogAccesses_SetBoxDevices_DeviceId",
                table: "SetBoxDeviceLogAccesses",
                column: "DeviceId",
                principalTable: "SetBoxDevices",
                principalColumn: "DeviceId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SetBoxFilesDevices_SetBoxFileCheckSum_FileId",
                table: "SetBoxFilesDevices",
                column: "FileId",
                principalTable: "SetBoxFileCheckSum",
                principalColumn: "FileId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
