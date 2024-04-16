using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace ConsoleApp1;

internal class Program
{
    static void Main(string[] args)
    {

        Microsoft.Data.Sqlite.SqliteConnection _database = new($"Data Source = document.db");

        var fsql = new FreeSql.FreeSqlBuilder()
                .UseConnectionFactory(FreeSql.DataType.Sqlite, ()=>_database,typeof(FreeSql.Sqlite.SqliteProvider<>))
                .UseAutoSyncStructure(true)
                .UseLazyLoading(true)
                .UseNoneCommandParameter(true)
                .UseMonitorCommand(cmd => Console.WriteLine(cmd.CommandText))
                .Build();

        User user = new User()
        {
            time = DateTime.Now
        };

        int f = fsql.Insert(user).ExecuteAffrows();
        Console.WriteLine($"已写入数据库{f}条数据");

        var v = fsql.Select<User>().ToList();
        Console.WriteLine($"已查询到数据库{v.Count}条数据");

        foreach (var item in v)
        {
            Console.WriteLine(item.time);
        }

        Console.WriteLine("结束");
        Console.ReadLine();

    }

    public class User
    {
        [FreeSql.DataAnnotations.Column(IsIdentity = true, IsPrimary = true)]
        public int Id { get; set; }
        public DateTime time { get; set; }
    }
}
