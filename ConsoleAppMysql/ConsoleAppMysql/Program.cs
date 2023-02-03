// See https://aka.ms/new-console-template for more information
using FreeSql.DataAnnotations;
using MyTestDB;
using System;
using System.Data.Common;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json;
using System.Threading;
using System.Linq.Expressions;

 class Program
{
    private static async Task Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        string connstr = File.ReadAllText(File.Exists("c:\\uuid\\config.txt") ? "c:\\uuid\\config.txt" : "config.txt");


        var fsql = new FreeSql.FreeSqlBuilder()
            .UseConnectionString(FreeSql.DataType.MySql, connstr)
            .UseMonitorCommand(cmd => Console.WriteLine("\r\n SQL==> \r\n" + cmd.CommandText))
            .UseAutoSyncStructure(true)
            .UseNoneCommandParameter(true)
            .Build();


        var res2 = fsql.Ado.CommandFluent(nameof(proc1))
            .CommandType(CommandType.StoredProcedure)
            .CommandTimeout(60)
            .WithParameter(nameof(proc1.parameter1), null, NewMethod(nameof(proc1.parameter1)))
            .ExecuteScalar(); //执行存储过程

        Console.WriteLine(res2);

        return;

        for (int i = 0; i < 10; i++)
        {
            var t = new TestTable() { id = i, remark = "remark " + i.ToString() };
            var res = fsql.InsertOrUpdate<TestTable>().SetSource(t).ExecuteAffrows();
            Console.WriteLine($"{i}: 插入或改写记录 {res}");
            await Task.Delay(100);
        }
    }

    private static Action<DbParameter> NewMethod(string parameter) => p => p = p.DefaultValue(parameter);
    

    [JsonObject(MemberSerialization.OptIn), Table(DisableSyncStructure = true)]
    public partial class proc1
    {

        public string parameter1 { get; set; } = Guid.NewGuid().ToString();

    }
}


