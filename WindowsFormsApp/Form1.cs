using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp
{
    public partial class Form1 : Form
    {
        static IFreeSql fsql = new FreeSql.FreeSqlBuilder()
           .UseConnectionString(FreeSql.DataType.Sqlite, "Data Source=document.db; Pooling=true;Min Pool Size=1")
           .UseAutoSyncStructure(true) //自动同步实体结构【开发环境必备】
           .UseMonitorCommand(cmd => Console.Write(cmd.CommandText))//使用azure的app service 应用服务 Linux版本, 好像不能使用  
           .Build();

        List<Item> ItemList = new List<Item>();

        public Form1()
        {
            InitializeComponent();
            //初始化demo数据

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

        private void buttonADD_Click(object sender, EventArgs e)
        {
            var newone = new Item {
                Text = "新的" + DateTime .Now.Ticks.ToString(),
                Description = "This is an new item description."
            };

            fsql.Insert<Item>().AppendData(newone).ExecuteAffrows();
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
