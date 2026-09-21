using Kabayan.Api.Authentication;
using Kabayan.Api.Authentication.Login;
using Kabayan.Api.Authentication.Register;
using Kabayan.Api.Tests.Fixtures;
using Kabayan.Infrastructure.Common.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace Kabayan.Api.Tests.Authentication;

public sealed class AuthenticationTests(
    LaunchpadApiFactory factory)
    : IClassFixture<LaunchpadApiFactory>
{
    private const string ValidPassword =
        "ValidPassword1!";

    [Fact]
    public async Task Register_ValidRequest_CreatesUser()
    {
        var email = CreateUniqueEmail();
        using var client = CreateClient();

        var response = await client.PostAsJsonAsync(
            "/api/authentication/register",
            new RegisterUserRequest(
                email,
                ValidPassword));

        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);

        var userId =
            await response.Content.ReadFromJsonAsync<Guid>();

        Assert.NotEqual(
            Guid.Empty,
            userId);

        using var scope =
            factory.Services.CreateScope();

        var userManager =
            scope.ServiceProvider
                .GetRequiredService<UserManager<ApplicationUser>>();

        var user =
            await userManager.FindByEmailAsync(email);

        Assert.NotNull(user);
        Assert.Equal(
            userId,
            user.Id);
    }

    [Fact]
    public async Task Register_DuplicateEmail_ReturnsBadRequestProblemDetails()
    {
        var email = CreateUniqueEmail();
        using var client = CreateClient();

        var firstResponse = await client.PostAsJsonAsync(
            "/api/authentication/register",
            new RegisterUserRequest(
                email,
                ValidPassword));

        firstResponse.EnsureSuccessStatusCode();

        var response = await client.PostAsJsonAsync(
            "/api/authentication/register",
            new RegisterUserRequest(
                email,
                ValidPassword));

        Assert.Equal(
            HttpStatusCode.BadRequest,
            response.StatusCode);
        using var document =
            JsonDocument.Parse(
                await response.Content.ReadAsStringAsync());

        var root = document.RootElement;

        Assert.Equal(
            StatusCodes.Status400BadRequest,
            root.GetProperty("status").GetInt32());
        Assert.Equal(
            "User registration failed.",
            root.GetProperty("title").GetString());
        Assert.NotEqual(
            0,
            root.GetProperty("errors").GetArrayLength());
    }

    [Fact]
    public async Task Login_InvalidCredentials_ReturnsUnauthorized()
    {
        var email = CreateUniqueEmail();
        using var client = CreateClient();

        await RegisterUserAsync(
            client,
            email,
            ValidPassword);

        var response = await client.PostAsJsonAsync(
            "/api/authentication/login",
            new LoginUserRequest(
                email,
                "WrongPassword1!"));

        Assert.Equal(
            HttpStatusCode.Unauthorized,
            response.StatusCode);
    }

    [Fact]
    public async Task Login_ValidCredentials_TokenAuthenticatesCurrentUser()
    {
        var email = CreateUniqueEmail();
        using var client = CreateClient();

        var expectedUserId = await RegisterUserAsync(
            client,
            email,
            ValidPassword);

        var loginResponse = await client.PostAsJsonAsync(
            "/api/authentication/login",
            new LoginUserRequest(
                email,
                ValidPassword));

        Assert.Equal(
            HttpStatusCode.OK,
            loginResponse.StatusCode);

        var login =
            await loginResponse.Content
                .ReadFromJsonAsync<LoginUserResponse>();

        Assert.NotNull(login);
        Assert.False(
            string.IsNullOrWhiteSpace(
                login.AccessToken));

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(
                "Bearer",
                login.AccessToken);

        var currentUserResponse =
            await client.GetAsync(
                "/api/authentication/me");

        Assert.Equal(
            HttpStatusCode.OK,
            currentUserResponse.StatusCode);

        var currentUser =
            await currentUserResponse.Content
                .ReadFromJsonAsync<CurrentUserResponse>();

        Assert.NotNull(currentUser);
        Assert.Equal(
            expectedUserId,
            currentUser.UserId);
    }

    [Fact]
    public async Task GetCurrentUser_Unauthenticated_ReturnsUnauthorized()
    {
        using var client = CreateClient();

        var response = await client.GetAsync(
            "/api/authentication/me");

        Assert.Equal(
            HttpStatusCode.Unauthorized,
            response.StatusCode);
    }

    [Fact]
    public async Task GetCurrentUser_TokenWithEmptyUserId_ReturnsUnauthorized()
    {
        using var client = CreateClient();

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(
                "Bearer",
                CreateToken(Guid.Empty.ToString()));

        var response = await client.GetAsync(
            "/api/authentication/me");

        Assert.Equal(
            HttpStatusCode.Unauthorized,
            response.StatusCode);
    }

    private HttpClient CreateClient()
    {
        return factory.CreateClient(
            new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
                BaseAddress = new Uri("https://localhost")
            });
    }

    private static async Task<Guid> RegisterUserAsync(
        HttpClient client,
        string email,
        string password)
    {
        var response = await client.PostAsJsonAsync(
            "/api/authentication/register",
            new RegisterUserRequest(
                email,
                password));

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<Guid>();
    }

    private static string CreateUniqueEmail()
    {
        return $"user-{Guid.NewGuid():N}@example.com";
    }

    private static string CreateToken(
        string subject)
    {
        var now = DateTime.UtcNow;

        var credentials =
            new SigningCredentials(
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(
                        LaunchpadApiFactory.JwtSigningKey)),
                SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: LaunchpadApiFactory.JwtIssuer,
            audience: LaunchpadApiFactory.JwtAudience,
            claims:
            [
                new Claim(
                    JwtRegisteredClaimNames.Sub,
                    subject)
            ],
            notBefore: now,
            expires: now.AddMinutes(5),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler()
            .WriteToken(token);
    }
}
