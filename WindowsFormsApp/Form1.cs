using FreeSql.DataAnnotations;
using FreeSql.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace WindowsFormsApp
{
    public partial class Form1 : Form
    {
        static IFreeSql fsql = new FreeSql.FreeSqlBuilder()
           .UseConnectionString(FreeSql.DataType.Sqlite, "Data Source=document.db; Pooling=true;Min Pool Size=1")
           .UseAutoSyncStructure(true) //自动同步实体结构【开发环境必备】
           .UseMonitorCommand(cmd => Console.Write(cmd.CommandText))//使用azure的app service 应用服务 Linux版本, 好像不能使用  
           .Build();

        #region PostgreSQL, 自己注释掉上面的sqlite方式

        //static string connstr = $"Host=127.0.0.1;Port=5432;Username=u;Password=p; Database=demo;Pooling=true;Minimum Pool Size=1";

        //static IFreeSql fsql = new FreeSql.FreeSqlBuilder()
        //     .UseConnectionString(FreeSql.DataType.PostgreSQL, connstr)
        //     .UseNameConvert(NameConvertType.ToLower)
        //     .UseAutoSyncStructure(true) //自动同步实体结构【开发环境必备】
        //     .UseMonitorCommand(cmd => Console.Write(cmd.CommandText))//使用azure的app service 应用服务 Linux版本, 好像不能使用  
        //     .Build();

        #endregion

        List<Item> ItemList = new List<Item>();

        public Form1()
        {
            InitializeComponent();
            //初始化demo数据
            // In Main method:
            mainThreadId = System.Threading.Thread.CurrentThread.ManagedThreadId;

            var ItemList = new List<Item>()
            {
                new Item {  Text = "假装 First item", Description="This is an item description." },
                new Item {  Text = "的哥 Second item", Description="This is an item description." },
                new Item { Text = "四风 Third item", Description="This is an item description." },
                new Item {  Text = "加州 Fourth item", Description="This is an item description." },
                new Item { Text = "阳光 Fifth item", Description="This is an item description." },
                new Item {  Text = "孔雀 Sixth item", Description="This is an item description." }
            };

            if (fsql.Select<Item>().Count() == 0)
            {
                fsql.Insert<Item>().AppendData(ItemList).ExecuteAffrows();
            }

            RefreshData();
        }

        void RefreshData()
        {
            ItemList = fsql.Select<Item>().ToList();

            Console.WriteLine("ItemList: " + ItemList.Count());

            dataGridView1.DataSource = ItemList;
        }

        static int mainThreadId;

        // If called in the non main thread, will return false;
        public static bool IsMainThread
        {
            get { return System.Threading.Thread.CurrentThread.ManagedThreadId == mainThreadId; }
        }

        private async void buttonADD_Click(object sender, EventArgs e)
        {
            Debug.WriteLine(Thread.CurrentThread.ManagedThreadId.ToString());
            Debug.WriteLine(IsMainThread ? "MainThread" : "f");
            this.Text=$"start add in Thread {Thread.CurrentThread.ManagedThreadId}";
            await Task.Run(async () =>
            {

                var items = new List<Item>();
                var _threadId = 0;
                for (int i = 0; i < 20; i++)
                {
                    _threadId = Thread.CurrentThread.ManagedThreadId;
                    Debug.WriteLine(_threadId.ToString());
                    Debug.WriteLine(IsMainThread ? "MainThread" : "f");
                    this.Invoke(() => this.Text = $"work in Thread {_threadId} ,  {i}  / 20");
                    await Task.Delay(500);
                    var newone = new Item
                    {
                        Text = "新的" + DateTime.Now.Ticks.ToString(),
                        Description = "This is an new item description."
                    };
                    items.Add(newone);
                }

                await fsql.Insert<Item>().AppendData(items).ExecuteAffrowsAsync();
            });

            Debug.WriteLine(Thread.CurrentThread.ManagedThreadId.ToString());
            Debug.WriteLine(IsMainThread ? "MainThread" : "f");
            MessageBox.Show("Done");
            RefreshData();
        }

        private void buttonREFRESH_Click(object sender, EventArgs e)
        {

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
}
