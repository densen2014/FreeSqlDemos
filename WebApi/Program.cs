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
    .UseAutoSyncStructure(true) //�Զ�ͬ��ʵ��ṹ�����������ر���
    .UseMonitorCommand(cmd => Console.Write(cmd.CommandText))//ʹ��azure��app service Ӧ�÷��� Linux�汾, ������ʹ��  
    .Build();


#region ��ʼ��demo����

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

#endregion 

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();
builder.Services.AddSingleton(fsql);
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.IncludeXmlComments(System.IO.Path.Combine(AppContext.BaseDirectory, "WebApi.xml"), true);
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApi", Version = "v1", Description = "����Դ�� https://github.com/densen2014/FreeSqlDemos" });
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
