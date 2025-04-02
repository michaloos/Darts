using System.Collections.ObjectModel;
using darts.Model;
using darts.Services.Interfaces;

namespace darts.Services;

public class GameService : IGameService
{
    public required ObservableCollection<UserGame> GameUsers { get; set; }
    public required GameMode GameMode { get; set; }
    public void StartNewGame(GameMode gameMode, List<User> users)
    {
        GameUsers = new ObservableCollection<UserGame>();
        GameMode = gameMode;
        users.ForEach(user =>
        {
            GameUsers.Add(new UserGame
            {
                Id = Guid.NewGuid(),
                User = user,
                Position = null,
                Shoots = new List<UserGameShoot>(),
                CurrentScore = null,
                CurrentPlayer = users.First().Equals(user),
            });
        });
    }
}