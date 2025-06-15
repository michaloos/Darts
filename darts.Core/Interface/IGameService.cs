using System.Collections.ObjectModel;
using darts.Core.Model;

namespace darts.Core.Interface;

public interface IGameService
{
    ObservableCollection<UserGame> GameUsers { get; set; }
    UserGame? CurrentUserGame { get; set; }
    void StartNewGame(GameMode gameMode, List<User> users);
    void EndGame();
    void MoveToTheNextPlayer();
    void MoveToThePreviousPlayer();
    void UndoShoot();
    void AddScore(int score, int multiplier);
}