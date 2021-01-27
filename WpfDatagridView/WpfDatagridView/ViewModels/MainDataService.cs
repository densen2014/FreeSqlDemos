using AME.Helpers;
using FreeSql.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.InputB;
using WPFORMDGV;

namespace AME.Services
{
    public class MainDataService : BaseService<Item>
    {
        private List<Item> itemList;
        public List<Item> ItemList { get => itemList; set => SetProperty(ref itemList, value); }
        public DelegateCommand 保存Command { get; set; }
        public DelegateCommand 刷新Command { get; set; }

        public MainDataService(AppSetting appSetting, MyService restService)
        {
            Title = "主界面";
            this.appSetting = appSetting;
            保存Command = new DelegateCommand(async () => await UpdateItemsAsync());
            刷新Command = new DelegateCommand(async () => await GetItemsAsync(true));


            ItemList = new List<Item>()
            {
                new Item {  Text = "假装 First item", Description="This is an item description." },
                new Item {  Text = "的哥 Second item", Description="This is an item description." },
                new Item { Text = "四风 Third item", Description="This is an item description." },
                new Item {  Text = "加州 Fourth item", Description="This is an item description." },
                new Item { Text = "阳光 Fifth item", Description="This is an item description." },
                new Item {  Text = "孔雀 Sixth item", Description="This is an item description." }
            };

            if (App.fsql.Select<Item>().Count() == 0)
            {
                App.fsql.Insert<Item>().AppendData(ItemList).ExecuteAffrows();
            }
            ItemList = App.fsql.Select<Item>().ToList();
        
        }
        public async Task<bool> AddItemAsync(Item item)
        {
            ItemList.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemsAsync()
        {
            try
            {
                Badge = App.fsql.InsertOrUpdate<Item>().SetSource(ItemList).ExecuteAffrows();
                await GetItemsAsync(true);
                Foot = $"保存 {Badge} 行";
                return await Task.FromResult(true);

            }
            catch (System.Exception ex)
            {
                Foot = $"保存出错 {ex.Message} ";
                return await Task.FromResult(false);
            }
        }
        public async Task<bool> UpdateItemAsync(Item item)
        {

            if (item.Id == 0)
            {
                App.fsql.Insert<Item>().AppendData(item).ExecuteAffrows();
            }
            else
            {
                App.fsql.Update<Item>().SetSource(item).ExecuteAffrows();
            }
            await GetItemsAsync(true);
            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(int id)
        {
            App.fsql.Delete<Item>().Where(a => a.Id == id).ExecuteAffrows();
            await GetItemsAsync(true);
            return await Task.FromResult(true);
        }


        public async Task<Item> GetItemAsync(int id)
        {
            return await Task.FromResult(ItemList.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Item>> GetItemsAsync(bool forceRefresh = false)
        {
            if (forceRefresh)
            {
                ItemList.Clear();
                var result = App.fsql.Select<Item>().ToList();
                result.ForEach(a => ItemList.Add(a));
                Badge = result.Count();
                Foot = $"数据 {Badge} 行";
            }
            return await Task.FromResult(ItemList);
        }

    }


    /// <summary>
    /// 项目
    /// </summary>
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
