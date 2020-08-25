using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Afonsoft.SetBox.Configuration;
using Afonsoft.SetBox.Web;

namespace Afonsoft.SetBox.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class SetBoxDbContextFactory : IDesignTimeDbContextFactory<SetBoxDbContext>
    {
        public SetBoxDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<SetBoxDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder(), addUserSecrets: true);

            SetBoxDbContextConfigurer.Configure(builder, configuration.GetConnectionString(SetBoxConsts.ConnectionStringName));

            return new SetBoxDbContext(builder.Options);
        }
    }
}