using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi
{
    //     <TabItem Text="项目源码">
    //    <div>
    //        <span>项目地址 </span>
    //        <br />

    //        FreeSql的各种工程demo, console,xamarin app, ios, android, wpf, blazor, nf461, webapi
    //        <br />
    //        <ApiLinkComponent API = "https://github.com/densen2014/FreeSqlDemos" />

    //        < br />
    //        BootstrapBlazor的FreeSql数据注入服务扩展包
    //        < br />
    //        < ApiLinkComponent API="https://www.nuget.org/packages/Densen.FreeSql.Extensions.BootstrapBlazor" />

    //        <br />
    //        [Blazor] 快速维护数据: BootstrapBlazor + freesql
    //        <br />
    //        <ApiLinkComponent API = "https://github.com/densen2014/BootstrapBlazorApp1_freesql" />

    //        < br />
    //        [Blazor] ZXing Blazor 扫码组件 / 手写签名 Handwritten 组件
    //        <br />
    //        <ApiLinkComponent API = "https://github.com/densen2014/ZXingBlazor" />
    //    </ div >
    //</ TabItem >

    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
