namespace darts.Model;

public class UserGame
{
    public required Guid Id { get; set; }
    public required User User { get; set; }
    public required int? Position { get; set; }
    public required List<UserGameShoot> Shoots { get; set; }
    public required int? CurrentScore { get; set; }
    public required bool CurrentPlayer { get; set; }
}