using Kabayan.Web.Common.Api;
using Kabayan.Web.Common.Composition;
using Kabayan.Web.Components;
using Kabayan.Web.Features.Account;
using Kabayan.Web.Features.Authentication;
using Kabayan.Web.Features.Authentication.Security;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

var authenticationOptions = builder.Configuration
    .GetSection(AuthenticationCookieOptions.SectionName)
    .Get<AuthenticationCookieOptions>()
    ?? throw new InvalidOperationException(
        "Authentication configuration is missing.");

builder.Services
    .AddRazorComponents()
    .AddInteractiveServerComponents(options =>
    {
        options.DetailedErrors =
            builder.Environment.IsDevelopment();
    });

builder.Services
    .AddOptions<ApiOptions>()
    .Bind(
        builder.Configuration.GetSection(
            ApiOptions.SectionName))
    .Validate(
        options =>
            Uri.TryCreate(
                options.BaseUrl,
                UriKind.Absolute,
                out var baseUri) &&
            baseUri.Scheme is "http" or "https",
        "Api:BaseUrl must be an absolute HTTP or HTTPS URL.")
    .ValidateOnStart();

builder.Services
    .AddOptions<AuthenticationCookieOptions>()
    .Bind(
        builder.Configuration.GetSection(
            AuthenticationCookieOptions.SectionName))
    .Validate(
        options =>
            options.AuthenticationCookieExpirationInMinutes > 0,
        "Authentication:AuthenticationCookieExpirationInMinutes must be greater than zero.")
    .ValidateOnStart();

builder.Services
    .AddAuthentication(
        CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name =
            "__Host-Kabayan.Authentication";

        options.Cookie.Path = "/";
        options.Cookie.HttpOnly = true;
        options.Cookie.IsEssential = true;
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.Cookie.SecurePolicy =
            CookieSecurePolicy.Always;

        options.LoginPath = "/login";
        options.AccessDeniedPath = "/login";
        options.ReturnUrlParameter = "returnUrl";

        options.ExpireTimeSpan =
            TimeSpan.FromMinutes(
                authenticationOptions
                    .AuthenticationCookieExpirationInMinutes);

        // The Web session contains the API access token, so it must not
        // be extended beyond the token lifetime.
        options.SlidingExpiration = false;

        options.Events.OnValidatePrincipal = context =>
        {
            var accessToken = context.Principal?
                .FindFirst(
                    AuthenticationClaimTypes.AccessToken)?
                .Value;

            if (string.IsNullOrWhiteSpace(accessToken))
            {
                context.RejectPrincipal();
            }

            return Task.CompletedTask;
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();

builder.Services.AddHealthChecks();

ProductCapabilities.ConfigureServices(
    builder.Services,
    builder.Configuration);

builder.Services.AddHttpClient<AuthenticationApiClient>(
    (serviceProvider, client) =>
    {
        var apiOptions = serviceProvider
            .GetRequiredService<IOptions<ApiOptions>>()
            .Value;

        client.BaseAddress = new Uri(
            apiOptions.BaseUrl,
            UriKind.Absolute);
    });

builder.Services.AddHttpClient<AccountApiClient>(
    (serviceProvider, client) =>
    {
        var apiOptions = serviceProvider
            .GetRequiredService<IOptions<ApiOptions>>()
            .Value;

        client.BaseAddress = new Uri(
            apiOptions.BaseUrl,
            UriKind.Absolute);
    });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler(
        "/error",
        createScopeForErrors: true);

    app.UseHsts();
}

app.UseHttpsRedirection();

ProductCapabilities.ConfigurePipeline(app);

app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.MapHealthChecks("/health");

app.MapStaticAssets();

app.MapAuthenticationEndpoints();
app.MapAccountEndpoints();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

public partial class Program;
