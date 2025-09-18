namespace darts.Core.Model;

public class AroundTheClockConfiguration : GameModeConfiguration
{
    public override Guid GameModeId => Guid.Parse("246ED1BB-D57A-4778-B062-2B7445E356BD");
    public bool IncludeBullseye { get; set; } = true;
    public bool AllowAnySegment { get; set; } = false; // Jeśli false, wymagany jest tylko pojedynczy segment
    public int StartingNumber { get; set; } = 1;
    public int EndingNumber { get; set; } = 20;

    public override void Validate()
    {
        base.Validate();
        if (StartingNumber < 1 || StartingNumber > 20)
            throw new ArgumentOutOfRangeException(nameof(StartingNumber), "Numer początkowy musi być między 1 a 20");
        if (EndingNumber < StartingNumber || EndingNumber > 20)
            throw new ArgumentOutOfRangeException(nameof(EndingNumber), "Numer końcowy musi być między numerem początkowym a 20");
    }
}