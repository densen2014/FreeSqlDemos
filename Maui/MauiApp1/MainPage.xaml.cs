using LibraryShared;
#if WINDOWS
using Windows.Storage;
#endif

namespace MauiApp1
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();

#if WINDOWS
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "sqliteSample.db");
#else
            string dbpath = "sqliteSample.db";
#endif

            Microsoft.Data.Sqlite.SqliteConnection _database = new Microsoft.Data.Sqlite.SqliteConnection($"Data Source={dbpath}");

            //var fsql = new FreeSql.FreeSqlBuilder()
            //.UseConnectionFactory(FreeSql.DataType.Sqlite, () => _database, typeof(FreeSql.Sqlite.SqliteProvider<>))
            //.UseAutoSyncStructure(true)
            //.UseNoneCommandParameter(true) //必须开启,因为Microsoft.Data.Sqlite内插处理有bug
            //.UseMonitorCommand(cmd => Console.Write(cmd.CommandText))
            //.Build();

            //if (fsql.Select<Item>().Count() < 20)
            //{
            //    var itemList = Item.GenerateDatas();
            //    fsql.Insert<Item>().AppendData(itemList).ExecuteAffrows();
            //}
            //var ItemList = fsql.Select<Item>().ToList();

            var ItemList = Item.GenerateDatas();
            Console.WriteLine("\r\n\r\nItemListCount: " + ItemList.Count());
            Console.WriteLine("\r\n\r\nLastItem: " + ItemList.Last().Text);

            ListView Fruits = new ListView();
            //Fruits.ItemsSource = ItemList.Select(a => a.Text).ToList();
            Fruits.ItemsSource = ItemList;
            FruitsPanel.Children.Add(Fruits);

        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            count++;
            CounterLabel.Text = $"Current count: {count}";

            SemanticScreenReader.Announce(CounterLabel.Text);
        }
    }
}