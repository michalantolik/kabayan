using Kabayan.Api.Authentication.Login;
using Kabayan.Api.Authentication.Register;
using Kabayan.Application.Authentication.Login;
using Kabayan.Application.Authentication.Register;
using Kabayan.Application.Common.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kabayan.Api.Authentication;

/// <summary>
/// Provides authentication endpoints.
/// </summary>
[ApiController]
[Route("api/authentication")]
public sealed class AuthenticationController(
    RegisterUserHandler registerUserHandler,
    LoginUserHandler loginUserHandler,
    ICurrentUser currentUser)
    : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<Guid>> RegisterAsync(
        RegisterUserRequest request,
        CancellationToken cancellationToken)
    {
        var command = new RegisterUserCommand(
            request.Email,
            request.Password);

        var userId = await registerUserHandler.HandleAsync(
            command,
            cancellationToken);

        return Ok(userId);
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginUserResponse>> LoginAsync(
        LoginUserRequest request,
        CancellationToken cancellationToken)
    {
        var command = new LoginUserCommand(
            request.Email,
            request.Password);

        var result = await loginUserHandler.HandleAsync(
            command,
            cancellationToken);

        if (!result.Succeeded)
        {
            return Unauthorized();
        }

        var response = new LoginUserResponse(
            result.AccessToken!);

        return Ok(response);
    }

    [Authorize]
    [HttpGet("me")]
    public ActionResult<CurrentUserResponse> GetCurrentUser()
    {
        return Ok(
            new CurrentUserResponse(
                currentUser.UserId));
    }
}

/// <summary>
/// Current user data returned by the API.
/// </summary>
public sealed record CurrentUserResponse(
    Guid UserId);
