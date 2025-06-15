using System.Collections.ObjectModel;
using System.Windows.Input;
using darts.Core.Interface;
using darts.Core.Model;
using static System.Int32;

namespace darts.ViewModel;

public class GameViewModel : BaseViewModel
{
    private readonly IGameService _gameService;
    
    private ObservableCollection<UserGame> _userGames;
    public ObservableCollection<UserGame> UserGames
    {
        get => _userGames;
        set { _userGames = value; OnPropertyChanged(); }
    }
    
    public Command AddScoreCommand { get; }
    public Command OnToggleX2Command { get; }
    public Command OnToggleX3Command { get; }
    public Command UndoCommand { get; }
    public Command Add25Command { get; }
    public Command Add50Command { get; }

    private bool _x2IsChecked;
    private bool _x3IsChecked;

    public bool X2IsChecked
    {
        get => _x2IsChecked;
        set
        {
            if (SetProperty(ref _x2IsChecked, value))
                UpdateCommandStates();
        }
    }

    public bool X3IsChecked
    {
        get => _x3IsChecked;
        set
        {
            if (SetProperty(ref _x3IsChecked, value))
                UpdateCommandStates();
        }
    }

    public bool CanToggleX2 => !X3IsChecked;
    public bool CanToggleX3 => !X2IsChecked;
    public bool IsAnyMultiplierChecked => X2IsChecked || X3IsChecked;

    public GameViewModel(IGameService gameService)
    {
        _gameService = gameService;
        UserGames = gameService.GameUsers;

        AddScoreCommand = new Command<string>(AddScore);
        OnToggleX2Command = new Command(() => X2IsChecked = !X2IsChecked, () => CanToggleX2);
        OnToggleX3Command = new Command(() => X3IsChecked = !X3IsChecked, () => CanToggleX3);
        Add25Command = new Command<string>(AddScore, value => !IsAnyMultiplierChecked);
        Add50Command = new Command<string>(AddScore, value => !IsAnyMultiplierChecked);
        UndoCommand = new Command(Undo, () => !IsAnyMultiplierChecked);
    }

    private void UpdateCommandStates()
    {
        OnPropertyChanged(nameof(CanToggleX2));
        OnPropertyChanged(nameof(CanToggleX3));
        OnPropertyChanged(nameof(IsAnyMultiplierChecked));

        OnToggleX2Command.ChangeCanExecute();
        OnToggleX3Command.ChangeCanExecute();
        Add25Command.ChangeCanExecute();
        Add50Command.ChangeCanExecute();
        UndoCommand.ChangeCanExecute();
    }

    private void Undo()
    {
        _gameService.UndoShoot();
        OnPropertyChanged(nameof(UserGames));
    }

    private void AddScore(string score)
    {
        if (!TryParse(score, out var result)) return;

        var multiplier = X2IsChecked ? 2 : X3IsChecked ? 3 : 1;
        var finalShootScore = multiplier * result;
        _gameService.AddScore(finalShootScore, multiplier);
        
        OnPropertyChanged(nameof(UserGames));
        
        X2IsChecked = false;
        X3IsChecked = false;
    }
}
