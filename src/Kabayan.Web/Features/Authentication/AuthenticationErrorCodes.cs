namespace Kabayan.Web.Features.Authentication;

/// <summary>
/// Defines the small set of authentication errors owned by the Web login flow.
/// </summary>
public static class AuthenticationErrorCodes
{
    public const string InvalidCredentials =
        "invalid-credentials";

    public const string Required =
        "required";

    public const string ServiceUnavailable =
        "service-unavailable";
}
