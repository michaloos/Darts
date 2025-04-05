using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace darts.Model;

public class UserGame : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null) 
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    public required Guid Id { get; set; }
    public required User User { get; set; }

    private int? _position;
    public int? Position
    {
        get => _position;
        set { _position = value; OnPropertyChanged(); }
    }

    private ObservableCollection<UserGameShoot> _shoots = new();
    public ObservableCollection<UserGameShoot> Shoots
    {
        get => _shoots;
        set { _shoots = value; OnPropertyChanged(); }
    }

    private int? _currentScore;
    public int? CurrentScore
    {
        get => _currentScore;
        set { _currentScore = value; OnPropertyChanged(); }
    }

    private bool _currentPlayer;
    public bool CurrentPlayer
    {
        get => _currentPlayer;
        set { _currentPlayer = value; OnPropertyChanged(); }
    }

    private int _round;
    public int Round
    {
        get => _round;
        set { _round = value; OnPropertyChanged(); }
    }
    
    // public ObservableCollection<UserGameShoot> GetShootsForRound(int round)
    // {
    //     return new ObservableCollection<UserGameShoot>(
    //         Shoots.Where(shoot => shoot.Round == round)
    //     );
    // }
}