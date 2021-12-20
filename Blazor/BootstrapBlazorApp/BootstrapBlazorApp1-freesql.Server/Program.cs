using BootstrapBlazorApp1_freesql.Services;
using BootstrapBlazorServerApp.Data;
using Microsoft.AspNetCore.ResponseCompression;
using System.Globalization;
using System.IO.Compression;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddResponseCompression(options =>
{
    options.Providers.Add<BrotliCompressionProvider>();
    options.Providers.Add<GzipCompressionProvider>();
    options.MimeTypes =
    ResponseCompressionDefaults.MimeTypes.Concat(
                    new[] { "image/svg+xml" });
});
builder.Services.Configure<BrotliCompressionProviderOptions>(options =>
{
    options.Level = (CompressionLevel)4;
});

builder.Services.AddCors(); 
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor(); 


var cultureInfo = new CultureInfo("zh-CN");
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

builder.Services.AddBootstrapBlazor();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddScoped<MockDataStore>();

// 增加 Table 数据服务操作类
builder.Services.AddTableDemoDataService();

// 增加 FreeSql ORM 数据服务操作类
// 需要时打开下面代码
// 需要引入 FreeSql 对 SQLite 的扩展包 FreeSql.Provider.Sqlite
builder.Services.AddFreeSql(option =>
{
    option.UseConnectionString(FreeSql.DataType.Sqlite, builder.Configuration.GetConnectionString("bb"))
#if DEBUG
         //开发环境:自动同步实体
         .UseAutoSyncStructure(true)
         .UseNoneCommandParameter(true)
         //调试sql语句输出
         .UseMonitorCommand(cmd => System.Console.WriteLine(cmd.CommandText))
#endif
                    ;
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseResponseCompression(); 
app.UseHttpsRedirection();

app.UseStaticFiles(); 

app.UseRouting();

app.MapBlazorHub();  
app.MapFallbackToPage("/_Host");

app.Run();
