using Kabayan.Api.Authentication.Login;
using Kabayan.Api.Authentication.Register;
using Kabayan.Api.Tests.Fixtures;
using Kabayan.Application.Account;
using Kabayan.Infrastructure.Common.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Kabayan.Api.Tests.Account;

public sealed class AccountTests(
    LaunchpadApiFactory factory)
    : IClassFixture<LaunchpadApiFactory>
{
    private const string ValidPassword =
        "ValidPassword1!";

    [Fact]
    public async Task Get_Unauthenticated_ReturnsUnauthorized()
    {
        using var client = CreateClient();

        var response = await client.GetAsync(
            "/api/account");

        Assert.Equal(
            HttpStatusCode.Unauthorized,
            response.StatusCode);
    }

    [Fact]
    public async Task Get_Authenticated_ReturnsCurrentAccount()
    {
        var email = CreateUniqueEmail();
        using var client = CreateClient();

        var expectedUserId = await RegisterUserAsync(
            client,
            email);

        await AuthenticateAsync(
            client,
            email);

        var response = await client.GetAsync(
            "/api/account");

        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);

        var account =
            await response.Content
                .ReadFromJsonAsync<AccountDto>();

        Assert.NotNull(account);
        Assert.Equal(
            expectedUserId,
            account.Id);
        Assert.Equal(
            email,
            account.Email);
    }

    [Fact]
    public async Task Delete_Unauthenticated_ReturnsUnauthorized()
    {
        using var client = CreateClient();

        var response = await client.DeleteAsync(
            "/api/account");

        Assert.Equal(
            HttpStatusCode.Unauthorized,
            response.StatusCode);
    }

    [Fact]
    public async Task Delete_Authenticated_DeletesCurrentAccount()
    {
        var email = CreateUniqueEmail();
        using var client = CreateClient();

        var userId = await RegisterUserAsync(
            client,
            email);

        await AuthenticateAsync(
            client,
            email);

        var response = await client.DeleteAsync(
            "/api/account");

        Assert.Equal(
            HttpStatusCode.NoContent,
            response.StatusCode);

        using var scope =
            factory.Services.CreateScope();

        var userManager =
            scope.ServiceProvider
                .GetRequiredService<UserManager<ApplicationUser>>();

        var user =
            await userManager.FindByIdAsync(
                userId.ToString());

        Assert.Null(user);
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
        string email)
    {
        var response = await client.PostAsJsonAsync(
            "/api/authentication/register",
            new RegisterUserRequest(
                email,
                ValidPassword));

        response.EnsureSuccessStatusCode();

        return await response.Content
            .ReadFromJsonAsync<Guid>();
    }

    private static async Task AuthenticateAsync(
        HttpClient client,
        string email)
    {
        var response = await client.PostAsJsonAsync(
            "/api/authentication/login",
            new LoginUserRequest(
                email,
                ValidPassword));

        response.EnsureSuccessStatusCode();

        var login =
            await response.Content
                .ReadFromJsonAsync<LoginUserResponse>();

        Assert.NotNull(login);
        Assert.False(
            string.IsNullOrWhiteSpace(
                login.AccessToken));

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(
                "Bearer",
                login.AccessToken);
    }

    private static string CreateUniqueEmail()
    {
        return $"user-{Guid.NewGuid():N}@example.com";
    }
}
