using Abp.IdentityServer4;
using Abp.Zero.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Afonsoft.SetBox.Authorization.Roles;
using Afonsoft.SetBox.Authorization.Users;
using Afonsoft.SetBox.Chat;
using Afonsoft.SetBox.Editions;
using Afonsoft.SetBox.Friendships;
using Afonsoft.SetBox.MultiTenancy;
using Afonsoft.SetBox.MultiTenancy.Accounting;
using Afonsoft.SetBox.MultiTenancy.Payments;
using Afonsoft.SetBox.Storage;
using Afonsoft.SetBox.SetBox.Model;
using Afonsoft.SetBox.SetBox.Model.Files;
using Afonsoft.SetBox.SetBox.Model.Companies;

namespace Afonsoft.SetBox.EntityFrameworkCore
{
    public class SetBoxDbContext : AbpZeroDbContext<Tenant, Role, User, SetBoxDbContext>, IAbpPersistedGrantDbContext
    {
        /* Define an IDbSet for each entity of the application */

        public virtual DbSet<BinaryObject> BinaryObjects { get; set; }

        public virtual DbSet<Friendship> Friendships { get; set; }

        public virtual DbSet<ChatMessage> ChatMessages { get; set; }

        public virtual DbSet<SubscribableEdition> SubscribableEditions { get; set; }

        public virtual DbSet<SubscriptionPayment> SubscriptionPayments { get; set; }

        public virtual DbSet<Invoice> Invoices { get; set; }

        public virtual DbSet<PersistedGrantEntity> PersistedGrants { get; set; }

        public virtual DbSet<SubscriptionPaymentExtensionData> SubscriptionPaymentExtensionDatas { get; set; }

        //SetBox
        public virtual DbSet<Support> Supports { get; set; }
        public virtual DbSet<DeviceLogError> DeviceLogErrors { get; set; }
        public virtual DbSet<DeviceLogAccesses> DeviceLogAccesses { get; set; }
        public virtual DbSet<Device> Devices { get; set; }
        public virtual DbSet<Config> Configs { get; set; }
        public virtual DbSet<File> Files { get; set; }
        public virtual DbSet<DeviceFile> DeviceFiles { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<Address> Address { get; set; }

        public SetBoxDbContext(DbContextOptions<SetBoxDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BinaryObject>(b =>
            {
                b.HasIndex(e => new { e.TenantId });
            });

            modelBuilder.Entity<ChatMessage>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.UserId, e.ReadState });
                b.HasIndex(e => new { e.TenantId, e.TargetUserId, e.ReadState });
                b.HasIndex(e => new { e.TargetTenantId, e.TargetUserId, e.ReadState });
                b.HasIndex(e => new { e.TargetTenantId, e.UserId, e.ReadState });
            });

            modelBuilder.Entity<Friendship>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.UserId });
                b.HasIndex(e => new { e.TenantId, e.FriendUserId });
                b.HasIndex(e => new { e.FriendTenantId, e.UserId });
                b.HasIndex(e => new { e.FriendTenantId, e.FriendUserId });
            });

            modelBuilder.Entity<Tenant>(b =>
            {
                b.HasIndex(e => new { e.SubscriptionEndDateUtc });
                b.HasIndex(e => new { e.CreationTime });
            });

            modelBuilder.Entity<SubscriptionPayment>(b =>
            {
                b.HasIndex(e => new { e.Status, e.CreationTime });
                b.HasIndex(e => new { PaymentId = e.ExternalPaymentId, e.Gateway });
            });

            modelBuilder.Entity<SubscriptionPaymentExtensionData>(b =>
            {
                b.HasQueryFilter(m => !m.IsDeleted)
                    .HasIndex(e => new { e.SubscriptionPaymentId, e.Key, e.IsDeleted })
                    .IsUnique();
            });

            modelBuilder.ConfigurePersistedGrantEntity();
        }
    }
}
