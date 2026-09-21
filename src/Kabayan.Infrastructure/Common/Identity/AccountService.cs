using Kabayan.Application.Account;
using Kabayan.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Kabayan.Infrastructure.Common.Identity;

/// <summary>
/// Provides account operations using ASP.NET Core Identity.
/// </summary>
internal sealed class AccountService(
    KabayanDbContext dbContext,
    UserManager<ApplicationUser> userManager)
    : IAccountService
{
    public async Task<AccountDto?> GetAccountAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.Users
            .AsNoTracking()
            .Where(user => user.Id == userId)
            .Select(user => new AccountDto(
                user.Id,
                user.Email!))
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<DeleteAccountResult> DeleteAccountAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByIdAsync(
            userId.ToString());

        if (user is null)
        {
            return DeleteAccountResult.Failure(
            [
                new AccountServiceError(
                    "UserNotFound",
                    "User was not found.")
            ]);
        }

        var result = await userManager.DeleteAsync(user);

        if (result.Succeeded)
        {
            return DeleteAccountResult.Success();
        }

        return DeleteAccountResult.Failure(
            result.Errors.Select(error =>
                new AccountServiceError(
                    error.Code,
                    error.Description)));
    }
}
