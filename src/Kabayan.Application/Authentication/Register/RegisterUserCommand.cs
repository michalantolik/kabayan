namespace Kabayan.Application.Authentication.Register;

/// <summary>
/// Request to register a new user.
/// </summary>
public sealed record RegisterUserCommand(
    string Email,
    string Password);
