namespace Kabayan.Application.Common.Identity;

/// <summary>
/// Provides user identity operations for application use cases.
/// </summary>
public interface IIdentityService
{
    Task<Guid> CreateUserAsync(
        string email,
        string password,
        CancellationToken cancellationToken = default);

    Task<Guid?> AuthenticateUserAsync(
        string email,
        string password,
        CancellationToken cancellationToken = default);
}
