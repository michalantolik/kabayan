namespace Kabayan.Application.Account.DeleteAccount;

/// <summary>
/// Handles deletion of the current account.
/// </summary>
public sealed class DeleteAccountHandler(
    IAccountService accountService)
{
    public async Task<DeleteAccountResult> HandleAsync(
        DeleteAccountCommand command,
        CancellationToken cancellationToken = default)
    {
        return await accountService.DeleteAccountAsync(
            command.UserId,
            cancellationToken);
    }
}
