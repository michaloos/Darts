namespace darts.Core.Model;

public class UserGameShoot
{
    public required int? Score { get; set; }
    public required int ShootNumber { get; set; }
    public required int Round { get; set; }
    public required int Multiplier { get; set; }
    public required bool ApplyToScore { get; set; }

}