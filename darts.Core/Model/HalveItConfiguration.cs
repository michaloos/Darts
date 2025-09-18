namespace darts.Core.Model;

public class HalveItConfiguration : GameModeConfiguration
{
    public override Guid GameModeId => Guid.Parse("6088201C-6649-4904-B24C-CEFBEC355F24");
    public List<int> TargetNumbers { get; set; } = new() { 15, 16, 17, 18, 19, 20, 25 };
    public bool IncludeDoubles { get; set; } = true; // Czy dodać rundę na podwójne pola
    public bool IncludeTriples { get; set; } = true; // Czy dodać rundę na potrójne pola
    public bool StrictRules { get; set; } = true; // Czy wymagane jest trafienie w określony cel w danej rundzie

    public override void Validate()
    {
        base.Validate();
        if (TargetNumbers == null || !TargetNumbers.Any())
            throw new ArgumentException("Lista numerów celów nie może być pusta", nameof(TargetNumbers));
        foreach (var number in TargetNumbers)
        {
            if ((number < 1 || number > 20) && number != 25)
                throw new ArgumentOutOfRangeException(nameof(TargetNumbers), $"Nieprawidłowy numer celu: {number}");
        }
    }
}