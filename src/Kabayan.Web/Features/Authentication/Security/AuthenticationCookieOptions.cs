namespace Kabayan.Web.Features.Authentication.Security;

/// <summary>
/// Configuration for the Web authentication cookie.
/// </summary>
public sealed class AuthenticationCookieOptions
{
    public const string SectionName = "Authentication";

    /// <summary>
    /// Lifetime of the Web authentication cookie.
    /// Keep this aligned with the API access-token lifetime.
    /// </summary>
    public int AuthenticationCookieExpirationInMinutes { get; init; }
}
