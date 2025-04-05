using System.Collections.ObjectModel;
using darts.Model;

namespace darts.Services.Interfaces;

public interface IGameService
{
    ObservableCollection<UserGame> GameUsers { get; set; }
    UserGame? CurrentUserGame { get; set; }
    void StartNewGame(GameMode gameMode, List<User> users);
    void MoveToTheNextPlayer();
    void UndoShoot(Guid userGameId);
    void AddScore(int score);
}