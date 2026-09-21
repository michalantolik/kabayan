using Kabayan.Application.Account;
using Kabayan.Application.Account.GetAccount;
using Moq;

namespace Kabayan.Application.Tests.Account.GetAccount;

public sealed class GetAccountHandlerTests
{
    [Fact]
    public async Task HandleAsync_ExistingAccount_ReturnsAccount()
    {
        var userId = Guid.NewGuid();

        var account = new AccountDto(
            userId,
            "user@example.com");

        using var cancellationTokenSource =
            new CancellationTokenSource();

        var accountService =
            new Mock<IAccountService>();

        accountService
            .Setup(service => service.GetAccountAsync(
                userId,
                cancellationTokenSource.Token))
            .ReturnsAsync(account);

        var handler = new GetAccountHandler(
            accountService.Object);

        var result = await handler.HandleAsync(
            userId,
            cancellationTokenSource.Token);

        Assert.Equal(
            account,
            result);

        accountService.Verify(
            service => service.GetAccountAsync(
                userId,
                cancellationTokenSource.Token),
            Times.Once);
    }
}
