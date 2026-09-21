namespace Kabayan.Web.Features.Account;

/// <summary>
/// Account data used by the web application.
/// </summary>
public sealed record AccountDetails(
    Guid Id,
    string Email);
