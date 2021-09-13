using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace FreeSqlDemos
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!"); 
            var fsql = new FreeSql.FreeSqlBuilder()
            .UseConnectionString(FreeSql.DataType.Sqlite, "Data Source=document.db; Pooling=true;Min Pool Size=1")
            //.UseConnectionString(FreeSql.DataType.MsAccess, "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=document.mdb")  //如果提示Microsoft.ACE.OLEDB.12.0未注册下载安装 https://download.microsoft.com/download/E/4/2/E4220252-5FAE-4F0A-B1B9-0B48B5FBCCF9/AccessDatabaseEngine_X64.exe
            .UseAutoSyncStructure(true) //自动同步实体结构【开发环境必备】
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

            if (fsql.Select<Item>().Count() <100)
            {
                fsql.Insert<Item>().AppendData(ItemList).ExecuteAffrows();
            }
            ItemList = fsql.Select<Item>().ToList();


            Console.WriteLine("\r\n\r\nItemListCount: " + ItemList.Count());
            Console.WriteLine("\r\n\r\nLastItem: " + ItemList.Last().Text);

            var one = fsql.Select<Item>().Skip(2).ToOne(); 

            var sql = fsql.Update<Item>()
                            .SetSource(one)
                            .IgnoreColumns(a => a.Id)
                            //.Where(a=>a.Idu==one.Idu)
                            .ToSql();
        }
    }

    [Index("Idu001", "Idu",true)]
    public class Item
    {
        [Column(IsIdentity = true)]
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

    [Table (DisableSyncStructure =true,Name = "Item")]
    public class ItemDto:Item
    {
        [Column(IsIdentity = false)]
        new public int Id { get; set; }

    }
}
