using System.Collections.Generic;
using darts.Core.Interface;
using darts.Core.Model;

namespace darts.Core.Mechanics
{
    public class StandardX01Mechanics : IGameMechanics
    {
        private StandardX01Configuration? _config;

        public int SetNumber { get; set; }

        public void Initialize(IList<UserGame> players, GameModeConfiguration configuration)
        {
            _config = configuration as StandardX01Configuration
                      ?? throw new ArgumentException("Invalid configuration type for StandardX01Mechanics");

            foreach (var p in players)
            {
                p.CurrentScore = _config.StartingScore;
                p.CurrentSetNumber = SetNumber;
            }
        }

        public void OnThrow(UserGame player, int score, int multiplier)
        {
            if (_config is null) return;

            var delta = score * multiplier;
            var current = player.CurrentScore ?? _config.StartingScore;
            
            var roundCount = player.Shoots.Count(s => s.Round == player.Round && s.SetNumber == SetNumber);

            if (_config.DoubleOut)
            {
                if ((current == multiplier * score && multiplier == 2) || (score == 50 && current == score))
                {
                    CreateShoot(player, delta, multiplier, roundCount + 1, true, SetNumber);
                    player.CurrentScore = current - delta;
                    player.OnPropertyChanged(nameof(UserGame.CurrentScore));
                    return;
                }
            }
            else
            {
                if (current == delta)
                {
                    CreateShoot(player, delta, multiplier, roundCount + 1, true, SetNumber);
                    player.CurrentScore = current - delta;
                    player.OnPropertyChanged(nameof(UserGame.CurrentScore));
                    return;
                }
            }
            
            if (delta >= current || current - delta == 1)
            {
                CreateShoot(player, delta, multiplier, roundCount + 1, false, SetNumber);
                return;
            }
            
            CreateShoot(player, delta, multiplier, roundCount + 1, true, SetNumber);

            player.CurrentScore = current - delta;
            player.OnPropertyChanged(nameof(UserGame.CurrentScore));
        }

        public bool CheckForWin(UserGame player) => player.CurrentScore == 0;

        public void Recalculate(UserGame player)
        {
            if (_config is null) return;
            player.CurrentScore = _config.StartingScore - player.Shoots
                                      .Where(x => x.ApplyToScore && x.SetNumber == SetNumber)
                                      .Sum(x => x.Score);
        }

        public void OnCancelThrow(UserGame player)
        {
            if(player.Shoots.Any())
                player.Shoots.RemoveAt(player.Shoots.Count - 1);
        }
        
        public void StartNewSet(IList<UserGame> players, UserGame currentWinner)
        {
            if (_config is null) return;
            
            SetNumber++;
            currentWinner.SetWins++;

            foreach (var userGame in players)
            {
                userGame.CurrentScore = _config.StartingScore;
                userGame.Round = 1;
                userGame.CurrentSetNumber = SetNumber;
            }
        }

        private static void CreateShoot(UserGame player, int? score, int multiplier, int roundCount, bool applyToScore, int setNumber)
        {
            player.Shoots.Add(new UserGameShoot
            {
                Score = score,
                ShootNumber = roundCount,
                Round = player.Round,
                Multiplier = multiplier,
                ApplyToScore = applyToScore,
                SetNumber = setNumber,
            });
        }
    }
}
