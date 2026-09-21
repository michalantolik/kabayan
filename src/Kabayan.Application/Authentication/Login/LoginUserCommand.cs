namespace Kabayan.Application.Authentication.Login;

/// <summary>
/// Credentials submitted to authenticate a user.
/// </summary>
public sealed record LoginUserCommand(
    string Email,
    string Password);
