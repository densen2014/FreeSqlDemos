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
            .UseAutoSyncStructure(true) //自动同步实体结构【开发环境必备】
            .UseMonitorCommand(cmd => Console.Write(cmd.CommandText))
            .Build();

            var r = new Random();

            var ItemList = new List<Item>()
            {
                new Item {  Text = "假装 First item" , Description="This is an item description." },
                new Item {  Text = "的哥 Second item", Description="This is an item description." },
                new Item { Text = "四风 Third item", Description="This is an item description." },
                new Item {  Text = "加州 Fourth item", Description="This is an item description." },
                new Item { Text = "阳光 Fifth item", Description="This is an item description." },
                new Item {  Text = "孔雀 Sixth item - "+ r.Next(11000).ToString(), Description="This is an item description." }
            };

            if (fsql.Select<Item>().Count() <100)
            {
                fsql.Insert<Item>().AppendData(ItemList).ExecuteAffrows();
            }
            ItemList = fsql.Select<Item>().ToList();


            Console.WriteLine("\r\n\r\nItemListCount: " + ItemList.Count());
            Console.WriteLine("\r\n\r\nLastItem: " + ItemList.Last().Text);


        }
    }


    public class Item
    {
        [Column(IsIdentity = true)]
        [DisplayName("序号")]
        public int Id { get; set; }

        [DisplayName("名称")]
        public string Text { get; set; }

        [DisplayName("描述")]
        public string Description { get; set; }
    }

}
