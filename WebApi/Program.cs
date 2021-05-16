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
    //     <TabItem Text="��ĿԴ��">
    //    <div>
    //        <span>��Ŀ��ַ </span>
    //        <br />

    //        FreeSql�ĸ��ֹ���demo, console,xamarin app, ios, android, wpf, blazor, nf461, webapi
    //        <br />
    //        <ApiLinkComponent API = "https://github.com/densen2014/FreeSqlDemos" />

    //        < br />
    //        BootstrapBlazor��FreeSql����ע�������չ��
    //        < br />
    //        < ApiLinkComponent API="https://www.nuget.org/packages/Densen.FreeSql.Extensions.BootstrapBlazor" />

    //        <br />
    //        [Blazor] ����ά������: BootstrapBlazor + freesql
    //        <br />
    //        <ApiLinkComponent API = "https://github.com/densen2014/BootstrapBlazorApp1_freesql" />

    //        < br />
    //        [Blazor] ZXing Blazor ɨ����� / ��дǩ�� Handwritten ���
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
