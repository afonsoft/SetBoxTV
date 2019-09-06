using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Rollbar.NetCore.AspNet;
using Rollbar;
using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SetBoxWebUI.Repository;
using Microsoft.EntityFrameworkCore;
using Afonsoft.Logger;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SetBoxWebUI.Interfaces;

namespace SetBoxWebUI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.Configure<IISOptions>(o =>
            {
                o.ForwardClientCertificate = true;
            });

            ConfigureRollbarSingleton();
            services.AddAfonsoftLogging();
            services.AddRollbarMiddleware();
            services.AddRollbarLogger(loggerOptions =>
            {
                loggerOptions.Filter = (loggerName, loglevel) => loglevel >= LogLevel.Trace;
            });

            services.AddMemoryCache();
            services.AddDistributedMemoryCache();
            services.AddEntityFrameworkSqlServer();

            string connectionString = Configuration.GetConnectionString("Default"); 
           
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
            services.AddSingleton(typeof(IRepository<>), typeof(Repository<>));

            services.AddIdentity<ApplicationIdentityUser, ApplicationIdentityRole>()
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    // .AddDefaultUI(UIFramework.Bootstrap4)
                    .AddDefaultTokenProviders();
          

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(20);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                options.LoginPath = new PathString("/Auth/Login");
                options.AccessDeniedPath = new PathString("/Auth/Denied");
                options.SlidingExpiration = true;
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
                    options =>
                    {
                        options.LoginPath = new PathString("/Auth/Login");
                        options.AccessDeniedPath = new PathString("/Auth/Denied");
                        options.SlidingExpiration = true;
                    })
                 .AddJwtBearer(options => {
                     options.Audience = "https://setbox.afonsoft.com.br/";
                     options.Authority = "https://setbox.afonsoft.com.br/";
                     options.RequireHttpsMetadata = false;
                     options.SaveToken = true;
                     options.TokenValidationParameters = new TokenValidationParameters
                     {
                         ValidateIssuerSigningKey = true,
                         IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["JwtBearer:IssuerSigningKey"])),
                         ValidateIssuer = true,
                         ValidateAudience = true
                     };
                 });


            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+#";
                options.User.RequireUniqueEmail = false;
            });


            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(
                options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                });

            // Configurando o serviço de documentação do Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new Info
                    {
                        Title = "SetBox API",
                        Version = "v1",
                        Description = "API for SetBox Update",
                        Contact = new Contact
                        {
                            Name = "Afonsoft",
                            Email = "afonsoft@gmail.com",
                            Url = "https://github.com/Afonsoft"
                        }
                    });
                c.EnableAnnotations();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseDatabaseErrorPage();
            app.UseHsts();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseSession();
            app.UseAuthentication();
            //app.UseHttpContextItemsMiddleware();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

            });

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "SetBox API");
            });
        }

        private void ConfigureRollbarSingleton()
        {
            string rollbarAccessToken = Configuration["Rollbar:AccessToken"];
            string rollbarEnvironment = Configuration["Rollbar:Environment"];
            RollbarLocator.RollbarInstance.Configure(new RollbarConfig(rollbarAccessToken)
            {
                Environment = rollbarEnvironment
            });
        }
    }
}
