using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryShared;

#if WINDOWS
using Windows.Storage;
#endif

namespace MauiBlazorApp.Pages
{
    public partial class Index
    {
        List<Item> ItemList=new List<Item>();

        protected override void OnInitialized()
        {
 
#if WINDOWS
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "sqliteSample.db");
#else
            string dbpath = "sqliteSample.db";
#endif

            Microsoft.Data.Sqlite.SqliteConnection _database = new Microsoft.Data.Sqlite.SqliteConnection($"Data Source={dbpath}");

            var fsql = new FreeSql.FreeSqlBuilder()
            .UseConnectionFactory(FreeSql.DataType.Sqlite, () => _database, typeof(FreeSql.Sqlite.SqliteProvider<>))
            .UseAutoSyncStructure(true)
            .UseNoneCommandParameter(true) //必须开启,因为Microsoft.Data.Sqlite内插处理有bug
            .UseMonitorCommand(cmd => Console.Write(cmd.CommandText))
            .Build();

            if (fsql.Select<Item>().Count() < 20)
            {
                var itemList = Item.GenerateDatas();
                fsql.Insert<Item>().AppendData(itemList).ExecuteAffrows();
            }
            ItemList = fsql.Select<Item>().ToList();


            Console.WriteLine("\r\n\r\nItemListCount: " + ItemList.Count());
            Console.WriteLine("\r\n\r\nLastItem: " + ItemList.Last().Text);

        }
    }
}
