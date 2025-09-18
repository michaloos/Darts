namespace darts.Core.Model;

public class ShanghaiConfiguration : GameModeConfiguration
{
    public override Guid GameModeId => Guid.Parse("98C579D9-A550-416F-88B1-C896C5ED62FA");
    public int StartingNumber { get; set; } = 1;
    public int EndingNumber { get; set; } = 7;
    public bool AllowInstantWin { get; set; } = true; // Czy zdobycie Shanghai (pojedynczy, podwójny i potrójny) daje natychmiastowe zwycięstwo
    public int PointsForSingle { get; set; } = 1;
    public int PointsForDouble { get; set; } = 2;
    public int PointsForTriple { get; set; } = 3;

    public override void Validate()
    {
        base.Validate();
        if (StartingNumber < 1 || StartingNumber > 20)
            throw new ArgumentOutOfRangeException(nameof(StartingNumber), "Numer początkowy musi być między 1 a 20");
        if (EndingNumber < StartingNumber || EndingNumber > 20)
            throw new ArgumentOutOfRangeException(nameof(EndingNumber), "Numer końcowy musi być między numerem początkowym a 20");
        if (PointsForSingle <= 0 || PointsForDouble <= 0 || PointsForTriple <= 0)
            throw new ArgumentOutOfRangeException("Punkty za trafienie muszą być większe od zera");
    }
}