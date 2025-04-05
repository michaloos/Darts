using System.Collections.ObjectModel;
using darts.Model;
using darts.Services.Interfaces;

namespace darts.Services;

public class GameService : IGameService
{
    public required ObservableCollection<UserGame> GameUsers { get; set; }
    public UserGame? CurrentUserGame { get; set; }
    public required GameMode GameMode { get; set; }
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
        CurrentUserGame.OnPropertyChanged(nameof(CurrentUserGame.VisibleShoots));
        var currentUserGameIndex = GameUsers.IndexOf(CurrentUserGame);
        if (currentUserGameIndex < 0) return;
        CurrentUserGame.CurrentPlayer = false;
        if(currentUserGameIndex == GameUsers.Count - 1)
        {
            CurrentUserGame = GameUsers.ElementAt(0);
            UpdatePositions();
        }
        else
        {
            CurrentUserGame = GameUsers.ElementAt(currentUserGameIndex + 1);
        }
        CurrentUserGame.CurrentPlayer = true;
        CurrentUserGame.OnPropertyChanged(nameof(CurrentUserGame.VisibleShoots));
        if (currentUserGameIndex != GameUsers.Count - 1) return;
        foreach (var gameUser in GameUsers)
            gameUser.Round++;
    }

    private void UpdatePositions()
    {
        var iteration = 1;
        foreach (var userGame in GameUsers.OrderByDescending(x => x.CurrentScore)) {
            userGame.Position = iteration;
            iteration++;
        }
    }

    public void UndoShoot(Guid userGameId)
    {
        throw new NotImplementedException();
    }

    public void AddScore(int score)
    {
        if (CurrentUserGame is null) return;
        CurrentUserGame.Shoots.Add(new UserGameShoot
        {
            Score = score,
            ShootNumber = CurrentUserGame.Shoots.Count + 1,
            Round = CurrentUserGame.Round,
        });
        var updatedShoots = new ObservableCollection<UserGameShoot>(CurrentUserGame.Shoots);
        CurrentUserGame.Shoots = updatedShoots;
        CurrentUserGame.CurrentScore = CurrentUserGame.Shoots.Sum(x => x.Score);
        CurrentUserGame.OnPropertyChanged(nameof(CurrentUserGame.VisibleShoots));
        var currentRound = CurrentUserGame.Round;
        CurrentUserGame.VisibleShoots = new ObservableCollection<UserGameShoot>(
            CurrentUserGame.Shoots.Where(s => s.Round == currentRound)
        );
        if(CurrentUserGame.Shoots.Count % 3 == 0)
            MoveToTheNextPlayer();
    }
}