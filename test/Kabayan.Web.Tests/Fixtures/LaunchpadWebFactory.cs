using Kabayan.Web.Features.Account;
using Kabayan.Web.Features.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Net;
using System.Net.Http.Json;

namespace Kabayan.Web.Tests.Fixtures;

public sealed class LaunchpadWebFactory
    : WebApplicationFactory<Program>
{
    public static readonly Guid TestUserId =
        Guid.Parse("4eeb1de3-1cf4-44cb-8e57-95fc2f927094");

    public const string ValidEmail = "user@example.com";
    public const string RegistrationEmail = "new-user@example.com";
    public const string ValidPassword = "ValidPassword1!";
    public const string AccessToken = "launchpad-test-access-token";

    protected override void ConfigureWebHost(
        IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            services.RemoveAll<AuthenticationApiClient>();
            services.RemoveAll<AccountApiClient>();

            var authenticationHttpClient = new HttpClient(
                new AuthenticationApiHandler())
            {
                BaseAddress = new Uri("https://api.launchpad.test/")
            };

            services.AddSingleton(
                new AuthenticationApiClient(
                    authenticationHttpClient));

            var accountHttpClient = new HttpClient(
                new AccountApiHandler())
            {
                BaseAddress = new Uri("https://api.launchpad.test/")
            };

            services.AddSingleton(
                new AccountApiClient(
                    accountHttpClient));
        });
    }

    private sealed class AuthenticationApiHandler
        : HttpMessageHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            if (request.Method == HttpMethod.Post &&
                request.RequestUri?.AbsolutePath ==
                    "/api/authentication/register")
            {
                var registrationRequest = await request.Content!
                    .ReadFromJsonAsync<RegistrationRequest>(
                        cancellationToken);

                if (registrationRequest?.Email ==
                        RegistrationEmail &&
                    registrationRequest.Password ==
                        ValidPassword)
                {
                    return new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = JsonContent.Create(TestUserId)
                    };
                }

                return new HttpResponseMessage(
                    HttpStatusCode.BadRequest)
                {
                    Content = JsonContent.Create(
                        new
                        {
                            Status = 400,
                            Title = "User registration failed.",
                            Errors = new[]
                            {
                                "Email is already registered."
                            }
                        })
                };
            }

            if (request.Method == HttpMethod.Post &&
                request.RequestUri?.AbsolutePath ==
                    "/api/authentication/login")
            {
                var loginRequest = await request.Content!
                    .ReadFromJsonAsync<LoginRequest>(
                        cancellationToken);

                if (loginRequest?.Email == ValidEmail &&
                    loginRequest.Password == ValidPassword)
                {
                    return new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = JsonContent.Create(
                            new
                            {
                                AccessToken
                            })
                    };
                }

                return new HttpResponseMessage(
                    HttpStatusCode.Unauthorized);
            }

            if (request.Method == HttpMethod.Get &&
                request.RequestUri?.AbsolutePath ==
                    "/api/authentication/me")
            {
                if (HasValidBearerToken(request))
                {
                    return new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = JsonContent.Create(
                            new
                            {
                                UserId = TestUserId
                            })
                    };
                }

                return new HttpResponseMessage(
                    HttpStatusCode.Unauthorized);
            }

            return new HttpResponseMessage(
                HttpStatusCode.NotFound);
        }

        private sealed record RegistrationRequest(
            string Email,
            string Password);

        private sealed record LoginRequest(
            string Email,
            string Password);
    }

    private sealed class AccountApiHandler
        : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            if (!HasValidBearerToken(request))
            {
                return Task.FromResult(
                    new HttpResponseMessage(
                        HttpStatusCode.Unauthorized));
            }

            if (request.Method == HttpMethod.Get &&
                request.RequestUri?.AbsolutePath ==
                    "/api/account")
            {
                return Task.FromResult(
                    new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = JsonContent.Create(
                            new
                            {
                                Id = TestUserId,
                                Email = ValidEmail
                            })
                    });
            }

            if (request.Method == HttpMethod.Delete &&
                request.RequestUri?.AbsolutePath ==
                    "/api/account")
            {
                return Task.FromResult(
                    new HttpResponseMessage(
                        HttpStatusCode.NoContent));
            }

            return Task.FromResult(
                new HttpResponseMessage(
                    HttpStatusCode.NotFound));
        }
    }

    private static bool HasValidBearerToken(
        HttpRequestMessage request)
    {
        return request.Headers.Authorization?.Scheme == "Bearer" &&
               request.Headers.Authorization.Parameter == AccessToken;
    }
}
