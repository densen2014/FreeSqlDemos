using FreeSql.DataAnnotations;
using System.ComponentModel;
#if WINDOWS
using Windows.Storage;
#endif

namespace LibraryShared
{
     public class DataDock
    {
        static IFreeSql fsql;

        public List<Item> Init(IFreeSql fsqlt=null)
        {
#if WINDOWS
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "sqliteSample.db");
#else
            string dbpath = "sqliteSample.db";
#endif

            Microsoft.Data.Sqlite.SqliteConnection _database = new Microsoft.Data.Sqlite.SqliteConnection($"Data Source={dbpath}");

            fsql ??= fsqlt??=new FreeSql.FreeSqlBuilder()
            .UseConnectionFactory(FreeSql.DataType.Sqlite, () => _database, typeof(FreeSql.Sqlite.SqliteProvider<>))
            .UseAutoSyncStructure(true)
            .UseNoneCommandParameter(true) //必须开启,因为Microsoft.Data.Sqlite内插处理有bug
            .UseMonitorCommand(cmd => Console.Write(cmd.CommandText))
            .Build();

            if (fsql.Select<Item>().Count() < 4)
            {
                var itemList = Item.GenerateDatas();
                fsql.Insert<Item>().AppendData(itemList.Take(2)).ExecuteAffrows();
            }
            var ItemList = fsql.Select<Item>().ToList();

            //var ItemList = Item.GenerateDatas();
            Console.WriteLine("\r\n\r\nItemListCount: " + ItemList.Count());
            Console.WriteLine("\r\n\r\nLastItem: " + ItemList.Last().Text);

            return ItemList;
         }

        public Item Add(string name)
        {
            var item = new Item { Text = name, Description = name, Idu = Guid.NewGuid() };
            fsql.Insert(item).ExecuteAffrows();
            return item;
        }
        public bool Delete()
        {
            return fsql.Select<Item>().Take(1).ToDelete().ExecuteAffrows()==1;
        }
        }


    }