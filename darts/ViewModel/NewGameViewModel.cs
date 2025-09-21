using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using darts.Core.Interface;
using darts.Core.Model;
using IPopupService = darts.Core.Interface.IPopupService;

namespace darts.ViewModel;

public class NewGameViewModel : BaseViewModel
{
    private ObservableCollection<User> _users = [];
    public ObservableCollection<User> Users
    {
        get => _users;
        set => SetProperty(ref _users, value);
    }
    private ObservableCollection<object> _selectedUsers = [];
    public ObservableCollection<object> SelectedUsers
    {
        get => _selectedUsers;
        set => SetProperty(ref _selectedUsers, value);
    }

    private readonly IGameService _gameService;
    private readonly ILoadingService _loadingService;
    private readonly IPopupService _popupService;
    private readonly IUnitOfWork _unitOfWork;
    public ICommand StartNewGameCommand { get; set; }
    public ICommand SelectUserCommand { get; set; }
    public ICommand GamesSelectionChangedCommand { get; set; }

    public bool CanStartNewGame
    {
        get => _canStartNewGame;
        set => SetProperty(ref _canStartNewGame, value);
    }
    public ObservableCollection<GameMode> Games { get; set; }
    private GameMode? _selectedGameMode;
    private GameMode? _lastValidGameMode;

    public GameMode? SelectedGameMode
    {
        get => _selectedGameMode;
        set
        {
            // Nie pozwalaj wybrać Disabled – przywróć poprzednią poprawną selekcję
            if (value is not null && value.Disabled)
            {
                OnPropertyChanged(nameof(SelectedGameMode));
                SetCanStartNewGame();
                return;
            }

            SetProperty(ref _selectedGameMode, value);
            if (value is not null && !value.Disabled)
                _lastValidGameMode = value;
            SetCanStartNewGame();
        }
    }

    private bool _canStartNewGame;

    public NewGameViewModel(IGameService gameService,
        ILoadingService loadingService, IPopupService popupService,
        IUnitOfWork unitOfWork)
    {
        _gameService = gameService;
        _loadingService = loadingService;
        _popupService = popupService;
        _unitOfWork = unitOfWork;

        Games = GameModes.Modes;
        StartNewGameCommand = new Command(StartNewGame);
        SelectUserCommand = new Command(SelectUserChanged);
        GamesSelectionChangedCommand = new Command<object>(OnGamesSelectionChanged);
    }

    public async Task InitializeAsync()
    {
        await LoadUsers();
    }

    private async Task LoadUsers()
    {
        var users = await _unitOfWork.Users.GetAllAsync();
        Users = new ObservableCollection<User>(users);
        SetCanStartNewGame();
    }

    private void SelectUserChanged() => SetCanStartNewGame();

    private async void StartNewGame()
    {
        if (SelectedGameMode is null) 
        {
            var toast = Toast.Make("Nie został wybrany żaden tryb gry", ToastDuration.Long);
            await toast.Show();
            return;
        }

        object? result;
        using (await _loadingService.Show("Konfigurowanie gry"))
        {
            result = await _popupService.ShowPopupAsync<NewGamePropsPopupViewModel>(SelectedGameMode);
        }

        if (result is null)
            return;

        using (await _loadingService.Show("Tworzenie gry"))
        {
            _gameService.StartNewGame(SelectedGameMode, SelectedUsers.Cast<User>().ToList(), (result as GameModeConfiguration)!);
            await Shell.Current.GoToAsync(nameof(GamePage));
        }
    }

    private void SetCanStartNewGame()
        => CanStartNewGame = SelectedUsers.Count >= 2 && SelectedGameMode is not null;

    private void OnGamesSelectionChanged(object parameter)
    {
        // Parametr to SelectedItem z CollectionView
        if (parameter is GameMode gm)
        {
            if (gm.Disabled)
            {
                // Anuluj i przywróć poprzednią poprawną selekcję
                SelectedGameMode = _lastValidGameMode;
            }
            else
            {
                SelectedGameMode = gm;
            }
        }
    }
}