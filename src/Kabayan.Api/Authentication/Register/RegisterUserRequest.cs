namespace Kabayan.Api.Authentication.Register;

/// <summary>
/// Request to register a new user.
/// </summary>
public sealed record RegisterUserRequest(
    string Email,
    string Password);
