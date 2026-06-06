namespace Api.Models;

public class UserLeague
{
  public int Id { get; set; }
  public int UserId { get; set; }
  public User User { get; set; }
  public int LeagueId { get; set; }
  public League League { get; set; }
}
