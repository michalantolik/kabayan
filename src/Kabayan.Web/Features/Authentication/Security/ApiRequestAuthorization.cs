using System.Net.Http.Headers;
using System.Security.Claims;

namespace Kabayan.Web.Features.Authentication.Security;

/// <summary>
/// Adds the API bearer credential stored in the protected Web session.
/// </summary>
public static class ApiRequestAuthorization
{
    public static void AddBearerToken(
        HttpRequestMessage request,
        ClaimsPrincipal user)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(user);

        var accessToken = user
            .FindFirst(AuthenticationClaimTypes.AccessToken)?
            .Value;

        if (string.IsNullOrWhiteSpace(accessToken))
        {
            throw new InvalidOperationException(
                "The authenticated session does not contain an API access token.");
        }

        request.Headers.Authorization =
            new AuthenticationHeaderValue(
                "Bearer",
                accessToken);
    }
}
