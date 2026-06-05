namespace Api.Models;

public class User
{
  public int Id { get; }
  public string Username { get; set; }
  public string Email { get; set; }
  public string Password { get; set; }
  public string Formation { get; set; }
  public int Budget { get; set; }
  public int Points { get; set; }
  public bool Verified { get; set; }
}
