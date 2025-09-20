using System.Collections.Generic;
using darts.Core.Model;

namespace darts.Core.Interface
{
    public interface IGameMechanics
    {
        int SetNumber { get; set; }
        // Inicjalizacja stanu graczy zgodnie z konfiguracją trybu gry
        void Initialize(IList<UserGame> players, GameModeConfiguration configuration);

        // Reakcja na pojedynczy rzut (score, multiplier) bieżącego gracza
        void OnThrow(UserGame player, int score, int multiplier);

        // Przeliczenie bieżącego stanu gracza (np. po anulacji rzutu)
        void Recalculate(UserGame player);
        
        void OnCancelThrow(UserGame player);

        bool CheckForWin(UserGame player);
        void StartNewSet(IList<UserGame> players, UserGame currentWinner);
    }
}
