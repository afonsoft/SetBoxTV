using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
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
using Microsoft.AspNetCore.Http.Features;
using Hangfire;
using Hangfire.Console;
using SetBoxWebUI.Jobs;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;
using Hangfire.Dashboard;
using SetBoxWebUI.Helpers;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.DataProtection;

namespace SetBoxWebUI
{
    /// <summary>
    /// Startup
    /// </summary>
    public class Startup
    {

        /// <summary>
        /// Startup
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Configuration
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.Configure<IISOptions>(o =>
            {
                o.ForwardClientCertificate = true;
            });

            services.AddAfonsoftLogging();
            services.AddMemoryCache();
            services.AddDistributedMemoryCache();
            services.AddEntityFrameworkSqlServer();

            services.AddHttpContextAccessor();
            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();

            services.AddDataProtection()
            .SetDefaultKeyLifetime(TimeSpan.FromDays(365));

            string connectionString = Configuration.GetConnectionString("Default");

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseLazyLoadingProxies(true);
                options.UseSqlServer(connectionString, options =>
                {
                    options.CommandTimeout(300);
                    options.EnableRetryOnFailure(10, TimeSpan.FromSeconds(15), null);
                });
            }, ServiceLifetime.Transient);

            services.AddIdentity<ApplicationIdentityUser, ApplicationIdentityRole>()
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddDefaultTokenProviders();

            services.AddSingleton(typeof(IRepository<,>), typeof(Repository<,>));
            services.AddSingleton<IHangfireJob, HangfireJob>();

            services.AddHangfire(configuration => configuration
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseConsole()
            .UseSqlServerStorage(connectionString, new SqlServerStorageOptions
            {
                CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                QueuePollInterval = TimeSpan.Zero,
                UseRecommendedIsolationLevel = true,
                DisableGlobalLocks = true
            }));

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
                options.IdleTimeout = TimeSpan.FromHours(2);
                options.Cookie.IsEssential = true;
            });

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.IsEssential = true;
                options.ExpireTimeSpan = TimeSpan.FromHours(24);
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
                        options.Cookie.IsEssential = true;
                        options.ExpireTimeSpan = TimeSpan.FromHours(24);
                    })
                 .AddJwtBearer(options =>
                 {
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
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+#!$";
                options.User.RequireUniqueEmail = true;
            });

            services.Configure<FormOptions>(options =>
            {
                options.ValueLengthLimit = int.MaxValue;
                options.MultipartBodyLengthLimit = int.MaxValue;
                options.MultipartBoundaryLengthLimit = 512;
                options.MultipartHeadersLengthLimit = int.MaxValue;
            });

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddControllersAsServices()
                .AddJsonOptions(
                options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                });

            services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
                options.Providers.Add<BrotliCompressionProvider>();
                options.EnableForHttps = true;
            });

            services.Configure<BrotliCompressionProviderOptions>(options => { options.Level = CompressionLevel.Fastest; });
            services.Configure<GzipCompressionProviderOptions>(options => { options.Level = CompressionLevel.Fastest; });

            services.AddHangfireServer(options => {
                options.WorkerCount = 1;
                options.SchedulePollingInterval = TimeSpan.FromSeconds(30);
                options.ServerCheckInterval = TimeSpan.FromSeconds(30);
                options.HeartbeatInterval = TimeSpan.FromSeconds(30);
            });

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
                c.DocInclusionPredicate((docName, description) => true);
                c.AddSecurityDefinition("Bearer", new BasicAuthScheme());
            });

           
        }
        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        public void Configure(IApplicationBuilder app, IServiceProvider serviceProvider)
        {
            app.UseDeveloperExceptionPage();
            app.UseDatabaseErrorPage();
            app.UseHsts();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseSession();
            app.UseAuthentication();
            app.UseCors("CorsPolicy");

            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                DisplayStorageConnectionString = false,
                Authorization = new[] { new AllowAllDashboardAuthorizationFilter() }
            });

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "SetBox API");
            });

            InitializeHangfire(serviceProvider.GetService<IHangfireJob>());
        }

        private void InitializeHangfire(IHangfireJob job)
        {
            job.Initialize();
        }
    }
}
