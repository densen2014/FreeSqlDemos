using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Microsoft.Extensions.Configuration;
using BootstrapBlazorApp1_freesql.Services;
using BootstrapBlazorApp1_freesql.Models;
using Microsoft.AspNetCore.Components.Authorization;
using System.IO;
 using BootstrapBlazor.Components; 

namespace BootstrapBlazorApp.Server.Pages
{
    public partial class Index
    {
        [Inject] protected IJSRuntime JsRuntime { get; set; }
        [Inject] protected MockDataStore dataService { get; set; }


        //protected override async Task OnInitializedAsync() // 在组件执行异步操作时使用 OnInitializedAsync，并应在操作完成后刷新
        //{
        //      StateHasChanged();
        // }


        #region 表格相关

        protected Task<QueryData<Item>> OnEditQueryAsync(QueryPageOptions options) => BindItemQueryAsync(dataService.items, options);

        protected Task<Item> OnAddAsync()
        {
             return  Task.FromResult(new Item() { Id = 0 });
        }
        protected async Task<bool> OnSaveAsync(Item item,ItemChangedType itemChangedType)
        {
            await dataService.UpdateItemAsync(item, itemChangedType);
            return true;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        protected Task<bool> OnDeleteAsync(IEnumerable<Item> items)
        {
            items.ToList().ForEach(async item =>
            await dataService.DeleteItemAsync(item.Id));
            return Task.FromResult(true);
        }

        private static readonly ConcurrentDictionary<Type, Func<IEnumerable<Item>, string, SortOrder, IEnumerable<Item>>> SortLambdaCache = new ConcurrentDictionary<Type, Func<IEnumerable<Item>, string, SortOrder, IEnumerable<Item>>>();

        /// <summary>
        ///
        /// </summary>
        protected Item SearchModel { get; set; } = new Item();

        /// <summary>
        ///
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        protected Task OnResetSearchAsync(Item item)
        {
            item.Text = "";
            return Task.CompletedTask;
        }

        /// <summary>
        ///
        /// </summary>
        protected IEnumerable<int> PageItemsSource => new int[] { 20, 50, 100 };

        /// <summary>
        ///
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        protected Task<QueryData<Item>> OnQueryAsync(QueryPageOptions options) => BindItemQueryAsync(dataService.items, options);

        /// <summary>
        ///
        /// </summary>
        /// <param name="items"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        protected Task<QueryData<Item>> BindItemQueryAsync(IEnumerable<Item> items, QueryPageOptions options)
        {
            //TODO: 此处代码后期精简
            if (!string.IsNullOrEmpty(SearchModel.Text)) items = items.Where(item => item.Text?.Contains(SearchModel.Text, StringComparison.OrdinalIgnoreCase) ?? false);
            if (!string.IsNullOrEmpty(options.SearchText)) items = items.Where(item => (item.Text?.Contains(options.SearchText) ?? false)
                 || (item.Text?.Contains(options.SearchText) ?? false));

            // 过滤
            var isFiltered = false;
            if (options.Filters.Any())
            {
                items = items.Where(options.Filters.GetFilterFunc<Item>());

                // 通知内部已经过滤数据了
                isFiltered = true;
            }

            // 排序
            var isSorted = false;
            if (!string.IsNullOrEmpty(options.SortName))
            {
                // 外部未进行排序，内部自动进行排序处理
                //var invoker = SortLambdaCache.GetOrAdd(typeof(Item), key => items.GetSortLambda().Compile());
                //items = invoker(items, options.SortName, options.SortOrder);

                // 通知内部已经过滤数据了
                isSorted = true;
            }

            // 设置记录总数
            var total = items.Count();

            // 内存分页
            items = items.Skip((options.PageIndex - 1) * options.PageItems).Take(options.PageItems).ToList();

            return Task.FromResult(new QueryData<Item>()
            {
                Items = items,
                TotalCount = total,
                IsSorted = isSorted,
                IsFiltered = isFiltered,
                IsSearch = !string.IsNullOrEmpty(SearchModel.Text) || !string.IsNullOrEmpty(SearchModel.Description)
            });
        }
         
 
        #endregion

    }
}
