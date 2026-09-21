using Kabayan.Application.Authentication;
using Kabayan.Application.Authentication.Login;
using Kabayan.Application.Common.Identity;
using Moq;

namespace Kabayan.Application.Tests.Authentication.Login;

public sealed class LoginUserHandlerTests
{
    [Fact]
    public async Task HandleAsync_ValidCredentials_ReturnsGeneratedAccessToken()
    {
        var userId = Guid.NewGuid();
        const string email = "user@example.com";
        const string password = "ValidPassword1!";
        const string accessToken = "access-token";

        using var cancellationTokenSource =
            new CancellationTokenSource();

        var identityService =
            new Mock<IIdentityService>();

        identityService
            .Setup(service => service.AuthenticateUserAsync(
                email,
                password,
                cancellationTokenSource.Token))
            .ReturnsAsync(userId);

        var accessTokenGenerator =
            new Mock<IAccessTokenGenerator>();

        accessTokenGenerator
            .Setup(generator => generator.GenerateToken(userId))
            .Returns(accessToken);

        var handler = new LoginUserHandler(
            identityService.Object,
            accessTokenGenerator.Object);

        var result = await handler.HandleAsync(
            new LoginUserCommand(
                email,
                password),
            cancellationTokenSource.Token);

        Assert.True(result.Succeeded);
        Assert.Equal(
            accessToken,
            result.AccessToken);

        identityService.Verify(
            service => service.AuthenticateUserAsync(
                email,
                password,
                cancellationTokenSource.Token),
            Times.Once);

        accessTokenGenerator.Verify(
            generator => generator.GenerateToken(userId),
            Times.Once);
    }

    [Fact]
    public async Task HandleAsync_InvalidCredentials_ReturnsFailureWithoutGeneratingAccessToken()
    {
        const string email = "user@example.com";
        const string password = "WrongPassword1!";

        var identityService =
            new Mock<IIdentityService>();

        identityService
            .Setup(service => service.AuthenticateUserAsync(
                email,
                password,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((Guid?)null);

        var accessTokenGenerator =
            new Mock<IAccessTokenGenerator>();

        var handler = new LoginUserHandler(
            identityService.Object,
            accessTokenGenerator.Object);

        var result = await handler.HandleAsync(
            new LoginUserCommand(
                email,
                password));

        Assert.False(result.Succeeded);
        Assert.Null(result.AccessToken);

        accessTokenGenerator.Verify(
            generator => generator.GenerateToken(
                It.IsAny<Guid>()),
            Times.Never);
    }
}
