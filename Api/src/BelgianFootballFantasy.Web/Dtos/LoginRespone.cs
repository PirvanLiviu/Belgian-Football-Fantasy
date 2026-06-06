namespace Api.Dtos;

public class LoginResponse
{
  public string Token { get; set; }
  public string Username { get; set; }
  public string Email { get; set; }
  public string Formation { get; set; }
  public string Verified { get; set; }
}
