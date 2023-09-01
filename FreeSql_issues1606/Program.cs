// ********************************** 
// Densen Informatica 中讯科技 
// 作者：Alex Chow
// e-mail:zhouchuanglin@gmail.com 
// **********************************

using AME.Models.Restaurant;
using System;
using System.Threading.Tasks;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var connstr = $"Data Source=.,1433;Initial Catalog=res_data;Persist Security Info=True;User ID=sa;Password=xxx;Connect Timeout=5";

        var fsql = new FreeSql.FreeSqlBuilder()
        //.UseConnectionString(FreeSql.DataType.Sqlite, "Data Source=document.db; Pooling=true;Min Pool Size=1")
        .UseConnectionString(FreeSql.DataType.SqlServer, connstr, typeof(FreeSql.SqlServer.SqlServerProvider<>))
        //.UseAutoSyncStructure(true) //自动同步实体结构【开发环境必备】
        .UseMonitorCommand(cmd => Console.Write(cmd.CommandText))
        .Build();

        await Task.Delay(100);
        await Task.Run(() =>
        {
            Console.WriteLine("异步查询");
            //fsql.Select<object>().AsType(typeof(ResCustomersLite));
            var selectDto = fsql.Select<ResCustomersLite>()
                 .Include(a => a.groupName)
                 .Where(a => a.groupName.CusGroupname != "" && a.groupName.CusGroupname != "隐藏")
                 .GroupBy(a => new
                 {
                     a.CusGroup,
                     a.groupName.CusGroupname,
                     a.groupName.UsePrice2
                 })
                 .OrderBy(a => a.Key.CusGroup);
            selectDto = selectDto.Limit(100);
            var res = selectDto.ToList(a => new CustomerGroupDto()
            {
                CusGroup = a.Key.CusGroup ?? 0,
                UsePrice2 = a.Key.UsePrice2 ?? false,
                CusGroupname = a.Key.CusGroupname,
            });
            Console.WriteLine("\r\n\r\n结果");
            res.ForEach(a => Console.WriteLine(a.CusGroupname));
        });

        Console.ReadKey();
    }
}
