using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using darts.Core.Interface;

namespace darts.Core.Model;

public class GameMode
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }


    public required int StartingScore { get; set; }
    public required bool RequiresDoubleOut { get; set; }
    public required bool UsesSpecificTargets { get; set; }
    public required bool HasScoringSystem { get; set; }
    public required bool Disabled { get; set; }

    public required Func<GameModeConfiguration> CreateDefaultConfiguration { get; set; }
    public GameModeConfiguration Configuration => CreateDefaultConfiguration();

    // Opcjonalna fabryka mechanik dla trybu gry
    public Func<GameModeConfiguration, IGameMechanics>? CreateMechanics { get; set; }

    [SetsRequiredMembers]
    public GameMode(Guid id, string name, string description, Func<GameModeConfiguration> defaultConfig,
        int startingScore = 0, bool requiresDoubleOut = false, bool usesSpecificTargets = false, bool hasScoringSystem = false, bool disabled = true)
    {
        Name = name;
        Description = description;
        StartingScore = startingScore;
        RequiresDoubleOut = requiresDoubleOut;
        UsesSpecificTargets = usesSpecificTargets;
        HasScoringSystem = hasScoringSystem;
        Disabled = disabled;
        CreateDefaultConfiguration = defaultConfig;
    }
}