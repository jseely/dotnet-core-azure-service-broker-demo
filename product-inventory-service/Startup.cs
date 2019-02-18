using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using product_inventory_service.Data;
using Microsoft.EntityFrameworkCore;

namespace product_inventory_service
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


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            try
            {
                var sqldb_config = JsonConvert.DeserializeObject<dynamic>(Environment.GetEnvironmentVariable("VCAP_SERVICES"))["azure-sqldb"][0];
                Console.WriteLine($"SQLDB Config: {sqldb_config["credentials"].ToString().Replace(System.Environment.NewLine, " ")}");
                services.AddDbContext<InventoryContext>(options => options.UseSqlServer((string)CredentialsToConnectionString(sqldb_config["credentials"])));
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Failed to create SQLDB context: {ex.ToString()}");
                throw;
            }
        }

        public static string CredentialsToConnectionString(dynamic credentials)
        {
            return $"Data Source={credentials["sqlServerFullyQualifiedDomainName"]},{credentials["port"]};Initial Catalog={credentials["sqldbName"]};User ID={credentials["username"]};Password={credentials["password"]}";
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            using (var context = new InventoryContext(app.ApplicationServices.GetRequiredService<DbContextOptions<InventoryContext>>()))
            {
                context.Database.EnsureCreated();
                context.Database.Migrate();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
