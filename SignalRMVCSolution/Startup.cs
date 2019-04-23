using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SignalRMVCSolution.Hubs;
using SignalRMVCSolution.Models;

namespace SignalRMVCSolution
{
    public class Startup
    {
        private readonly IConfigurationRoot configuration;

        public Startup(IHostingEnvironment env)//(IConfiguration configuration)
        {
            //Configuration = configuration;
            ///<summary>
            ///Defining configuration file and building setting from it that we can use later on
            ///</summary>
            configuration = new ConfigurationBuilder()
                                .AddEnvironmentVariables()
                                .AddJsonFile(env.ContentRootPath + "/config.json")
                                .AddJsonFile(env.ContentRootPath + "/config.development.json", true)
                                .Build();
        }

        //public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<InstrumentDataContext>(options =>
                {
                    var connectionString = configuration.GetConnectionString("InstrumentDataContext");
                    options.UseSqlServer(connectionString);
                });

            services.AddHttpClient<IEXService>();
            services.AddTransient<DummyDataCls>();
            services.AddTransient<EnvironmentConfig>(
                x=> new EnvironmentConfig
                {
                    EnableDeveloperException = configuration.GetValue<bool>("EnvironmentConfig:EnableDeveloperException")
                });
            

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, EnvironmentConfig environmentConfig)
        {

            app.UseExceptionHandler("/error.html");
            
            ///<summary>
            ///Using configuration file instead of using direct environment variable in the code that is comment out below
            ///</summary>
            if (environmentConfig.EnableDeveloperException)//(configuration.GetValue<bool>("ListZdefiniowana:EnableDeveloperException")) //(env.IsDevelopment())
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

            app.UseSignalR(routes =>
            {
                routes.MapHub<ChatHub>("/chatHub");
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

            });
        }
    }
}
