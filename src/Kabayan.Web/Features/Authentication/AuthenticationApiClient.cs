using Kabayan.Web.Features.Authentication.Security;
using System.Net;
using System.Security.Claims;
using System.Text.Json;

namespace Kabayan.Web.Features.Authentication;

/// <summary>
/// Provides access to authentication-related Launchpad API operations.
/// </summary>
public sealed class AuthenticationApiClient(
    HttpClient httpClient)
{
    public async Task<RegistrationOperationResult> RegisterAsync(
        string email,
        string password,
        CancellationToken cancellationToken = default)
    {
        using var response = await httpClient.PostAsJsonAsync(
            "api/authentication/register",
            new
            {
                Email = email,
                Password = password
            },
            cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            return RegistrationOperationResult.Success();
        }

        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            var errors = await ReadRegistrationErrorsAsync(
                response,
                cancellationToken);

            return RegistrationOperationResult.Failure(errors);
        }

        return RegistrationOperationResult.Failure(
        [
            "The account could not be created."
        ]);
    }

    public async Task<LoginOperationResult> LoginAsync(
        string email,
        string password,
        CancellationToken cancellationToken = default)
    {
        using var response = await httpClient.PostAsJsonAsync(
            "api/authentication/login",
            new
            {
                Email = email,
                Password = password
            },
            cancellationToken);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            return LoginOperationResult.Failure();
        }

        response.EnsureSuccessStatusCode();

        var loginResponse = await response.Content
            .ReadFromJsonAsync<LoginResponse>(
                cancellationToken);

        if (string.IsNullOrWhiteSpace(
                loginResponse?.AccessToken))
        {
            throw new InvalidOperationException(
                "The authentication API returned an empty access token.");
        }

        return LoginOperationResult.Success(
            loginResponse.AccessToken);
    }

    public async Task<CurrentUserResponse> GetCurrentUserAsync(
        ClaimsPrincipal user,
        CancellationToken cancellationToken = default)
    {
        using var request = new HttpRequestMessage(
            HttpMethod.Get,
            "api/authentication/me");

        ApiRequestAuthorization.AddBearerToken(
            request,
            user);

        using var response = await httpClient.SendAsync(
            request,
            cancellationToken);

        response.EnsureSuccessStatusCode();

        return await response.Content
            .ReadFromJsonAsync<CurrentUserResponse>(
                cancellationToken)
            ?? throw new InvalidOperationException(
                "The authentication API returned an empty current-user response.");
    }

    private static async Task<IReadOnlyCollection<string>>
        ReadRegistrationErrorsAsync(
            HttpResponseMessage response,
            CancellationToken cancellationToken)
    {
        try
        {
            await using var contentStream =
                await response.Content.ReadAsStreamAsync(
                    cancellationToken);

            using var document = await JsonDocument.ParseAsync(
                contentStream,
                cancellationToken: cancellationToken);

            if (document.RootElement.TryGetProperty(
                    "errors",
                    out var errorsElement) &&
                errorsElement.ValueKind == JsonValueKind.Array)
            {
                var errors = errorsElement
                    .EnumerateArray()
                    .Where(error =>
                        error.ValueKind == JsonValueKind.String)
                    .Select(error => error.GetString())
                    .Where(error =>
                        !string.IsNullOrWhiteSpace(error))
                    .Select(error => error!)
                    .ToArray();

                if (errors.Length > 0)
                {
                    return errors;
                }
            }
        }
        catch (JsonException)
        {
        }

        return
        [
            "The account could not be created."
        ];
    }

    private sealed record LoginResponse(
        string AccessToken);
}
