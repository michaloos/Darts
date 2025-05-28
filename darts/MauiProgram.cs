using CommunityToolkit.Maui;
using darts.Core.Interface;
using darts.Data.Context;
using darts.Services;
using darts.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mopups.Hosting;

namespace darts;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var dbPath = Path.Combine(FileSystem.AppDataDirectory, "darts.db");
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			// .UsePrism(prism =>
			// {
			// 	prism.RegisterTypes(container =>
			// 	{
			// 		// Rejestracja typów, w tym np. popupów
			// 		container.RegisterForNavigation<LoadingPopup>("Loading");
			// 		container.RegisterForNavigation<MainPage, MainPageViewModel>();
			// 	});
			//
			// 	// Określenie na czym aplikacja ma się rozpocząć
			// 	prism.OnAppStart(async () =>
			// 	{
			// 		var navigationService = prism.ApplicationContainer.Resolve<INavigationService>();
			// 		await navigationService.NavigateAsync("MainPage");
			// 	});
			// })
			.UseMauiCommunityToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});
		builder.Services.AddDbContext<DartsDbContext>(options =>
			options.UseSqlite($"Data Source={dbPath}"));
		builder.Services.AddSingleton<IGameService, GameService>();
		builder.Services.AddTransient<ILoadingService, LoadingService>();
		builder.Services.AddTransient<NewGamePropsPopup>();
		builder.Services.AddTransient<NewGamePropsPopupViewModel>(); 
		builder.Services.AddTransientPopup<NewGamePropsPopup, NewGamePropsPopupViewModel>();
		builder.Services.AddTransient<NewGameViewModel>();
		builder.Services.AddTransient<NewGamePage>();
		builder.Services.AddTransient<GameViewModel>();
		builder.Services.AddTransient<GamePage>();
		builder.Services.AddTransient<LoadingPopup>();
		builder.Services.AddTransient<HistoryViewModel>();
		builder.Services.AddTransient<HistoryPage>();
		builder.Services.AddTransient<UsersPage>();
		builder.Services.AddTransient<UsersPageViewModel>();

		builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
		builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
		builder.Services.AddScoped<IUserService, UserService>();
		builder.ConfigureMopups();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
