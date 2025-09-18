namespace darts.Core.Model;

public class Bob27Configuration : GameModeConfiguration
{
    public override Guid GameModeId => Guid.Parse("97544A51-2217-449A-BA61-D83BA90664CD");
    public int StartingPoints { get; set; } = 27;
    public bool GameOverOnNegativeScore { get; set; } = false; // Czy gra kończy się, gdy gracz osiągnie wynik ujemny
    public bool IncludeBullseye { get; set; } = true; // Czy uwzględnić bullseye jako ostatni cel
    public int DartsPerDouble { get; set; } = 3; // Liczba rzutów na każde podwójne pole

    public override void Validate()
    {
        base.Validate();
        if (StartingPoints <= 0)
            throw new ArgumentOutOfRangeException(nameof(StartingPoints), "Punkty startowe muszą być większe od zera");
        if (DartsPerDouble <= 0)
            throw new ArgumentOutOfRangeException(nameof(DartsPerDouble), "Liczba rzutów na podwójne pole musi być większa od zera");
    }
}