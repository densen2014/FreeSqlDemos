using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FreeSql.DataAnnotations;
using FreeSql.Internal;
using ColumnAttribute = FreeSql.DataAnnotations.ColumnAttribute;
using System.Diagnostics;
using System.Data;

namespace WebFormsNF48
{
    public partial class _Default : Page
    {
        static IFreeSql fsql = new FreeSql.FreeSqlBuilder()
           .UseConnectionString(FreeSql.DataType.Sqlite, "Data Source=document.db; Pooling=true;Min Pool Size=1")
           .UseAutoSyncStructure(true) //自动同步实体结构【开发环境必备】
           .UseMonitorCommand(cmd => Debug.Write(cmd.CommandText))//使用azure的app service 应用服务 Linux版本, 好像不能使用  
           .Build();

        DataTable ItemList = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
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

        }

        void RefreshData()
        {
            ItemList = fsql.Select<Item>().ToDataTable();

            Console.WriteLine("ItemList: " + ItemList.Rows.Count);

            GridView1.DataSource = ItemList; ;
            GridView1.DataBind();
            LabelMsg.Text = "Records: " + ItemList.Rows.Count;
        }

        protected void buttonADD_Click(object sender, EventArgs e)
        {
            var newone = new Item
            {
                Text = "新的" + DateTime.Now.Ticks.ToString(),
                Description = "This is an new item description."
            };

            fsql.Insert<Item>().AppendData(newone).ExecuteAffrows();
            RefreshData();
        }

        protected void buttonREFRESH_Click(object sender, EventArgs e)
        {
            RefreshData();
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