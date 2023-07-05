using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using WebApi;



IFreeSql fsql = new FreeSql.FreeSqlBuilder()
    .UseConnectionString(FreeSql.DataType.Sqlite, "Data Source=document.db; Pooling=true;Min Pool Size=1")
    .UseAutoSyncStructure(true) //自动同步实体结构【开发环境必备】
    .UseMonitorCommand(cmd => Console.Write(cmd.CommandText))//使用azure的app service 应用服务 Linux版本, 好像不能使用  
    .Build();


#region 初始化demo数据

var ItemList = new List<Item>()
            {
                new Item {  Text = "假装 First item", Description="This is an item description.",UserID =1 },
                new Item {  Text = "的哥 Second item", Description="This is an item description.",UserID =1 },
                new Item { Text = "四风 Third item", Description="This is an item description.",UserID =2 },
                new Item {  Text = "加州 Fourth item", Description="This is an item description.",UserID =2 },
                new Item { Text = "阳光 Fifth item", Description="This is an item description.",UserID =2 },
                new Item {  Text = "孔雀 Sixth item", Description="This is an item description.",UserID =2 }
            };

var Userss = new List<Users>()
            {
                new Users {  Name = "老张", Password="This is an item description." }, 
                new Users {  Name = "周", Password="This is an item description." }, 
            };

if (fsql.Select<Item>().Count() == 0)
{
    fsql.Insert<Users>().AppendData(Userss).ExecuteAffrows();
    fsql.Insert<Item>().AppendData(ItemList).ExecuteAffrows();
}
ItemList = fsql.Select<Item>().ToList();


Console.WriteLine("ItemList: " + ItemList.Count());

#endregion 

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();
builder.Services.AddSingleton(fsql);
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.IncludeXmlComments(System.IO.Path.Combine(AppContext.BaseDirectory, "WebApi.xml"), true);
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApi", Version = "v1", Description = "工程源码 https://github.com/densen2014/FreeSqlDemos" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "FreesSQL Demo WebApi v1");
    c.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();
app.UseCors(options =>
{
    options.AllowAnyOrigin()
          .AllowAnyMethod()
          .AllowAnyHeader();
});

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
