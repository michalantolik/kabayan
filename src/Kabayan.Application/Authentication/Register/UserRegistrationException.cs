namespace Kabayan.Application.Authentication.Register;

/// <summary>
/// Represents a user registration failure.
/// </summary>
public sealed class UserRegistrationException(
    IReadOnlyCollection<string> errors)
    : Exception("User registration failed.")
{
    public IReadOnlyCollection<string> Errors { get; } = errors;
}
