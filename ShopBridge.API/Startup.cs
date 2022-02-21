using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using ShopBridge.Data.DbContexts;
using ShopBridge.Data.Interfaces;
using ShopBridge.Data.Repository;
using ShopBridge.Extensions;
using ShopBridge.Models.AppSettings;

namespace ShopBridge
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env,IConfiguration configuration)
        {
            Configuration = configuration;
            HostingEnvironment = env;
        }

        public IConfiguration Configuration { get; }
        private AppSettings appSettings;
        public IWebHostEnvironment HostingEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Startup));
            services.Configure<AppSettings>(Configuration);
            appSettings = GetAppSettings(services);

            services
                .AddUserSwagger(appSettings)
                .AddControllers();


            var sqlConnectionString = appSettings.DatabaseConfiguration.DatabaseConnectionString(HostingEnvironment.IsProduction());
            services.AddDbContext<ShopBridgeDbContext>(options => options.UseSqlServer(sqlConnectionString));


            services.AddTransient<IItemRepository, ItemRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseUserSwaggerUi(appSettings);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private AppSettings GetAppSettings(IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            return serviceProvider.GetService<IOptions<AppSettings>>().Value;
        }
    }
}
