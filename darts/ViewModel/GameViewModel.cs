using System.Collections.ObjectModel;
using System.Windows.Input;
using darts.Model;
using darts.Services.Interfaces;
using static System.Int32;

namespace darts.ViewModel;

// public class GameViewModel : BaseViewModel
// {
//     private IGameService GameService { get; set; }
//     public ObservableCollection<UserGame> UserGames { get; set; }
//     public ICommand AddScoreCommand { get; set; }
//     public ICommand OnToggleX2Command { get; set; }
//     public ICommand OnToggleX3Command { get; set; }
//     public ICommand UndoCommand { get; set; }
//
//     public bool X2IsChecked
//     {
//         get => _x2IsChecked;
//         set {
//             if (!SetProperty(ref _x2IsChecked, value)) return;
//             OnPropertyChanged(nameof(CanToggleX3));
//             OnPropertyChanged(nameof(CanToggleX2));
//             OnPropertyChanged(nameof(IsAnyMultiplierChecked));
//         }
//     }
//
//     public bool X3IsChecked
//     {
//         get => _x3IsChecked; 
//         set {
//             if (!SetProperty(ref _x3IsChecked, value)) return;
//             OnToggleX2Command.CanExecuteChanged += (s, e) => { };
//             UndoCommand.CanExecuteChanged += (s, e) => { };
//             OnPropertyChanged(nameof(CanToggleX3));
//             OnPropertyChanged(nameof(CanToggleX2));
//             OnPropertyChanged(nameof(IsAnyMultiplierChecked));
//         }
//     }
//     
//     public bool CanToggleX2 => !_x3IsChecked;
//     public bool CanToggleX3 => !_x2IsChecked;
//     private bool _x2IsChecked;
//     private bool _x3IsChecked;
//     public GameViewModel(IGameService gameService)
//     {
//         GameService = gameService;
//         UserGames = gameService.GameUsers;
//         AddScoreCommand = new Command<string>(AddScore);
//         OnToggleX2Command = new Command(() => X2IsChecked = !X2IsChecked, () => CanToggleX2);
//         OnToggleX3Command = new Command(() => X3IsChecked = !X3IsChecked, () => CanToggleX3);
//         UndoCommand = new Command(Undo, () => !IsAnyMultiplierChecked());
//     }
//
//     private bool IsAnyMultiplierChecked()
//         => _x2IsChecked || _x3IsChecked;
//     
//     private void Undo()
//     {
//         
//     }
//
//     private void AddScore(string score)
//     {
//         var tryParse = TryParse(score, out var result);
//         if (!tryParse) return;
//         var multiplier = X2IsChecked ? 2 : X3IsChecked ? 3 : 1;
//         X2IsChecked = false;
//         X3IsChecked = false;
//     }
// }

public class GameViewModel : BaseViewModel
{
    private readonly IGameService _gameService;
    
    public ObservableCollection<UserGame> UserGames { get; set; }
    
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
        Add25Command = new Command<string>(AddScore, (string value) => !IsAnyMultiplierChecked);
        Add50Command = new Command<string>(AddScore, (string value) => !IsAnyMultiplierChecked);
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
        // Logika cofania
    }

    private void AddScore(string score)
    {
        if (!int.TryParse(score, out var result)) return;

        var multiplier = X2IsChecked ? 2 : X3IsChecked ? 3 : 1;
        X2IsChecked = false;
        X3IsChecked = false;
    }
}
