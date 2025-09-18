namespace darts.Core.Model;

public class StandardX01Configuration : GameModeConfiguration
{
    public override Guid GameModeId => Guid.Parse("9EED0709-B22F-4F78-8DAC-FE84675505A3");
    public int StartingScore { get; set; } = 501;
    public bool DoubleOut { get; set; } = true;
    public bool DoubleIn { get; set; } = false;
    public int NumberOfLegs { get; set; } = 3; // Liczba legów w meczu (Best of X)
    public bool MasterOut { get; set; } = false; // Czy zakończenie wymaga podwójnego lub potrójnego pola

    public override void Validate()
    {
        base.Validate();
        if (StartingScore <= 0)
            throw new ArgumentOutOfRangeException(nameof(StartingScore), "Punkty startowe muszą być większe od zera");
        if (StartingScore > 1001) // Praktyczne ograniczenie
            throw new ArgumentOutOfRangeException(nameof(StartingScore), "Punkty startowe nie mogą przekraczać 1001");
        if (NumberOfLegs <= 0)
            throw new ArgumentOutOfRangeException(nameof(NumberOfLegs), "Liczba legów musi być większa od zera");
        if (DoubleOut && MasterOut)
            throw new ArgumentException("Nie można jednocześnie używać DoubleOut i MasterOut");
    }
    
    public void Deconstruct(out int startingScore, out bool doubleOut, out bool doubleIn,
        out int numberOfLegs, out bool masterOut)
    {
        startingScore = StartingScore;
        doubleOut = DoubleOut;
        doubleIn = DoubleIn;
        numberOfLegs = NumberOfLegs;
        masterOut = MasterOut;
    }

}