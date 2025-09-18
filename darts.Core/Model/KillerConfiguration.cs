namespace darts.Core.Model;

public class KillerConfiguration : GameModeConfiguration
{
    public override Guid GameModeId => Guid.Parse("3D58E5C8-EFE6-4DC4-8BA9-7DBC9E8D718D");
    public int HitsToQualifyAsKiller { get; set; } = 5;
    public int Lives { get; set; } = 3; // Liczba żyć każdego gracza
    public bool RequireDoubleToKill { get; set; } = false; // Czy tylko podwójne trafiają przeciwnika
    public bool AutoAssignTargets { get; set; } = true; // Automatycznie przypisz numery celów dla graczy

    public override void Validate()
    {
        base.Validate();
        if (HitsToQualifyAsKiller <= 0)
            throw new ArgumentOutOfRangeException(nameof(HitsToQualifyAsKiller), "Liczba trafień do zostania Killerem musi być większa od zera");
        if (Lives <= 0)
            throw new ArgumentOutOfRangeException(nameof(Lives), "Liczba żyć musi być większa od zera");
    }
}