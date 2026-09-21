using Microsoft.AspNetCore.Components;

namespace Kabayan.Web.Features.Authentication.Pages;

public partial class Login
{
    [SupplyParameterFromQuery(Name = "error")]
    private string? Error { get; set; }

    [SupplyParameterFromQuery(Name = "returnUrl")]
    private string? ReturnUrl { get; set; }

    private string? ErrorMessage => Error switch
    {
        AuthenticationErrorCodes.Required =>
            "Email and password are required.",

        AuthenticationErrorCodes.InvalidCredentials =>
            "Invalid email or password.",

        AuthenticationErrorCodes.ServiceUnavailable =>
            "Authentication service is unavailable.",

        _ => null
    };

    private string GetSafeReturnUrl()
    {
        if (string.IsNullOrWhiteSpace(ReturnUrl) ||
            !ReturnUrl.StartsWith(
                "/",
                StringComparison.Ordinal) ||
            ReturnUrl.StartsWith(
                "//",
                StringComparison.Ordinal))
        {
            return "/";
        }

        return ReturnUrl;
    }
}
