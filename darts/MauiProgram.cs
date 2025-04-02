using CommunityToolkit.Maui;
using darts.Services;
using darts.Services.Interfaces;
using darts.ViewModel;
using Microsoft.Extensions.Logging;

namespace darts;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});
		builder.Services.AddSingleton<IGameService, GameService>();
		builder.Services.AddTransient<NewGameViewModel>();
		builder.Services.AddTransient<NewGamePage>();
		builder.Services.AddTransient<GameViewModel>();
		builder.Services.AddTransient<GamePage>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
