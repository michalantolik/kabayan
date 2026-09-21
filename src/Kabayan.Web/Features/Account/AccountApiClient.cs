using Kabayan.Web.Features.Authentication.Security;
using System.Net.Http.Json;
using System.Security.Claims;

namespace Kabayan.Web.Features.Account;

/// <summary>
/// Provides access to account-related Launchpad API operations.
/// </summary>
public sealed class AccountApiClient(
    HttpClient httpClient)
{
    public async Task<AccountDetails> GetAccountAsync(
        ClaimsPrincipal user,
        CancellationToken cancellationToken = default)
    {
        using var request = new HttpRequestMessage(
            HttpMethod.Get,
            "api/account");

        ApiRequestAuthorization.AddBearerToken(
            request,
            user);

        using var response = await httpClient.SendAsync(
            request,
            cancellationToken);

        response.EnsureSuccessStatusCode();

        return await response.Content
            .ReadFromJsonAsync<AccountDetails>(
                cancellationToken)
            ?? throw new InvalidOperationException(
                "The account API returned an empty account response.");
    }

    public async Task<bool> DeleteAccountAsync(
        ClaimsPrincipal user,
        CancellationToken cancellationToken = default)
    {
        using var request = new HttpRequestMessage(
            HttpMethod.Delete,
            "api/account");

        ApiRequestAuthorization.AddBearerToken(
            request,
            user);

        using var response = await httpClient.SendAsync(
            request,
            cancellationToken);

        return response.IsSuccessStatusCode;
    }
}
