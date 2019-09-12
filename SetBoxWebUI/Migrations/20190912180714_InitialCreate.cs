using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SetBoxWebUI.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SetBoxCompany",
                columns: table => new
                {
                    CompanyId = table.Column<Guid>(nullable: false, defaultValue: new Guid("1cded759-c3ce-41de-a10b-eba9659bb93e")),
                    FullName = table.Column<string>(maxLength: 500, nullable: true),
                    CNPJ = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SetBoxCompany", x => x.CompanyId);
                });

            migrationBuilder.CreateTable(
                name: "SetBoxFileCheckSum",
                columns: table => new
                {
                    FileId = table.Column<Guid>(nullable: false, defaultValue: new Guid("1a47514e-55fb-4033-9b95-0b1c38eb6a23")),
                    Name = table.Column<string>(maxLength: 255, nullable: true),
                    Extension = table.Column<string>(maxLength: 10, nullable: true),
                    Size = table.Column<long>(nullable: false),
                    Url = table.Column<string>(maxLength: 4000, nullable: true),
                    CheckSum = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SetBoxFileCheckSum", x => x.FileId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<Guid>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<Guid>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    RoleId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SetBoxAddress",
                columns: table => new
                {
                    AddressId = table.Column<Guid>(nullable: false, defaultValue: new Guid("6f9c8c80-aa96-4447-ab24-182c34b2c370")),
                    CompanyId = table.Column<Guid>(nullable: true),
                    City = table.Column<string>(maxLength: 255, nullable: true),
                    State = table.Column<string>(maxLength: 2, nullable: true),
                    Street = table.Column<string>(maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SetBoxAddress", x => x.AddressId);
                    table.ForeignKey(
                        name: "FK_SetBoxAddress_SetBoxCompany_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "SetBoxCompany",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SetBoxDevices",
                columns: table => new
                {
                    DeviceId = table.Column<Guid>(nullable: false, defaultValue: new Guid("19986d8c-aba0-48b9-b7f9-30ed3646ff9c")),
                    DeviceIdentifier = table.Column<string>(maxLength: 255, nullable: false),
                    Platform = table.Column<string>(maxLength: 255, nullable: true),
                    Version = table.Column<string>(maxLength: 255, nullable: true),
                    License = table.Column<string>(maxLength: 255, nullable: true),
                    CreationDateTime = table.Column<DateTime>(nullable: false, defaultValue: new DateTime(2019, 9, 12, 15, 7, 2, 183, DateTimeKind.Local).AddTicks(333)),
                    CompanyId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SetBoxDevices", x => x.DeviceId);
                    table.ForeignKey(
                        name: "FK_SetBoxDevices_SetBoxCompany_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "SetBoxCompany",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FilesDevices",
                columns: table => new
                {
                    FileId = table.Column<Guid>(nullable: false),
                    DeviceId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilesDevices", x => new { x.DeviceId, x.FileId });
                    table.ForeignKey(
                        name: "FK_FilesDevices_SetBoxDevices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "SetBoxDevices",
                        principalColumn: "DeviceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FilesDevices_SetBoxFileCheckSum_FileId",
                        column: x => x.FileId,
                        principalTable: "SetBoxFileCheckSum",
                        principalColumn: "FileId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SetBoxConfigs",
                columns: table => new
                {
                    ConfigId = table.Column<Guid>(nullable: false, defaultValue: new Guid("e9c43e6c-abbb-4f5d-acbb-1bb0bd4702a2")),
                    DeviceId = table.Column<Guid>(nullable: false),
                    EnableVideo = table.Column<bool>(nullable: false),
                    EnablePhoto = table.Column<bool>(nullable: false),
                    EnableWebVideo = table.Column<bool>(nullable: false),
                    EnableWebImage = table.Column<bool>(nullable: false),
                    EnableTransaction = table.Column<bool>(nullable: false),
                    TransactionTime = table.Column<int>(nullable: false),
                    CreationDateTime = table.Column<DateTime>(nullable: false, defaultValue: new DateTime(2019, 9, 12, 15, 7, 2, 174, DateTimeKind.Local).AddTicks(4892))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SetBoxConfigs", x => x.ConfigId);
                    table.ForeignKey(
                        name: "FK_SetBoxConfigs_SetBoxDevices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "SetBoxDevices",
                        principalColumn: "DeviceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SetBoxDeviceLogAccesses",
                columns: table => new
                {
                    DeviceLogAccessesId = table.Column<Guid>(nullable: false, defaultValue: new Guid("de494b93-8c7f-472e-93fd-caeaee6f93e4")),
                    DeviceId = table.Column<Guid>(nullable: true),
                    CreationDateTime = table.Column<DateTime>(nullable: false, defaultValue: new DateTime(2019, 9, 12, 15, 7, 2, 178, DateTimeKind.Local).AddTicks(3569)),
                    IpAcessed = table.Column<string>(nullable: true),
                    Message = table.Column<string>(maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SetBoxDeviceLogAccesses", x => x.DeviceLogAccessesId);
                    table.ForeignKey(
                        name: "FK_SetBoxDeviceLogAccesses_SetBoxDevices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "SetBoxDevices",
                        principalColumn: "DeviceId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_FilesDevices_FileId",
                table: "FilesDevices",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_SetBoxAddress_CompanyId",
                table: "SetBoxAddress",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_SetBoxConfigs_DeviceId",
                table: "SetBoxConfigs",
                column: "DeviceId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SetBoxDeviceLogAccesses_DeviceId",
                table: "SetBoxDeviceLogAccesses",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_SetBoxDevices_CompanyId",
                table: "SetBoxDevices",
                column: "CompanyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "FilesDevices");

            migrationBuilder.DropTable(
                name: "SetBoxAddress");

            migrationBuilder.DropTable(
                name: "SetBoxConfigs");

            migrationBuilder.DropTable(
                name: "SetBoxDeviceLogAccesses");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "SetBoxFileCheckSum");

            migrationBuilder.DropTable(
                name: "SetBoxDevices");

            migrationBuilder.DropTable(
                name: "SetBoxCompany");
        }
    }
}
