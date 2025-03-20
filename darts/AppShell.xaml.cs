namespace darts;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		Routing.RegisterRoute(nameof(NewGamePage), typeof(NewGamePage));
		Routing.RegisterRoute(nameof(HistoryPage), typeof(HistoryPage));
		Routing.RegisterRoute(nameof(GamePage), typeof(GamePage));
	}
}
