using AME.Colors;
using AME.Helpers;
using AME.Services;
using AME.View;
using AME.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace WPFORMDGV
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public static IFreeSql fsql;
        public IConfiguration Configuration { get; }
        public static ServiceProvider serviceProvider { get; set; }
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            fsql = new FreeSql.FreeSqlBuilder()
                .UseConnectionString(FreeSql.DataType.Sqlite, "Data Source=document.db; Pooling=true;Min Pool Size=1")
                .UseAutoSyncStructure(true) //自动同步实体结构【开发环境必备】
                .UseMonitorCommand(cmd => Console.Write(cmd.CommandText))
                .Build();

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            serviceProvider = serviceCollection.BuildServiceProvider();
            //StartupUri = new Uri("MainWindow.xaml", UriKind.Relative);
            serviceProvider.GetRequiredService<MainWindow>().Show();

        }
        public void ConfigureServices(IServiceCollection services)
        {
            string FileName = Process.GetCurrentProcess().MainModule.ModuleName.ToString() + ".ini";
            AppSetting appSetting = AppSetting.Create(FileName);

            services.AddSingleton(appSetting);
            services.AddTransient<MyService>();
            services.AddTransient<MainDataService>();
            services.AddSingleton<PaletteSelectorViewModel>();
            services.AddSingleton<ConfigViewModel>();

            services.AddSingleton<MainWindow>();
            services.AddTransient<ConfigPage>();
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {

        }

        private void Application_Activated(object sender, EventArgs e)
        {

        }

    }
}
