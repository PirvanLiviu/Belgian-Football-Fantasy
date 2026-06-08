using System.ComponentModel.DataAnnotations;

namespace Api.Dtos;

public record RegisterPayload
{
  [EmailAddress]
  public string Email { get; set; }
  [MinLength(2, ErrorMessage = "Username must be at least 2 characters")]
  [MaxLength(20, ErrorMessage = "Username must be at most 20 characters")]
  public string Username { get; set; }
  public string Password { get; set; }
  public string Formation { get; set; }
}
