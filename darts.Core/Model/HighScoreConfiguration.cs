namespace darts.Core.Model;

public class HighScoreConfiguration : GameModeConfiguration
{
    public override Guid GameModeId => Guid.Parse("66B95992-2771-4BD8-8CB6-EE36FD0507E5");
    public int TargetScore { get; set; } = 170; // Domyślny cel punktowy
    public int NumberOfRounds { get; set; } = 10; // Liczba rund w grze
    public bool AllowExceedingTargetScore { get; set; } = true; // Czy można przekroczyć cel punktowy

    public override void Validate()
    {
        base.Validate();
        if (TargetScore <= 0)
            throw new ArgumentOutOfRangeException(nameof(TargetScore), "Cel punktowy musi być większy od zera");
        if (NumberOfRounds <= 0)
            throw new ArgumentOutOfRangeException(nameof(NumberOfRounds), "Liczba rund musi być większa od zera");
    }
}