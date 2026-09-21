using Kabayan.Application.Authentication.Register;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Kabayan.Api.Common.ExceptionHandling;

/// <summary>
/// Handles application exceptions and maps them to HTTP responses.
/// </summary>
public sealed class ApiExceptionHandler
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not UserRegistrationException registrationException)
        {
            return false;
        }

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "User registration failed."
        };

        problemDetails.Extensions["errors"] =
            registrationException.Errors;

        httpContext.Response.StatusCode =
            StatusCodes.Status400BadRequest;

        httpContext.Response.ContentType =
            "application/problem+json";

        await httpContext.Response.WriteAsJsonAsync(
            problemDetails,
            cancellationToken);

        return true;
    }
}
