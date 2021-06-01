using FreeSql.DataAnnotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace WorkerService1
{
    public class Program
    {

        static IFreeSql fsql = new FreeSql.FreeSqlBuilder()
            .UseConnectionString(FreeSql.DataType.Sqlite, "Data Source=document.db; Pooling=true;Min Pool Size=1")
            .UseAutoSyncStructure(true) //�Զ�ͬ��ʵ��ṹ�����������ر���
            .UseMonitorCommand(cmd => Console.Write(cmd.CommandText))//ʹ��azure��app service Ӧ�÷��� Linux�汾, ������ʹ��  
            .Build();

        public static void Main(string[] args)
        {

            //��ʼ��demo����

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


            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton(fsql);
                    services.AddHostedService<Worker>();
                });
    }


    public class Item
    {
        [Column(IsIdentity = true)]
        [DisplayName("���")]
        public int Id { get; set; }

        [DisplayName("����")]
        public string Text { get; set; }

        [DisplayName("����")]
        public string Description { get; set; }
    }
}
