using System.ComponentModel;
using System.Windows.Input;
using darts.Core.Model;
using darts.Core.Services;

namespace darts.ViewModel;

public class NewGamePropsPopupViewModel : BaseViewModel
{
    private GameMode _gameMode;
    private GameModeConfiguration _configuration;

    // Wspólne właściwości dla wszystkich trybów gry
    public string GameModeName => _gameMode?.Name ?? "Wybierz tryb gry";
    public string GameModeDescription => _gameMode?.Description ?? "";

    // Komendy
    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }
    public ICommand ResetToDefaultsCommand { get; }

    // Flagi określające, jaki typ konfiguracji jest aktualnie wybrany
    public bool IsX01Configuration => _configuration is StandardX01Configuration;
    public bool IsCricketConfiguration => _configuration is CricketConfiguration;
    public bool IsAroundTheClockConfiguration => _configuration is AroundTheClockConfiguration;
    public bool IsShanghaiConfiguration => _configuration is ShanghaiConfiguration;
    public bool IsKillerConfiguration => _configuration is KillerConfiguration;
    public bool IsHighScoreConfiguration => _configuration is HighScoreConfiguration;
    public bool IsLegsConfiguration => _configuration is LegsConfiguration;
    public bool IsHalveItConfiguration => _configuration is HalveItConfiguration;
    public bool IsGotchaConfiguration => _configuration is GotchaConfiguration;
    public bool IsBob27Configuration => _configuration is Bob27Configuration;

    // Właściwości dla StandardX01Configuration
    public int X01StartingScore 
    { 
        get => IsX01Configuration ? ((StandardX01Configuration)_configuration).StartingScore : 501;
        set
        {
            if (IsX01Configuration)
            {
                ((StandardX01Configuration)_configuration).StartingScore = value;
                OnPropertyChanged();
            }
        }
    }

    public bool X01DoubleOut
    {
        get => !IsX01Configuration || ((StandardX01Configuration)_configuration).DoubleOut;
        set
        {
            if (IsX01Configuration)
            {
                ((StandardX01Configuration)_configuration).DoubleOut = value;
                OnPropertyChanged();
            }
        }
    }

    public bool X01DoubleIn
    {
        get => IsX01Configuration && ((StandardX01Configuration)_configuration).DoubleIn;
        set
        {
            if (IsX01Configuration)
            {
                ((StandardX01Configuration)_configuration).DoubleIn = value;
                OnPropertyChanged();
            }
        }
    }

    public int X01NumberOfLegs
    {
        get => IsX01Configuration ? ((StandardX01Configuration)_configuration).NumberOfLegs : 3;
        set
        {
            if (IsX01Configuration)
            {
                ((StandardX01Configuration)_configuration).NumberOfLegs = value;
                OnPropertyChanged();
            }
        }
    }

    public bool X01MasterOut
    {
        get => IsX01Configuration && ((StandardX01Configuration)_configuration).MasterOut;
        set
        {
            if (IsX01Configuration)
            {
                ((StandardX01Configuration)_configuration).MasterOut = value;
                OnPropertyChanged();
            }
        }
    }

    // Właściwości dla CricketConfiguration
    public bool CricketUseCutThroatRules
    {
        get => IsCricketConfiguration && ((CricketConfiguration)_configuration).UseCutThroatRules;
        set
        {
            if (IsCricketConfiguration)
            {
                ((CricketConfiguration)_configuration).UseCutThroatRules = value;
                OnPropertyChanged();
            }
        }
    }

    public bool CricketEnableScoring
    {
        get => !IsCricketConfiguration || ((CricketConfiguration)_configuration).EnableScoring;
        set
        {
            if (IsCricketConfiguration)
            {
                ((CricketConfiguration)_configuration).EnableScoring = value;
                OnPropertyChanged();
            }
        }
    }

    // Właściwości dla GotchaConfiguration
    public int GotchaTargetScore
    {
        get => IsGotchaConfiguration ? ((GotchaConfiguration)_configuration).TargetScore : 250;
        set
        {
            if (IsGotchaConfiguration)
            {
                ((GotchaConfiguration)_configuration).TargetScore = value;
                OnPropertyChanged();
            }
        }
    }

    public bool GotchaBustRule
    {
        get => !IsGotchaConfiguration || ((GotchaConfiguration)_configuration).BustRule;
        set
        {
            if (IsGotchaConfiguration)
            {
                ((GotchaConfiguration)_configuration).BustRule = value;
                OnPropertyChanged();
            }
        }
    }

    public bool GotchaCanAttackOthers
    {
        get => !IsGotchaConfiguration || ((GotchaConfiguration)_configuration).CanAttackOthers;
        set
        {
            if (IsGotchaConfiguration)
            {
                ((GotchaConfiguration)_configuration).CanAttackOthers = value;
                OnPropertyChanged();
            }
        }
    }
    
    public event EventHandler<object> CloseRequested;
    
    public NewGamePropsPopupViewModel()
    {
        SaveCommand = new Command(Save);
        CancelCommand = new Command(Cancel);
        ResetToDefaultsCommand = new Command(ResetToDefaults);
    }
    
    public void Initialize(GameMode gameMode)
    {
        _gameMode = gameMode;
        _configuration = gameMode.CreateDefaultConfiguration();
        
        OnPropertyChanged(nameof(GameModeName));
        OnPropertyChanged(nameof(GameModeDescription));

        OnPropertyChanged(nameof(IsX01Configuration));
        OnPropertyChanged(nameof(IsCricketConfiguration));
        OnPropertyChanged(nameof(IsAroundTheClockConfiguration));
        OnPropertyChanged(nameof(IsShanghaiConfiguration));
        OnPropertyChanged(nameof(IsKillerConfiguration));
        OnPropertyChanged(nameof(IsHighScoreConfiguration));
        OnPropertyChanged(nameof(IsLegsConfiguration));
        OnPropertyChanged(nameof(IsHalveItConfiguration));
        OnPropertyChanged(nameof(IsGotchaConfiguration));
        OnPropertyChanged(nameof(IsBob27Configuration));
        
        OnPropertyChanged(nameof(X01StartingScore));
        OnPropertyChanged(nameof(X01DoubleOut));
        OnPropertyChanged(nameof(X01DoubleIn));
        OnPropertyChanged(nameof(X01NumberOfLegs));
        OnPropertyChanged(nameof(X01MasterOut));
        OnPropertyChanged(nameof(CricketUseCutThroatRules));
        OnPropertyChanged(nameof(CricketEnableScoring));
        OnPropertyChanged(nameof(GotchaTargetScore));
        OnPropertyChanged(nameof(GotchaBustRule));
        OnPropertyChanged(nameof(GotchaCanAttackOthers));
    }

    private void Save()
    {
        try
        {
            _configuration.Validate();
            
            var optionsManager = new GameOptionsManager();
            optionsManager.SaveRecentConfiguration(_configuration);
            
            // optionsManager.SetAsDefault(_configuration);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Błąd walidacji: {ex.Message}");
            return;
        }
        
        CloseRequested.Invoke(this, _configuration);
    }

    private void Cancel() => CloseRequested.Invoke(this, null);

    private void ResetToDefaults()
    {
        _configuration = _gameMode.CreateDefaultConfiguration();
        OnPropertyChanged(string.Empty);
    }
    
}