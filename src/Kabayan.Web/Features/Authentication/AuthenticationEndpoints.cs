using Kabayan.Web.Features.Authentication.Security;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace Kabayan.Web.Features.Authentication;

/// <summary>
/// Provides server-side authentication endpoints for the Web application.
/// </summary>
public static class AuthenticationEndpoints
{
    public static IEndpointRouteBuilder MapAuthenticationEndpoints(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
            "/authentication/register",
            RegisterAsync);

        endpoints.MapPost(
            "/authentication/login",
            LoginAsync);

        endpoints.MapPost(
            "/authentication/logout",
            LogoutAsync);

        return endpoints;
    }

    private static async Task<IResult> RegisterAsync(
        HttpContext context,
        AuthenticationApiClient authenticationApiClient,
        IAntiforgery antiforgery,
        CancellationToken cancellationToken)
    {
        if (!await IsAntiforgeryRequestValidAsync(
                context,
                antiforgery))
        {
            return Results.BadRequest();
        }

        var form = await context.Request.ReadFormAsync(
            cancellationToken);

        var email = form["email"]
            .ToString()
            .Trim();

        var password = form["password"]
            .ToString();

        var confirmPassword = form["confirmPassword"]
            .ToString();

        if (string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(password) ||
            string.IsNullOrWhiteSpace(confirmPassword))
        {
            return RedirectToRegister(
            [
                "Email, password and password confirmation are required."
            ]);
        }

        if (!string.Equals(
                password,
                confirmPassword,
                StringComparison.Ordinal))
        {
            return RedirectToRegister(
            [
                "The passwords do not match."
            ]);
        }

        RegistrationOperationResult registrationResult;

        try
        {
            registrationResult =
                await authenticationApiClient.RegisterAsync(
                    email,
                    password,
                    cancellationToken);
        }
        catch (HttpRequestException)
        {
            return RedirectToRegister(
            [
                "Registration service is unavailable."
            ]);
        }

        if (!registrationResult.Succeeded)
        {
            return RedirectToRegister(
                registrationResult.Errors);
        }

        return Results.LocalRedirect(
            "/register?status=created");
    }

    private static async Task<IResult> LoginAsync(
        HttpContext context,
        AuthenticationApiClient authenticationApiClient,
        IOptions<AuthenticationCookieOptions> authenticationOptions,
        IAntiforgery antiforgery,
        CancellationToken cancellationToken)
    {
        if (!await IsAntiforgeryRequestValidAsync(
                context,
                antiforgery))
        {
            return Results.BadRequest();
        }

        var form = await context.Request.ReadFormAsync(
            cancellationToken);

        var email = form["email"]
            .ToString()
            .Trim();

        var password = form["password"]
            .ToString();

        var returnUrl = GetSafeReturnUrl(
            form["returnUrl"].ToString());

        if (string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(password))
        {
            return RedirectToLogin(
                returnUrl,
                AuthenticationErrorCodes.Required);
        }

        LoginOperationResult loginResult;

        try
        {
            loginResult = await authenticationApiClient.LoginAsync(
                email,
                password,
                cancellationToken);
        }
        catch (HttpRequestException)
        {
            return RedirectToLogin(
                returnUrl,
                AuthenticationErrorCodes.ServiceUnavailable);
        }

        if (!loginResult.Succeeded ||
            string.IsNullOrWhiteSpace(loginResult.AccessToken))
        {
            return RedirectToLogin(
                returnUrl,
                AuthenticationErrorCodes.InvalidCredentials);
        }

        var claims = new[]
        {
            new Claim(
                ClaimTypes.Name,
                email),

            new Claim(
                ClaimTypes.Email,
                email),

            new Claim(
                AuthenticationClaimTypes.AccessToken,
                loginResult.AccessToken)
        };

        var identity = new ClaimsIdentity(
            claims,
            CookieAuthenticationDefaults.AuthenticationScheme);

        var principal = new ClaimsPrincipal(identity);

        var properties = new AuthenticationProperties
        {
            AllowRefresh = false,
            IsPersistent = false,
            IssuedUtc = DateTimeOffset.UtcNow,
            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(
                authenticationOptions.Value
                    .AuthenticationCookieExpirationInMinutes)
        };

        await context.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal,
            properties);

        return Results.LocalRedirect(returnUrl);
    }

    private static async Task<IResult> LogoutAsync(
        HttpContext context,
        IAntiforgery antiforgery)
    {
        if (!await IsAntiforgeryRequestValidAsync(
                context,
                antiforgery))
        {
            return Results.BadRequest();
        }

        await context.SignOutAsync(
            CookieAuthenticationDefaults.AuthenticationScheme);

        return Results.LocalRedirect("/login");
    }

    private static async Task<bool> IsAntiforgeryRequestValidAsync(
        HttpContext context,
        IAntiforgery antiforgery)
    {
        try
        {
            await antiforgery.ValidateRequestAsync(context);
            return true;
        }
        catch (AntiforgeryValidationException)
        {
            return false;
        }
    }

    private static IResult RedirectToRegister(
        IReadOnlyCollection<string> errors)
    {
        var serializedErrors = string.Join(
            '\n',
            errors);

        var location =
            $"/register?errors={Uri.EscapeDataString(serializedErrors)}";

        return Results.LocalRedirect(location);
    }

    private static IResult RedirectToLogin(
        string returnUrl,
        string error)
    {
        var location =
            $"/login?returnUrl={Uri.EscapeDataString(returnUrl)}&error={Uri.EscapeDataString(error)}";

        return Results.LocalRedirect(location);
    }

    private static string GetSafeReturnUrl(
        string? returnUrl)
    {
        if (string.IsNullOrWhiteSpace(returnUrl) ||
            !returnUrl.StartsWith(
                "/",
                StringComparison.Ordinal) ||
            returnUrl.StartsWith(
                "//",
                StringComparison.Ordinal))
        {
            return "/";
        }

        return returnUrl;
    }
}
