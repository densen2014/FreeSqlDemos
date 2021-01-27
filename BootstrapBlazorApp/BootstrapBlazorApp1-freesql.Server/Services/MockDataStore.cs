using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BootstrapBlazorApp1_freesql.Models;

namespace BootstrapBlazorApp1_freesql.Services
{
    public class MockDataStore : IDataStore<Item>
    {
        public BindingList<Item> items;
        public static IFreeSql fsql { get; set; }


        public MockDataStore()
        {
            test();
            items = new BindingList<Item>();
            var result = fsql.Select<Item>().ToList();
            result.ForEach(a => items.Add(a));
        }

        void test()
        {
            List<Item> items;


            items = new List<Item>()
            {
                new Item {  Text = "假装 First item", Description="This is an item description." },
                new Item {  Text = "的哥 Second item", Description="This is an item description." },
                new Item { Text = "四风 Third item", Description="This is an item description." },
                new Item {  Text = "加州 Fourth item", Description="This is an item description." },
                new Item { Text = "阳光 Fifth item", Description="This is an item description." },
                new Item {  Text = "孔雀 Sixth item", Description="This is an item description." }
            };



            try
            {
                #region mssql

                //        fsql = new FreeSql.FreeSqlBuilder()
                //.UseConnectionString(FreeSql.DataType.SqlServer, "Data Source=192.168.1.100;Initial Catalog=demo;Persist Security Info=True;MultipleActiveResultSets=true;User ID=sa;Password=a123456;Connect Timeout=30;min pool size=1;connection lifetime=15")
                //.UseAutoSyncStructure(true) //自动同步实体结构【开发环境必备】
                //.UseMonitorCommand(cmd => Console.Write(cmd.CommandText))
                //.Build();

                #endregion

                #region Sqlite

                fsql = new FreeSql.FreeSqlBuilder()
                        .UseConnectionString(FreeSql.DataType.Sqlite, "Data Source=document.db; Pooling=true;Min Pool Size=1")
                        .UseAutoSyncStructure(true) //自动同步实体结构【开发环境必备】
                        .UseMonitorCommand(cmd => Console.Write(cmd.CommandText))
                        .Build();

                #endregion

                #region mysql

                //fsql = new FreeSql.FreeSqlBuilder()
                //.UseConnectionString(FreeSql.DataType.MySql, "Data Source=192.168.1.100;Port=3306;User ID=root;Password=a123456; Initial Catalog=test;Charset=utf8; SslMode=none;Min pool size=1")
                //.UseAutoSyncStructure(true) //自动同步实体结构【开发环境必备】
                //.UseMonitorCommand(cmd => Console.Write(cmd.CommandText))
                //.Build();
                #endregion

                fsql.CodeFirst.SyncStructure<Item>();
                if (fsql.Select<Item>().Count() == 0)
                {
                    fsql.Insert<Item>().AppendData(items).ExecuteAffrows();
                }
                var res = fsql.Select<Item>().ToList(a => a.Text);
                res.ForEach(a =>
                {
                    Debug.WriteLine(" <== 测试测试测试 ==> " + a);
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }


        public async Task<bool> AddItemAsync(Item item)
        {
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Item item)
        {
 
            if (item.Id==0)
            {
                fsql.Insert<Item>().AppendData(item).ExecuteAffrows();
            }
            else
            {
                fsql.Update <Item>().SetSource(item).ExecuteAffrows();
            }
            await GetItemsAsync(true);
            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(int id)
        {
            fsql.Delete<Item>().Where(a => a.Id == id).ExecuteAffrows();
            await GetItemsAsync(true);
            return await Task.FromResult(true);
        }


        public async Task<Item> GetItemAsync(int id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Item>> GetItemsAsync(bool forceRefresh = false)
        {
            if (forceRefresh)
            {
                items.Clear();
                var result = fsql.Select<Item>().ToList();
                result.ForEach(a => items.Add(a));
            }
            return await Task.FromResult(items);
        }
    }
}