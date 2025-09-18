namespace darts.Core.Model;

public class GotchaConfiguration : GameModeConfiguration
{
    public override Guid GameModeId => Guid.Parse("59E102BA-719D-4A1A-8271-BE5A637792C1");
    public int TargetScore { get; set; } = 250; // Cel punktowy do osiągnięcia
    public bool BustRule { get; set; } = true; // Czy przekroczenie celu cofa do poprzedniego wyniku
    public bool CanAttackOthers { get; set; } = true; // Czy można atakować innych graczy
    public bool RequireExactScore { get; set; } = true; // Czy wymagane jest dokładne trafienie celu

    public override void Validate()
    {
        base.Validate();
        if (TargetScore <= 0)
            throw new ArgumentOutOfRangeException(nameof(TargetScore), "Cel punktowy musi być większy od zera");
    }
}