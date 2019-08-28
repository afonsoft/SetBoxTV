using Afonsoft.EFCore;
using Microsoft.EntityFrameworkCore;
using SetBoxWebUI.Models;
using SetBoxWebUI.Repository.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SetBoxWebUI.Repository
{
    public class SetBoxContext : AfonsoftDbContext
    {
        public SetBoxContext(DbContextOptions<DbContext> options) : base(options) { }
        public SetBoxContext(Action<AfonsoftEFOptions> configure) : base(configure) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new DeviceMap());
        }

        public DbSet<Device> Devices { get; set; }
    }
}
