using System.ComponentModel.DataAnnotations;

namespace Api.Dtos;

public record LoginPayload
{
  [EmailAddress]
  public string Email { get; set; }
  public string Password { get; set; }
}
