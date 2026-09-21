using Kabayan.Application.Common.Identity;

namespace Kabayan.Application.Authentication.Login;

/// <summary>
/// Handles user login requests.
/// </summary>
public sealed class LoginUserHandler
{
    private readonly IIdentityService _identityService;
    private readonly IAccessTokenGenerator _accessTokenGenerator;

    public LoginUserHandler(
        IIdentityService identityService,
        IAccessTokenGenerator accessTokenGenerator)
    {
        _identityService = identityService;
        _accessTokenGenerator = accessTokenGenerator;
    }

    public async Task<LoginUserResult> HandleAsync(
        LoginUserCommand command,
        CancellationToken cancellationToken = default)
    {
        var userId =
            await _identityService.AuthenticateUserAsync(
                command.Email,
                command.Password,
                cancellationToken);

        if (userId is null)
        {
            return LoginUserResult.Failure();
        }

        var accessToken =
            _accessTokenGenerator.GenerateToken(userId.Value);

        return LoginUserResult.Success(accessToken);
    }
}

/// <summary>
/// Represents the result of a user login attempt.
/// </summary>
public sealed record LoginUserResult(
    bool Succeeded,
    string? AccessToken)
{
    public static LoginUserResult Success(
        string accessToken)
        => new(
            true,
            accessToken);

    public static LoginUserResult Failure()
        => new(
            false,
            null);
}
