using Kabayan.Web.Tests.Fixtures;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Text.RegularExpressions;

namespace Kabayan.Web.Tests.Authentication;

public sealed class CookieAuthenticationTests(
    LaunchpadWebFactory factory)
    : IClassFixture<LaunchpadWebFactory>
{
    [Fact]
    public async Task Home_Unauthenticated_RedirectsToLogin()
    {
        using var client = CreateClient();

        var response = await client.GetAsync("/");

        Assert.Equal(
            HttpStatusCode.Redirect,
            response.StatusCode);

        AssertLoginRedirect(
            response.Headers.Location);
    }

    [Fact]
    public async Task CurrentUser_Unauthenticated_RedirectsToLogin()
    {
        using var client = CreateClient();

        var response = await client.GetAsync(
            "/authentication/me");

        Assert.Equal(
            HttpStatusCode.Redirect,
            response.StatusCode);

        AssertLoginRedirect(
            response.Headers.Location);
    }

    [Fact]
    public async Task Login_ValidCredentials_CookieAuthenticatesApiRequest()
    {
        using var client = CreateClient();

        var antiforgeryToken = await GetAntiforgeryTokenAsync(
            client,
            "/login");

        var response = await client.PostAsync(
            "/authentication/login",
            new FormUrlEncodedContent(
                new Dictionary<string, string>
                {
                    ["email"] =
                        LaunchpadWebFactory.ValidEmail,

                    ["password"] =
                        LaunchpadWebFactory.ValidPassword,

                    ["returnUrl"] =
                        "/authentication/me",

                    ["__RequestVerificationToken"] =
                        antiforgeryToken
                }));

        Assert.Equal(
            HttpStatusCode.Redirect,
            response.StatusCode);

        Assert.Equal(
            "/authentication/me",
            response.Headers.Location?.OriginalString);

        var authenticationCookie = response.Headers
            .GetValues("Set-Cookie")
            .Single(value =>
                value.StartsWith(
                    "__Host-Kabayan.Authentication=",
                    StringComparison.Ordinal));

        Assert.Contains(
            "httponly",
            authenticationCookie.ToLowerInvariant());

        Assert.Contains(
            "secure",
            authenticationCookie.ToLowerInvariant());

        Assert.Contains(
            "samesite=lax",
            authenticationCookie.ToLowerInvariant());

        var currentUserResponse = await client.GetAsync(
            "/authentication/me");

        Assert.Equal(
            HttpStatusCode.OK,
            currentUserResponse.StatusCode);

        var currentUserHtml = await currentUserResponse.Content
            .ReadAsStringAsync();

        Assert.Contains(
            LaunchpadWebFactory.TestUserId.ToString(),
            currentUserHtml);
    }

    [Fact]
    public async Task Login_InvalidCredentials_RedirectsWithSafeError()
    {
        using var client = CreateClient();

        var antiforgeryToken = await GetAntiforgeryTokenAsync(
            client,
            "/login");

        var response = await client.PostAsync(
            "/authentication/login",
            new FormUrlEncodedContent(
                new Dictionary<string, string>
                {
                    ["email"] =
                        LaunchpadWebFactory.ValidEmail,

                    ["password"] = "wrong-password",

                    ["returnUrl"] =
                        "/authentication/me",

                    ["__RequestVerificationToken"] =
                        antiforgeryToken
                }));

        Assert.Equal(
            HttpStatusCode.Redirect,
            response.StatusCode);

        Assert.Equal(
            "/login?returnUrl=%2Fauthentication%2Fme&error=invalid-credentials",
            response.Headers.Location?.OriginalString);

        Assert.False(
            response.Headers.TryGetValues(
                "Set-Cookie",
                out var cookies) &&
            cookies.Any(value =>
                value.StartsWith(
                    "__Host-Kabayan.Authentication=",
                    StringComparison.Ordinal)));
    }

    [Fact]
    public async Task Login_ExternalReturnUrl_FallsBackToHome()
    {
        using var client = CreateClient();

        var antiforgeryToken = await GetAntiforgeryTokenAsync(
            client,
            "/login");

        var response = await client.PostAsync(
            "/authentication/login",
            new FormUrlEncodedContent(
                new Dictionary<string, string>
                {
                    ["email"] =
                        LaunchpadWebFactory.ValidEmail,

                    ["password"] =
                        LaunchpadWebFactory.ValidPassword,

                    ["returnUrl"] =
                        "https://example.com/steal-session",

                    ["__RequestVerificationToken"] =
                        antiforgeryToken
                }));

        Assert.Equal(
            HttpStatusCode.Redirect,
            response.StatusCode);

        Assert.Equal(
            "/",
            response.Headers.Location?.OriginalString);
    }

    [Fact]
    public async Task Login_WithoutAntiforgeryToken_ReturnsBadRequest()
    {
        using var client = CreateClient();

        var response = await client.PostAsync(
            "/authentication/login",
            new FormUrlEncodedContent(
                new Dictionary<string, string>
                {
                    ["email"] =
                        LaunchpadWebFactory.ValidEmail,

                    ["password"] =
                        LaunchpadWebFactory.ValidPassword
                }));

        Assert.Equal(
            HttpStatusCode.BadRequest,
            response.StatusCode);
    }

    [Fact]
    public async Task Logout_WithoutAntiforgeryToken_ReturnsBadRequest()
    {
        using var client = CreateClient();

        await SignInAsync(client);

        var response = await client.PostAsync(
            "/authentication/logout",
            new FormUrlEncodedContent(
                new Dictionary<string, string>()));

        Assert.Equal(
            HttpStatusCode.BadRequest,
            response.StatusCode);
    }

    [Fact]
    public async Task Logout_AuthenticatedSession_ClearsAccess()
    {
        using var client = CreateClient();

        await SignInAsync(client);

        var antiforgeryToken = await GetAntiforgeryTokenAsync(
            client,
            "/authentication/me");

        var logoutResponse = await client.PostAsync(
            "/authentication/logout",
            new FormUrlEncodedContent(
                new Dictionary<string, string>
                {
                    ["__RequestVerificationToken"] =
                        antiforgeryToken
                }));

        Assert.Equal(
            HttpStatusCode.Redirect,
            logoutResponse.StatusCode);

        Assert.Equal(
            "/login",
            logoutResponse.Headers.Location?.OriginalString);

        var currentUserResponse = await client.GetAsync(
            "/authentication/me");

        Assert.Equal(
            HttpStatusCode.Redirect,
            currentUserResponse.StatusCode);

        AssertLoginRedirect(
            currentUserResponse.Headers.Location);
    }

    private static void AssertLoginRedirect(
        Uri? location)
    {
        Assert.NotNull(location);

        var path = location.IsAbsoluteUri
            ? location.AbsolutePath
            : location.OriginalString.Split('?', 2)[0];

        Assert.Equal(
            "/login",
            path);
    }

    private HttpClient CreateClient()
    {
        return factory.CreateClient(
            new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
                BaseAddress = new Uri("https://localhost"),
                HandleCookies = true
            });
    }

    private static async Task SignInAsync(
        HttpClient client)
    {
        var antiforgeryToken = await GetAntiforgeryTokenAsync(
            client,
            "/login");

        var response = await client.PostAsync(
            "/authentication/login",
            new FormUrlEncodedContent(
                new Dictionary<string, string>
                {
                    ["email"] =
                        LaunchpadWebFactory.ValidEmail,

                    ["password"] =
                        LaunchpadWebFactory.ValidPassword,

                    ["__RequestVerificationToken"] =
                        antiforgeryToken
                }));

        Assert.Equal(
            HttpStatusCode.Redirect,
            response.StatusCode);

        Assert.Equal(
            "/",
            response.Headers.Location?.OriginalString);
    }

    private static async Task<string> GetAntiforgeryTokenAsync(
        HttpClient client,
        string path)
    {
        var page = await client.GetAsync(path);

        page.EnsureSuccessStatusCode();

        var html = await page.Content
            .ReadAsStringAsync();

        var match = Regex.Match(
            html,
            "name=\"__RequestVerificationToken\"[^>]*value=\"([^\"]+)\"");

        Assert.True(match.Success);

        return WebUtility.HtmlDecode(
            match.Groups[1].Value);
    }
}
