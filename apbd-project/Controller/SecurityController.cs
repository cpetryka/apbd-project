using apbd_project.Model.Dto;
using apbd_project.Service;
using Microsoft.AspNetCore.Mvc;

namespace apbd_project.Controller;

[Route("api/security")]
public class SecurityController : ControllerBase
{
    private ISecurityService _securityService;

    public SecurityController(ISecurityService securityService)
    {
        _securityService = securityService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterOrLoginUserDto registerOrLoginUserDto)
    {
        var authorizationResult = await _securityService.RegisterAsync(registerOrLoginUserDto);

        if (!authorizationResult.Success)
        {
            return BadRequest(authorizationResult.Errors);
        }

        return Ok(authorizationResult);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(RegisterOrLoginUserDto registerOrLoginUserDto)
    {
        var authorizationResult = await _securityService.LoginAsync(registerOrLoginUserDto);

        if (!authorizationResult.Success)
        {
            return BadRequest(authorizationResult.Errors);
        }

        return Ok(new { AccessToken = authorizationResult.AccessToken, RefreshToken = authorizationResult.RefreshToken });
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(string refreshToken)
    {
        var authorizationResult = await _securityService.RefreshTokenAsync(refreshToken);

        if (!authorizationResult.Success)
        {
            return BadRequest(authorizationResult.Errors);
        }

        return Ok(new { AccessToken = authorizationResult.AccessToken });
    }
}