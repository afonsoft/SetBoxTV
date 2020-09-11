using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Afonsoft.SetBox.Migrations
{
    public partial class SetBoxTablesRef : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppSetBoxCompany",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Fatansy = table.Column<string>(nullable: true),
                    Document = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppSetBoxCompany", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppSetBoxFile",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    Extension = table.Column<string>(nullable: false),
                    Size = table.Column<long>(nullable: false),
                    Url = table.Column<string>(nullable: true),
                    Path = table.Column<string>(nullable: true),
                    CheckSum = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppSetBoxFile", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppSetBoxSupport",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    Company = table.Column<string>(nullable: false),
                    Telephone = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    UrlLogo = table.Column<string>(nullable: true),
                    UrlApk = table.Column<string>(nullable: true),
                    VersionApk = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppSetBoxSupport", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppSetBoxAddress",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    State = table.Column<string>(nullable: true),
                    Street = table.Column<string>(nullable: true),
                    CompanyId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppSetBoxAddress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppSetBoxAddress_AppSetBoxCompany_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "AppSetBoxCompany",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AppSetBoxContact",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Telephone1 = table.Column<string>(nullable: true),
                    Telephone2 = table.Column<string>(nullable: true),
                    Email1 = table.Column<string>(nullable: true),
                    Email2 = table.Column<string>(nullable: true),
                    Document = table.Column<string>(nullable: true),
                    CompanyId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppSetBoxContact", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppSetBoxContact_AppSetBoxCompany_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "AppSetBoxCompany",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AppSetBoxDevice",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    DeviceIdentifier = table.Column<string>(nullable: false),
                    Platform = table.Column<string>(nullable: false),
                    Version = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    LastAccessDateTime = table.Column<DateTime>(nullable: true),
                    ApkVersion = table.Column<string>(nullable: true),
                    Model = table.Column<string>(nullable: true),
                    Manufacturer = table.Column<string>(nullable: true),
                    DeviceName = table.Column<string>(nullable: true),
                    CompanyId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppSetBoxDevice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppSetBoxDevice_AppSetBoxCompany_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "AppSetBoxCompany",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AppSetBoxConfig",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    DeviceId = table.Column<long>(nullable: false),
                    EnableVideo = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppSetBoxConfig", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppSetBoxConfig_AppSetBoxDevice_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "AppSetBoxDevice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppSetBoxDeviceFile",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    FileId = table.Column<long>(nullable: false),
                    Order = table.Column<int>(nullable: true),
                    DeviceId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppSetBoxDeviceFile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppSetBoxDeviceFile_AppSetBoxDevice_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "AppSetBoxDevice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppSetBoxDeviceFile_AppSetBoxFile_FileId",
                        column: x => x.FileId,
                        principalTable: "AppSetBoxFile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppSetBoxLogAccesses",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    DeviceId = table.Column<long>(nullable: true),
                    IpAcessed = table.Column<string>(nullable: true),
                    Message = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppSetBoxLogAccesses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppSetBoxLogAccesses_AppSetBoxDevice_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "AppSetBoxDevice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AppSetBoxLogError",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    DeviceId = table.Column<long>(nullable: true),
                    IpAcessed = table.Column<string>(nullable: true),
                    Message = table.Column<string>(nullable: true),
                    Exception = table.Column<string>(nullable: true),
                    Level = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppSetBoxLogError", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppSetBoxLogError_AppSetBoxDevice_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "AppSetBoxDevice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppSetBoxAddress_CompanyId",
                table: "AppSetBoxAddress",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_AppSetBoxConfig_DeviceId",
                table: "AppSetBoxConfig",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_AppSetBoxContact_CompanyId",
                table: "AppSetBoxContact",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_AppSetBoxDevice_CompanyId",
                table: "AppSetBoxDevice",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_AppSetBoxDeviceFile_DeviceId",
                table: "AppSetBoxDeviceFile",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_AppSetBoxDeviceFile_FileId",
                table: "AppSetBoxDeviceFile",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_AppSetBoxLogAccesses_DeviceId",
                table: "AppSetBoxLogAccesses",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_AppSetBoxLogError_DeviceId",
                table: "AppSetBoxLogError",
                column: "DeviceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppSetBoxAddress");

            migrationBuilder.DropTable(
                name: "AppSetBoxConfig");

            migrationBuilder.DropTable(
                name: "AppSetBoxContact");

            migrationBuilder.DropTable(
                name: "AppSetBoxDeviceFile");

            migrationBuilder.DropTable(
                name: "AppSetBoxLogAccesses");

            migrationBuilder.DropTable(
                name: "AppSetBoxLogError");

            migrationBuilder.DropTable(
                name: "AppSetBoxSupport");

            migrationBuilder.DropTable(
                name: "AppSetBoxFile");

            migrationBuilder.DropTable(
                name: "AppSetBoxDevice");

            migrationBuilder.DropTable(
                name: "AppSetBoxCompany");
        }
    }
}
