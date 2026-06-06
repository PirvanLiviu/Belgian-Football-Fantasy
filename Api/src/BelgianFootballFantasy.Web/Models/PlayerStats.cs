namespace Api.Models;

public class PlayerStats 
{
  public int PlayerId { get; set; }
  public Player Player { get; set; }
  public int MatchId { get; set; }
  public Match Match { get; set; }
  public int Goals { get; set; }
  public int Assists { get; set; }
  public int TacklesPct { get; set; }
  public int PassesPct { get; set; }
  public int Points { get; set; }
}
