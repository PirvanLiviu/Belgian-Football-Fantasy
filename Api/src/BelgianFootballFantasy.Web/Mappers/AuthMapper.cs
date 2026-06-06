using Api.Models;
using Api.Dtos;

namespace Api.Mappers;

public static class AuthMapper
{
  public static User RegisterPayloadToUser(RegisterPayload body, string password)
  {
    return new User 
    { 
      Username = body.Username,
      Email = body.Email,
      Formation = body.Formation,
      Password = password,
      Verified = false,
      Budget = 40,
      Points = 0
    };
  }
}
