namespace Kabayan.Api.Authentication.Login;

/// <summary>
/// Request to authenticate a user.
/// </summary>
public sealed record LoginUserRequest(
    string Email,
    string Password);
