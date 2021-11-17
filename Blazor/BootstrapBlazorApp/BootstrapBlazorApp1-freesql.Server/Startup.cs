using BootstrapBlazor.Components;
using BootstrapBlazorApp1_freesql.Services;
using BootstrapBlazorServerApp.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BootstrapBlazorApp.Server
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();

            services.AddBootstrapBlazor();

            services.AddSingleton<WeatherForecastService>();
            services.AddScoped<MockDataStore>();
   
            // 增加 Table 数据服务操作类
            services.AddTableDemoDataService();

            // 增加 FreeSql ORM 数据服务操作类
            // 需要时打开下面代码
            // 需要引入 FreeSql 对 SQLite 的扩展包 FreeSql.Provider.Sqlite
            services.AddFreeSql(option =>
            {
                option.UseConnectionString(FreeSql.DataType.Sqlite, Configuration.GetConnectionString("bb"))
#if DEBUG
                     //开发环境:自动同步实体
                     .UseAutoSyncStructure(true)
                     .UseNoneCommandParameter(true)
                     //调试sql语句输出
                     .UseMonitorCommand(cmd => System.Console.WriteLine(cmd.CommandText))
#endif
                    ;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }
            app.ApplicationServices.RegisterProvider();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
