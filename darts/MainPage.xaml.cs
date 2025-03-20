namespace darts;

public partial class MainPage : ContentPage
{

	public MainPage()
	{
		InitializeComponent();
	}

	private async void NewGameButtonClicked(object? sender, EventArgs e)
		 => await Shell.Current.GoToAsync(nameof(NewGamePage));
	
	private async void HistoryButtonClicked(object? sender, EventArgs e)
		=> await Shell.Current.GoToAsync(nameof(HistoryPage));
}

