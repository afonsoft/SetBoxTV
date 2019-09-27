using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SetBoxWebUI.Migrations
{
    public partial class AddDeviceLogError : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SetBoxDeviceLog",
                columns: table => new
                {
                    DeviceLogId = table.Column<Guid>(nullable: false),
                    DeviceId = table.Column<Guid>(nullable: true),
                    CreationDateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    IpAcessed = table.Column<string>(nullable: true),
                    Message = table.Column<string>(type: "TEXT", nullable: true),
                    Level = table.Column<string>(maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SetBoxDeviceLog", x => x.DeviceLogId);
                    table.ForeignKey(
                        name: "FK_SetBoxDeviceLog_SetBoxDevices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "SetBoxDevices",
                        principalColumn: "DeviceId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SetBoxDeviceLog_DeviceId",
                table: "SetBoxDeviceLog",
                column: "DeviceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SetBoxDeviceLog");
        }
    }
}
