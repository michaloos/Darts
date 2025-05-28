using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace darts.Data.Model;

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
    
    public ObservableCollection<UserGameShoot> VisibleShoots
    {
        get
        {
            var currentRound = Round;
            var currentShoots = Shoots
                .Where(s => s.Round == currentRound)
                .ToList();

            while (currentShoots.Count < 3)
            {
                currentShoots.Add(new UserGameShoot
                {
                    Score = null,
                    Round = currentRound,
                    ShootNumber = Shoots.Count + 1
                });
            }

            return new ObservableCollection<UserGameShoot>(currentShoots);
        }
    }
}