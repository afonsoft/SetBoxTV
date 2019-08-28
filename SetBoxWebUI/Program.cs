using System;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SetBoxWebUI.Repository;

namespace SetBoxWebUI
{
    public class Program
    {
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
                    var result = await roleManager.CreateAsync(identityRole);
                    if (!result.Succeeded)
                    {
                        // FIXME: Do not throw an Exception object
                        throw new Exception("Creating role failed");
                    }

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
                    var result = await userManager.CreateAsync(user, "Senha#2019");

                    if (!result.Succeeded)
                    {
                        // FIXME: Do not throw an Exception object
                        throw new Exception("Creating user failed");
                    }

                    // Assign all roles to the default user
                    result = await userManager.AddToRolesAsync(user, new string[] { "Admin" });
                    // If you add a role to the enumafter the user is created,
                    // the role will not be assigned to the user as of now

                    if (!result.Succeeded)
                    {
                        // FIXME: Do not throw an Exception object
                        throw new Exception("Adding user to role failed");
                    }
                }
            }

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .CaptureStartupErrors(true)
            .UseKestrel()
            //.UseIIS()
            .UseContentRoot(Directory.GetCurrentDirectory())
            .UseIISIntegration()
            .UseStartup<Startup>();
    }
}
