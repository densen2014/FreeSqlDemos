using LibraryShared;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace MauiApp2;

public class MainViewModel : INotifyPropertyChanged
{
    #region INotifyPropertyChanged
    protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = "", Action onChanged = null, bool force = false)
    {
        if (!force && EqualityComparer<T>.Default.Equals(backingStore, value))
            return false;

        backingStore = value;
        onChanged?.Invoke();
        OnPropertyChanged(propertyName);
        return true;
    }


    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        var changed = PropertyChanged;
        if (changed == null)
            return;

        changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
        #endregion
 
    public ObservableCollection<TaskItem> ItemList { get;set; } = new ObservableCollection<TaskItem>();
    public ICommand CounterCommand => new Command(OnCounterClicked);
    public Command<SelectedItemChangedEventArgs> ItemSelectCommand => new Command<SelectedItemChangedEventArgs>(OnItemSelected);
    public ICommand RefreshCommand => new Command(OnRefreshClicked);
    public ICommand SelectOneCommand => new Command(OnSelectOneClicked);
    public ICommand AddCommand => new Command(OnAddClicked);
    public ICommand ModifyCommand => new Command(OnModifyClicked);
    public ICommand DeleteCommand => new Command(OnDeleteClicked);
    public int count = 0;
    public string CounterLabel { get => counterLabel; set { SetProperty(ref counterLabel, value); } }
    string counterLabel = "Current count: 0";

    public DataService dataService;
    public string Title2 { get => title2; set { SetProperty(ref title2, value); } }
    string title2 = "TEST";
    public TaskItem ItemSelected { get => itemSelected; set { SetProperty(ref itemSelected, value); } }
    TaskItem itemSelected;
    public MainViewModel()
    {
        dataService = new DataService();

        foreach (var item in dataService.Init(initdemodatas:true))
        {
            this.ItemList.Add(item);
        }
        ItemSelected = this.ItemList.LastOrDefault();
        OnCounterClicked();
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
        var item = dataService.Add($"New {count+1}");
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
