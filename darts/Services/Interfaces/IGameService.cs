using System.Collections.ObjectModel;
using darts.Model;

namespace darts.Services.Interfaces;

public interface IGameService
{
    ObservableCollection<UserGame> GameUsers { get; set; }
    void StartNewGame(GameMode gameMode, List<User> users);
}