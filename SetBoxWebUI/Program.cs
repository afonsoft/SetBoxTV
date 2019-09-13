using System;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SetBoxWebUI.Repository;

namespace SetBoxWebUI
{
    /// <summary>
    /// Program
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static async System.Threading.Tasks.Task Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetService<UserManager<ApplicationIdentityUser>>();
                var roleManager = scope.ServiceProvider.GetService<RoleManager<ApplicationIdentityRole>>();


                // Create an identity role object out of the enum value
                var identityRole = new ApplicationIdentityRole
                {
                    Id = Guid.NewGuid(),
                    Name = "Admin"
                };

                // Create the role if it doesn't already exist
                if (!await roleManager.RoleExistsAsync(identityRole.Name))
                {
                    await roleManager.CreateAsync(identityRole);
                }

                // Our default user
                var user = new ApplicationIdentityUser
                {
                    Email = "afonsoft@afonsoft.com.br",
                    UserName = "afonsoft@afonsoft.com.br",
                    LockoutEnabled = false
                };

                // Add the user to the database if it doesn't already exist
                if (await userManager.FindByEmailAsync(user.Email) == null)
                {
                    // WARNING: Do NOT check in credentials of any kind into source control
                    await userManager.CreateAsync(user, "Senha#2019");
                    // Assign all roles to the default user
                    await userManager.AddToRolesAsync(user, new string[] { "Admin" });
                    
                }
            }

            host.Run();
        }

        /// <summary>
        /// CreateWebHostBuilder
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .CaptureStartupErrors(true)
            .UseKestrel()
            .UseContentRoot(Directory.GetCurrentDirectory())
            .UseIISIntegration()
            .UseStartup<Startup>();
    }
}
