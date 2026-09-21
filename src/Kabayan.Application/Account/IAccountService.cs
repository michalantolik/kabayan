namespace Kabayan.Application.Account;

/// <summary>
/// Provides account operations for application use cases.
/// </summary>
public interface IAccountService
{
    Task<AccountDto?> GetAccountAsync(
        Guid userId,
        CancellationToken cancellationToken = default);

    Task<DeleteAccountResult> DeleteAccountAsync(
        Guid userId,
        CancellationToken cancellationToken = default);
}

public sealed record AccountServiceError(
    string Code,
    string Description);

public sealed record DeleteAccountResult(
    bool Succeeded,
    IReadOnlyCollection<AccountServiceError> Errors)
{
    public static DeleteAccountResult Success()
        => new(
            true,
            []);

    public static DeleteAccountResult Failure(
        IEnumerable<AccountServiceError> errors)
        => new(
            false,
            errors.ToArray());
}
