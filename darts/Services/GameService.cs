using System.Collections.ObjectModel;
using darts.Core.Interface;
using darts.Core.Model;
using darts.Core.Mechanics;

namespace darts.Services;

public class GameService : IGameService
{
    public required ObservableCollection<UserGame> GameUsers { get; set; }
    public UserGame? CurrentUserGame { get; set; }
    public required GameMode GameMode { get; set; }
    private bool _isNewRoundStarting = false;
    private IGameMechanics _mechanics = new DefaultMechanics();

    public void StartNewGame(GameMode gameMode, List<User> users, GameModeConfiguration configuration)
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
                SetWins = 0,
            });
        });
        CurrentUserGame =  GameUsers.First();
        CurrentUserGame!.CurrentPlayer = true;
        
        _mechanics = GameMode.CreateMechanics?.Invoke(configuration) ?? new DefaultMechanics();
        _mechanics.Initialize(GameUsers, configuration);
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

    public void UndoShoot()
    {
        if (CurrentUserGame is null) throw new InvalidOperationException("No player found");

        var previousIndex = GetPreviousPlayerIndex();
        if (GameUsers.ElementAt(previousIndex).Shoots.Count % 3 == 0 && GameUsers.ElementAt(previousIndex).Shoots.Any())
            MoveToThePreviousPlayer();
        
        _mechanics.OnCancelThrow(CurrentUserGame);
        UpdateShoots();
    }

    public void AddScore(int score, int multiplier)
    {
        if (CurrentUserGame is null) return;
        
        _mechanics.OnThrow(CurrentUserGame, score, multiplier);
        UpdateShoots();
        
        if(_mechanics.CheckForWin(CurrentUserGame))
        {
            _mechanics.StartNewSet(GameUsers, CurrentUserGame);
            MoveToTheNextPlayer();
            //todo start new round gam,e
            return;
        }
        
        var currentRoundShoots = CurrentUserGame.Shoots
            .Where(s => s.Round == CurrentUserGame.Round && s.SetNumber == _mechanics.SetNumber)
            .ToList();
        
        if (currentRoundShoots.Any() && !currentRoundShoots.Last().ApplyToScore)
        {
            MoveToTheNextPlayer();
            return;
        }
        
        if (currentRoundShoots.Count == 3)
            MoveToTheNextPlayer();
    }

    private void UpdateShoots()
    {
        var updatedShoots = new ObservableCollection<UserGameShoot>(CurrentUserGame!.Shoots);
        CurrentUserGame.Shoots = updatedShoots;
        _mechanics.Recalculate(CurrentUserGame);
        
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