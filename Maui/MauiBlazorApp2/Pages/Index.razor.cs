using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryShared;
 

namespace MauiBlazorApp2.Pages
{
    public partial class Index
    {
        List<TaskItem> ItemList=new List<TaskItem>();
        DataService dataService;
        string CounterLabel  = "Current count: 0";
        TaskItem ItemSelected;
        int count = 0;
        protected override void OnInitialized()
        {
            dataService = new DataService();
            ItemList = dataService.Init(initdemodatas: true);

        }

        private void OnCounterClicked()
        {
            count = ItemList.Count;
            CounterLabel = $"Current count: {count}";
            SemanticScreenReader.Announce(CounterLabel);

        }
        private void OnItemSelected(SelectedItemChangedEventArgs e)
        {
            if (e == null) return;
            TaskItem item = e.SelectedItem as TaskItem;
            if (item == null) return;
            ItemSelected = item;
            CounterLabel = $"Select item: {ItemSelected}";
            SemanticScreenReader.Announce(CounterLabel);

        }
        private void OnSelectOneClicked()
        {
            ItemSelected = null;// ItemList[3];
            CounterLabel = $"Select item: {ItemSelected}";
            SemanticScreenReader.Announce(CounterLabel);

        }

        private void OnAddClicked()
        {
            var item = dataService.Add($"New {count + 1}");
            ItemList.Add(item);

            OnCounterClicked();
        }
        private void OnRefreshClicked()
        {
            ItemList.Clear();
            foreach (var item in dataService.Init())
            {
                this.ItemList.Add(item);
            }
            OnCounterClicked();
        }

        private void OnModifyClicked()
        {
            ItemList[0] = dataService.Modify() ?? ItemList[0];
            OnCounterClicked();
        }

        private void OnDeleteClicked()
        {
            if (dataService.Delete()) ItemList.RemoveAt(ItemList.Count - 1);
            OnCounterClicked();
        }
    }
}
