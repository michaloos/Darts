namespace darts.Core.Model;

public class CricketConfiguration : GameModeConfiguration
{
    public override Guid GameModeId => Guid.Parse("237587C5-3223-4B1C-AECC-FADA6FCF10DE");
    public bool UseCutThroatRules { get; set; } = false;
    public bool EnableScoring { get; set; } = true;
    public List<int> TargetNumbers { get; set; } = new() { 15, 16, 17, 18, 19, 20, 25 }; 
    public int HitsToClose { get; set; } = 3; // Ile trafień potrzeba, żeby zamknąć liczbę
    public bool RequireAllNumbersClosed { get; set; } = true; // Czy wszytkie liczby muszą być zamknięte, aby wygrać
    public bool AllowRandomNumbers { get; set; } = false; // Czy liczby mogą być losowo wybrane

    public override void Validate()
    {
        base.Validate();
        if (TargetNumbers == null || !TargetNumbers.Any())
            throw new ArgumentException("Lista numerów celów nie może być pusta", nameof(TargetNumbers));
        if (HitsToClose <= 0)
            throw new ArgumentOutOfRangeException(nameof(HitsToClose), "Liczba trafień do zamknięcia musi być większa od zera");
        foreach (var number in TargetNumbers)
        {
            if ((number < 1 || number > 20) && number != 25)
                throw new ArgumentOutOfRangeException(nameof(TargetNumbers), $"Nieprawidłowy numer celu: {number}");
        }
    }
}