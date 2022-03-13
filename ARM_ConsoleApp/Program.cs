using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;

namespace FreeSqlDemos
{
    class Program
    {
        // arm,树莓派使用freesql说明
        // 1.添加包
        //<PackageReference Include="FreeSql.Provider.Sqlite" Version="3.0.100" />
        //<PackageReference Include = "Microsoft.Data.Sqlite" Version="6.0.3" />

        // 2.代码
        //Microsoft.Data.Sqlite.SqliteConnection _database = new Microsoft.Data.Sqlite.SqliteConnection($"Data Source=document.db");

        //var fsql = new FreeSql.FreeSqlBuilder()
        //.UseConnectionFactory(FreeSql.DataType.Sqlite, () => _database, typeof(FreeSql.Sqlite.SqliteProvider<>))
        //.UseAutoSyncStructure(true)
        //.UseNoneCommandParameter(true) //必须开启,因为Microsoft.Data.Sqlite内插处理有bug
        //.UseMonitorCommand(cmd => Console.Write(cmd.CommandText))
        //.Build();

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");


            Console.WriteLine("系统：" + (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? "Linux" :
                    RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ? "OSX" :
                    RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "Windows" : "未知"));
            Console.WriteLine($"系统架构：{RuntimeInformation.OSArchitecture}");
            Console.WriteLine($"系统名称：{RuntimeInformation.OSDescription}");
            Console.WriteLine($"进程架构：{RuntimeInformation.ProcessArchitecture}");
            Console.WriteLine($"是否64位操作系统：{Environment.Is64BitOperatingSystem}");
            Console.WriteLine("CPU CORE:" + Environment.ProcessorCount);
            Console.WriteLine("HostName:" + Environment.MachineName);
            Console.WriteLine("Version:" + Environment.OSVersion);

            Microsoft.Data.Sqlite.SqliteConnection _database = new Microsoft.Data.Sqlite.SqliteConnection($"Data Source=document.db");

            var fsql = new FreeSql.FreeSqlBuilder()
            .UseConnectionFactory(FreeSql.DataType.Sqlite, () => _database, typeof(FreeSql.Sqlite.SqliteProvider<>))
            .UseAutoSyncStructure(true) 
            .UseNoneCommandParameter(true) //必须开启,因为Microsoft.Data.Sqlite内插处理有bug
            .UseMonitorCommand(cmd => Console.Write(cmd.CommandText))
            .Build();

            var r = new Random();

            var ItemList = new List<Item>()
            {
                new Item {  Text = "假装 First item" , Description="This is an item description." ,Idu=Guid.NewGuid()},
                new Item {  Text = "的哥 Second item", Description="This is an item description." ,Idu=Guid.NewGuid()},
                new Item { Text = "四风 Third item", Description="This is an item description." ,Idu=Guid.NewGuid()},
                new Item {  Text = "加州 Fourth item", Description="This is an item description." ,Idu=Guid.NewGuid()},
                new Item { Text = "阳光 Fifth item", Description="This is an item description." ,Idu=Guid.NewGuid()},
                new Item {  Text = "孔雀 Sixth item - "+ r.Next(11000).ToString(), Description="This is an item description." ,Idu=Guid.NewGuid()}
            };

            if (fsql.Select<Item>().Count() < 100)
            {
                fsql.Insert<Item>().AppendData(ItemList).ExecuteAffrows();
            }
            ItemList = fsql.Select<Item>().ToList();


            Console.WriteLine("\r\n\r\nItemListCount: " + ItemList.Count());
            Console.WriteLine("\r\n\r\nLastItem: " + ItemList.Last().Text);


            //测试dto去掉Key
            var one = fsql.Select<ItemDto>().Skip(2).ToOne();
            one.Id = null;

            var sql = fsql.Update<ItemDto>()
                            .SetSource(one)
                            .IgnoreColumns(a => a.Id)
                            //.Where(a=>a.Idu==one.Idu)
                            .ToSql();
        }
    }

    [Index("Idu001", "Idu", true)]
    public class Item
    {
        [Column(IsIdentity = true, IsPrimary = true)]
        [DisplayName("序号")]
        public int Id { get; set; }

        [DisplayName("名称")]
        public string Text { get; set; }

        [DisplayName("描述")]
        public string Description { get; set; }

        [Column(IsPrimary = true)]
        [DisplayName("序号U")]
        public Guid Idu { get; set; }
    }

    [Table(DisableSyncStructure = true, Name = "Item")]
    public class ItemDto : Item
    {
        new public int? Id { get; set; }

    }
}
