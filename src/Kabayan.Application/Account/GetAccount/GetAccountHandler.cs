namespace Kabayan.Application.Account.GetAccount;

/// <summary>
/// Handles requests for the current account.
/// </summary>
public sealed class GetAccountHandler(
    IAccountService accountService)
{
    public async Task<AccountDto?> HandleAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        return await accountService.GetAccountAsync(
            userId,
            cancellationToken);
    }
}
