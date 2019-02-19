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
using product_image_service.Data;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;

namespace product_image_service
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
                var storage_config = JsonConvert.DeserializeObject<dynamic>(Environment.GetEnvironmentVariable("VCAP_SERVICES"))["azure-storage"][0];
                Console.WriteLine($"Azure Storage Config: {storage_config["credentials"].ToString().Replace(System.Environment.NewLine, " ")}");
                Console.WriteLine($"Azure Storage Account Name: {storage_config["credentials"]["storage_account_name"]}, Key: {storage_config["credentials"]["primary_access_key"]}");
                var creds = new StorageCredentials((string)storage_config["credentials"]["storage_account_name"], (string)storage_config["credentials"]["primary_access_key"]);
                var storageAccount = new CloudStorageAccount(creds, true);
                var blobClient = storageAccount.CreateCloudBlobClient();
                var containerClient = blobClient.GetContainerReference("uploads");
                containerClient.CreateIfNotExistsAsync(BlobContainerPublicAccessType.Off, null, null).Wait();
                var storageClient = new AzureContainerStorageClient(containerClient);
                services.AddSingleton<IStorageClient>(storageClient);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Failed to create Azure Storage Blob client: {ex.ToString()}");
                throw;
            }
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
