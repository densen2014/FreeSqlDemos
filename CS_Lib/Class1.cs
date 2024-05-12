// ********************************** 
// Densen Informatica 中讯科技 
// 作者：Alex Chow
// e-mail:zhouchuanglin@gmail.com 
// **********************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using FreeSql.DataAnnotations;

namespace CS_Lib
{
    public class Class1
    {
        static IFreeSql fsql = new FreeSql.FreeSqlBuilder()
           .UseConnectionString(FreeSql.DataType.Sqlite, "Data Source=document.db; Pooling=true;Min Pool Size=1")
           .UseAutoSyncStructure(true) //自动同步实体结构【开发环境必备】
           .UseMonitorCommand(cmd => Console.Write(cmd.CommandText))//使用azure的app service 应用服务 Linux版本, 好像不能使用  
           .Build();

        public List<Item> ItemList = new List<Item>()
            {
                new Item {  Text = "假装 First item", Description="This is an item description." },
                new Item {  Text = "的哥 Second item", Description="This is an item description." },
                new Item { Text = "四风 Third item", Description="This is an item description." },
                new Item {  Text = "加州 Fourth item", Description="This is an item description." },
                new Item { Text = "阳光 Fifth item", Description="This is an item description." },
                new Item {  Text = "孔雀 Sixth item", Description="This is an item description." }
            };


        public void Init()
        {
            if (fsql.Select<Item>().Count() == 0)
            {
                fsql.Insert<Item>().AppendData(ItemList).ExecuteAffrows();
            } 

        }

        public void RefreshData()
        {
            
            ItemList = fsql.Select<Item>().ToList();

            Console.WriteLine("ItemList: " + ItemList.Count());

        }

        public void Add()
        {
            var newone = new Item
            {
                Text = "新的" + DateTime.Now.Ticks.ToString(),
                Description = "This is an new item description."
            };

            fsql.Insert<Item>().AppendData(newone).ExecuteAffrows();
            RefreshData();

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
