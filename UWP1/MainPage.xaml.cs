using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using Windows.Storage;
using System.Data.SQLite;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace UWP1
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "sqliteSample.db");
            
            SQLiteConnection _database = new SQLiteConnection($"Data Source={dbpath}");


            var fsql = new FreeSql.FreeSqlBuilder()
                      .UseConnectionFactory(FreeSql.DataType.Sqlite, () => _database, typeof(FreeSql.Sqlite.SqliteProvider<>))
                      //.UseConnectionString(FreeSql.DataType.Sqlite, "Data Source=document.db; Pooling=true;Min Pool Size=1") //UWP需要反射SqliteProvider
                      //.UseConnectionString(FreeSql.DataType.MsAccess, "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=document.mdb")  //如果提示Microsoft.ACE.OLEDB.12.0未注册下载安装 https://download.microsoft.com/download/E/4/2/E4220252-5FAE-4F0A-B1B9-0B48B5FBCCF9/AccessDatabaseEngine_X64.exe
                      .UseAutoSyncStructure(true) //自动同步实体结构【开发环境必备】
                      .UseMonitorCommand(cmd => Console.Write(cmd.CommandText))
                      .Build();

            var r = new Random();

            var ItemList = new List<ItemUWP>()
            {
                new ItemUWP {  Text = "假装 First item" , Description="This is an item description." ,Idu=Guid.NewGuid()},
                new ItemUWP {  Text = "的哥 Second item", Description="This is an item description." ,Idu=Guid.NewGuid()},
                new ItemUWP { Text = "四风 Third item", Description="This is an item description." ,Idu=Guid.NewGuid()},
                new ItemUWP {  Text = "加州 Fourth item", Description="This is an item description." ,Idu=Guid.NewGuid()},
                new ItemUWP { Text = "阳光 Fifth item", Description="This is an item description." ,Idu=Guid.NewGuid()},
                new ItemUWP {  Text = "孔雀 Sixth item - "+ r.Next(11000).ToString(), Description="This is an item description." ,Idu=Guid.NewGuid()}
            };

            if (fsql.Select<ItemUWP>().Count() < 20)
            {
                fsql.Insert<ItemUWP>().AppendData(ItemList).ExecuteAffrows();
            }
            ItemList = fsql.Select<ItemUWP>().ToList();
            ListView Fruits = new ListView();
            Fruits.ItemsSource = ItemList.Select(a => a.Text).ToList();
            FruitsPanel.Children.Add(Fruits);
        }

        [Index("Idu001", "Idu", true)]
        public class ItemUWP
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
        public class ItemDto : ItemUWP
        {
            new public int? Id { get; set; }

        }
    }
}
