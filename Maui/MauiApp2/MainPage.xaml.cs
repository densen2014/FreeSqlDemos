using LibraryShared;

namespace MauiApp2;

using System.Collections.ObjectModel;
#if WINDOWS
using Windows.Storage;
#endif

public partial class MainPage : ContentPage
{
	int count = 0;
	DataDock dataDock;
	ObservableCollection<Item> ItemList=new ObservableCollection<Item> ();
	public MainPage()
	{
		InitializeComponent();
		dataDock = new DataDock();
		foreach (var item in dataDock.Init())
		{
			ItemList.Add (item);
		} 
		Fill();

    }

	private void OnCounterClicked(object sender, EventArgs e)
	{
		count++;
		CounterLabel.Text = $"Current count: {count}";

		SemanticScreenReader.Announce(CounterLabel.Text);
		
		ItemList.Add(dataDock.Add($"New {count}"));
	}

	private void OnDelete1Clicked(object sender, EventArgs e)
	{ 		
		if (dataDock.Delete()) ItemList.RemoveAt(ItemList.Count -1);
	}

	void Fill()
    {
		ListView Fruits = new ListView();
		//Fruits.ItemsSource = ItemList.Select(a => a.Text).ToList();
		Fruits.ItemsSource = ItemList;
		FruitsPanel.Children.Add(Fruits);

	}
}

