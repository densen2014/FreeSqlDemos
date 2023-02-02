// See https://aka.ms/new-console-template for more information
using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;

 class Program
{
    private static async Task Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        string connstr = File.ReadAllText(File.Exists ("c:\\uuid\\config.txt")? "c:\\uuid\\config.txt" : "config.txt");


        var fsql = new FreeSql.FreeSqlBuilder()
            .UseConnectionString(FreeSql.DataType.MySql, connstr)
            .UseMonitorCommand(cmd => Console.WriteLine("\r\n SQL==> \r\n" + cmd.CommandText))
            .UseAutoSyncStructure(true)
            .UseNoneCommandParameter(true)
            .Build();

        
        for (int i = 0; i < 10000; i++)
        {
            var t = new TestTable() { id=i,remark = "remark " + i.ToString ()};
            var res=fsql.InsertOrUpdate<TestTable>().SetSource(t).ExecuteAffrows();
            Console.WriteLine($"{i}: 插入或改写记录 {res}");
            await Task.Delay(100);
        }
    }
    
    public class TestTable
    {
        public int id { get; set; }
        public string remark { get; set; }
    }
     
}