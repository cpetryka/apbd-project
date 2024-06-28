using apbd_project.Model.Dto;

namespace apbd_project.Service;

public interface ISecurityService
{
    Task<AuthorizationResultDto> RegisterAsync(RegisterOrLoginUserDto registerOrLoginUserDto);
    Task<AuthorizationResultDto> LoginAsync(RegisterOrLoginUserDto registerOrLoginUserDto);
    Task<AuthorizationResultDto> RefreshTokenAsync(string refreshToken);
}