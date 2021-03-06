using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using FreeSql.DataAnnotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using SignalRChat.Hubs;

namespace SignalRChat
{
    public class Startup
    {


        public class Item
        {
            [Column(IsIdentity = true)]
            [DisplayName("序号")]
            public int Id { get; set; }

            [DisplayName("时间")]
            public DateTime Date { get; set; } = DateTime.Now;

            [DisplayName("名称")]
            public string Text { get; set; }

            [DisplayName("描述")]
            public string Description { get; set; }
        }



        IFreeSql fsql = new FreeSql.FreeSqlBuilder()
            .UseConnectionString(FreeSql.DataType.Sqlite, "Data Source=document.db; Pooling=true;Min Pool Size=1")
            .UseAutoSyncStructure(true) //自动同步实体结构【开发环境必备】
            .UseMonitorCommand(cmd => Console.Write(cmd.CommandText))//使用azure的app service 应用服务 Linux版本, 好像不能使用  
            .Build();


        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            //初始化demo数据

            var ItemList = new List<Item>()
            {
                new Item {  Text = "假装 First item", Description="This is an item description." },
                new Item {  Text = "的哥 Second item", Description="This is an item description." },
                new Item { Text = "四风 Third item", Description="This is an item description." },
                new Item {  Text = "加州 Fourth item", Description="This is an item description." },
                new Item { Text = "阳光 Fifth item", Description="This is an item description." },
                new Item {  Text = "孔雀 Sixth item", Description="This is an item description." }
            };

            if (fsql.Select<Item>().Count() == 0)
            {
                fsql.Insert<Item>().AppendData(ItemList).ExecuteAffrows();
            }
            ItemList = fsql.Select<Item>().ToList();


            Console.WriteLine("ItemList: " + ItemList.Count());



            services.AddCors();
            services.AddSingleton(fsql);
            services.AddRazorPages();
            //services.AddSwaggerGen(c =>
            //{
            //    c.IncludeXmlComments(System.IO.Path.Combine(AppContext.BaseDirectory, "SignalRChat.xml"), true);
            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApi", Version = "v1", Description = "工程源码 https://github.com/densen2014/FreeSqlDemos" });
            //});
            //配置 SignalR 服务器，以将 SignalR 请求传递到 SignalR
            //将 SignalR 添加到 ASP.NET Core 依赖关系注入和路由系统
            services.AddSignalR();
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //app.UseSwagger();
            //app.UseSwaggerUI(c =>
            //{
            //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "FreesSQL Demo WebApi v1");
            //    c.RoutePrefix = string.Empty;
            //});

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors(options =>
            {
                options.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader();
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();

                //配置 SignalR 服务器，以将 SignalR 请求传递到 SignalR
                //将 SignalR 添加到 ASP.NET Core 依赖关系注入和路由系统
                endpoints.MapHub<ChatHub>("/chatHub");
            });
        }
    }
}
