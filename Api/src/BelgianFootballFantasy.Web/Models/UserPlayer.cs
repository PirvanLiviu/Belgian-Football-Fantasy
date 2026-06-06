namespace Api.Models;

public class UserPlayer
{
  public int UserId { get; set; }
  public User User { get; set; }
  public int PlayerId { get; set; }
  public Player Player { get; set; }
  public bool Captain { get; set; }
  public bool Starting { get; set; }
}
