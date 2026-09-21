using Kabayan.Application.Authentication.Register;
using Kabayan.Application.Common.Identity;
using Microsoft.AspNetCore.Identity;

namespace Kabayan.Infrastructure.Common.Identity;

/// <summary>
/// Provides user identity operations using ASP.NET Core Identity.
/// </summary>
internal sealed class IdentityService(
    UserManager<ApplicationUser> userManager)
    : IIdentityService
{
    public async Task<Guid> CreateUserAsync(
        string email,
        string password,
        CancellationToken cancellationToken = default)
    {
        var user = new ApplicationUser
        {
            UserName = email,
            Email = email
        };

        var result = await userManager.CreateAsync(
            user,
            password);

        if (!result.Succeeded)
        {
            var errors = result.Errors
                .Select(error => error.Description)
                .ToArray();

            throw new UserRegistrationException(errors);
        }

        return user.Id;
    }

    public async Task<Guid?> AuthenticateUserAsync(
        string email,
        string password,
        CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByEmailAsync(email);

        if (user is null)
        {
            return null;
        }

        var passwordIsValid = await userManager.CheckPasswordAsync(
            user,
            password);

        return passwordIsValid
            ? user.Id
            : null;
    }
}
