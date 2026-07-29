using GearShare.Api.DTOs;

namespace GearShare.Api.Services;

public interface IAuthService
{
    Task<LoginResponseDto> LoginAsync(LoginRequestDto dto);
}