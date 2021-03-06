﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SetBoxWebUI.Models;
using SetBoxWebUI.Repository.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SetBoxWebUI.Repository
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationIdentityUser, ApplicationIdentityRole, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            try
            {
                Database.Migrate();
            }
            catch
            {
                //Ignore
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new ConfigMap());
            builder.ApplyConfiguration(new DeviceLogAccessesMap());
            builder.ApplyConfiguration(new DeviceLogErrorMap());
            builder.ApplyConfiguration(new DeviceMap());
            builder.ApplyConfiguration(new AddressMap());
            builder.ApplyConfiguration(new ContactMap());
            builder.ApplyConfiguration(new CompanyMap());
            builder.ApplyConfiguration(new SupportMap());

            builder.ApplyConfiguration(new FileCheckSumMap());
            builder.ApplyConfiguration(new FilesDevicesMap());
            
            base.OnModelCreating(builder);
        }

        public DbSet<Device> Devices { get; set; }
        public DbSet<Config> Configs { get; set; }
        public DbSet<DeviceLogAccesses> DeviceLogAccesses { get; set; }
        public DbSet<DeviceLogError> DeviceLogs { get; set; }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Address> Address { get; set; }

        public DbSet<FilesDevices> FilesDevices { get; set; }
        public DbSet<FileCheckSum> Files { get; set; }

        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Support> Supports { get; set; }
    }
    public class ApplicationIdentityUser : IdentityUser<Guid>
    {

    }

    public class ApplicationIdentityRole : IdentityRole<Guid>
    {

    }
}
