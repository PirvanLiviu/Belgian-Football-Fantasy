namespace Api.Models;

public class Gameweek
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public int Points { get; set; }
    public int Gameweek { get; set; }
}
