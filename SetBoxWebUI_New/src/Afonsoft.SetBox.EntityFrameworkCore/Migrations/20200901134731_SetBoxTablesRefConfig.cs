using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Afonsoft.SetBox.Migrations
{
    public partial class SetBoxTablesRefConfig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppSetBoxConfig_AppSetBoxDevice_DeviceId",
                table: "AppSetBoxConfig");

            migrationBuilder.DropIndex(
                name: "IX_AppSetBoxConfig_DeviceId",
                table: "AppSetBoxConfig");

            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "AppSetBoxLogError");

            migrationBuilder.DropColumn(
                name: "CreatorUserId",
                table: "AppSetBoxLogError");

            migrationBuilder.DropColumn(
                name: "DeleterUserId",
                table: "AppSetBoxLogError");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "AppSetBoxLogError");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AppSetBoxLogError");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "AppSetBoxLogError");

            migrationBuilder.DropColumn(
                name: "LastModifierUserId",
                table: "AppSetBoxLogError");

            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "AppSetBoxLogAccesses");

            migrationBuilder.DropColumn(
                name: "CreatorUserId",
                table: "AppSetBoxLogAccesses");

            migrationBuilder.DropColumn(
                name: "DeleterUserId",
                table: "AppSetBoxLogAccesses");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "AppSetBoxLogAccesses");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AppSetBoxLogAccesses");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "AppSetBoxLogAccesses");

            migrationBuilder.DropColumn(
                name: "LastModifierUserId",
                table: "AppSetBoxLogAccesses");

            migrationBuilder.DropColumn(
                name: "DeviceId",
                table: "AppSetBoxConfig");

            migrationBuilder.AddColumn<long>(
                name: "ConfigurationId",
                table: "AppSetBoxDevice",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppSetBoxDevice_ConfigurationId",
                table: "AppSetBoxDevice",
                column: "ConfigurationId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppSetBoxDevice_AppSetBoxConfig_ConfigurationId",
                table: "AppSetBoxDevice",
                column: "ConfigurationId",
                principalTable: "AppSetBoxConfig",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppSetBoxDevice_AppSetBoxConfig_ConfigurationId",
                table: "AppSetBoxDevice");

            migrationBuilder.DropIndex(
                name: "IX_AppSetBoxDevice_ConfigurationId",
                table: "AppSetBoxDevice");

            migrationBuilder.DropColumn(
                name: "ConfigurationId",
                table: "AppSetBoxDevice");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "AppSetBoxLogError",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "CreatorUserId",
                table: "AppSetBoxLogError",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeleterUserId",
                table: "AppSetBoxLogError",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "AppSetBoxLogError",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AppSetBoxLogError",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "AppSetBoxLogError",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LastModifierUserId",
                table: "AppSetBoxLogError",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "AppSetBoxLogAccesses",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "CreatorUserId",
                table: "AppSetBoxLogAccesses",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeleterUserId",
                table: "AppSetBoxLogAccesses",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "AppSetBoxLogAccesses",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AppSetBoxLogAccesses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "AppSetBoxLogAccesses",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LastModifierUserId",
                table: "AppSetBoxLogAccesses",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeviceId",
                table: "AppSetBoxConfig",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_AppSetBoxConfig_DeviceId",
                table: "AppSetBoxConfig",
                column: "DeviceId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppSetBoxConfig_AppSetBoxDevice_DeviceId",
                table: "AppSetBoxConfig",
                column: "DeviceId",
                principalTable: "AppSetBoxDevice",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
