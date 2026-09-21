namespace Kabayan.Web.Features.Authentication;

/// <summary>
/// Result of a Web registration request to the Launchpad API.
/// </summary>
public sealed record RegistrationOperationResult(
    bool Succeeded,
    IReadOnlyCollection<string> Errors)
{
    public static RegistrationOperationResult Success()
        => new(true, []);

    public static RegistrationOperationResult Failure(
        IReadOnlyCollection<string> errors)
        => new(false, errors);
}

/// <summary>
/// Result of a Web login request to the Launchpad API.
/// </summary>
public sealed record LoginOperationResult(
    bool Succeeded,
    string? AccessToken)
{
    public static LoginOperationResult Success(
        string accessToken)
        => new(true, accessToken);

    public static LoginOperationResult Failure()
        => new(false, null);
}

/// <summary>
/// Current authenticated user data returned by the Launchpad API.
/// </summary>
public sealed record CurrentUserResponse(
    Guid UserId);
