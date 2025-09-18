namespace darts.Core.Model;

public class LegsConfiguration : GameModeConfiguration
{
    public override Guid GameModeId => Guid.Parse("B23C5C17-230D-4ECF-986B-0D01B8E2F678");
    public int LegsToWin { get; set; } = 3; // Ilość legów potrzebnych do wygrania (np. Best of 5 = 3 legi)
    public int SetsToWin { get; set; } = 0; // Ilość setów potrzebnych do wygrania (0 = gra tylko na legi)
    public int LegsPerSet { get; set; } = 3; // Ilość legów potrzebnych do wygrania setu
    public bool AlternateStart { get; set; } = true; // Czy gracze naprzemiennie rozpoczynają legi

    public override void Validate()
    {
        base.Validate();
        if (LegsToWin <= 0 && SetsToWin <= 0)
            throw new ArgumentOutOfRangeException("Należy określić liczbę legów lub setów do wygrania");
        if (SetsToWin > 0 && LegsPerSet <= 0)
            throw new ArgumentOutOfRangeException(nameof(LegsPerSet), "Liczba legów w secie musi być większa od zera");
    }
}