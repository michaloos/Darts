using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace darts.Core.Model;

public class UserGame : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    public void OnPropertyChanged([CallerMemberName] string? propertyName = null) 
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    public required Guid Id { get; set; }
    public required User User { get; set; }

    private int? _position;
    public int? Position
    {
        get => _position;
        set { _position = value; OnPropertyChanged(); }
    }
    
    private int _setWins;

    public int SetWins
    {
        get => _setWins;
        set { _setWins = value; OnPropertyChanged(); }
    }
    
    public ObservableCollection<UserGameShoot> Shoots
    {
        get => _shoots;
        set
        {
            if (_shoots != value)
            {
                _shoots = value;
                _shoots.CollectionChanged += Shoots_CollectionChanged;

                OnPropertyChanged();
                OnPropertyChanged(nameof(VisibleShoots));
            }
        }
    }

    private void Shoots_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        OnPropertyChanged(nameof(VisibleShoots));
    }

    public bool CurrentPlayer
    {
        get => _currentPlayer;
        set
        {
            if (_currentPlayer != value)
            {
                _currentPlayer = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(VisibleShoots));
            }
        }
    }

    private ObservableCollection<UserGameShoot> _shoots = new();

    private int? _currentScore;
    public int? CurrentScore
    {
        get => _currentScore;
        set { _currentScore = value; OnPropertyChanged(); }
    }

    private bool _currentPlayer;

    private int _round;
    public int Round
    {
        get => _round;
        set { _round = value; OnPropertyChanged(); OnPropertyChanged(nameof(VisibleShoots)); }
    }
    
    private int _currentSetNumber;
    public int CurrentSetNumber
    {
        get => _currentSetNumber;
        set { _currentSetNumber = value; OnPropertyChanged(); OnPropertyChanged(nameof(VisibleShoots)); }
    }

    public ObservableCollection<UserGameShoot> VisibleShoots
    {
        get
        {
            var currentRound = Round;
            var currentSet = CurrentSetNumber;

            var currentShoots = Shoots
                .Where(s => s.Round == currentRound && s.SetNumber == currentSet)
                .ToList();

            while (currentShoots.Count < 3)
            {
                currentShoots.Add(new UserGameShoot
                {
                    Score = null,
                    Round = currentRound,
                    ShootNumber = currentShoots.Count + 1,
                    Multiplier = 0,
                    ApplyToScore = false,
                    SetNumber = currentSet,
                });
            }

            return new ObservableCollection<UserGameShoot>(currentShoots);
        }
    }
}