using Kabayan.Application.Common.Identity;

namespace Kabayan.Application.Authentication.Register;

/// <summary>
/// Handles user registration.
/// </summary>
public sealed class RegisterUserHandler(
    IIdentityService identityService)
{
    public async Task<Guid> HandleAsync(
        RegisterUserCommand command,
        CancellationToken cancellationToken = default)
    {
        return await identityService.CreateUserAsync(
            command.Email,
            command.Password,
            cancellationToken);
    }
}
