using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using darts.Model;
using darts.Services.Interfaces;

namespace darts.ViewModel;

public class NewGameViewModel : BaseViewModel
{
    private readonly IGameService _gameService;
    private readonly ILoadingService _loadingService;
    public ICommand StartNewGameCommand { get; set; }
    public ICommand AddNewUserCommand { get; set; }
    public ICommand RemoveUserCommand { get; set; }
    public ICommand SelectGameModeCommand { get; set; }

    public bool CanStartNewGame
    {
        get => _canStartNewGame;
        set => SetProperty(ref _canStartNewGame, value);
    }
    public ObservableCollection<GameMode> Games { get; set; }

    private string? _newUserName;
    private bool _canStartNewGame;

    public string NewUserName
    {
        get =>  _newUserName ?? "";
        set => SetProperty(ref _newUserName, value);
    }
    public ObservableCollection<User> Users { get; set; }
    
    public NewGameViewModel(IGameService gameService, ILoadingService loadingService)
    {
        _gameService = gameService;
        _loadingService = loadingService;

        Games = GameModes.Modes;
        Users = [];
        StartNewGameCommand = new Command(StartNewGame);
        AddNewUserCommand = new Command(AddNewUser);
        RemoveUserCommand = new Command<Guid>(RemoveUser);
        SelectGameModeCommand = new Command<GameMode>(OnGameModeSelected);
    }

    private void OnGameModeSelected(GameMode? selectedGameMode)
    {
        if (selectedGameMode is null)
            return;

        foreach (var gameMode in Games)
            gameMode.IsSelected = false;

        selectedGameMode.IsSelected = true;
        
        var index = Games.IndexOf(selectedGameMode);
        if (index >= 0)
            Games[index] = selectedGameMode;

        SetCanStartNewGame();
    }

    private async void StartNewGame()
    {
        using (await _loadingService.Show())
        {
            var selectedGameMode = Games.SingleOrDefault(x => x.IsSelected);
            if (selectedGameMode is null) 
            {
                var toast = Toast.Make("Nie został wybrany żaden tryb gry", ToastDuration.Long);
                await toast.Show();
                return;
            }
            _gameService.StartNewGame(selectedGameMode, Users.ToList());
            await Shell.Current.GoToAsync(nameof(GamePage));
        }
        
    }

    private async void AddNewUser()
    {
        var currentPage = Application.Current?.MainPage;
        if (Users.Select(x => x.Name).Contains(NewUserName))
        {
            if (currentPage is not null)
            {
                var toast = Toast.Make("Taki użytkownik już istnieje", ToastDuration.Long);
                await toast.Show();
            }
            return;
        }

        if (string.IsNullOrWhiteSpace(NewUserName))
        {
            if (currentPage is not null)
            {
                var toast = Toast.Make("Należy wpisać poprawnego użytkownika", ToastDuration.Long);
                await toast.Show();
            }
            return;
        }
        Users.Add(new User{
            Id = Guid.NewGuid(),
            Name = NewUserName
        });
        NewUserName = string.Empty;
        SetCanStartNewGame();
    }

    private void SetCanStartNewGame()
    {
        CanStartNewGame = Users.Count >= 2 && Games.Any(x => x.IsSelected);
    }

    private async void RemoveUser(Guid userId)
    {
        var currentPage = Application.Current?.MainPage;
        var userToRemove = Users.SingleOrDefault(x => x.Id == userId);
        if (userToRemove is not null && currentPage is not null)
        {
            var confirmation = await currentPage.DisplayAlert(
                "Potwierdzenie", 
                "Czy na pewno chcesz usunąć tego użytkownika?", 
                "Tak", 
                "Nie");
            if(confirmation)
                Users.Remove(userToRemove);
        }
        SetCanStartNewGame();
    }

}