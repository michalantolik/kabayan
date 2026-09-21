namespace Kabayan.Api.Authentication.Login;

/// <summary>
/// Authentication data returned by the API.
/// </summary>
public sealed record LoginUserResponse(
    string AccessToken);
