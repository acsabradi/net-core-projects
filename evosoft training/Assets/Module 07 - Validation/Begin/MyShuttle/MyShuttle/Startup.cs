
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyShuttle.Data;
using MyShuttle.Model;
using MyShuttle.Web.AppBuilderExtensions;


namespace MyShuttle
{
    public class Startup
    {
        public IConfiguration Configuration { get; private set; }

        public Startup(IWebHostEnvironment env)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("config.json", optional: true)
                .SetBasePath(env.ContentRootPath)
                .Build();

            Configuration = config;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureDataContext(Configuration);

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<MyShuttleContext>()
                .AddDefaultTokenProviders();

            services.ConfigureDependencies();
            services.AddMvc();
            services.AddMvc(options => options.EnableEndpointRouting = false);


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.ConfigureRoutes();
            app.UseStaticFiles();
        }
    }
}
