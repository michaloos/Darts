using System.Collections.ObjectModel;
using darts.Core.Interface;
using darts.Core.Model;

namespace darts.Services;

public class GameService : IGameService
{
    public required ObservableCollection<UserGame> GameUsers { get; set; }
    public UserGame? CurrentUserGame { get; set; }
    public required GameMode GameMode { get; set; }
    private bool _isNewRoundStarting = false;

    public void StartNewGame(GameMode gameMode, List<User> users)
    {
        GameUsers = [];
        GameMode = gameMode;
        users.ForEach(user =>
        {
            GameUsers.Add(new UserGame
            {
                Id = Guid.NewGuid(),
                User = user,
                Position = null,
                Shoots = new ObservableCollection<UserGameShoot>(),
                CurrentScore = null,
                CurrentPlayer = false,
                Round = 1,
            });
        });
        CurrentUserGame =  GameUsers.First();
        CurrentUserGame!.CurrentPlayer = true;
    }

    public void EndGame()
    {
        throw new NotImplementedException();
    }

    public void MoveToTheNextPlayer()
    {
        if (CurrentUserGame is null) throw new InvalidOperationException("No player found");

        var currentIndex = GameUsers.IndexOf(CurrentUserGame);
        if (currentIndex < 0) return;

        var previousPlayer = CurrentUserGame;
        previousPlayer.CurrentPlayer = false;
        previousPlayer.OnPropertyChanged(nameof(UserGame.VisibleShoots));
        
        if (currentIndex == GameUsers.Count - 1)
        {
            CurrentUserGame = GameUsers.First();
            UpdatePositions();
        }
        else
        {
            CurrentUserGame = GameUsers[currentIndex + 1];
        }

        CurrentUserGame.CurrentPlayer = true;
        CurrentUserGame.OnPropertyChanged(nameof(UserGame.VisibleShoots));
        CurrentUserGame.Round++;
    }
    private void UpdatePositions()
    {
        if (GameUsers.All(x => x.CurrentScore == 0)) {
            foreach (var gameUser in GameUsers)
                gameUser.Position = null;
            return;
        }

        var iteration = 1;
        foreach (var userGame in GameUsers.OrderByDescending(x => x.CurrentScore)) {
            userGame.Position = iteration;
            iteration++;
        }
    }
    
    public void MoveToThePreviousPlayer()
    {
        if (CurrentUserGame is null) throw new InvalidOperationException("No player found");

        if (CurrentUserGame.Shoots.Count % 3 != 0)
            return;
        
        var previousIndex = GetPreviousPlayerIndex();
        
        CurrentUserGame.CurrentPlayer = false;
        CurrentUserGame.OnPropertyChanged(nameof(UserGame.VisibleShoots));
        CurrentUserGame.Round--;
        
        var previousPlayer = GameUsers.ElementAt(previousIndex);
        CurrentUserGame = previousPlayer;
        CurrentUserGame.CurrentPlayer = true;
        UpdatePositions();
        
    }
    
    private void RemoveLastScore()
    {
        if (CurrentUserGame is null) throw new InvalidOperationException("No player found");

        var previousIndex = GetPreviousPlayerIndex();
        if (GameUsers.ElementAt(previousIndex).Shoots.Count % 3 == 0 && GameUsers.ElementAt(previousIndex).Shoots.Any())
            MoveToThePreviousPlayer();
        
        if (CurrentUserGame.Shoots.Any())
            CurrentUserGame.Shoots.RemoveAt(CurrentUserGame.Shoots.Count - 1);
        UpdateShoots();

    }

    public void UndoShoot()
    {
        RemoveLastScore();
        UpdateShoots();
    }

    public void AddScore(int score, int multiplier)
    {
        if (CurrentUserGame is null) return;
        
        CurrentUserGame.Shoots.Add(new UserGameShoot
        {
            Score = score,
            ShootNumber = CurrentUserGame.Shoots.Count + 1,
            Round = CurrentUserGame.Round,
            Multiplier = multiplier,
        });
        
        UpdateShoots();

        if (CurrentUserGame.Shoots.Count % 3 == 0)
            MoveToTheNextPlayer();
    }

    private void UpdateShoots()
    {
        var updatedShoots = new ObservableCollection<UserGameShoot>(CurrentUserGame!.Shoots);
        CurrentUserGame.Shoots = updatedShoots;
        CurrentUserGame.CurrentScore = CurrentUserGame.Shoots.Sum(x => x.Score);
        
        CurrentUserGame.OnPropertyChanged(nameof(CurrentUserGame.VisibleShoots));
    }

    private int GetPreviousPlayerIndex()
    {
        if(CurrentUserGame is null) throw new InvalidOperationException("No player found");
        var currentIndex = GameUsers.IndexOf(CurrentUserGame);
        if (currentIndex < 0) throw new InvalidOperationException("Invalid Current Player Index");;
        
        var previousIndex = currentIndex == 0 ? GameUsers.Count - 1 : currentIndex - 1;
        if(previousIndex < 0) throw new InvalidOperationException("Invalid Previous Player Index");
        return previousIndex;
    }
}