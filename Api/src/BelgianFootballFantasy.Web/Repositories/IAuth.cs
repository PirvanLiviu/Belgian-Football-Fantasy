using Api.Dtos;

namespace Api.Interfaces;

public interface IAuth
{
  public Task<ApiResponse<LoginResponse>> Register(RegisterPayload body);
  public Task<ApiResponse<LoginResponse>> Login(LoginPayload body);
}
