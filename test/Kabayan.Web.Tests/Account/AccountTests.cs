using Kabayan.Web.Tests.Fixtures;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Text.RegularExpressions;

namespace Kabayan.Web.Tests.Account;

public sealed class AccountTests(
    LaunchpadWebFactory factory)
    : IClassFixture<LaunchpadWebFactory>
{
    [Fact]
    public async Task Account_Unauthenticated_RedirectsToLogin()
    {
        using var client = CreateClient();

        var response = await client.GetAsync(
            "/account");

        Assert.Equal(
            HttpStatusCode.Redirect,
            response.StatusCode);

        AssertLoginRedirect(
            response.Headers.Location);
    }

    [Fact]
    public async Task Account_Authenticated_ShowsCurrentAccount()
    {
        using var client = CreateClient();

        await SignInAsync(client);

        var response = await client.GetAsync(
            "/account");

        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);

        var html = await response.Content
            .ReadAsStringAsync();

        Assert.Contains(
            LaunchpadWebFactory.ValidEmail,
            html);
    }

    [Fact]
    public async Task AccountDeleted_WithoutAntiforgeryToken_ReturnsBadRequest()
    {
        using var client = CreateClient();

        await SignInAsync(client);

        var response = await client.PostAsync(
            "/account/deleted",
            new FormUrlEncodedContent(
                new Dictionary<string, string>()));

        Assert.Equal(
            HttpStatusCode.BadRequest,
            response.StatusCode);
    }

    [Fact]
    public async Task AccountDeleted_AuthenticatedSession_ClearsAccess()
    {
        using var client = CreateClient();

        await SignInAsync(client);

        var antiforgeryToken =
            await GetAntiforgeryTokenAsync(
                client,
                "/login");

        var response = await client.PostAsync(
            "/account/deleted",
            new FormUrlEncodedContent(
                new Dictionary<string, string>
                {
                    ["__RequestVerificationToken"] =
                        antiforgeryToken
                }));

        Assert.Equal(
            HttpStatusCode.Redirect,
            response.StatusCode);

        Assert.Equal(
            "/login?status=account-deleted",
            response.Headers.Location?.OriginalString);

        var accountResponse = await client.GetAsync(
            "/account");

        Assert.Equal(
            HttpStatusCode.Redirect,
            accountResponse.StatusCode);

        AssertLoginRedirect(
            accountResponse.Headers.Location);
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
        var antiforgeryToken =
            await GetAntiforgeryTokenAsync(
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
                        "/account",

                    ["__RequestVerificationToken"] =
                        antiforgeryToken
                }));

        Assert.Equal(
            HttpStatusCode.Redirect,
            response.StatusCode);

        Assert.Equal(
            "/account",
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
}
