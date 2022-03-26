namespace MauiFreeSql;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private void Fruits_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        var vm = (MainViewModel)BindingContext;
        vm.ItemSelectCommand.Execute(e);
    }
}

