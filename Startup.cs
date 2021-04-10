using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SandboxBackend.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json.Serialization;
using Microsoft.Net.Http.Headers;

namespace SandboxBackend
{
    public class Startup
    {
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        private IWebHostEnvironment env;
        public IConfiguration Configuration { get; }
        public Startup(IWebHostEnvironment env)
        {
            this.env = env;

            var builder = new ConfigurationBuilder()
            .SetBasePath(env.ContentRootPath)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
            .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  builder =>
                                  {
                                      // Allow multiple methods
                                      builder.WithMethods("GET", "POST", "PATCH", "DELETE", "OPTIONS")
                                        .WithHeaders(
                                          HeaderNames.Accept,
                                          HeaderNames.ContentType,
                                          HeaderNames.Authorization)
                                        .AllowCredentials()
                                        .SetIsOriginAllowed(origin =>
                                        {
                                            if (string.IsNullOrWhiteSpace(origin)) return false;
                                            // Only add this to allow testing with localhost, remove this line in production!
                                            if (origin.ToLower().StartsWith("http://localhost")) return true;
                                            // Insert your production domain here.
                                            if (origin.ToLower().StartsWith("https://10")) return true;
                                            return false;
                                        });
                                  });
            });

            services.AddDbContext<SandboxDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("SandboxHackathonDb")));

            services.AddIdentity<IdentityUser, IdentityRole>(config =>
            {
                config.Password.RequiredLength = 8;
                config.Password.RequireDigit = true;
                config.Password.RequireLowercase = true;
                config.Password.RequireUppercase = true;
            })
                .AddEntityFrameworkStores<SandboxDbContext>()
                .AddDefaultTokenProviders();



            services.ConfigureApplicationCookie(config =>
            {
                config.Cookie.Name = "Identity.Cookie";
                config.LoginPath = "/Home/NotAuthorized";
                config.Cookie.HttpOnly = true;
            });

            services.AddAuthorization(config =>
            {
                //Default policy for [Authorize] is they need to be authenticated.  Use [Authorize(Policy = "exampleRole")] to use other policies
                config.AddPolicy("recruiter", policyBuilder =>
                {
                    policyBuilder.RequireRole("recruiter"); 
                });

                config.AddPolicy("student", policyBuilder =>
                {
                    policyBuilder.RequireRole("student");
                });
            });
            services.AddControllersWithViews().AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, SandboxDbContext db)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            db.Database.Migrate();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseCors(MyAllowSpecificOrigins);

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
