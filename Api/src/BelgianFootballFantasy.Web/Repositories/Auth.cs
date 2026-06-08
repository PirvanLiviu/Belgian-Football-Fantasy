using Microsoft.EntityFrameworkCore;
using Api.Dtos;
using Api.Data;
using Api.Helpers;
using Api.Interfaces;
using Api.Mappers;

namespace Api.Repoistories;

public class AuthRepository : IAuth
{
  // Init
  private readonly FootballFantasyDb _context;

  public AuthRepository(FootballFantasyDb context)
  {
    _context = context;
  }

  private async Task<bool> UsernameExists(string username)
  {
    return await _context.Users.AnyAsync(u => u.Username == username);
  }


  public async Task<ApiResponse<LoginResponse>> Register(RegisterPayload body)
  {
    try
    {
      var IsValid = AuthValidator.Validate(body.Password, body.Formation);
      if (!IsValid.IsValid)
        return new ApiResponse<LoginResponse> { Success = false, Message = IsValid.Error, Data = null };
      if (await UsernameExists(body.Username))
        return new ApiResponse<LoginResponse> { Success = false, Message = "Username already exists", Data = null };

      var password = BCrypt.Net.BCrypt.HashPassword(body.Password);
      var user = AuthMapper.RegisterPayloadToUser(body, password);

      await _context.Users.AddAsync(user);
      await _context.SaveChangesAsync();

      return new ApiResponse<LoginResponse> { Success = true, Message = "User created", Data = null };
    } catch (Exception e) 
    {
      return new ApiResponse<LoginResponse> { Success = false, Message = e.Message, Data = null}; 
    }
  }
  
  public async Task<ApiResponse<LoginResponse>> Login(LoginPayload body)
  {
    return null;
  }
}
