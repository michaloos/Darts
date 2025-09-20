using System.Collections.Generic;
using System.Linq;
using darts.Core.Interface;
using darts.Core.Model;

namespace darts.Core.Mechanics
{
    public class DefaultMechanics : IGameMechanics
    {
        public int SetNumber { get; set; }

        public void Initialize(IList<UserGame> players, GameModeConfiguration configuration)
        {
            foreach (var p in players)
            {
                p.CurrentScore = 0;
            }
        }

        public void OnThrow(UserGame player, int score, int multiplier)
        {
            // Zachowanie zgodne z dotychczasowym UpdateShoots (sumujemy Score bez mnożnika)
            var current = player.CurrentScore ?? 0;
            player.CurrentScore = current + score;
            player.OnPropertyChanged(nameof(UserGame.CurrentScore));
        }

        public void Recalculate(UserGame player)
        {
            player.CurrentScore = player.Shoots.Sum(x => x.Score);
            player.OnPropertyChanged(nameof(UserGame.CurrentScore));
        }

        public void OnCancelThrow(UserGame player)
        {
             //todo
        }

        public bool CheckForWin(UserGame player)
        {
            return false;
        }
        
        public void StartNewSet(IList<UserGame> players, UserGame currentWinner)
        {
            
        }
    }
}
