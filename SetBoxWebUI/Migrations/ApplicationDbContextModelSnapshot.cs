﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SetBoxWebUI.Repository;

namespace SetBoxWebUI.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<Guid>("RoleId");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<Guid>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<Guid>("UserId");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId");

                    b.Property<Guid>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("SetBoxWebUI.Models.Address", b =>
                {
                    b.Property<Guid>("AddressId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("AddressId")
                        .HasDefaultValue(new Guid("04455014-506e-4719-ba51-5d2cb07d11b5"));

                    b.Property<string>("City")
                        .HasColumnName("City")
                        .HasMaxLength(255);

                    b.Property<Guid?>("CompanyId");

                    b.Property<string>("State")
                        .HasColumnName("State")
                        .HasMaxLength(2);

                    b.Property<string>("Street")
                        .HasColumnName("Street")
                        .HasMaxLength(1000);

                    b.HasKey("AddressId");

                    b.HasIndex("CompanyId");

                    b.ToTable("SetBoxAddress");
                });

            modelBuilder.Entity("SetBoxWebUI.Models.Company", b =>
                {
                    b.Property<Guid>("CompanyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("CompanyId")
                        .HasDefaultValue(new Guid("34cb1fcb-f57d-4414-895c-42ebe1a079f0"));

                    b.Property<string>("Document")
                        .HasColumnName("Document")
                        .HasMaxLength(255);

                    b.Property<string>("Fatansy")
                        .HasColumnName("Fatansy")
                        .HasMaxLength(500);

                    b.Property<string>("Name")
                        .HasColumnName("Name")
                        .HasMaxLength(500);

                    b.HasKey("CompanyId");

                    b.ToTable("SetBoxCompany");
                });

            modelBuilder.Entity("SetBoxWebUI.Models.Config", b =>
                {
                    b.Property<Guid>("ConfigId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ConfigId")
                        .HasDefaultValue(new Guid("9e48dadf-233c-4c05-b9d4-5affbb10ca9b"));

                    b.Property<DateTime>("CreationDateTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("CreationDateTime")
                        .HasDefaultValue(new DateTime(2019, 9, 16, 10, 47, 38, 70, DateTimeKind.Local).AddTicks(219));

                    b.Property<Guid>("DeviceId");

                    b.Property<bool>("EnablePhoto")
                        .HasColumnName("EnablePhoto");

                    b.Property<bool>("EnableTransaction")
                        .HasColumnName("EnableTransaction");

                    b.Property<bool>("EnableVideo")
                        .HasColumnName("EnableVideo");

                    b.Property<bool>("EnableWebImage")
                        .HasColumnName("EnableWebImage");

                    b.Property<bool>("EnableWebVideo")
                        .HasColumnName("EnableWebVideo");

                    b.Property<int>("TransactionTime");

                    b.HasKey("ConfigId");

                    b.HasIndex("DeviceId")
                        .IsUnique();

                    b.ToTable("SetBoxConfigs");
                });

            modelBuilder.Entity("SetBoxWebUI.Models.Contact", b =>
                {
                    b.Property<Guid>("ContactId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ContactId")
                        .HasDefaultValue(new Guid("3e2382c6-3851-496d-8e83-ffad9c3a9697"));

                    b.Property<Guid?>("CompanyId");

                    b.Property<string>("Document")
                        .HasColumnName("Document")
                        .HasMaxLength(100);

                    b.Property<string>("Email1")
                        .HasColumnName("Email1")
                        .HasMaxLength(500);

                    b.Property<string>("Email2")
                        .HasColumnName("Email2")
                        .HasMaxLength(500);

                    b.Property<string>("Name")
                        .HasColumnName("Name")
                        .HasMaxLength(255);

                    b.Property<string>("Telephone1")
                        .HasColumnName("Telephone1")
                        .HasMaxLength(50);

                    b.Property<string>("Telephone2")
                        .HasColumnName("Telephone2")
                        .HasMaxLength(50);

                    b.HasKey("ContactId");

                    b.HasIndex("CompanyId");

                    b.ToTable("SetBoxContact");
                });

            modelBuilder.Entity("SetBoxWebUI.Models.Device", b =>
                {
                    b.Property<Guid>("DeviceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("DeviceId")
                        .HasDefaultValue(new Guid("ec0bceda-87c1-424a-bfb9-136d428f5eb9"));

                    b.Property<Guid?>("CompanyId");

                    b.Property<DateTime>("CreationDateTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("CreationDateTime")
                        .HasDefaultValue(new DateTime(2019, 9, 16, 10, 47, 38, 77, DateTimeKind.Local).AddTicks(620));

                    b.Property<string>("DeviceIdentifier")
                        .IsRequired()
                        .HasColumnName("DeviceIdentifier")
                        .HasMaxLength(255);

                    b.Property<string>("License")
                        .HasColumnName("License")
                        .HasMaxLength(255);

                    b.Property<string>("Platform")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Platform")
                        .HasMaxLength(255)
                        .HasDefaultValue("Android");

                    b.Property<Guid?>("SupportId");

                    b.Property<string>("Version")
                        .HasColumnName("Version")
                        .HasMaxLength(255);

                    b.HasKey("DeviceId");

                    b.HasIndex("CompanyId");

                    b.HasIndex("SupportId");

                    b.ToTable("SetBoxDevices");
                });

            modelBuilder.Entity("SetBoxWebUI.Models.DeviceLogAccesses", b =>
                {
                    b.Property<Guid>("DeviceLogAccessesId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("DeviceLogAccessesId")
                        .HasDefaultValue(new Guid("75cfa058-a2d4-4d13-a3d8-9719cfefec42"));

                    b.Property<DateTime>("CreationDateTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("CreationDateTime")
                        .HasDefaultValue(new DateTime(2019, 9, 16, 10, 47, 38, 72, DateTimeKind.Local).AddTicks(9246));

                    b.Property<Guid?>("DeviceId");

                    b.Property<string>("IpAcessed")
                        .HasColumnName("IpAcessed");

                    b.Property<string>("Message")
                        .HasColumnName("Message")
                        .HasMaxLength(2000);

                    b.HasKey("DeviceLogAccessesId");

                    b.HasIndex("DeviceId");

                    b.ToTable("SetBoxDeviceLogAccesses");
                });

            modelBuilder.Entity("SetBoxWebUI.Models.FileCheckSum", b =>
                {
                    b.Property<Guid>("FileId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("FileId")
                        .HasDefaultValue(new Guid("fbaf1494-acd8-4682-adcf-5e7a306d0524"));

                    b.Property<string>("CheckSum");

                    b.Property<string>("Extension")
                        .HasColumnName("Extension")
                        .HasMaxLength(10);

                    b.Property<string>("Name")
                        .HasColumnName("Name")
                        .HasMaxLength(255);

                    b.Property<string>("Path")
                        .HasColumnName("Path")
                        .HasMaxLength(500);

                    b.Property<long>("Size")
                        .HasColumnName("Size");

                    b.Property<string>("Url")
                        .HasColumnName("Url")
                        .HasMaxLength(4000);

                    b.HasKey("FileId");

                    b.ToTable("SetBoxFileCheckSum");
                });

            modelBuilder.Entity("SetBoxWebUI.Models.FilesDevices", b =>
                {
                    b.Property<Guid>("DeviceId");

                    b.Property<Guid>("FileId");

                    b.HasKey("DeviceId", "FileId");

                    b.HasIndex("FileId");

                    b.ToTable("SetBoxFilesDevices");
                });

            modelBuilder.Entity("SetBoxWebUI.Models.Support", b =>
                {
                    b.Property<Guid>("SupportId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("SupportId")
                        .HasDefaultValue(new Guid("bed31a2b-fc91-4919-b5ff-566b826f3b97"));

                    b.Property<string>("Company")
                        .HasColumnName("Company")
                        .HasMaxLength(200);

                    b.Property<string>("Email")
                        .HasColumnName("Email")
                        .HasMaxLength(255);

                    b.Property<string>("Name")
                        .HasColumnName("Name")
                        .HasMaxLength(200);

                    b.Property<string>("Telephone")
                        .HasColumnName("Telephone")
                        .HasMaxLength(50);

                    b.HasKey("SupportId");

                    b.ToTable("SetBoxSupport");
                });

            modelBuilder.Entity("SetBoxWebUI.Repository.ApplicationIdentityRole", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("SetBoxWebUI.Repository.ApplicationIdentityUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.HasOne("SetBoxWebUI.Repository.ApplicationIdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.HasOne("SetBoxWebUI.Repository.ApplicationIdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.HasOne("SetBoxWebUI.Repository.ApplicationIdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.HasOne("SetBoxWebUI.Repository.ApplicationIdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SetBoxWebUI.Repository.ApplicationIdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.HasOne("SetBoxWebUI.Repository.ApplicationIdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SetBoxWebUI.Models.Address", b =>
                {
                    b.HasOne("SetBoxWebUI.Models.Company", "Company")
                        .WithMany("Address")
                        .HasForeignKey("CompanyId");
                });

            modelBuilder.Entity("SetBoxWebUI.Models.Config", b =>
                {
                    b.HasOne("SetBoxWebUI.Models.Device", "Device")
                        .WithOne("Configuration")
                        .HasForeignKey("SetBoxWebUI.Models.Config", "DeviceId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SetBoxWebUI.Models.Contact", b =>
                {
                    b.HasOne("SetBoxWebUI.Models.Company", "Company")
                        .WithMany("Contacts")
                        .HasForeignKey("CompanyId");
                });

            modelBuilder.Entity("SetBoxWebUI.Models.Device", b =>
                {
                    b.HasOne("SetBoxWebUI.Models.Company", "Company")
                        .WithMany("Devices")
                        .HasForeignKey("CompanyId");

                    b.HasOne("SetBoxWebUI.Models.Support", "Support")
                        .WithMany("Devices")
                        .HasForeignKey("SupportId");
                });

            modelBuilder.Entity("SetBoxWebUI.Models.DeviceLogAccesses", b =>
                {
                    b.HasOne("SetBoxWebUI.Models.Device", "Device")
                        .WithMany("LogAccesses")
                        .HasForeignKey("DeviceId");
                });

            modelBuilder.Entity("SetBoxWebUI.Models.FilesDevices", b =>
                {
                    b.HasOne("SetBoxWebUI.Models.Device", "Device")
                        .WithMany("Files")
                        .HasForeignKey("DeviceId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SetBoxWebUI.Models.FileCheckSum", "File")
                        .WithMany("Devices")
                        .HasForeignKey("FileId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
