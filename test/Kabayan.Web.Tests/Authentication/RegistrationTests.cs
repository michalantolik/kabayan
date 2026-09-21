using Kabayan.Web.Tests.Fixtures;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Text.RegularExpressions;

namespace Kabayan.Web.Tests.Authentication;

public sealed class RegistrationTests(
    LaunchpadWebFactory factory)
    : IClassFixture<LaunchpadWebFactory>
{
    [Fact]
    public async Task Register_ValidCredentials_ShowsCreatedState()
    {
        using var client = CreateClient();

        var antiforgeryToken = await GetAntiforgeryTokenAsync(
            client);

        var response = await client.PostAsync(
            "/authentication/register",
            RegistrationForm(
                LaunchpadWebFactory.RegistrationEmail,
                LaunchpadWebFactory.ValidPassword,
                LaunchpadWebFactory.ValidPassword,
                antiforgeryToken));

        Assert.Equal(
            HttpStatusCode.Redirect,
            response.StatusCode);

        Assert.Equal(
            "/register?status=created",
            response.Headers.Location?.OriginalString);

        var resultPage = await client.GetAsync(
            response.Headers.Location);

        resultPage.EnsureSuccessStatusCode();

        var html = await resultPage.Content
            .ReadAsStringAsync();

        Assert.Contains(
            "class=\"registration-result\"",
            html);
    }

    [Fact]
    public async Task Register_PasswordMismatch_ShowsSafeError()
    {
        using var client = CreateClient();

        var antiforgeryToken = await GetAntiforgeryTokenAsync(
            client);

        var response = await client.PostAsync(
            "/authentication/register",
            RegistrationForm(
                LaunchpadWebFactory.RegistrationEmail,
                LaunchpadWebFactory.ValidPassword,
                "DifferentPassword1!",
                antiforgeryToken));

        Assert.Equal(
            HttpStatusCode.Redirect,
            response.StatusCode);

        var resultPage = await client.GetAsync(
            response.Headers.Location);

        resultPage.EnsureSuccessStatusCode();

        var html = await resultPage.Content
            .ReadAsStringAsync();

        Assert.Contains(
            "The passwords do not match.",
            html);
    }

    [Fact]
    public async Task Register_ApiFailure_ShowsRegistrationError()
    {
        using var client = CreateClient();

        var antiforgeryToken = await GetAntiforgeryTokenAsync(
            client);

        var response = await client.PostAsync(
            "/authentication/register",
            RegistrationForm(
                LaunchpadWebFactory.ValidEmail,
                LaunchpadWebFactory.ValidPassword,
                LaunchpadWebFactory.ValidPassword,
                antiforgeryToken));

        Assert.Equal(
            HttpStatusCode.Redirect,
            response.StatusCode);

        var resultPage = await client.GetAsync(
            response.Headers.Location);

        resultPage.EnsureSuccessStatusCode();

        var html = await resultPage.Content
            .ReadAsStringAsync();

        Assert.Contains(
            "Email is already registered.",
            html);
    }

    [Fact]
    public async Task Register_WithoutAntiforgeryToken_ReturnsBadRequest()
    {
        using var client = CreateClient();

        var response = await client.PostAsync(
            "/authentication/register",
            RegistrationForm(
                LaunchpadWebFactory.RegistrationEmail,
                LaunchpadWebFactory.ValidPassword,
                LaunchpadWebFactory.ValidPassword));

        Assert.Equal(
            HttpStatusCode.BadRequest,
            response.StatusCode);
    }

    private static FormUrlEncodedContent RegistrationForm(
        string email,
        string password,
        string confirmPassword,
        string? antiforgeryToken = null)
    {
        var values = new Dictionary<string, string>
        {
            ["email"] = email,
            ["password"] = password,
            ["confirmPassword"] = confirmPassword
        };

        if (!string.IsNullOrWhiteSpace(antiforgeryToken))
        {
            values["__RequestVerificationToken"] =
                antiforgeryToken;
        }

        return new FormUrlEncodedContent(values);
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

    private static async Task<string> GetAntiforgeryTokenAsync(
        HttpClient client)
    {
        var page = await client.GetAsync(
            "/register");

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
