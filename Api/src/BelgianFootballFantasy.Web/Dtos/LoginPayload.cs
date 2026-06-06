using System.ComponentModel.DataAnnotations;

namespace Api.Dtos;

public class LoginPayload
{
  [EmailAddress]
  public string Email { get; set; }
  public string Password { get; set; }
}
