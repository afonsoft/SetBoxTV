using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace Afonsoft.SetBox.EntityFrameworkCore
{
    public static class SetBoxDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<SetBoxDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<SetBoxDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}