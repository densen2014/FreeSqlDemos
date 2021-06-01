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
            [DisplayName("���")]
            public int Id { get; set; }

            [DisplayName("ʱ��")]
            public DateTime Date { get; set; } = DateTime.Now;

            [DisplayName("����")]
            public string Text { get; set; }

            [DisplayName("����")]
            public string Description { get; set; }
        }



        IFreeSql fsql = new FreeSql.FreeSqlBuilder()
            .UseConnectionString(FreeSql.DataType.Sqlite, "Data Source=document.db; Pooling=true;Min Pool Size=1")
            .UseAutoSyncStructure(true) //�Զ�ͬ��ʵ��ṹ�����������ر���
            .UseMonitorCommand(cmd => Console.Write(cmd.CommandText))//ʹ��azure��app service Ӧ�÷��� Linux�汾, ������ʹ��  
            .Build();


        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            //��ʼ��demo����

            var ItemList = new List<Item>()
            {
                new Item {  Text = "��װ First item", Description="This is an item description." },
                new Item {  Text = "�ĸ� Second item", Description="This is an item description." },
                new Item { Text = "�ķ� Third item", Description="This is an item description." },
                new Item {  Text = "���� Fourth item", Description="This is an item description." },
                new Item { Text = "���� Fifth item", Description="This is an item description." },
                new Item {  Text = "��ȸ Sixth item", Description="This is an item description." }
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
            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApi", Version = "v1", Description = "����Դ�� https://github.com/densen2014/FreeSqlDemos" });
            //});
            //���� SignalR ���������Խ� SignalR ���󴫵ݵ� SignalR
            //�� SignalR ��ӵ� ASP.NET Core ������ϵע���·��ϵͳ
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

                //���� SignalR ���������Խ� SignalR ���󴫵ݵ� SignalR
                //�� SignalR ��ӵ� ASP.NET Core ������ϵע���·��ϵͳ
                endpoints.MapHub<ChatHub>("/chatHub");
            });
        }
    }
}
