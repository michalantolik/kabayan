using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Kabayan.Web.Features.Account;

/// <summary>
/// Provides server-side account endpoints for the Web application.
/// </summary>
public static class AccountEndpoints
{
    public static IEndpointRouteBuilder MapAccountEndpoints(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
            "/account/deleted",
            AccountDeletedAsync);

        return endpoints;
    }

    private static async Task<IResult> AccountDeletedAsync(
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

        return Results.LocalRedirect(
            "/login?status=account-deleted");
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
}
